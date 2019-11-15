using OxyplotEx.GMap;
using OxyplotEx.Model.CoordinateAxises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model
{
    public class AxisManager:ICollection<IAxis>
    {
        private Dictionary<string, IAxis> _axises = new Dictionary<string, IAxis>();
        public AxisManager()
        { 
        
        }

        public IAxis this[string index]
        {
            get
            {
                IAxis axis;
                _axises.TryGetValue(index, out axis);
                return axis;
            }
        }

        public void Add(IAxis item)
        {
            _axises[item.Name] = item;
        }

        public void Clear()
        {
            _axises.Clear();
        }

        public bool Contains(IAxis item)
        {
            return _axises.ContainsKey(item.Name);
        }

        public void CopyTo(IAxis[] array, int arrayIndex)
        {
            _axises.Values.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _axises.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(IAxis item)
        {
            return _axises.Remove(item.Name);
        }

        public IEnumerator<IAxis> GetEnumerator()
        {
            return _axises.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _axises.Values.GetEnumerator();
        }
    }
}
