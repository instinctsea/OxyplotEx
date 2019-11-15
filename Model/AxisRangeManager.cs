/*
 * Copyright (c) 2014-2015 jiayongqiang. All rights reserved";
 * CLR version: 4.0.30319.18444"
 * File name:   AxisRangeManager.cs "
 * Date:        2015/8/10 11:55:43 
 * Author :  jiayongqiang Developer
 * Description: 
	
 * History:  created by jiayongqiang 2015/8/10 11:55:43
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model
{
    public sealed class AxisRangeManager
    {
        private Dictionary<string, RangeValuePair> _ranges = new Dictionary<string, RangeValuePair>();
        public AxisRangeManager()
        { 
        }

        public RangeValuePair this[string key]
        {
            get 
            {
                return _ranges[key];
            }
            set
            {
                _ranges[key] = value;
            }
        }

        public void Add(string key,RangeValuePair range)
        {
            _ranges[key] = range;
        }

        public bool TryGetValue(string key, out RangeValuePair range)
        {
            return _ranges.TryGetValue(key, out range);
        }

        public bool ContainsKey(string key)
        {
            return _ranges.ContainsKey(key);
        }

        public void Clear()
        {
            _ranges.Clear();
        }
    }
}
