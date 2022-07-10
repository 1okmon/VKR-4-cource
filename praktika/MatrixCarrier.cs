using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CudafyByExample
{
    

    class MatrixCarrier
    {
        public static int NumOfThreads = 3840;
        public static int N = 9;
        public static int L = 8;
        public static int differ=3;
        public static int S = 2;
        public static int d = S+2;
        //public static int d = 7;
        public static int D = 0;
        public static int NumOfOnes = 3 + ((L + 1) % 2);
        public static List<Node> results = new List<Node>();
        public static List<Node> toDelete = new List<Node>();
        public static bool found = false;
        public static int maxD = 9;



        public static void Start()
        {
           

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            while (D==0)
            {
                found = false;
                Node startNode = new Node(GetStartStringForFirstNode());
                FillOneNode(startNode);
                startNode.Print();
                FillTree(startNode);
                
                
                Console.WriteLine("------------");
                Console.WriteLine("num of 1=" + NumOfOnes);
                if (L % 2 == 0)
                    NumOfOnes += 2;
                else
                    NumOfOnes++;
                
                if (NumOfOnes > d)
                {
                    
                    NumOfOnes = 3 + ((L + 1) % 2);
                    Console.WriteLine("d=" + d);
                    Console.WriteLine(" не найдено решение => увеличиваем d");
                    d++;
                }
                if (d == maxD)
                {
                    N++;
                    d = S + 2;
                    NumOfOnes = 3 + ((L + 1) % 2);
                    Console.WriteLine(" не найдено решение при d="+ (maxD-1) +"=> увеличиваем N");
                }
                Console.WriteLine("N=" + N + " :S=" + S + " :L=" + L + " :d=" + d);
                Console.WriteLine("------------");

            }
            Console.WriteLine("Finish"+found);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.WriteLine("D=" + D);
            Console.ReadLine();

        }

        public static void FillTree(Node node)
        {
            //////////threads
            foreach (Node item in node.childs)
            {
                if (item.Rang < N)
                {
                    FasterFillOneNode(item);
                    FillTree(item);
                }                
            }
            
        }

        public static void FasterFillOneNode(Node startNode)
        {
            foreach(Node node in startNode.Parent.childs)
            {
                if (startNode.Parent.childs.IndexOf(node) >= startNode.Parent.childs.IndexOf(startNode))
                {
                    if (CheckStrInTree(startNode, node.Data))
                    {

                        Node add = new Node(node.Data);
                        startNode.Add(add);
                        if (add.Rang >= L)
                            if (!ChechBySumInColumnsInProcess(add))
                            {
                                add.Delete();
                            }
                    }
                }
                if (found)
                {
                    break;
                }
            }
        }


        public static void FillOneNode(Node startNode)
        {
            string startStr = GetStartStringForNode();
            while (!found && !CheckStrOnLast(startStr))
            {
                if (CheckStrInTree(startNode, startStr))
                {

                    Node add = new Node(startStr);

                    startNode.Add(add);
                    if (add.Rang >= L)
                        if (!ChechBySumInColumnsInProcess(add))
                        {
                            add.Delete();
                        }
                }
                //startStr = MoveOne(TurnStringToMass(startStr));
                startStr = GetNextStr3(startStr);
                //Console.WriteLine(startStr);
                //Console.ReadLine();


            }
            if (!found && CheckStrInTree(startNode, startStr))
            {
                Node add = new Node(startStr);
                startNode.Add(add);
                if (add.Rang >= L)
                    if (!ChechBySumInColumnsInProcess(add))
                    {
                        add.Delete();
                    }
            }
        }

        public static void FindSolution(Node node)
        {
            
            foreach(Node child in node.childs)
            {
                if (found)
                    break;
                if (child.childs.Count > 0)
                    FindSolution(child);
                else
                {
                    if (ChechBySumInColumns(child))
                    {
                        if (ChechByDifferentColumns(child))
                        {
                            D = d;
                            results.Add(child);
                            //Node.PrintTrace(child);
                            //PrintToFile(results);
                            found = true;
                            break;
                        }
                        //Node.PrintTrace(child);
                    }
                }
                
            }
        }


        public static bool ChechByDifferentColumns(Node node)
        {
            int[,] matr = new int[N,d];
            int i = 0;
            do
            {
                for (int j = 0; j < d; j++)
                {
                    string str = "";
                        str+= node.Data[j];
                    matr[i, j] =int.Parse(str);
                }

                node = node.Parent;
                i++;
            } while (node != null);
            bool check = false;
            for (int j1 = 0; j1 < d - 1; j1++)
            {
                for (int j2 = j1+1; j2 < d; j2++)
                {
                    check = false;
                    for(int row = 0; row < N; row++)
                    {
                        if (matr[row, j1] != matr[row, j2])
                        {
                            check = true;
                        }
                    }
                    if (!check)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool ChechBySumInColumnsInProcess(Node node)
        {
            int[] sumInCol = new int[d];
            bool checkInProcess = true;
            Node node1 = node;
            for (int i = 0; i < d; i++)
                sumInCol[i] = 0;
            do
            {
                for (int i = 0; i < node.Data.Length; i++)
                {
                    if (node.Data[i].Equals('1'))
                    {
                        sumInCol[i]++;
                    }
                }

                node = node.Parent;
            } while (node != null);

            for (int i = 0; i < d; i++)
                if (sumInCol[i] != L)
                {
                    checkInProcess=false;
                    break;
                }
            if (checkInProcess&& ChechByDifferentColumns(node1))
            {
                results.Add(node1);
                found = true;
                D = d;
                PrintToFile(results);
                return true;
            }
                    
            for (int i = 0; i < d; i++)
                if (sumInCol[i] > L)
                    return false;
            return true;
        }


        public static bool ChechBySumInColumns(Node node)
        {
            int[] sumInCol = new int[d];
            for (int i = 0; i < d; i++)
                sumInCol[i] = 0;
            do
            {
                for (int i = 0; i < node.Data.Length; i++)
                {
                    if (node.Data[i].Equals('1'))
                    {
                        sumInCol[i]++;
                    }
                }
       
                node = node.Parent;
            } while (node != null);
            for (int i = 0; i < d; i++)
                if (sumInCol[i] != L)
                    return false;
            return true;
        }

       

        public static bool CheckStrInTree(Node node, string str)
        {
            do
            {
                string strNode = node.Data;
                int sum = 0;
                for(int i=0; i < str.Length; i++)
                {
                    if ((str[i] == strNode[i])&&(str[i].Equals('1')))
                    {
                        sum++;
                    }
                }
                if (sum % 2 == 1)
                    return false;

                node = node.Parent;
            } while (node!= null);

            return true;
        }



        public static bool CheckStrOnLast(string str)
        {
            string last = "";
            if((L) % 2 == 0)
            {
                for (int i = 0; i < (d / 2) * 2; i++)
                {
                    last += "1";
                }
                for (int i = (d / 2) * 2; i < d; i++)
                {
                    last += "0";
                }
            }
            else
            {
                for (int i = 0; i < d; i++)
                {
                    last += "1";
                }
            }
            //Console.WriteLine("last "+last);

            for(int i = 0; i < str.Length; i++)
            {
                if (str[i]!=last[i])
                    return false;
            }
            return true;
        }
 
        public static string GetStartStringForNode()
        {
            string str = "";
            for (int i = 0; i < d - (3 + ((L + 1) % 2)); i++)
                str += "0";
            for (int i = 0; i < 3 + ((L + 1) % 2); i++)
                str += "1";
            
            return str;
        }
        public static string GetStartStringForFirstNode()
        {
            string str = "";
            for (int i = 0; i < d - NumOfOnes; i++)
                str += "0";
            for (int i = 0; i < NumOfOnes; i++)
                str += "1";
            return str;
        }
        public static void PrintToFile(List<Node> nodes)
        {
            string path = "C:\\Users\\1okmon\\Desktop\\MatrixCarier\\W_" + S + "_" + L;
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            try
            {
                StreamWriter sw = new StreamWriter("C:\\Users\\1okmon\\Desktop\\MatrixCarier\\W_" + S + "_" + L + "\\N-" + N +"_D-"+D+ ".csv");
                sw.WriteLine(N+";"+"N");
                sw.WriteLine(D + ";" + "D" );
                sw.WriteLine(L + ";" + "L");
                sw.WriteLine();
                foreach (Node node in nodes)
                {
                    Node node1 = node;
                    do
                    {
                        for (int i = node1.Data.Length - 1; i >= 0; i--)
                        {
                            sw.Write(node1.Data[i] + ";");
                        }
                        node1 = node1.Parent;
                        sw.WriteLine();
                    } while (node1 != null);
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();
                }
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Successfull output!");
            }

        }




        public static string GetNextStr3(string str)
        {         
            bool move = false;          
            int lastOne = d;
            int[] strMass = new int[d];
            strMass = TurnStringToMass(str);         
            int numOfOne = 0;
            while (!move)
            {
                for (int j = 0; j < d; j++)
                {   
                    if (strMass[j] == 1)
                    {
                        lastOne = j;
                        numOfOne++;
                    }
                }
                if (numOfOne == 0)
                {
                    for (int j = d - 1; j > d - 1 - 3; j--)                   
                        strMass[j] = 1;
                    move = true;
                }
                else if (lastOne == numOfOne - 1)
                {
                    if ((d - 1 - numOfOne - ((L + 1) % 2)>-1))
                    {

                    }
                    for (int j = d - 1; j >= d - 1 - numOfOne - ((L + 1) % 2); j--)
                    {
                        strMass[j] = 1;
                    }
                    for (int j = 0; j < d - 1 - numOfOne - ((L + 1) % 2); j++)
                    {
                        strMass[ j] = 0;
                    }
                    move = true;           
                    numOfOne = 0;
                }
                else
                {
                    MoveOne(strMass);
                    move = true;
                }
            }
            string movedString = "";
            for(int i=0; i < d; i++)
            {
                movedString += strMass[i];
            }
            return movedString;
        }

        public static int[] TurnStringToMass(string str)
        {
            int[] mass = new int[d];
            for( int i = 0; i < str.Length; i++)
            {
                string a = "";
                a += str[i];
                mass[i] = int.Parse(a);
            }
            return mass;
        }


        public static void MoveOne(int[]mass)
        {
            for (int i = d - 1; i >= 0; i--)
            {
                if (mass[i] == 1)
                {
                    int k = i - 1;
                    int count = 0;
                    while (mass[k] == 1)
                    {
                        k--;
                        count++;
                    }
                    mass[k] = 1;
                    for (int j = k + 1; j < d; j++)
                    {
                        mass[j] = 0;
                    }
                    for (int j = k - 1; j >= 0; j--)
                    {
                        if (mass[j] != 1)
                            mass[j] = 0;
                    }
                    int p = d - 1;
                    while (count > 0)
                    {
                        mass[ p--] = 1;
                        count--;
                    }
                    break;
                }
                else
                    mass[i] = 0;
            }
            string movedString = "";
            for (int i = 0; i < d; i++)
            {
                movedString += mass[i];
            }
            //return movedString;
        }
    }
}
