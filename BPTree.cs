using System.Text;

namespace BPTestOne
{

    public class BPlusTree 
    {

        // The maximum number of keys in a node.  
        int m;

        public InternalNode? Root;

        LeafNode FirstLeaf;

        // Binary Search.  Search for t.
        private int BinarySearch(DictionaryPair[] dps, int numPairs, int t)
        {
            return Array.BinarySearch(dps, 0, numPairs, new DictionaryPair(t, 0));
        }

        // Find the leaf node which contains key in the tree.
        private LeafNode FindLeafNode(int key)
        {
            //  Initialize keys and index variable
            int[] keys = this.Root.keys;
            int i;
            //  Find next node on path to appropriate leaf node
            for (i = 0; i < this.Root.degree - 1; i++)
            {
                if (key < keys[i])
                    break;
            }

            Node child = this.Root.childPointers[i];
            if (child is LeafNode)
                return (LeafNode)child;
            else
                return this.FindLeafNode((InternalNode)child, key);

        }

        // Find the leaf node that contains key in a node.  Recursive.
        private LeafNode FindLeafNode(InternalNode node, int key)
        {
            if (node is null)
            {
                Console.WriteLine("node is null");
            }

            //  Initialize keys and index variable
            int[] keys = node.keys;

            int i;
            //  Find next node on path to appropriate leaf node
            for (i = 0; i < node.degree - 1; i++)
            {
                if (key < keys[i])
                    break;
            }

            Node childNode = node.childPointers[i];
            if (childNode is null)
            {
                Console.WriteLine("childNode is null");
            }

            if (childNode is LeafNode)
                return (LeafNode)childNode;
            else
                return this.FindLeafNode((InternalNode)node.childPointers[i], key);

        }

        private int FindIndexOfPointer(Node[] pointers, LeafNode node)
        {
            int i;
            for (i = 0; (i < pointers.Length); i++)
            {
                if (pointers[i] == node)
                    break;
            }

            return i;
        }

        private int GetMidpoint()
        {
            return (this.m + 1) / 2;
        }

        private void HandleDeficiency(InternalNode innerNode)
        {
            InternalNode sibling;
            InternalNode parent = innerNode.parent;
            //  Remedy deficient root node
            if (this.Root == innerNode)
            {
                for (int i = 0; i < innerNode.childPointers.Length; i++)
                {
                    if (innerNode.childPointers[i] != null)
                    {
                        if (innerNode.childPointers[i] is InternalNode)
                        {
                            this.Root = (InternalNode)innerNode.childPointers[i];
                            this.Root.parent = null;
                        }
                        else if (innerNode.childPointers[i] is LeafNode)
                        {
                            this.Root = null;
                        }
                    }
                }
            }

            //  Borrow:
            if (innerNode.leftSibling != null && innerNode.leftSibling.IsLendable())
            {
                sibling = innerNode.leftSibling;
            }
            else if (innerNode.rightSibling != null && innerNode.rightSibling.IsLendable())
            {
                sibling = innerNode.rightSibling;
                //  Copy 1 key and pointer from sibling (atm just 1 key)
                int borrowedKey = sibling.keys[0];
                Node pointer = sibling.childPointers[0];
                //  Copy root key and pointer into parent
                innerNode.keys[innerNode.degree - 1] = parent.keys[0];
                innerNode.childPointers[innerNode.degree] = pointer;
                //  Copy borrowedKey into root
                parent.keys[0] = borrowedKey;
                //  Delete key and pointer from sibling
                sibling.RemovePointer(0);
                Array.Sort(sibling.keys);
                sibling.RemovePointer(0);
                this.ShiftDown(innerNode.childPointers, 1);
            }

            //  Merge:
            if (innerNode.leftSibling != null && innerNode.leftSibling.IsMergeable())
            {
                // nothing
            }
            else if (innerNode.rightSibling != null && innerNode.rightSibling.IsMergeable())
            {
                sibling = innerNode.rightSibling;
                //  Copy rightmost key in parent to beginning of sibling's keys &
                //  delete key from parent
                sibling.keys[sibling.degree - 1] = parent.keys[parent.degree - 2];
                Array.Sort(sibling.keys, 0, sibling.degree);
                parent.keys[parent.degree - 2] = 0;
                //  Copy inner node's child pointer over to sibling's list of child pointers
                for (int i = 0; (i < innerNode.childPointers.Length); i++)
                {
                    if (innerNode.childPointers[i] != null)
                    {
                        sibling.PrependChildPointer(innerNode.childPointers[i]);
                        innerNode.childPointers[i].parent = sibling;
                        innerNode.RemovePointer(i);
                    }
                }

                //  Delete child pointer from grandparent to deficient node
                parent.RemovePointer(innerNode);
                //  Remove left sibling
                sibling.leftSibling = innerNode.leftSibling;
            }

            //  Handle deficiency a level up if it exists
            if (parent != null && parent.IsDeficient())
                this.HandleDeficiency(parent);

        }

