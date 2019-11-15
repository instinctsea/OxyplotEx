using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model.TimeSeries
{
    interface ICollection<T>:IEnumerable<T>
    {
        int Count { get; }
        T this[int index] { get; }
        void Add(T item);
        void AddRange(IEnumerable<T> items);
        void Insert(int index, T item);
        int IndexOf(T item);
        void Remove(T item);
        bool Contains(T item);
        bool Contains(Predicate<T> pre);
        T Find(Predicate<T> pre);
        void Clear();
    }
}
