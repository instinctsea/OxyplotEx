using OxyplotEx.GMap;
using OxyplotEx.Model.DataSeries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model
{
    public  class SeriesManager:ICollection<ISeries>
    {
        private Dictionary<string,ISeries> _series = new Dictionary<string, ISeries>();
        public SeriesManager()
        { 
            
        }

        public ISeries this[string index]
        {
            get
            {
                ISeries series;
                _series.TryGetValue(index, out series);
                return series;
            }
        }

        public void Add(ISeries item)
        {
            _series[item.Id] = item;
        }

        public void Clear()
        {
            _series.Clear();
        }

        public bool Contains(ISeries item)
        {
            return _series.ContainsKey(item.Id);
        }

        public void CopyTo(ISeries[] array, int arrayIndex)
        {
            _series.Values.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _series.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(ISeries item)
        {
            return _series.Remove(item.Id);
        }

        public IEnumerator<ISeries> GetEnumerator()
        {
            return _series.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _series.Values.GetEnumerator();
        }
    }
}
