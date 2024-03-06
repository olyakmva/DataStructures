using HashTablesLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DataStructuresDotNetUnitTestProject
{
    [TestClass]
    public class ChainHashTableUnitTest
    {
        [TestMethod]
        public void ContainsNotExistingKeyReturnsFalse()
        {
            int capacity = 17;

            var hashTable = new ChainHashTable<int, int>(capacity);

            for (int i = 0; i < capacity/2; i++)
            {
                hashTable.Add(i, 0);
            }

            var result = hashTable.ContainsKey(capacity);

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void ContainsExistingKeyReturnsTrue()
        {
            int capacity = 17;
                      
            var hashTable = new ChainHashTable<int, int>(capacity);

            for (int i = 0; i < capacity; i++)
            {
                hashTable.Add(i, 0);
            }

            var result = hashTable.ContainsKey(capacity/2);

            Assert.AreEqual(true, result);
        }

    }
}
