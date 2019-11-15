using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model.TimeSeries
{
    public class Collection<T> : ICollection<T>
    {
        public Collection()
        {
            
        }
        List<T> _list = new List<T>();

        public T this[int index]
        {
            get
            {
                return _list[index];
            }
        }

        public int Count
        {
            get
            {
                return _list.Count;
            }
        }

        public void Add(T item)
        {
            _list.Add(item);
        }

        public void AddRange(IEnumerable<T> items)
        {
            _list.AddRange(items);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(T item)
        {
            return _list.Contains(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public void Insert(int index, T item)
        {
            _list.Insert(index, item);
        }

        public int IndexOf(T item)
        {
            return _list.IndexOf(item);
        }

        public void Remove(T item)
        {
            _list.Remove(item);
        }

        public static Collection<T> Revert(Collection<T> items)
        {
            Collection<T> reverts = new Collection<T>();

            for (int i = items.Count - 1; i >= 0; i--)
                reverts.Add(items[i]);

            return reverts;
        }

        public bool Contains(Predicate<T> pre)
        {
            return _list.Find(pre) != null;
        }

        public T Find(Predicate<T> pre)
        {
            return _list.Find(pre);
        }
    }
}
