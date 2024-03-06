using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace HashTablesLib
{
    public class ChainHashTable<TKey, TValue>:IEnumerable<KeyValuePair<TKey,TValue>>, IDictionary <TKey, TValue>
    {
        List<Pair<TKey, TValue>>[] _table;
        public int Count {get;  private set;}
        public bool IsReadOnly { get; private set; } 

        private const int MaxChainLength = 8;
        private int _currentChainLength;
        private readonly GetPrimeNumber _primeNumber = new GetPrimeNumber();
        private HashMaker <TKey> _hashMaker;

        public ChainHashTable()
        {
            Init();     
        }
        private void Init()
        {
            var capacity = _primeNumber.GetMin();
            _table = new List<Pair<TKey, TValue>>[capacity];
            _hashMaker = new HashMaker<TKey>(capacity);
        }
        public ChainHashTable( int primeNum)
        {
            var capacity = _primeNumber.Next();
            while (capacity < primeNum)
            {
                capacity = _primeNumber.Next();
            }
            _table = new List<Pair<TKey, TValue>>[primeNum];
            _hashMaker = new HashMaker<TKey>(primeNum);
        }

        public void Add(TKey key, TValue value)
        {
            var h = _hashMaker.ReturnHash(key);
            
            if (_table[h] == null || !_table[h].Exists(p=>p.Key.Equals(key)))
            {
                if (_table[h] == null)
                    _table[h] = new List<Pair<TKey, TValue>>(MaxChainLength);
                var item = new Pair<TKey, TValue>(key, value);
                _table[h].Add(item);
                _currentChainLength = Math.Max(_currentChainLength, _table[h].Count);
                Count++;
            }
            else 
            {
                throw new ArgumentException();
            }
            if (_currentChainLength>=MaxChainLength) // проверка размера
            {
                IncreaseTable();
            }
        }
        /// <summary>
        /// TODO
        /// </summary>
        private void IncreaseTable()
        {
            // получить следующее число и увеличить таблицу
            int size = _primeNumber.Next();
            _hashMaker.SimpleNumber = size;
            var tempTable = _table;
            _table = new List<Pair<TKey, TValue>>[size];
            _currentChainLength = 0;
            Count = 0;
            foreach (var tableItem in tempTable)
            {
                if (tableItem == null) continue;
                foreach (var pair in tableItem)
                {
                    Add(pair.Key, pair.Value);
                }
            }
        }
        //TODO
        public bool Remove(TKey key)
        {
            throw new NotImplementedException();
        }
        public TValue this[TKey key]
        {
            get
            {
                var pair = Find(key);
                if (pair != null)
                    return pair.Value;
                throw new KeyNotFoundException();
            }
            set
            {
                var pair = Find(key) ?? throw new KeyNotFoundException();
                pair.Value = value;
            }
        }

        
     private Pair<TKey, TValue> Find(TKey key)
     {
         int index = _hashMaker.ReturnHash(key);
         if (_table[index] == null) return null;
         foreach (var pair in _table[index])
         {
             if (pair.Key.Equals(key))
                 return pair;
         }
         return null;
     }

     public bool ContainsKey(TKey key)
     {
         if(key == null)
             throw new ArgumentNullException();
         return Find(key) != null;
     }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return (from list in _table where list != null from pair in list select new KeyValuePair<TKey, TValue>(pair.Key, pair.Value)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            throw new NotImplementedException();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            Init();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ContainsKey(item.Key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }
        public ICollection<TKey> Keys => throw new NotImplementedException();

        public ICollection<TValue> Values => throw new NotImplementedException();
    }

    
}