        public bool IsEmpty()
        {
            return this.FirstLeaf == null;
        }

        // Get the number of children in the pointers.
        public int GetDegree(Node[] pointers)
        {
            // Find first null pointer.
            for (int i = 0; i < pointers.Length; i++)
            {
                if (pointers[i] == null)
                    return i;
            }

            return -1;
        }

        // Shift pointers down by amount.
        private void ShiftDown(Node[] pointers, int amount)
        {
            Node[] newPointers = new Node[this.m + 1];
            for (int i = amount; i < pointers.Length; i++)
            {
                newPointers[i - amount] = pointers[i];
            }

            pointers = newPointers;
        }

        private Node[] SplitChildPointers(InternalNode innerNode, int split)
        {
            Node[] pointers = innerNode.childPointers;
            Node[] halfPointers = new Node[this.m + 1];
            //  Copy half of the values into halfPointers while updating original keys
            for (int i = split + 1; i < pointers.Length; i++)
            {
                halfPointers[i - split - 1] = pointers[i];
                innerNode.RemovePointer(i);
            }

            return halfPointers;
        }

        // Split the dictionary in a leaf node.
        private DictionaryPair[] SplitDictionary(LeafNode ln, int split)
        {
            DictionaryPair[] dictionary = ln.Dictionary;
            DictionaryPair[] halfDict = new DictionaryPair[this.m];
            //  Copy half of the values into halfDict
            for (int i = split; i < dictionary.Length; i++)
            {
                halfDict[i - split] = dictionary[i];
                ln.Delete(i);
            }

            return halfDict;
        }

        // Split an internal node.
        private void SplitInternalNode(InternalNode innerNode)
        {
            //  Acquire parent
            InternalNode parent = innerNode.parent;
            //  Split keys and pointers in half
            int midpoint = this.GetMidpoint();
            int newParentKey = innerNode.keys[midpoint];
            int[] halfKeys = this.SplitKeys(innerNode.keys, midpoint);
            Node[] halfPointers = this.SplitChildPointers(innerNode, midpoint);
            //  Change degree of original InternalNode innerNode.
            innerNode.degree = this.GetDegree(innerNode.childPointers);
            //  Create new sibling internal node and add half of keys and pointers
            InternalNode sibling = new InternalNode(this.m, halfKeys, halfPointers);
            foreach (Node pointer in halfPointers)
            {
                if (pointer != null)
                {
                    pointer.parent = sibling;
                }
            }

            //  Make internal nodes siblings of one another
            sibling.rightSibling = innerNode.rightSibling;
            if (sibling.rightSibling != null)
            {
                sibling.rightSibling.leftSibling = sibling;
            }

            innerNode.rightSibling = sibling;
            sibling.leftSibling = innerNode;
            if (parent == null)
            {
                //  Create new root node and add midpoint key and pointers
                int[] keys = new int[this.m];
                keys[0] = newParentKey;
                InternalNode newRoot = new InternalNode(this.m, keys);
                newRoot.AppendChildPointer(innerNode);
                newRoot.AppendChildPointer(sibling);
                this.Root = newRoot;
                //  Add pointers from children to parent
                innerNode.parent = newRoot;
                sibling.parent = newRoot;
            }
            else
            {
                //  Add key to parent
                parent.keys[parent.degree - 1] = newParentKey;
                Array.Sort(parent.keys, 0, parent.degree);
                //  Set up pointer to new sibling
                int pointerIndex = (parent.FindIndexOfPointer(innerNode) + 1);
                parent.InsertChildPointer(sibling, pointerIndex);
                sibling.parent = parent;
            }

        }

