using System;
using System.Collections.Generic;

namespace HashTablesLib
{
    public class Pair<TKey, TValue> : IEquatable<Pair<TKey, TValue>>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        bool _deleted;

        public bool Equals(Pair<TKey, TValue> other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<TKey>.Default.Equals(Key, other.Key);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Pair<TKey, TValue>) obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<TKey>.Default.GetHashCode(Key);
        }

        public static bool operator ==(Pair<TKey, TValue> left, Pair<TKey, TValue> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Pair<TKey, TValue> left, Pair<TKey, TValue> right)
        {
            return !Equals(left, right);
        }

        public Pair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
            _deleted = false;
        }

        public bool IsDeleted()
        {
            return _deleted;
        }
        public bool DeletePair()
        {
            if (_deleted) return false;
            _deleted = true;
            return true;
        }
    }
}
