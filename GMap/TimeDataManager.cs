using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.GMap
{
    public class TimeDataManager:IEnumerable<TimeModel>
    {
        List<TimeModel> _times = new List<TimeModel>();
        public int Count
        {
            get { return _times.Count; }
        }

        public TimeModel this[int index]
        {
            get { return _times[index]; }
        }

        public void Add(TimeModel time)
        {
            _times.Add(time);
        }

        public DateTime ForecastTime
        {
            get;set;
        }

        public void Clear()
        {
            _times.Clear();
        }

        public void Reverse()
        {
            _times.Reverse();
            foreach (TimeModel time in _times)
            {
                time.InverseIndex(Count);
            }
        }

        public bool GetRange(out DateTime min, out DateTime max)
        {
            min = DateTime.Now;
            max = DateTime.Now;
            if (Count == 0)
                return false;

            _times.Sort((l, r) =>
            {
                if (l.CompsiteTime > r.CompsiteTime)
                    return 1;
                else if (l.CompsiteTime < r.CompsiteTime)
                    return -1;
                return 0;
            });

            min = _times[0].CompsiteTime;
            max = _times[Count - 1].CompsiteTime;
            return true;
        }

        public IEnumerator<TimeModel> GetEnumerator()
        {
            return _times.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _times.GetEnumerator();
        }
    }
}
