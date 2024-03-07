using BPTreeOne;

namespace BPTreeOne
{
    internal class Program
    {
        public static void Main()
        {
            var tree = new BPlusTree<int, string>();
            tree.Insert(1, "One");
            tree.Insert(2, "Two");
            tree.Insert(3, "Three");
            tree.Insert(4, "Four");
            tree.Insert(5, "Five");
            tree.Insert(6, "Six");
            tree.Insert(7, "Seven");
            tree.Insert(8, "Eight");
            tree.Insert(9, "Nine");

            for (int i = 1; i < 10; i++)
            {
                Console.WriteLine($"Value for key {i}: {tree.Search(i)}");
            }

            tree.PrintLeaves();
            tree.PrintNodes();
        }

    }
}