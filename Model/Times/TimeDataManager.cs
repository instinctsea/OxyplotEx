using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model.Time
{
    /// <summary>
    /// TimeData Manager
    /// </summary>
    public class TimeDataManager : IEnumerable<TimeModel>
    {
        List<TimeModel> _times = new List<TimeModel>();
        /// <summary>
        /// timemodel count
        /// </summary>
        public int Count
        {
            get { return _times.Count; }
        }

        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TimeModel this[int index]
        {
            get { return _times[index]; }
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="time"></param>
        public void Add(TimeModel time)
        {
            _times.Add(time);
        }

        /// <summary>
        /// 预报时间
        /// </summary>
        public DateTime ForecastTime
        {
            get; set;
        }

        public bool ContainsRealTime(DateTime time)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].CompsiteTime == time)
                    return true;
            }

            return false;
        }

        public int GetRealTimeIndex(DateTime time)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].CompsiteTime == time)
                    return i;
            }

            return -1;
        }

        public IEnumerable<TimeModel> GetLiveTimes()
        {
            List<TimeModel> times = new List<TimeModel>();

            for (int i = 0; i < this.Count; i++)
            {
                bool is_live = true;
                for (int j = 0; j < this.Count; j++)
                {
                    if (this[i].Time == this[j].Time && i!=j)
                    {
                        is_live = false;
                        break;
                    }
                }

                if (is_live)
                    times.Add(this[i]);
            }

            return times;
        }

        void SortTimeModels(ref List<TimeModel> times)
        {
            if (times == null || times.Count <= 1)
                return;

            times.Sort((l, r) =>
            {
                if (l.CompsiteTime < r.CompsiteTime)
                    return -1;
                else if (l.CompsiteTime > r.CompsiteTime)
                    return 1;
                else
                    return 0;
            });
        }

        public TimeDataManager GetSub(TimeModel start, int duration)
        {
            DateTime start_time = start.CompsiteTime;
            DateTime end_time = start_time.AddHours(duration);

            TimeDataManager result = new TimeDataManager();
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].CompsiteTime >= start_time && this[i].CompsiteTime <= end_time)
                    result.Add(this[i]);
            }

            return result;
        }

        public bool GetForecastTimes(out TimeModel start,out TimeModel end)
        {
            List<TimeModel> times = new List<TimeModel>();

            start = null;
            end = null;
            for (int i = 0; i < this.Count; i++)
            {
                for (int j = 0; j < this.Count; j++)
                {
                    if (this[i].Time == this[j].Time && i != j)
                    {
                        times.Add(this[i]);
                        break;
                    }
                }
            }

            if (times.Count < 2)
                return false;

            SortTimeModels(ref times);
            start = times[0];
            end = times[times.Count - 1];
            return true;
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        public void Clear()
        {
            _times.Clear();
        }

        /// <summary>
        /// 反转
        /// </summary>
        public void Reverse()
        {
            _times.Reverse();
            foreach (TimeModel time in _times)
            {
                time.InverseIndex(Count);
            }
        }

        /// <summary>
        /// 获得时间范围
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public bool GetRange(out DateTime min, out DateTime max)
        {
            min = DateTime.Now;
            max = DateTime.Now;
            if (Count == 0)
                return false;

            min = this[0].CompsiteTime;
            max = this[0].CompsiteTime;
            for (int i = 1; i < this.Count; i++)
            {
                if (this[i].CompsiteTime < min)
                    min = this[i].CompsiteTime;
                if (this[i].CompsiteTime > max)
                    max = this[i].CompsiteTime;
            }
            return true;
        }

        /// <summary>
        /// 枚举器
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TimeModel> GetEnumerator()
        {
            return _times.GetEnumerator();
        }

        public void SortByTimeAsc()
        {
            _times.Sort((l, r) =>
            {
                if (l.CompsiteTime > r.CompsiteTime)
                    return 1;
                else if (l.CompsiteTime == r.CompsiteTime)
                    return 0;
                else
                    return -1;
            });
        }

        public void SortByTimeDesc()
        {
            _times.Sort((l, r) =>
            {
                if (l.CompsiteTime < r.CompsiteTime)
                    return 1;
                else if (l.CompsiteTime == r.CompsiteTime)
                    return 0;
                else
                    return -1;
            });
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _times.GetEnumerator();
        }
    }
}
