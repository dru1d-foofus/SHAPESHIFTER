using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace SHAPESHIFTER
{
    class Helpers
    {
        public static bool ValidateIP(string ip)
        {
            // Check if the string has 4 octets with Linq
            if (ip.Count(d => d == '.') != 3) return false;
            // Validate the IP
            return IPAddress.TryParse(ip, out IPAddress addr);
             
        }
        
        public static string GenerateRandomFileName()
        {
            Random random = new Random();
            const string charset = "abcdefghijklmnopqrstuvwxyz0123456789";
            char[] name = Enumerable.Repeat(charset, 12).Select(s => s[random.Next(s.Length)]).ToArray();
            return new string(name) + ".exe";
        }

        public static string KeyParser(byte[] results)
        {
            string keyString = Encoding.ASCII.GetString(results).Replace("\0", string.Empty);
            char[] delimiter = { ':' };
            string[] key = keyString.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            return key[1];
        }
        public static IList<string> ResultsParser(byte[] results)
        {
            IList<string> hookedFunctions = new List<string>();
            string[] functions =
            {
                "NtClose",
                "NtAllocateVirtualMemory",
                "NtAllocateVirtualMemoryEx",
                "NtCreateThread",
                "NtCreateThreadEx",
                "NtCreateUserProcess",
                "NtFreeVirtualMemory",
                "NtLoadDriver",
                "NtMapViewOfSection",
                "NtOpenProcess",
                "NtProtectVirtualMemory",
                "NtQueueApcThread",
                "NtQueueApcThreadEx",
                "NtResumeThread",
                "NtSetContextThread",
                "NtSetInformationProcess",
                "NtSuspendThread",
                "NtUnloadDriver",
                "NtWriteVirtualMemory"
            };

            int i = 0;
            foreach(byte result in results)
            {
                if(result == 1) hookedFunctions.Add(functions[i]);
                i++;
            }

            byte[] data = null;


            return hookedFunctions;
        }

        public static string ByteArrayToFormattedString(byte[] array)
        {
            StringBuilder formatted = new StringBuilder(BitConverter.ToString(array).Replace("-", ", 0x"));
            formatted.Insert(0, "0x");

            for (int i = 0; i < (formatted.Length / 9998); i++)
            {
                int insertPosition = i * 9998;
                formatted.Insert(insertPosition, Environment.NewLine);
            }

            return formatted.ToString();
            
        }

        public static byte[][] BufferSplit(byte[] buffer, int blockSize)
        {
            byte[][] blocks = new byte[(buffer.Length + blockSize - 1) / blockSize][];

            for (int i = 0, j = 0; i < blocks.Length; i++, j += blockSize)
            {
                blocks[i] = new byte[Math.Min(blockSize, buffer.Length - j)];
                Array.Copy(buffer, j, blocks[i], 0, blocks[i].Length);
            }

            return blocks;
        }
        public static string ExpandString(string str, int length)
        {
            var result = new StringBuilder(length, length);
            var whole = length / str.Length;
            for (var i = 0; i < whole; i++)
            {
                result.Append(str);
            }
            result.Append(str, 0, length % str.Length);
            return result.ToString();
        }
        public static byte[] exclusiveOR(byte[] key, byte[] str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                str[i] = (byte)(str[i] ^ key[i]);
            }
            return str;
        }
    }
}