        // Split the keys at the split index.
        private int[] SplitKeys(int[] keys, int split)
        {
            int[] halfKeys = new int[this.m];
            //  Remove split-indexed value from keys
            keys[split] = 0;
            //  Copy half of the values into halfKeys while updating original keys
            for (int i = split + 1; i < keys.Length; i++)
            {
                halfKeys[i - split - 1] = keys[i];
                keys[i] = 0;
            }

            return halfKeys;
        }

        // Delete a key in the B+ tree.
        public void Delete(int key)
        {
            if (this.IsEmpty())
            {
                Console.WriteLine("The tree is empty.");
            }
            else
            {
                LeafNode ln;
                if (Root == null)
                    ln = this.FirstLeaf;
                else
                    ln = FindLeafNode(key);

                //  Get leaf node and attempt to find index of key to delete
                int dpIndex = this.BinarySearch(ln.Dictionary, ln.NumPairs, key);
                if (dpIndex < 0)
                {
                    //Console.WriteLine("Invalid Delete: Key unable to be found.");
                    return;
                }
                else
                {
                    //  Successfully delete the dictionary pair
                    ln.Delete(dpIndex);
                    //  Check for deficiencies
                    if (ln.IsDeficient())
                    {
                        LeafNode sibling;
                        InternalNode parent = ln.parent;
                        //  Borrow: First, check the left sibling, then the right sibling
                        if (ln.LeftSibling != null
                              && ln.LeftSibling.parent == ln.parent
                              && ln.LeftSibling.IsLendable())
                        {
                            sibling = ln.LeftSibling;
                            DictionaryPair borrowedDP = sibling.Dictionary[sibling.NumPairs - 1];
                            ln.Insert(borrowedDP);
                            ln.SortDictionary();
                            sibling.Delete(sibling.NumPairs - 1);
                            //  Update key in parent if necessary
                            int pointerIndex = this.FindIndexOfPointer(parent.childPointers, ln);
                            if (borrowedDP.key < parent.keys[pointerIndex - 1])
                                parent.keys[pointerIndex - 1] = ln.Dictionary[0].key;
                        }
                        else if (ln.RightSibling != null
                                    && ln.RightSibling.parent == ln.parent
                                    && ln.RightSibling.IsLendable())
                        {
                            sibling = ln.RightSibling;
                            DictionaryPair borrowedDP = sibling.Dictionary[0];
                            ln.Insert(borrowedDP);
                            sibling.Delete(0);
                            sibling.SortDictionary();
                            //  Update key in parent if necessary
                            int pointerIndex = this.FindIndexOfPointer(parent.childPointers, ln);
                            if (borrowedDP.key >= parent.keys[pointerIndex])
                                parent.keys[pointerIndex] = sibling.Dictionary[0].key;
                        }

                        //  Merge: First, check the left sibling, then the right sibling
                        else if (ln.LeftSibling != null
                                    && ln.LeftSibling.parent == ln.parent
                                    && ln.LeftSibling.IsMergeable())
                        {
                            sibling = ln.LeftSibling;
                            int pointerIndex = this.FindIndexOfPointer(parent.childPointers, ln);
                            //  Remove key and child pointer from parent
                            parent.RemoveKey(pointerIndex - 1);
                            parent.RemovePointer(ln);
                            //  Update sibling pointer
                            sibling.RightSibling = ln.RightSibling;
                            //  Check for deficiencies in parent
                            if (parent.IsDeficient())
                            {
                                this.HandleDeficiency(parent);
                            }

                        }
                        else if (ln.RightSibling != null
                                    && ln.RightSibling.parent == ln.parent
                                    && ln.RightSibling.IsMergeable())
                        {
                            sibling = ln.RightSibling;
                            int pointerIndex = this.FindIndexOfPointer(parent.childPointers, ln);
                            //  Remove key and child pointer from parent
                            parent.RemoveKey(pointerIndex);
                            parent.RemovePointer(pointerIndex);
                            //  Update sibling pointer
                            sibling.LeftSibling = ln.LeftSibling;
                            if (sibling.LeftSibling == null)
                                this.FirstLeaf = sibling;
                            if (parent.IsDeficient())
                                this.HandleDeficiency(parent);
                        }

                    }
                    else if (this.Root == null && this.FirstLeaf.NumPairs == 0)
                    {
                        //  Set first leaf as null to indicate B+ tree is empty
                        this.FirstLeaf = null;
                    }
                    else
                    {
                        ln.SortDictionary();
                    }
                }
            }
        }

