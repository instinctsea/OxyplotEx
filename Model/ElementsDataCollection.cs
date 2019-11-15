/*
 * Copyright (c) 2014-2015 jiayongqiang. All rights reserved";
 * CLR version: 4.0.30319.18444"
 * File name:   ElementsDataCollection.cs "
 * Date:        2015/8/6 10:48:49 
 * Author :  jiayongqiang Developer
 * Description: 
	
 * History:  created by jiayongqiang 2015/8/6 10:48:49
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model
{
    public class ElementsDataCollection : IComparer<ElementsDataCollection>
    {
        private IDictionary<string, double> _values= new Dictionary<string, double>();
        private readonly int _seq;

        public ElementsDataCollection(int seq)
        {
            _seq = seq;
        }

        public double this[string element]
        {
            get { return _values[element]; }
        }

        public bool TryGetValue(string name, out double value)
        {
            return _values.TryGetValue(name, out value);
        }

        public IEnumerable<string> Elements
        {
            get { return _values.Keys; }
        }

        public void AddValue(string name, double value)
        {
            _values[name] = value;
        }

        public int Sequence
        {
            get { return _seq; }
        }

        public int Compare(ElementsDataCollection x, ElementsDataCollection y)
        {
            return x._seq - y._seq;
        }
    }
}
