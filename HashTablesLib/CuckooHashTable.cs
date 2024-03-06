using System;
using System.Collections;
using System.Collections.Generic;


namespace HashTablesLib
{
    public class CuckooHashTable<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private IEqualityComparer<TKey> _comparer;
        private Pair<TKey, TValue>[] _items1, _items2;
        private int _capacity;
        private int p1,p2;
        private GetPrimeNumber primeNumber = new GetPrimeNumber();
        private double FillFactor = 0.5;

        private int Hash1(TKey key)
        {
            int hashCode = _comparer.GetHashCode(key) & 0x7FFFFFFF;
            return ((2* hashCode+3)%p1)%_capacity;
        }

        private int Hash2(TKey key)
        {
            int hashCode = _comparer.GetHashCode(key) & 0x7FFFFFFF;
            return ((hashCode + 1) % p2) % _capacity;
        }
        public CuckooHashTable()
        {
            Init();
        }
        private void Init()
        {
            _comparer = EqualityComparer<TKey>.Default;
            _capacity = primeNumber.GetMin();
            (p1, p2) = primeNumber.GetPrimes(_capacity);
            _items1 = new Pair<TKey, TValue>[_capacity];
            _items2 = new Pair<TKey, TValue>[_capacity];
            Count = 0;
        }

        public TValue this[TKey key] 
        {
            get => throw new NotImplementedException(); 
            set => throw new NotImplementedException(); 
        }

        public ICollection<TKey> Keys => throw new NotImplementedException();
        public ICollection<TValue> Values => throw new NotImplementedException();

        public int Count { get; private set; }

        public bool IsReadOnly { get; private set; }

        public void Add(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException();
            int i = Hash1(key);
            int j = Hash2(key);
            if (_items1[i] != null && _comparer.Equals(_items1[i].Key, key) ||
                _items2[j] != null && _comparer.Equals(_items2[j].Key, key))
            {
                throw new ArgumentException("Such key already exists");
            }
            TKey currentKey = key;
            TValue currentValue = value;
            int tryNumber = 1;
            bool NoCycle = false;
            while (!_comparer.Equals(currentKey, key) || tryNumber == 1)
            {
                if (_items1[Hash1(currentKey)] == null)
                {
                    _items1[Hash1(currentKey)] = new Pair<TKey, TValue>(currentKey, currentValue);
                    Count++;
                    NoCycle = true;
                    break;
                }
                else
                {
                    var temp = _items1[Hash1(currentKey)];
                    _items1[Hash1(currentKey)] = new Pair<TKey, TValue>(currentKey, currentValue);
                    currentKey = temp.Key;
                    currentValue = temp.Value;
                }
                if (_items2[Hash2(currentKey)] == null)
                {
                    _items2[Hash2(currentKey)] = new Pair<TKey, TValue>(currentKey, currentValue);
                    Count++;
                    NoCycle = true;
                    break;
                }
                else
                {
                    var temp = _items2[Hash2(currentKey)];
                    _items2[Hash2(currentKey)] = new Pair<TKey, TValue>(currentKey, currentValue);
                    currentKey = temp.Key;
                    currentValue = temp.Value;
                }
                tryNumber++;
            }
            if ((double)Count / (2.0*_capacity) >= FillFactor || !NoCycle)
            {
                IncreaseTable();
            }
            if(!NoCycle)
                Add (key, value);
        }

        private void IncreaseTable()
        {
            _capacity = primeNumber.Next();
            (p1, p2) = primeNumber.GetPrimes(_capacity);
            var temp1 = _items1;
            var temp2 = _items2;
            _items1 = new Pair<TKey, TValue>[_capacity];
            _items2 = new Pair<TKey, TValue>[_capacity];
            Count = 0;
            for ( int  i = 0; i < temp1.Length; i++)
            {
                if (temp1[i] == null)
                    continue;
                Add(temp1[i].Key, temp1[i].Value);
            }
            for (int i = 0; i < temp2.Length; i++)
            {
                if (temp2[i] == null)
                    continue;
                Add(temp2[i].Key, temp2[i].Value);
            }

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

        public bool ContainsKey(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException();
            int i = Hash1(key);
            int j = Hash2(key);
            if (_items1[i] != null && _comparer.Equals(_items1[i].Key, key) ||
                _items2[j] != null && _comparer.Equals(_items2[j].Key, key))
            {
                return true;
            }
            return false;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
