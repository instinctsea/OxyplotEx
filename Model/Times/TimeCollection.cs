using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/************************************************************************ 
 * 项目名称 :  CMA.MICAPS.Resolver.Time   
 * 项目描述 :      
 * 类 名 称 :  TimeCollection 
 * 版 本 号 :  v4.0 
 * CLR 版本 :  4.0.30319.42000
 * 说    明 :      
 * 作    者 :  jiayongqiang 
 * 创建时间 :  2016/12/15 16:10:15 
 * 更新时间 :  2016/12/15 16:10:15 
************************************************************************ 
 * Copyright  2016. All rights reserved. 
************************************************************************/
namespace OxyplotEx.Model.Time
{
    public class TimesCollection:IComparer<DateTime>
    {
        public TimesCollection()
        { 
        
        }
        List<DateTime> _times = new List<DateTime>();

        public int Count
        {
            get
            {
                return _times.Count;
            }
        }

        public DateTime this[int index]
        {
            get
            {
                return _times[index];
            }
            set
            {
                _times[index] = value;
            }
        }

        public void Add(DateTime time)
        {
            _times.Add(time);
        }

        public int Compare(DateTime x, DateTime y)
        {
            if (x < y)
                return -1;
            else if (x == y)
                return 0;
            else
                return 1;
        }

        public void Clear()
        {
            _times.Clear();
        }

        public void Sort()
        {
            _times.Sort(this);
        }

        public void Sort(IComparer<DateTime> comparer)
        {
            _times.Sort(comparer);
        }

        public void Sort(Comparison<DateTime> comparison)
        {
            _times.Sort(comparison);
        }

        public DateTime GetLatest()
        {
            if (Count == 0)
                throw new Exception("datetime count is 0");

            Sort();

            return this[Count - 1];
        }

        public DateTime GetEarliest()
        {
            if (Count == 0)
                throw new Exception("datetime count is 0");

            Sort();

            return this[0];
        }
    }
}
