using BPTreeOne;

namespace BPTreeTest
{
    [TestClass]
    public class BPlusTreeTests
    {
        [TestMethod]
        public void Test_InsertAndSearch_ValidKey()
        {
            var tree = new BPlusTree<int, string>();
            tree.Insert(1, "One");
            tree.Insert(2, "Two");
            tree.Insert(3, "Three");

            Assert.AreEqual("One", tree.Search(1));
            Assert.AreEqual("Two", tree.Search(2));
            Assert.AreEqual("Three", tree.Search(3));
        }

        [TestMethod]
        public void Test_InsertAndSearch_InvalidKey()
        {
            var tree = new BPlusTree<int, string>();
            tree.Insert(1, "One");
            tree.Insert(2, "Two");
            tree.Insert(3, "Three");

            Assert.AreEqual(null, tree.Search(4));
        }

        [TestMethod]
        public void Test_Delete_ValidKey()
        {
            var tree = new BPlusTree<int, string>();
            tree.Insert(1, "One");
            tree.Insert(2, "Two");
            tree.Insert(3, "Three");

            tree.Delete(2);

            Assert.AreEqual("One", tree.Search(1));
            Assert.AreEqual(null, tree.Search(2));
            Assert.AreEqual("Three", tree.Search(3));
        }

        [TestMethod]
        public void Test_Delete_InvalidKey()
        {
            var tree = new BPlusTree<int, string>();
            tree.Insert(1, "One");
            tree.Insert(2, "Two");
            tree.Insert(3, "Three");

            tree.Delete(4);

            Assert.AreEqual("One", tree.Search(1));
            Assert.AreEqual("Two", tree.Search(2));
            Assert.AreEqual("Three", tree.Search(3));
        }
    }
}