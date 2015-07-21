using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TajTetrisGame
{
    /// <summary>
    /// This is a high entropy Random Generator.
    /// This is one designed by Jesse Mitchell.
    /// Quite a few of the versions are also cryptographically secure.
    /// </summary>
    class PseudoRandomGenerator
    {
        private long[] Generation;
        private byte[] Generated;
        private const int ROUNDS = 1105;
        private int position;

        public PseudoRandomGenerator() : this(1024*16) //16 KB.
        {
           
        }

        public PseudoRandomGenerator(int size) : this(size, (DateTime.Now.ToString()))
        {
           
        }

        public PseudoRandomGenerator(int size, string key)
        {
            position = 0;
            Generation = new long[size];
            Generated = new byte[size];
            Generate(key);
        }

        /// <summary>
        /// Generate the Random Data using the key.
        /// </summary>
        /// <param name="key"></param>
        private void Generate(string key)
        {
            int seed = 1;
            char[] keyN = key.ToCharArray();
            Generation[0] = Generation.Length;

            int pos = 0;
            //Seed the array with the password, and also make the seed.
            foreach(char let in keyN)
            {
                Generation[pos++ + 1] += let;
                seed += let;
            }
            
            //Seed the data with generated values from a seed function.
            for(int i = 0; i < Generation.Length; i++)
            {
                Generation[i] += Math.Abs(SeedFunction(i, seed));
            }

            //Cipher it.
            for(int i = 0; i < ROUNDS; i++)
            {
                Cipher();
                i++; //Just because.
                CipherB(); 
            }

            ShrinkToBytes();
        }


        /// <summary>
        /// This method adds previous numbers in the array, and it gets moduloed and mutated
        /// through waterfalling. The process is not reversible, and generates high entropy.
        /// </summary>
        private void Cipher()
        {
            for(int i = 1; i < Generation.Length; i++)
            {
                Generation[i] += Generation[i - 1];

                if (Generation[i] < 0) Generation[i] *= -1;
                if (Generation[i] > 400000000) Generation[i] %= 913131;
            }
        }

        /// <summary>
        /// This method will mutate the data again for a new fresh start.
        /// </summary>
        private void Recycle()
        {
            for (int i = 0;i < 3;i++)
            {
                Cipher();
                CipherB();
            }
            ShrinkToBytes();
            position = 0;
        }

        /// <summary>
        /// Same here. It just does it in reverse.
        /// </summary>
        private void CipherB()
        {
            for (int i = Generation.Length - 2; i >= 0; i--)
            {
                Generation[i] += Generation[i + 1];

                if (Generation[i] < 0) Generation[i] *= -1;
                if (Generation[i] > 400000000) Generation[i] %= 913131;
            }
        }

        /// <summary>
        /// Shrinks the data into the byte range to make it more managable and highly entropic. No way of reversal.
        /// </summary>
        private void ShrinkToBytes()
        {
            for(int i = 0; i < Generation.Length; i++)
            {
                Generation[i] %= 256;
                Generated[i] = (byte)Generation[i];
            }
        }

        /// <summary>
        /// Returns a random byte from the next position.
        /// </summary>
        /// <returns></returns>
        public byte GetRandomByte()
        {
            if (position >= Generated.Length) Recycle();

            return Generated[position++];
        }

        /// <summary>
        /// Returns a random 4 byte integer.
        /// </summary>
        /// <returns></returns>
        public int GetRandomInt()
        {
            if (position + 4 >= Generated.Length) Recycle();

            return (Generated[position++] << (3 * 8)) | (Generated[position++] << (2 * 8)) | (Generated[position++] << (1 * 8)) | (Generated[position++] << (0 * 8));
        }


        /// <summary>
        /// Fills the array with random bytes.
        /// </summary>
        /// <param name="arr"></param>
        public void FillBytes(byte[] arr)
        {
            for(int i = 0; i < arr.Length; i++)
            {
                arr[i] = GetRandomByte();
            }
        }

        //This should be swapped out. For different programs.
        private long SeedFunction(long pos, long seed)
        {
            return pos * pos + 2 * pos + pos * pos * pos + seed * pos + seed;
        }


    }
}
