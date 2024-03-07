
namespace BPTreeOne
{
    public partial class BPlusTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        public class Node
        {
            public List<TKey> Keys { get; set; }
            public List<TValue> Values { get; set; }
            public List<Node> Children { get; set; }
            public bool IsLeaf { get; set; }

            public Node(bool isLeaf)
            {
                this.Keys = new List<TKey>();
                this.Values = new List<TValue>();
                this.Children = new List<Node>();
                this.IsLeaf = isLeaf;
            }


            // Split node.
            public Node Split()
            {
                Node right = new Node(this.IsLeaf);

                int mid = this.Keys.Count / 2;
                for (int i = mid; i < this.Keys.Count; i++)
                {
                    right.Keys.Add(this.Keys[i]);
                }
                for (int i = mid; i < this.Values.Count; i++)
                {
                    right.Values.Add(this.Values[i]);
                }
                for (int i = mid; i < this.Children.Count; i++)
                {
                    right.Children.Add(this.Children[i]);
                }
                for (int i = this.Keys.Count - 1; i >= mid; i--)
                {
                    this.Keys.RemoveAt(i);
                }
                for (int i = this.Values.Count - 1; i >= mid; i--)
                {
                    this.Values.RemoveAt(i);
                }
                for (int i = this.Children.Count - 1; i >= mid; i--)
                {
                    this.Values.RemoveAt(i);
                }

                return right;
            }

            // Add to leaf node.
            public void Add(TKey key, TValue value)
            {
                int index = 0;
                while (index < this.Keys.Count && key.CompareTo(this.Keys[index]) >= 0)
                {
                    index++;
                }

                this.Keys.Insert(index, key);
                this.Values.Insert(index, value);
            }


            // Remove from Leaf.
            public void RemoveFromLeaf(TKey key)
            {
                int index = this.Keys.IndexOf(key);
                if (index < 0)
                    return;

                this.Keys.RemoveAt(index);
                if (index < this.Values.Count)
                    this.Values.RemoveAt(index);
            }

        }
    }
}
