using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LRUCache
{
    internal class Program
    {
        static void Main(string[] args)
        {

            LRUCache cache = new LRUCache(2);

            Console.WriteLine(cache.Put(1, "1"));
            Console.WriteLine(cache.Put(2, "2"));
            Console.WriteLine(cache.Put(3, "3"));
            Console.WriteLine(cache.Get(1));
            Console.WriteLine(cache.Get(2));
            Console.WriteLine(cache.Put(4, "4"));

            Console.ReadLine();

        }
    }

    public class LRUCache
    {
        private Node head = null;
        private Node tail = null;

        int capacity;
        int currentSize;
        Dictionary<int, Node> hashTable;
        
        public LRUCache(int capacity)
        {
            this.capacity = capacity;
            hashTable = new Dictionary<int, Node>(capacity);
            currentSize = 0;
        }
        
        public void PrintCache()
        {
            while (head != tail)
            {
                Console.Write(head.Value + " ");
                head = head.previous;
            }
        }

        public string Get(int key)
        {
            Node result;

            if (hashTable.TryGetValue(key, out result))
            {
                //extract keynode and push it at the head
                Node n  = extractAndPush(result);
                return n.Value;
            }
            else
            {
                return "-1";
            }
        }

        public string Put(int key, string value)
        {
            Node result;

            if (hashTable.TryGetValue(key, out result))
            {
                result.Value = value;
                Node n = extractAndPush(result);
                return n.Value;
            }
            else
            {
                // if cache is full, then purge out LRU node
                var newNode = new Node(key, value);
                if (currentSize == capacity)
                {
                    var penultimate = tail.next;
                    hashTable.Remove(tail.Key);
                    if (penultimate != null)
                    {
                        penultimate.previous = null;
                    }
                    tail = penultimate;
                    currentSize--;
                }

                if (head == null)
                {
                    head = tail = newNode;
                }
                else
                {

                    // there is only one node
                    head.next = newNode;
                    newNode.previous = tail;
                    head = newNode;

                }
                currentSize++;

                hashTable[key] = head;
                return newNode.Value;
            }
        }

        private Node extractAndPush(Node n)
        {
            if(n == head)
            {
                return n;
            }
            
            var prevNode = n.previous;
            var nextNode = n.next;
           
            if(prevNode != null)
            {
                nextNode.previous = prevNode;
                prevNode.next = nextNode;
            }
            else
            {
                tail = nextNode;
            }
            n.previous = head;
            n.next = null;
            head.next = n;
            head = n;

            return head;
        }

        public class Node
        {
            public int Key { get; set; }
            public string Value { get; set; }

            public Node next;

            public Node previous;

            public Node(int key, string val)
            {
                this.Key = key;
                Value = val;
            }

        }

    }

}
