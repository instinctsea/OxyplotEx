using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model.Time
{
    /// <summary>
    /// 时间模型
    /// </summary>
    public class TimeModel
    {
        /// <summary>
        /// ctr.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="duration"></param>
        /// <param name="index"></param>
        public TimeModel(DateTime time, int duration, double index,bool islive)
        {
            Time = time;
            Duration = duration;
            Index = index;
            this.IsLive = islive;
            //Count = count;
        }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; private set; }

        /// <summary>
        /// 时效
        /// </summary>
        public int Duration { get; private set; }

        /// <summary>
        /// 索引
        /// </summary>
        public double Index { get; private set; }

        /// <summary>
        /// 是否是实况
        /// </summary>
        public bool IsLive { get; private set; }
        /// <summary>
        /// 求索引的反转
        /// </summary>
        /// <param name="count"></param>
        public void InverseIndex(int count)
        {
            Index = count - 1 - Index;
        }

        /// <summary>
        /// 预报时间
        /// </summary>
        public DateTime CompsiteTime
        {
            get { return Time.AddHours(Duration); }
        }
    }
}
