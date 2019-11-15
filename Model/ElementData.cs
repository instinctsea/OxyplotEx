/* ***********************************************
 * Copyright (c) 2014-2015 luoshasha. All rights reserved";
 * CLR version: 4.0.30319.34014"
 * File name:   Class1.cs"
 * Date:        3/16/2015 5:34:00 PM
 * Author :  sand
 * Email  :  luoshasha@foxmail.com
 * Description: 
	
 * History:  created by sand 3/16/2015 5:34:00 PM
 
 * ***********************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace OxyplotEx.Model
{
    public class ElementData : IComparer<ElementData>, IEnumerable<KeyValuePair<string, string>>
    {
        private IDictionary<string, string> _values;
        private readonly int _seq;

        public ElementData(int seq)
        {
            _seq = seq;
            _values = new Dictionary<string, string>();
        }

        public string this[string element]
        {
            get { return _values[element]; }
        }

        public bool TryGetValue(string name, out string value)
        {
            return _values.TryGetValue(name, out value);
        }

        public IEnumerable<string> Elements
        {
            get { return _values.Keys; }
        }

        public void AddValue(string name, string value)
        {
            _values[name] = value;
        }

        public int Sequence
        {
            get { return _seq; }
        }

        public int Compare(ElementData x, ElementData y)
        {
            return x._seq - y._seq;
        }

        public KeyValuePair<string, string> Current
        {
            get { throw new NotImplementedException(); }
        }



        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _values.GetEnumerator();
        }
    }
}
