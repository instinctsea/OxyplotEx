using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model.Time
{
    /// <summary>
    /// 时间线集合
    /// </summary>
    public class TimeLinesCollection : IEnumerable<TimeLine>
    {
        List<TimeLine> _times = new List<TimeLine>();
        /// <summary>
        /// ctr
        /// </summary>
        public TimeLinesCollection()
        {
        }

        /// <summary>
        /// 时间线个数
        /// </summary>
        public int Count
        {
            get { return _times.Count; }
        }

        public int IndexCount
        {

            get;
            set;
        }

        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TimeLine this[int index]
        {
            get { return _times[index]; }
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="time"></param>
        public void Add(TimeLine time)
        {
            _times.Add(time);
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            _times.Clear();
        }

        public void Reverse()
        {
            for (int i = 0; i < _times.Count; i++)
            {
                _times[i].Inverse(this.Count);
            }
        }

        public void GetLive(out TimeLine live)
        {
            live = null;
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].TimeStyle == TimeStyle.Live)
                {
                    live = this[i];
                }
            }
        }

        public void GetForecast(out TimeLine forecast)
        {
            forecast = null;
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].TimeStyle == TimeStyle.Forecast)
                {
                    forecast = this[i];
                }
            }
        }

        public void GetSeperator(out TimeLine seperator)
        {
            seperator = null;
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].TimeStyle == TimeStyle.Seperator)
                {
                    seperator = this[i];
                }
            }
        }

        /// <summary>
        /// 枚举器
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TimeLine> GetEnumerator()
        {
            return _times.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _times.GetEnumerator();
        }
    }
}
