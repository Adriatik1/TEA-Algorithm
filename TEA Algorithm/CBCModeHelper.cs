using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TEA_Algorithm
{
    public static class CBCModeHelper
    {
        public static void CBCEncrypt()
        {
            Random rand = new Random();
            int[] key = { 10, 12, 13, 14 };                     //instantiating a key
            CBCMode cbc = new CBCMode(key);                 //instantiating a TEA class

            int[] img = new int[2];

            int[] IV = { rand.Next(), rand.Next() };      //generating a random IV

            FileStream fileStream = File.OpenRead(@"..\..\..\TestImages\Tux.bmp");
            FileStream outPutStream = new FileStream(@"..\..\..\TestImages\CBCEncryptedTux.bmp", FileMode.OpenOrCreate);

            BinaryReader dataIn = new BinaryReader(fileStream);
            BinaryWriter dataOut = new BinaryWriter(outPutStream);


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


            bool firstTime = true;       //to know when to apply IV or the previous encrypted block
            int[] cipher = new int[2];
            bool check = true;           //to catch where the reading from the file is stopped
            while (dataIn.BaseStream.Position != dataIn.BaseStream.Length)
            {
                try
                {
                    img[0] = dataIn.ReadInt32();
                    check = true;
                    img[1] = dataIn.ReadInt32();
                    if (firstTime)
                    {       //if true, the block is passed with IV to be encrypted by TEA algorithm
                        cipher = cbc.Encrypt(img, IV);
                        firstTime = false;      //set firstTime to false sense IV is only encrypted in the first block
                    }
                    else
                        cipher = cbc.Encrypt(img, cipher);      //pass the block with the previous encrypted block

                    dataOut.Write(cipher[0]);
                    dataOut.Write(cipher[1]);
                    check = false;
                }
                catch (EndOfStreamException e)
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
        }

        public static void CBCDecrypt()
        {
            Random rand = new Random();
            int[] key = { 10, 12, 13, 14 };                     //instantiating a key
            CBCMode cbc = new CBCMode(key);                 //instantiating a TEA class

            int[] img = new int[2];

            int[] IV = { rand.Next(), rand.Next() };      //generating a random IV

            FileStream fileStream = File.OpenRead(@"..\..\..\TestImages\CBCEncryptedTux.bmp");
            FileStream outPutStream = new FileStream(@"..\..\..\TestImages\CBCDecryptedTux.bmp", FileMode.OpenOrCreate);

            BinaryReader dataIn = new BinaryReader(fileStream);
            BinaryWriter dataOut = new BinaryWriter(outPutStream);

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

            int[] copyCipher = new int[2];
            bool firstTime = true;
            int[] plain = new int[2];
            bool check = true;

            while (dataIn.BaseStream.Position != dataIn.BaseStream.Length)
            {
                try
                {
                    img[0] = dataIn.ReadInt32();
                    check = true;
                    img[1] = dataIn.ReadInt32();

                    if (firstTime)
                    {                           //if true, the first block is passed with IV to be decrytped
                        plain = cbc.Decrypt(img, IV);
                        firstTime = false;                  //set first time to false
                    }
                    else                                    //if false, the block is passed with the previously encrypted block
                        plain = cbc.Decrypt(img, copyCipher);

                    dataOut.Write(plain[0]);
                    dataOut.Write(plain[1]);

                    copyCipher[0] = img[0];             //Save the previously encryted block in copyCipher to use it
                    copyCipher[1] = img[1];

                    check = false;
                }
                catch (EndOfStreamException e)
                {
                    if (!check)
                    {
                        dataOut.Write(img[0]);
                        dataOut.Write(img[1]);
                    }
                    else
                        dataOut.Write(img[0]); ;
                }

            }
            dataIn.Close();
            dataOut.Close();

        }

    }
}
