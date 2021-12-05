using System;
using System.IO;

namespace TEA_Algorithm
{
    public class ECBModeHelper
    {
        public ECBModeHelper()
        {

        }

        public static void ECBEncrypt()
        {
            int[] key = { 10, 12, 13, 14 };     //Instantiating a key
            var ecb = new ECBMode(key);         //instantiating a TEA class

            int[] img = new int[2];         //img Variable will contain the block to be encrypted
            int left;
            int right;

          

            var imgIn = new FileStream(@"..\..\..\TestImages\Tux.bmp", FileMode.OpenOrCreate);
            var imgOut = new FileStream(@"..\..\..\TestImages\ECBEncryptedTux.bmp", FileMode.OpenOrCreate);

            var dataIn = new BinaryReader(imgIn);
            var dataOut = new BinaryWriter(imgOut);

            /* Skipping the first 10 blocks
             * each block is 64 bit. Thus, ReadInt() is applied twice
             * because ReadInt() return 32 bits
             */
            for (int i = 0; i < 10; i++)
            {
                if (dataIn.PeekChar() != -1)
                {
                    img[0] = dataIn.ReadInt32();
                    img[1] = dataIn.ReadInt32();
                    dataOut.Write(img[0]);
                    dataOut.Write(img[1]);
                }
            }


            int[] cipher = new int[2];
            bool check = true;               //Check variable used to know where did the file end

            while (dataIn.BaseStream.Position != dataIn.BaseStream.Length)
            {
                try
                {
                    img[0] = dataIn.ReadInt32();      //left sub block 
                    check = true;
                    img[1] = dataIn.ReadInt32();      //right sub block
                    cipher = ecb.Encrypt(img);      //Passing the block to TEA algorithm to encrypt it
                    dataOut.Write(cipher[0]);    //writing back the encrypted block
                    dataOut.Write(cipher[1]);
                    check = false;
                }
                catch (Exception e)
                {               //excetion is thrown if the file ends and dataIn.readInt() is executed  
                    if (!check)
                    {                       //if false, it means last block were not encrypted
                        dataOut.Write(img[0]);
                        dataOut.Write(img[1]);
                    }
                    else                            //if true, it means only last half a block is not encrypted
                        dataOut.Write(img[0]);
                }

            }
            dataIn.Close();
            dataOut.Close();



            imgOut.Close();
            imgIn.Close();
        }
    }
}