        // Insert a key and value in the B+ Tree.
        public void Insert(int key, double value)
        {
            if (this.IsEmpty())
            {
                //  Create leaf node as first node in B plus tree (root is null)
                LeafNode ln = new LeafNode(this.m, new DictionaryPair(key, value));
                //  Set as first leaf node (can be used later for in-order leaf traversal)
                this.FirstLeaf = ln;
            }
            else
            {
                //  Find leaf node to insert into
                LeafNode ln;
                if (Root == null)
                    ln = this.FirstLeaf;
                else
                    ln = FindLeafNode(key);

                //  Insert into leaf node fails if node becomes overfull
                if (!ln.Insert(new DictionaryPair(key, value)))
                {
                    //  Sort all the dictionary pairs with the included pair to be inserted
                    ln.Dictionary[ln.NumPairs] = new DictionaryPair(key, value);
                    ln.NumPairs++;
                    ln.SortDictionary();
                    //  Split the sorted pairs into two halves
                    int midpoint = this.GetMidpoint();
                    DictionaryPair[] halfDict = this.SplitDictionary(ln, midpoint);
                    if (ln.parent == null)
                    {
                        //  Create internal node to serve as parent, use dictionary midpoint key
                        int[] parent_keys = new int[this.m];
                        parent_keys[0] = halfDict[0].key;
                        InternalNode parent = new InternalNode(this.m, parent_keys);
                        ln.parent = parent;
                        parent.AppendChildPointer(ln);
                    }
                    else
                    {
                        //  Add new key to parent for proper indexing
                        int newParentKey = halfDict[0].key;
                        ln.parent.keys[ln.parent.degree - 1] = newParentKey;
                        Array.Sort(ln.parent.keys, 0, ln.parent.degree);
                    }

                    //  Create new LeafNode that holds the other half
                    LeafNode newLeafNode = new LeafNode(this.m, halfDict, ln.parent);
                    //  Update child pointers of parent node
                    int pointerIndex = ln.parent.FindIndexOfPointer(ln) + 1;
                    ln.parent.InsertChildPointer(newLeafNode, pointerIndex);
                    //  Make leaf nodes siblings of one another
                    newLeafNode.RightSibling = ln.RightSibling;
                    if (newLeafNode.RightSibling != null)
                    {
                        newLeafNode.RightSibling.LeftSibling = newLeafNode;
                    }

                    ln.RightSibling = newLeafNode;
                    newLeafNode.LeftSibling = ln;
                    if (this.Root == null)
                    {
                        //  Set the root of B+ tree to be the parent
                        this.Root = ln.parent;
                    }
                    else
                    {
                        InternalNode innerNode = ln.parent;
                        while (innerNode != null)
                        {
                            if (innerNode.IsOverfull())
                                this.SplitInternalNode(innerNode);
                            else
                                break;

                            innerNode = innerNode.parent;
                        }

                    }

                }

            }

        }

        // Search the B+ Tree for key.
        // Return value or -1.
        public double Search(int key)
        {
            //  If B+ tree is completely empty, simply return -1.
            if (this.IsEmpty())
            {
                return -1;
            }

            //  Find leaf node that holds the dictionary key
            LeafNode ln;
            if (Root == null)
                ln = this.FirstLeaf;
            else
                ln = this.FindLeafNode(key);

            //  Perform binary search to find index of key within dictionary
            DictionaryPair[] dps = ln.Dictionary;
            int index = this.BinarySearch(dps, ln.NumPairs, key);
            //  If index negative, the key doesn't exist in B+ tree
            if (index < 0)
                return 0;
            else
                return dps[index].value;

        }

