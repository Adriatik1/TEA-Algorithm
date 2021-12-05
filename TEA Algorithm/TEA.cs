using System;
using System.Collections.Generic;
using System.Text;

namespace TEA_Algorithm
{
    public class TEA
    {
        protected static int DELTA = unchecked((int)0x9e3779b9);
        protected static int ROUNDS = 32;
        protected int sum;


        protected int[] key;


        /// <summary>
        /// Constructor is used to initialize key
        /// </summary>

        public TEA()
        {
            key = null;
        }

        public TEA(int[] keyAdd)
        {
            key = new int[4];
            key[0] = keyAdd[0];
            key[1] = keyAdd[1];
            key[2] = keyAdd[2];
            key[3] = keyAdd[3];

        }


        /// <summary>
        /// Add or change the key
        /// </summary>
        /// <param name="key"></param>
        public void AddKey(int[] key)
        {
            if (key.Length < 4)
                Console.WriteLine("Error: Key is less than 128 bits");

            else if (key.Length > 4)
                Console.WriteLine("Error: Key is more than 128 bits");

            else
                this.key = key;
        }

        /// <summary>
        /// Prints keys if they are set, if not it prints that key is missing
        /// </summary>
        public void printKeys()
        {
            if (key == null)
            {
                Console.WriteLine("Key is not set yet");
            }

            Console.WriteLine("Keys are\n");
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine(key[i]);
            }
        }
    }
}
