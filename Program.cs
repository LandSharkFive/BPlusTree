using System;

namespace BPTestOne
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TestOne();
        }

        static void TestOne()
        {
            BPlusTree tree = new BPlusTree(3);

            int[] array = { 42, 18, 62, 63, 44, 79, 90, 83, 10, 55 };

            for (int i = 0; i < array.Length; i++)
            {
                int value = array[i];
                tree.Insert(value, value);
            }

            for (int i = 0; i < array.Length; i++)
            {
                int value = array[i];
                Console.WriteLine(tree.Search(value));
            }

            Console.WriteLine();

            tree.PrintNodes();
        }

    }
}