        // Return all values between lowerBound and upperBound.
        public List<Double> SearchRange(int lowerBound, int upperBound)
        {
            //  Create Double array to hold values
            List<Double> values = new List<Double>();
            //  Iterate through the doubly linked list of leaves
            LeafNode currNode = this.FirstLeaf;
            while (currNode != null)
            {
                //  Iterate through the dictionary of each node
                DictionaryPair[] dps = currNode.Dictionary;
                foreach (DictionaryPair dp in dps)
                {
                    if (dp == null)
                        break;

                    //  Include value if its key fits within the provided range
                    if (lowerBound <= dp.key && dp.key <= upperBound)
                        values.Add(dp.value);
                }

                // Move to next leaf
                currNode = currNode.RightSibling;
            }

            return values;
        }

        public void PrintNodes()
        {
            foreach (var x in GetNodes())
            {
                Console.WriteLine(x.ToString());
            }

            Console.WriteLine();
        }

        public List<int> GetKeys()
        {
            List<int> list = new List<int>();

            foreach (var x in GetNodes())
            {
                if (x is LeafNode)
                {
                    var ln = x as LeafNode;
                    foreach (var dp in ln.Dictionary)
                    {
                        if (dp != null)
                        {
                            list.Add(dp.key);
                        }
                    }
                }
            }

            return list;
        }


        // Get all descendants of the root.
        public IEnumerable<Node> GetNodes()
        {
            if (Root == null)
                yield break;

            foreach (var item in NodeItems(Root))
                yield return item;
        }

        // Get all descendants of a node.  Recursive.
        public IEnumerable<Node> NodeItems(Node node)
        {
            if (node is null)
            {
                yield break;
            }
            if (node is LeafNode)
            {
                yield return node;
            }
            if (node is InternalNode)
            {
                yield return node;
                var n = node as InternalNode;
                foreach (var x in n.childPointers)
                    foreach (var item in NodeItems(x))
                        yield return item;
            }
        }


        public BPlusTree(int m)
        {
            this.m = m;
            this.Root = null;
        }

        public class Node
        {
            public InternalNode? parent;

        }

        public class InternalNode : Node
        {
            // The maximum number of keys in this node.
            public int maxDegree;

            // The minimum number of keys in this node.
            public int minDegree;

            // The number of children in the node.
            public int degree;

            public InternalNode? leftSibling;

            public InternalNode? rightSibling;

            public int[] keys;

            public Node[] childPointers;

            public void AppendChildPointer(Node pointer)
            {
                this.childPointers[this.degree] = pointer;
                this.degree++;
            }

            public int FindIndexOfPointer(Node pointer)
            {
                for (int i = 0; i < this.childPointers.Length; i++)
                {
                    if (this.childPointers[i] == pointer)
                        return i;
                }

                return -1;
            }

            public void InsertChildPointer(Node pointer, int index)
            {
                for (int i = this.degree - 1; i >= index; i--)
                {
                    this.childPointers[i + 1] = this.childPointers[i];
                }

                this.childPointers[index] = pointer;
                this.degree++;
            }

            public bool IsDeficient()
            {
                return this.degree < this.minDegree;
            }

            public bool IsLendable()
            {
                return this.degree > this.minDegree;
            }

            public bool IsMergeable()
            {
                return this.degree == this.minDegree;
            }

            public bool IsOverfull()
            {
                return this.degree == this.maxDegree + 1;
            }

            public void PrependChildPointer(Node pointer)
            {
                for (int i = this.degree - 1; i >= 0; i--)
                {
                    this.childPointers[i + 1] = this.childPointers[i];
                }

                this.childPointers[0] = pointer;
                this.degree++;
            }

            public void RemoveKey(int index)
            {
                this.keys[index] = 0;
            }

            public void RemovePointer(int index)
            {
                this.childPointers[index] = null;
                this.degree--;
                RemoveNullPointers();
            }

            public void RemovePointer(Node pointer)
            {
                for (int i = 0; i < this.childPointers.Length; i++)
                {
                    if (this.childPointers[i] == pointer)
                        this.childPointers[i] = null;
                }

                this.degree--;
                RemoveNullPointers();
            }

