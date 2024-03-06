using HashTablesLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataStructuresTestProject
{
    [TestClass]
    public class CuckooHashTableUnitTest
    {
        [TestMethod]
        public void ItemExistsAfterAdding()
        {
            int size = 100;
            var cTable = new CuckooHashTable<int, int>();
            for (int i = 0; i < size; i++)
            {
                cTable.Add(i, i);
            }
            for (int i = 0; i < size; i++)
            {
                Assert.IsTrue(cTable.ContainsKey(i));
            }
            Assert.AreEqual(size, cTable.Count);
        }
    }
}
