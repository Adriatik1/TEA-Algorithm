using System;
using System.Collections.Generic;
using System.Text;

namespace TEA_Algorithm
{
    public class CBCMode : TEA
    {
        public CBCMode()
        {
            this.key = null;
        }

        public CBCMode(int[] keyAdd)
        {
            this.key = new int[4];
            key[0] = keyAdd[0];
            key[1] = keyAdd[1];
            key[2] = keyAdd[2];
            key[3] = keyAdd[3];

        }

        /// <summary>
        /// CBD Mode Encryption. 
        /// It takes a block of 64 but size and the previous encrypted block, which is 64bit too.
        /// </summary>
        /// <param name="plainText">Block of size 64 bit</param>
        /// <param name="previous">Previously encrypted block of size 64 bit</param>
        /// <returns></returns>
        public int[] Encrypt(int[] plainText, int[] previous)
        {
            //This shouldnt be the case
            if (key == null)
            {
                Console.WriteLine("Error: There is no key!");
                Environment.Exit(0);
            }

            /* In CBC we XOR the block with the previously encrypted block */
            int left = plainText[0] ^ previous[0];
            int right = plainText[1] ^ previous[1];

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
        /// CBD Mode Decryption. 
        /// It takes a block of 64 but size and the previous encrypted block, which is 64bit too.
        /// </summary>
        /// <param name="plainText">Block of size 64 bit</param>
        /// <param name="previous">Previously encrypted block of size 64 bit</param>
        /// <returns></returns>
        public int[] Decrypt(int[] cipherText, int[] previous)
        {
            //This shouldnt be the case
            if (key == null)
            {
                Console.WriteLine("Error: There is no key!");
                Environment.Exit(0);
            }

            /* Diving the block into left and right sub blocks */
            int left = cipherText[0];
            int right = cipherText[1];

            sum = DELTA << 5;       //initialize the sum variable

            for (int i = 0; i < 32; i++)
            {
                right -= ((left << 4) + key[2]) ^ (left + sum) ^ ((left >> 5) + key[3]);
                left -= ((right << 4) + key[0]) ^ (right + sum) ^ ((right >> 5) + key[1]);
                sum -= DELTA;
            }

            /*XOR the result of TEA Algorithm with the previous block */
            int[] block = new int[2];
            block[0] = left ^ previous[0];
            block[1] = right ^ previous[1];

            return block;

        }
    }
}