            public void RemoveNullPointers()
            {
                Node[] newPointers = childPointers.Where(a => a != null).ToArray();
                Array.Clear(childPointers);
                Array.Copy(newPointers, childPointers, newPointers.Length);
            }

            public InternalNode(int m, int[] keys)
            {
                this.maxDegree = m;
                this.minDegree = m / 2;
                this.degree = 0;
                this.keys = keys;
                this.childPointers = new Node[this.maxDegree + 1];
            }

            public InternalNode(int m, int[] keys, Node[] pointers)
            {
                this.maxDegree = m;
                this.minDegree = m / 2;
                this.degree = GetDegree(pointers);
                this.keys = keys;
                this.childPointers = pointers;
            }


            // Get the number of children in the node.
            public int GetDegree(Node[] pointers)
            {
                for (int i = 0; i < pointers.Length; i++)
                {
                    if (pointers[i] == null)
                        return i;
                }

                return -1;
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("I");
                sb.Append(degree);
                sb.Append(" ");
                for (int i = 0; i < keys.Length; i++)
                    sb.Append(keys[i] + " ");
                return sb.ToString();
            }
        }

        public class LeafNode : Node
        {
            public int MaxNumPairs;

            public int MinNumPairs;

            public int NumPairs;

            public LeafNode? LeftSibling;

            public LeafNode? RightSibling;

            public DictionaryPair[] Dictionary;

            public void Delete(int index)
            {
                //  Delete dictionary pair from leaf
                this.Dictionary[index] = null;
                //  Decrement numPairs
                this.NumPairs--;
            }

            public bool Insert(DictionaryPair dp)
            {
                if (this.IsFull())
                {
                    return false;
                }

                //  Insert dictionary pair, increment numPairs, sort dictionary
                this.Dictionary[this.NumPairs] = dp;
                this.NumPairs++;
                Array.Sort(this.Dictionary, 0, this.NumPairs);
                return true;
            }

            public bool IsDeficient()
            {
                return this.NumPairs < this.MinNumPairs;
            }

            public bool IsFull()
            {
                return this.NumPairs == this.MaxNumPairs;
            }

            public bool IsLendable()
            {
                return this.NumPairs > this.MinNumPairs;
            }

            public bool IsMergeable()
            {
                return this.NumPairs == this.MinNumPairs;
            }

            public LeafNode(int m, DictionaryPair dp)
            {
                this.MaxNumPairs = m - 1;
                this.MinNumPairs = m / 2;
                this.Dictionary = new DictionaryPair[m];
                this.NumPairs = 0;
                this.Insert(dp);
            }

            public LeafNode(int m, DictionaryPair[] dps, InternalNode parent)
            {
                this.MaxNumPairs = m - 1;
                this.MinNumPairs = m / 2;
                this.Dictionary = dps;
                this.NumPairs = this.GetDegree(dps);
                this.parent = parent;
                this.LeftSibling = null;
                this.RightSibling = null;
            }

            // Get the number of keys and values in a leaf node.
            public int GetDegree(DictionaryPair[] dps)
            {
                for (int i = 0; i < dps.Length; i++)
                {
                    if (dps[i] == null)
                        return i;
                }

                return -1;
            }

            public void SortDictionary()
            {
                if (Dictionary == null)
                    return;

                Dictionary = Dictionary.Where(a => a != null).OrderBy(a => a.key).ToArray();
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("L");
                sb.Append(NumPairs);
                sb.Append(" ");
                foreach (DictionaryPair pair in this.Dictionary)
                {
                    if (pair != null)
                        sb.Append(pair.key + " ");
                }
                return sb.ToString();
            }

        }

        public class DictionaryPair : IComparable<DictionaryPair> 
        {
            public int key;

            public double value;

            public DictionaryPair(int key, double value)
            {
                this.key = key;
                this.value = value;
            }

            public int CompareTo(DictionaryPair a)
            {
                if (this == null && a == null)
                    return 0;

                if (this == null)
                    return -1;

                if (a == null)
                    return 1;

                if (this.key == a.key)
                    return 0;

                if (this.key > a.key)
                    return 1;

                return -1;
            }

        }

    }
}

