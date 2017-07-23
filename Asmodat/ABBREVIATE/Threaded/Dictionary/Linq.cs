using System;
using System.Collections.Generic;
using System.Linq;

namespace Asmodat.Abbreviate
{
    public partial class ThreadedDictionary<TKey, TValue> : SortedDictionary<TKey, TValue>
    {
        /// <summary>
        /// Thread Safe Linq Select
        /// </summary>
        public IEnumerable<TResult> Select<TResult>(Func<KeyValuePair<TKey, TValue>, TResult> selector)
        {
            lock (locker) return Enumerable.Select(this, selector);
        }

        /// <summary>
        /// Thread Safe Linq Select
        /// </summary>
        public IEnumerable<TResult> Select<TResult>(Func<KeyValuePair<TKey, TValue>, int, TResult> selector)
        {
            lock (locker) return Enumerable.Select(this, selector);
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> Where(Func<KeyValuePair<TKey, TValue>, int, bool> predicate)
        {
            lock (locker) return Enumerable.Where(this, predicate);
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> Where(Func<KeyValuePair<TKey, TValue>, bool> predicate)
        {
            lock (locker) return Enumerable.Where(this, predicate);
        }

        public bool Any()
        {
            lock (locker) return Enumerable.Any(this);
        }

        public bool Any(Func<KeyValuePair<TKey, TValue>, bool> predicate)
        {
            lock (locker) return Enumerable.Any(this, predicate);
        }
    }
}
