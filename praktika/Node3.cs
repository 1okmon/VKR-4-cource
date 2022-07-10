using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CudafyByExample
{
    public class Node3
    {
        public string Data { get; set; }
        public int Rang { get; set; }
        public int[] sumInLines { get; set; }
        public int[] sumInCols { get; set; }
        public Node3 Parent { get; set; }
        public List<Node3> childs { get; set; }

        public Node3(string data)
        {
            this.Data = data;
            this.Rang = 1;
            childs = new List<Node3>();
        }
        public Node3()
        {
            childs = new List<Node3>();
        }

        public void Add(Node3 item)
        {
            item.Parent = this;
            item.Rang = this.Rang + 1;
            this.childs.Add(item);
            item.FillArrays();
            //Console.ReadLine();
        }

        public void FillArrays()
        {
            int[,] matr = new int[MatrixCarrier3.N, MatrixCarrier3.d];
            int i = 0;
            Node3 node = this;
            do
            {
                for (int j = 0; j < MatrixCarrier3.d; j++)
                {
                    string str = "";
                    str += node.Data[j];
                    matr[i, j] = int.Parse(str);
                }

                node = node.Parent;
                i++;
            } while (node != null);
            this.sumInCols = new int[MatrixCarrier3.N + 1];
            this.sumInLines = new int[MatrixCarrier3.d + 1];
            for (int i1 = 0; i1 < this.Rang; i1++)
            {
                int sum = 0;
                for (int j1 = 0; j1 < MatrixCarrier3.d; j1++)
                {
                    sum += matr[i1, j1];
                }
                this.sumInLines[sum]++;
            }

            for (int j1 = 0; j1 < MatrixCarrier3.d; j1++)
            {
                int sum = 0;
                for (int i1 = 0; i1 < this.Rang; i1++)
                {
                    sum += matr[i1, j1];
                }
                this.sumInCols[sum]++;
            }
            // PrintTrace(this);
            // Program.PrintMat(this.sumInCols);
            // Program.PrintMat(this.sumInLines);
            // Console.WriteLine("-------------");




        }
        public void Delete()
        {
            this.Parent.childs.Remove(this);
            this.Parent = null;
            //item.Rang = this.Rang + 1;
            //this.childs.Clear();
        }

        public void Print()
        {
            Console.WriteLine("Parent:" + this.Data + " Rang:" + this.Rang + " childs:");
            int k = 1;
            foreach (Node3 child in this.childs)
            {
                Console.WriteLine("    " + k + ") Data:" + child.Data + " Rang:" + child.Rang);
                k++;
            }
        }

        public static void PrintTrace(Node3 node)
        {
            Node3 node1 = node;
            do
            {
                Console.WriteLine(node1.Data);
                node1 = node1.Parent;

            } while (node1 != null);
            //Console.WriteLine(node1.Data);
            Console.WriteLine("------------------");
        }
    }
}
