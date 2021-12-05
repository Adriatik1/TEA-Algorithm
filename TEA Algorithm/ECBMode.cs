using System;
using System.Collections.Generic;
using System.Text;

namespace TEA_Algorithm
{
    public class ECBMode : TEA
    {
        public ECBMode()
        {
            this.key = null;
        }

        public ECBMode(int[] keyAdd)
        {
            this.key = new int[4];
            key[0] = keyAdd[0];
            key[1] = keyAdd[1];
            key[2] = keyAdd[2];
            key[3] = keyAdd[3];

        }

        /// <summary>
        /// Encrypts cipher text of size 2
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public int[] Encrypt(int[] plainText)
        {
            //This shouldnt be the case
            if (key == null)
            {
                Console.WriteLine("Error: There is no key defined!");
                Environment.Exit(0);
            }

            /* Sub Blocks */
            int left = plainText[0];
            int right = plainText[1];

            sum = 0;

            for (int i = 0; i < 32; i++)
            {
                sum += DELTA;
                left += ((right << 4) + key[0]) ^ (right + sum) ^ ((right >> 5) + key[1]);
                right += ((left << 4) + key[2]) ^ (left + sum) ^ ((left >> 5) + key[3]);

            }

            int[] block = new int[2];
            block[0] = left;
            block[1] = right;

            return block;

        }

        /// <summary>
        /// Decrypts cipher text of size 2
        /// </summary>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public int[] Decrypt(int[] cipherText)
        {
            //This shouldnt be the case
            if (key == null)
            {
                Console.WriteLine("Error: There is no key defined!");
                Environment.Exit(0);
            }

            /* Sub blocks */
            int left = cipherText[0];
            int right = cipherText[1];

            sum = DELTA << 5;

            for (int i = 0; i < 32; i++)
            {
                right -= ((left << 4) + key[2]) ^ (left + sum) ^ ((left >> 5) + key[3]);
                left -= ((right << 4) + key[0]) ^ (right + sum) ^ ((right >> 5) + key[1]);
                sum -= DELTA;
            }

            int[] block = new int[2];
            block[0] = left;
            block[1] = right;

            return block;

        }
    }
}
