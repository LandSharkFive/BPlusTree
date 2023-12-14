using BPTestOne;

namespace BPTests
{
    [TestClass]
    public class BPTest1
    {
        [TestMethod]
        public void Test_Insert_Three()
        {
            BPlusTree tree = new BPlusTree(3);

            tree.Insert(3, 3.3);
            tree.Insert(4, 4.4);
            tree.Insert(5, 5.5);

            Assert.AreEqual(tree.Search(3), 3.3);
            Assert.AreEqual(tree.Search(4), 4.4);
            Assert.AreEqual(tree.Search(5), 5.5);

            List<double> list = tree.SearchRange(3, 5);
            Assert.AreEqual(list.Count, 3);
            List<double> check = new List<double> { 3.3, 4.4, 5.5 };
            Assert.IsTrue(list.SequenceEqual(check));
        }

        [TestMethod]
        public void Test_Insert_Ten()
        {
            const int MaxValue = 10;

            BPlusTree tree = new BPlusTree(3);

            for (int i = 1; i < MaxValue; i++)
            {
                tree.Insert(i, i * 1.1);
            }

            for (int i = 1; i < MaxValue; i++)
            {
                Assert.AreEqual(tree.Search(i), i * 1.1);
            }

        }

        [TestMethod]
        public void Test_Insert_Twenty()
        {
            const int MaxValue = 20;

            BPlusTree tree = new BPlusTree(3);

            for (int i = 1; i < MaxValue; i++)
            {
                tree.Insert(i, i * 1.1);
            }

            for (int i = 1; i < MaxValue; i++)
            {
                Assert.AreEqual(tree.Search(i), i * 1.1);
            }
        }

        [TestMethod]
        public void Test_Insert_Random_One()
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
                Assert.AreEqual(value, tree.Search(value));
            }
        }

        [TestMethod]
        public void Test_Insert_Random_Two()
        {
            BPlusTree tree = new BPlusTree(3);

            int[] array = { 6, 46, 22, 84, 83, 100, 92, 47, 35, 76, 27, 59, 96, 51 };

            for (int i = 0; i < array.Length; i++)
            {
                int value = array[i];
                tree.Insert(value, value);
            }

            for (int i = 0; i < array.Length; i++)
            {
                int value = array[i];
                Assert.AreEqual(value, tree.Search(value));
            }
        }

        [TestMethod]
        public void Test_Delete_Random_One()
        {
            BPlusTree tree = new BPlusTree(3);

            int[] array = { 79, 86, 38, 76, 87, 82, 95, 16, 65, 77, 30, 44, 88, 8 };

            for (int i = 0; i < array.Length; i++)
            {
                int value = array[i];
                tree.Insert(value, value);
            }

            for (int i = 0; i < array.Length; i++)
            {
                int value = array[i];
                Assert.AreEqual(value, tree.Search(value));
            }

            int mid = array.Length / 2;
            for (int i = mid; i < array.Length; i++)
            {
                int value = array[i];
                tree.Delete(value);
            }

            for (int i = 0; i < mid; i++)
            {
                // found
                int value = array[i];
                Assert.AreEqual(tree.Search(value), value);
            }

            for (int i = mid; i < array.Length; i++)
            {
                // not found
                int value = array[i];
                Assert.AreEqual(tree.Search(value), 0);
            }
        }

        [TestMethod]
        public void Test_Delete_Random_Two()
        {
            BPlusTree tree = new BPlusTree(3);

            int[] array = { 92, 22, 41, 99, 37, 34, 56, 17, 12, 40, 35, 84, 1, 75 };

            for (int i = 0; i < array.Length; i++)
            {
                int value = array[i];
                tree.Insert(value, value);
            }

            for (int i = 0; i < array.Length; i++)
            {
                int value = array[i];
                Assert.AreEqual(value, tree.Search(value));
            }

            int mid = array.Length / 2;
            for (int i = mid; i <  array.Length; i++)
            {
                int value = array[i];
                tree.Delete(value);
            }

            for (int i = 0; i < mid; i++)
            {
                // found
                int value = array[i];
                Assert.AreEqual(tree.Search(value), value);
            }

            for (int i = mid; i < array.Length; i++)
            {
                // not found
                int value = array[i];
                Assert.AreEqual(tree.Search(value), 0);
            }
        }

        [TestMethod]
        public void Test_Delete_Random_Three()
        {
            BPlusTree tree = new BPlusTree(3);

            int[] array = { 61, 12, 75, 41, 28, 32, 99, 57, 51, 85, 68, 91, 27, 87 };

            for (int i = 0; i < array.Length; i++)
            {
                int value = array[i];
                tree.Insert(value, value);
            }

            for (int i = 0; i < array.Length; i++)
            {
                int value = array[i];
                Assert.AreEqual(value, tree.Search(value));
            }

            int mid = array.Length / 2;
            for (int i = mid; i < array.Length; i++)
            {
                int value = array[i];
                tree.Delete(value);
            }

            for (int i = 0; i < mid; i++)
            {
                // found
                int value = array[i];
                Assert.AreEqual(tree.Search(value), value);
            }

            for (int i = mid; i < array.Length; i++)
            {
                // not found
                int value = array[i];
                Assert.AreEqual(tree.Search(value), 0);
            }
        }

        [TestMethod]
        public void Test_Insert_Random_Four()
        {
            BPlusTree tree = new BPlusTree(3);

            int[] array = { 142, 180, 163, 51, 166, 183, 13, 42, 68, 75, 116, 161, 135, 10, 81, 137, 6, 45, 58, 74 };

            for (int i = 0; i < array.Length; i++)
            {
                int value = array[i];
                tree.Insert(value, value);
            }

            for (int i = 0; i < array.Length; i++)
            {
                int value = array[i];
                Assert.AreEqual(tree.Search(value), value);
            }

            int[] keys = tree.GetKeys().ToArray();

            Assert.IsTrue(Util.IsSorted(keys));
            Assert.AreEqual(keys.Length, array.Length);

            double[] myValues = tree.SearchRange(0, 200).ToArray();

            Assert.IsTrue(Util.IsSorted(myValues));
            Assert.AreEqual(myValues.Length, array.Length);
        }

    }
}