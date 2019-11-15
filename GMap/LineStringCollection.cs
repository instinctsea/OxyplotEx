using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.GMap
{
    class LineStringCollection : IEnumerable<LineString>
    {
        List<LineString> _lines = new List<LineString>();
        public int Count
        {
            get { return _lines.Count; }
        }

        public LineString this[int index]
        {
            get { return _lines[index]; }
        }

        public void Add(LineString lineString)
        {
            _lines.Add(lineString);
        }

        public void Clear()
        {
            _lines.Clear();
        }

        public IEnumerator<LineString> GetEnumerator()
        {
            return _lines.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _lines.GetEnumerator();
        }
    }
}
