using System;

namespace TEA_Algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("DBD Encryption...");
            CBCModeHelper.CBCEncrypt();
            Console.WriteLine("DBD Decryption...");
            CBCModeHelper.CBCDecrypt();
        }
    }
}
