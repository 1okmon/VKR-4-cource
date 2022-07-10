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


    class MatrixCarrier3
    {
        public static int NumOfThreads = 3840;
        public static int counter = 0;
        public static int N = 18;
        public static int L = 7;
        public static int differ = 3;
        public static int S = 3;
        public static int d = 14;
        //public static int d = 7;
        public static int D = 0;
        public static int NumOfOnes = 7;
        public static List<Node3> results = new List<Node3>();
        public static List<Node3> toDelete = new List<Node3>();
        public static List<String> numerable = new List<String>();
        public static bool found = false;
        public static int maxD = 17;



        public static void Start()
        {
            ///////////////
            /*
            GetAllNumeration(3, 5, numerable);
            foreach(string str in numerable)
            {
                int[] a = new int[3];
                a = DevideStr2(str, 3);
                for(int i=0;i<a.Length;i++)
                   Console.Write(a[i]);
                Console.WriteLine();
            }
            Console.ReadLine();
            */
///////////////////////////////////////////////////////////////////////////////////////////////////////
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            while (D == 0)
            {
                found = false;
                Node3 startNode = new Node3(GetStartStringForFirstNode());
                FillOneNode(startNode);
                Console.WriteLine("Веток от начальной:"+startNode.childs.Count);
                //startNode.Print();
                FillTree(startNode);
                Console.WriteLine("------------");
                Console.WriteLine("num of 1=" + NumOfOnes);
                NumOfOnes++;
                counter = 0;
                if (NumOfOnes > d)
                {

                    NumOfOnes = 7;
                    Console.WriteLine("d=" + d);
                    Console.WriteLine(" не найдено решение => увеличиваем d");
                    d++;
                    counter = 0;
                }
                if (d == maxD)
                {
                    N++;
                    d = S + 2;
                    NumOfOnes = 7;
                    Console.WriteLine(" не найдено решение при d=" + (maxD - 1) + "=> увеличиваем N");
                    counter = 0;
                }
                Console.WriteLine("N=" + N + " :S=" + S + " :L=" + L + " :d=" + d);
                Console.WriteLine("------------");

            }
            Console.WriteLine("Finish" + found);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.WriteLine("D=" + D);
            Console.ReadLine();

        }
        
        public static void FillTree(Node3 node)
        {
            
            foreach (Node3 item in node.childs)
            {
                if (item.Parent.Rang == 1)
                {
                    Console.WriteLine(counter++);
                }
                if (item.Rang < N)
                {
                    FasterFillOneNode(item);
                    FillTree(item);
                }
            }
        }

        public static void FasterFillOneNode(Node3 startNode)
        {
            foreach (Node3 node in startNode.Parent.childs)
            {
                if (startNode.Parent.childs.IndexOf(node) >= startNode.Parent.childs.IndexOf(startNode))
                {
                    ///тест на неч L
                    //Console.WriteLine("tut");
                   // Console.WriteLine(startNode.Data + startNode.Rang);
                    //Console.WriteLine(node.Data + node.Rang);
                    //Console.WriteLine("-------");
                    if (L % 2 == 1 && CheckStrInTreeForOddL(startNode, node.Data))
                    {
                        //Console.WriteLine("tam");
                        //Console.WriteLine(startNode.Data + startNode.Rang);
                        //Console.WriteLine(node.Data + node.Rang);
                        //Console.WriteLine("-------");
                        if (CheckStrInTree(startNode, node))
                        {
                            //Console.WriteLine(node.Data + node.Rang);
                            //Console.WriteLine("tut");
                            //Console.ReadLine();
                            Node3 add = new Node3(node.Data);
                            startNode.Add(add);
                            if (add.Rang >= L)
                                if (!ChechBySumInColumnsInProcess(add))
                                {
                                    add.Delete();
                                }
                        }
                    }
                }
                if (found)
                {
                    break;
                }
            }
            //startNode.childs.Clear();
            GetRidOfNodes(startNode);
        }

        public static void FillOneNode(Node3 startNode)
        {
            string startStr = GetStartStringForNode();
            while (!found && !CheckStrOnLast(startStr))
            {
                if (CheckStrInTree(startNode, new Node3(startStr)))
                {

                    Node3 add = new Node3(startStr);

                    startNode.Add(add);
                    if (add.Rang >= L)
                        if (!ChechBySumInColumnsInProcess(add))
                        {
                            add.Delete();
                        }
                }
                startStr = GetNextStr3(startStr);
            }
            if (!found && CheckStrInTree(startNode, new Node3(startStr)))
            {
                Node3 add = new Node3(startStr);
                startNode.Add(add);
                if (add.Rang >= L)
                    if (!ChechBySumInColumnsInProcess(add))
                    {
                        add.Delete();
                    }
            }
            GetRidOfNodes(startNode);
        }

        public static void GetRidOfNodes(Node3 startNode)
        {
            for (int i = 0; i < startNode.childs.Count; i++)
            {
                if (startNode.childs.ElementAt(i).Rang <= N)
                {
                    for (int i2 = i + 1; i2 < startNode.childs.Count; i2++)
                    {
                        if (!CheckArraysOf2Nodes(startNode.childs.ElementAt(i), startNode.childs.ElementAt(i2)))
                        {
                            startNode.childs.ElementAt(i2).Rang = N + 1;
                            //startNode.childs.ElementAt(i2).Delete();
                        }
                    }
                }
            }
        }

        public static bool CheckArraysOf2Nodes(Node3 node, Node3 node2)
        {
            for (int i = 0; i <= L; i++)
            {
                if (node.sumInCols[i] != node2.sumInCols[i])
                    return true;
            }
            for (int i = 0; i <= d; i++)
            {
                if (node.sumInLines[i] != node2.sumInLines[i])
                    return true;
            }
            return false;
        }

/*
        public static void FindSolution(Node3 node)
        {
            foreach (Node3 child in node.childs)
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
                            found = true;
                            break;
                        }
                    }
                }
            }
        }
*/

        public static bool ChechByDifferentColumns(Node3 node)
        {
            int[,] matr = new int[N, d];
            int i = 0;
            do
            {
                for (int j = 0; j < d; j++)
                {
                    string str = "";
                    str += node.Data[j];
                    matr[i, j] = int.Parse(str);
                }
                node = node.Parent;
                i++;
            } while (node != null);
            bool check = false;
            for (int j1 = 0; j1 < d - 1; j1++)
            {
                for (int j2 = j1 + 1; j2 < d; j2++)
                {
                    check = false;
                    for (int row = 0; row < N; row++)
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

        public static bool ChechBySumInColumnsInProcess(Node3 node)
        {
            int[] sumInCol = new int[d];
            bool checkInProcess = true;
            Node3 node1 = node;
            for (int i = 0; i < d; i++)
                sumInCol[i] = 0;




            do
            {
                for (int i = 0; i < node.Data.Length; i++)
                {
                    if (node.Data[i].Equals('1'))
                    {
                        sumInCol[i]++;
                        if (sumInCol[i] > L)
                        {
                            checkInProcess = false;
                            break;
                        }
                    }
                }
                node = node.Parent;
            } while (checkInProcess&& node != null);




            for (int i = 0; i < d; i++)
                if (sumInCol[i] != L)
                {
                    checkInProcess = false;
                    break;
                }
            if (checkInProcess && ChechByDifferentColumns(node1))
            {
                results.Add(node1);
                found = true;
                //Console.ReadLine();
                D = d;
                PrintToFile(results);
                Node3.PrintTrace(node1);
                return true;
            }

            for (int i = 0; i < d; i++)
                if (sumInCol[i] > L)
                    return false;
            return true;
        }

/*
        public static bool ChechBySumInColumns(Node3 node)
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
*/

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        public static string[] GetMassFromTree(Node3 node)
        {
            string[] array = new string[node.Rang];
            int l = 0;
            int c = 0;
            do
            {
                array[l++] = node.Data;
                node = node.Parent;
            } while (node != null);

            return array;
        }



        public static bool CheckStrInTreeForOddL(Node3 node, string str)
        {
            do
            {
                string strNode = node.Data;
                int sum = 0;
                for (int i = 0; i < str.Length; i++)
                {
                    if ((str[i] == strNode[i]) && (str[i].Equals('1')))
                    {
                        sum++;
                    }
                }
                if (sum % 2 == 1)
                    return false;

                node = node.Parent;
            } while (node != null);

            return true;
        }



        public static bool CheckStrInTree(Node3 node, Node3 node2)
        {
            string str = node2.Data;
           
            do
            {
                string strNode = node.Data;
                int sum = 0;

                if (S == 3 && node2.Rang > 2&&node2.Rang<=N+1)
                {
                    bool rang = false;
                    if (node2.Rang == N + 1)
                    {
                        node2.Rang = node2.Parent.Rang + 1;
                        rang = true;
                    }
                    if (node2.Rang > 2 ) {
                        string[] array = GetMassFromTree(node2);

                        //Console.ReadLine();
                        numerable.Clear();
                        GetAllNumeration(S-1, node2.Rang-1, numerable);
                        //Console.WriteLine("=========================================");
                        //for(int i=0;i<numerable.Length;i++)
                        // Console.WriteLine(numerable);
                        //Console.WriteLine(node2.Rang);


                        foreach (string str2 in numerable)
                        {
                            //foreach (string str3 in numerable)
                               // Console.WriteLine(str2);
                            //Console.ReadLine();
                            sum = 0;
                            int[] b = new int[S-1];
                            b = DevideStr2(str2, S-1);
                            int[] a = new int[S];
                            for (int i = 0; i < b.Length; i++)
                            {
                                a[i] = b[i];
                                //Console.Write(a[i] + " ");
                            }
                            a[S - 1] = node2.Rang;
                            ///Console.WriteLine(a[S-1] + " ");

                            //Console.ReadLine();


                            ////////////////////////место для доп проверки по L /четн . нечетн


                            for(int i = 0; i < d; i++)
                            {
                                int sum2 = 0;
                                for(int j = 0; j < S; j++)
                                {
                                
                                    sum2 = sum2 + array[a[j]-1][i]-'0';
                                }
                                sum =sum+ sum2 / 3;
                                
                            }
                            //Console.WriteLine();
                            
                            //Console.WriteLine("sum = " + sum);
                            if (sum % 2 == 1)
                            {
                                //Console.WriteLine(found);
                                if (rang)
                                {
                                    node2.Rang = N + 1;
                                }
                                
                                return false;
                               
                                
                            }

                            //for (int i = 0; i < a.Length; i++)
                                //Console.Write(a[i]+" ");
                            //Console.WriteLine
                            
                        }
                        
                        //Console.WriteLine("piz");
                        
                        //Console.WriteLine("=========================================");
                        
                    }
                    if (rang)
                    {
                        node2.Rang = N + 1;
                    }
                    return true;
                }else if (S == 3 )
                {
                    
                    return true;
                }
                else 
                {

                    Thread.Sleep(100);
                    sum = 0;
                    for (int i = 0; i < str.Length; i++)
                    {
                        //int[] a = new int[S];
                        if ((str[i] == strNode[i]) && (str[i].Equals('1')))
                        {
                            sum++;
                        }
                    }
                    if (sum % 2 == 1)
                        return false;

                    node = node.Parent;
                }

            } while (node != null);
            
            return true;
        }



        public static void GetAllNumeration(int l, int n, List<string> list)
        {
            int[] str = new int[l];
            int[] stop = new int[l];
            for (int i = 1; i <= l; i++)
            {
                str[i - 1] = i;
            }

            list.Add(MassToString(str));
            for (int i = n - l + 1; i <= n; i++)
            {
                stop[i - n + l - 1] = i;
            }

            int lastIndex = l - 1;
            while (CheckEqualOfMass(str, stop, l))
            {
                if (str[lastIndex] != n)
                {
                    str[lastIndex]++;
                }
                else
                {
                    lastIndex = FindLastIndex(str, n, l);
                    str[lastIndex]++;
                    for (int i = lastIndex + 1; i < l; i++)
                    {
                        str[i] = str[i - 1] + 1;
                    }

                    lastIndex = l - 1;
                }

                list.Add(MassToString(str));
            }
        }

        public static string MassToString(int[] a)
        {
            string str = "";
            for (int i = 0; i < a.Length; i++)
            {
                str += a[i] + " ";
            }

            return str;
        }

        public static bool CheckEqualOfMass(int[] a, int[] b, int l)
        {
            for (int i = 0; i < l; i++)
            {
                if (a[i] != b[i])
                    return true;
            }

            return false;
        }

        public static int FindLastIndex(int[] a, int n, int l)
        {
            int la = l - 1;
            int max = n;
            for (int i = l - 1; i > 0; i--)
            {
                if (a[i] == max)
                {
                    max--;
                    la--;
                }
                else
                    break;
            }

            return la;
        }











        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        public static bool CheckStrOnLast(string str)
        {
            string last = "";
            for (int i = 0; i < d; i++)
            {
                last += "1";
            }
    
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != last[i])
                    return false;
            }
            return true;
        }

        public static string GetStartStringForNode()
        {
            string str = "";
            for (int i = 0; i < d - NumOfOnes; i++)
                str += "0";
            for (int i = 0; i < NumOfOnes ; i++)
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
        public static void PrintToFile(List<Node3> nodes)
        {
            string path = "C:\\Users\\1okmon\\Desktop\\MatrixCarier2\\W_" + S + "_" + L;
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            try
            {
                StreamWriter sw = new StreamWriter("C:\\Users\\1okmon\\Desktop\\MatrixCarier2\\W_" + S + "_" + L + "\\N-" + N + "_D-" + D + ".csv");
                sw.WriteLine(N + ";" + "N");
                sw.WriteLine(D + ";" + "D");
                sw.WriteLine(L + ";" + "L");
                sw.WriteLine();
                foreach (Node3 node in nodes)
                {
                    Node3 node1 = node;
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
                    for (int j = d - 1; j >= d - 1 - numOfOne; j--)
                    {
                        strMass[j] = 1;
                    }
                    for (int j = 0; j < d - 1 - numOfOne; j++)
                    {
                        strMass[j] = 0;
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
            for (int i = 0; i < d; i++)
            {
                movedString += strMass[i];
            }
            return movedString;
        }

        public static int[] DevideStr2(string str, int d)
        {
            int[] div = new int[d];
            string num = "";
            int k = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == ' ')
                {
                    div[k++] = int.Parse(num);
                    num = "";
                }
                else
                    num += str[i];
            }

            return div;
        }
        public static int[] TurnStringToMass(string str)
        {
            int[] mass = new int[d];
            for (int i = 0; i < str.Length; i++)
            {
                string a = "";
                a += str[i];
                mass[i] = int.Parse(a);
            }
            return mass;
        }


        public static void MoveOne(int[] mass)
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
                        mass[p--] = 1;
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
        }
    }
}
