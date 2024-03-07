using System.Text;

namespace BPTreeOne
{

    // <summary>
    // Represents a B+ tree of order five.
    // The BPlusTree class provides methods to insert, search, and delete elements in the tree.
    // </summary>
    public partial class BPlusTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        private const int MINCHILD = 3;
        private const int MAXCHILD = (MINCHILD * 2) - 1;

        private Node? root;

        // <summary>
        // Constructs a new BPlusTree.
        // </summary>
        public BPlusTree()
        {
            root = null;
        }

        // <summary>
        // Inserts a key-value pair into the B+ tree.
        // </summary>
        public void Insert(TKey key, TValue value)
        {
            // If the root is a leaf node, insert the key-value pair directly.
            if (root == null)
            {
                AddRoot(key, value);
                return;
            }

            if (root.IsLeaf)
            {
                AddRootLeaf(key, value);
                return;
            }

            AddRootInternal(key, value);
        }

        void AddRoot(TKey key, TValue value)
        {
            this.root = new Node(true);
            root.Keys.Add(key);
            root.Values.Add(value);
        }

        void AddRootLeaf(TKey key, TValue value)
        {
            root.Add(key, value);
            if (root.Values.Count > MAXCHILD)
                root = SplitNode(root);
        }

        void AddRootInternal(TKey key, TValue value)
        {
            AddInternal(null, root, key, value);
        }

        private void AddInternal(Node parent, Node node, TKey key, TValue value)
        {
            while (!node.IsLeaf)
            {
                if (node.Keys.Count > MAXCHILD)
                {
                    Node temp = SplitNode(node);
                    parent.Keys.Add(temp.Keys[0]);
                    parent.Children.Add(temp);
                    node = parent;
                }

                int index = 0;
                while (index < node.Keys.Count && key.CompareTo(node.Keys[index]) >= 0)
                {
                    index++;
                }

                parent = node;
                node = node.Children[index];
            }

            node.Add(key, value);

            if (node.Keys.Count > MAXCHILD)
            {
                var right = node.Split();
                parent.Children.Add(right);
                parent.Keys.Add(right.Keys[0]);
            }
        }


        // Split node. Return parent.
        private Node SplitNode(Node node)
        {
            Node parent = new Node(false);
            Node right = node.Split();
            parent.Keys.Add(right.Keys[0]);
            parent.Children.Add(node);
            parent.Children.Add(right);
            return parent;
        }

        // <summary>
        // Searches for a key in the B+ tree and returns its associated value.
        // </summary>
        public TValue? Search(TKey key)
        {
            Node node = root;

            // Traverse the tree to find the appropriate leaf node.
            while (!node.IsLeaf)
            {
                int index = 0;
                while (index < node.Keys.Count && key.CompareTo(node.Keys[index]) >= 0)
                {
                    index++;
                }

                node = node.Children[index];
            }

            if (node.IsLeaf)
            {
                // Search for the key in the leaf node.
                int keyIndex = node.Keys.IndexOf(key);
                if (0 <= keyIndex && keyIndex < node.Values.Count)
                {
                    return (TValue)node.Values[keyIndex];
                }
            }

            return default;
        }

        // <summary>
        // Deletes a key-value pair from the B+ tree.
        // </summary>
        public void Delete(TKey key)
        {
            Node node = root;

            // Traverse the tree to find the appropriate leaf node.
            while (!node.IsLeaf)
            {
                int index = 0;
                while (index < node.Keys.Count && key.CompareTo(node.Keys[index]) >= 0)
                {
                    index++;
                }

                node = node.Children[index];
            }

            if (node.IsLeaf)
                node.RemoveFromLeaf(key);

        }

        // Find Parent.  Recursive.  Search on root.
        Node FindParent(Node node, Node child)
        {
            if (node.IsLeaf)
                return null;

            if (node.Children.Count > 0 && node.Children[0].IsLeaf)
                return null;

            for (int i = 0; i < node.Children.Count; i++)
            {
                if (node.Children[i] == child)
                {
                    return node;
                }
                else
                {
                    // recursive
                    var parent = FindParent(node.Children[i], child);
                    if (parent != null)
                        return parent;
                }
            }

            return null;
        }



        public IEnumerable<Node> GetEnumerator()
        {
            return Descendants(root);
        }

        private IEnumerable<Node> Descendants(Node node)
        {
            yield return node;
            foreach (var x in node.Children)
            {
                foreach (var item in Descendants(x))
                    yield return item;
            }
        }

        public void PrintKeyValue()
        {
            foreach (var node in GetEnumerator())
            {
                for (int i = 0; i < node.Keys.Count; i++)
                {
                    if (node.IsLeaf)
                        Console.WriteLine("{0}:{1} ", node.Keys[i], node.Values[i]);
                    else
                        Console.WriteLine("{0}: ", node.Keys[i]);
                }
            }
        }

        public void PrintLeaves()
        {
            foreach (var node in GetEnumerator())
            {
                for (int i = 0; i < node.Keys.Count; i++)
                {
                    if (node.IsLeaf)
                        Console.WriteLine("{0}:{1} ", node.Keys[i], node.Values[i]);
                }
            }
        }


        public void PrintNodes()
        {
            foreach (var node in GetEnumerator())
            {
                Console.WriteLine(NodeToString(node));
            }
        }



        public string NodeToString(Node node)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("l: {0} ", Convert.ToInt32(node.IsLeaf));
            sb.AppendFormat("c: {0} ", node.Children.Count);

            sb.Append("k: ");
            foreach (var key in node.Keys)
            {
                sb.AppendFormat("{0} ", key);
            }

            sb.Append("v: ");
            foreach (var value in node.Values)
            {
                sb.AppendFormat("{0} ", value);
            }

            return sb.ToString();
        }


    }
}


