/* 
 * This software is based upon the book CUDA By Example by Sanders and Kandrot
 * and source code provided by NVIDIA Corporation.
 * It is a good idea to read the book while studying the examples!
*/
using System;
using System.Collections.Generic;
using System.Threading;
namespace CudafyByExample
{


    class Program
    {
        public const int N = 15;
        //public static Numeration numeration = new Numeration();
        //public static HyperCubeNumeration cubeNumeration = new HyperCubeNumeration();
  
       
        private static List<Thread> _threads = new List<Thread>();

        public static List<Thread> Threads
        {
            get => _threads;
            set => _threads = value;
        }



        static void Main(string[] args)
        {
            ////////////////////////////// Matrix Carrier 3 /////////////////////////////////
            //MatrixCarrier3.Start();


            ////////////////////////////// Matrix Carrier 2 /////////////////////////////////
            MatrixCarrier2.Start();

            ////////////////////////////// Matrix Carrier /////////////////////////////////
            //MatrixCarrier.Start();

            ////////////////////////////// hyper /////////////////////////////////
            //cubeNumeration.Start();
            /////////////////////////////// common ////////////////////////////////////// 
            //numeration.start();
        }




        public static void PrintMat(int[] matrix)
        {
            foreach (int i in matrix)
            {
                Console.Write(i + " ");
            }

            Console.WriteLine();
        }

        public static int GetValueOfCell(string column, string line)
        {
            int[] col = DevideStr(column);
            int[] row = DevideStr(line);
            for (int i = 0; i < row.Length; i++)
            {
                if (row[i] == 1)
                    if (row[i] != col[i])
                        return 0;
            }

            return 1;
        }

        public static int[] DevideStr(string str)
        {
            int[] div = new int[N + 1];
            string num = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == ',')
                {
                    div[int.Parse(num)] = 1;
                    num = "";
                }
                else
                    num += str[i];
            }

            return div;
        }

        public static int[] DevideStr2(string str, int d)
        {
            int[] div = new int[d];
            string num = "";
            int k = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == ',')
                {
                    div[k++] = int.Parse(num);
                    num = "";
                }
                else
                    num += str[i];
            }

            return div;
        }


    }
}
