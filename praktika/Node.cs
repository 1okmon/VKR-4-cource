using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CudafyByExample
{ 
    public class Node
    {
        public string Data { get; set; }
        public int Rang { get; set; }
        public Node Parent { get; set; }
        public List<Node> childs { get; set; }

        public Node(string data)
        {
            this.Data = data;
            this.Rang = 1;
            childs = new List<Node>();
        }
        public Node()
        {
            childs = new List<Node>();
        }

        public void Add(Node item)
        {
            item.Parent = this;
            item.Rang = this.Rang + 1;
            this.childs.Add(item);
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
            foreach (Node child in this.childs)
            {
                Console.WriteLine("    "+k +") Data:"+child.Data+" Rang:"+child.Rang);
                k++;
            }
        }

        public static void PrintTrace(Node node)
        {
            Node node1 = node;
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
