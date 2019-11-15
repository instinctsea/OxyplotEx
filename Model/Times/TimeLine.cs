using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model.Time
{
    /// <summary>
    /// 时间类型
    /// </summary>
    public enum TimeStyle
    {
        /// <summary>
        /// 实况线
        /// </summary>
        Live,
        /// <summary>
        /// 预报起始和实况结束分界线
        /// </summary>
        Seperator,
        /// <summary>
        /// 预报线
        /// </summary>
        Forecast
    }

    /// <summary>
    /// 时间线
    /// </summary>
    public class TimeLine
    {
        
        /// <summary>
        /// ctr.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="time"></param>
        /// <param name="interval"></param>
        /// <param name="timeStyle"></param>
        public TimeLine(DateTime start, DateTime time, IntervalParameter interval, TimeStyle timeStyle)
        {
            this.Start = start;
            this.Time = time;
            this.IntervalStyle = interval.Style;
            switch (interval.Style)
            {
                case eInterval.Hour:
                    double hours = (time - start).TotalHours;
                    this.Index = hours / interval.Value;
                    break;
                case eInterval.Minute:
                    double minutes = (time - start).TotalMinutes;
                    this.Index = minutes / interval.Value;
                    break;
            }
            
            this.TimeStyle = timeStyle;
        }

        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime Start { get; private set; }
        /// <summary>
        /// 索引
        /// </summary>
        public double Index { get; private set; }
        /// <summary>
        /// 实际时间
        /// </summary>
        public DateTime Time { get; private set; }
        public string GetTimeStyleDesc()
        {
            string desc=null;
            switch (this.TimeStyle)
            {
                case TimeStyle.Forecast:
                    desc = "预报";
                    break;
                case TimeStyle.Live:
                    desc = "实况 / 初始场";
                    break;
                case TimeStyle.Seperator:
                    desc = "分界线";
                    break;
            }

            return desc;
        }
        /// <summary>
        /// 当前时间样式
        /// </summary>
        public TimeStyle TimeStyle { get; private set; }

        /// <summary>
        /// 间隔样式
        /// </summary>
        public eInterval IntervalStyle { get; private set; }
        /// <summary>
        /// 反转
        /// </summary>
        /// <param name="IndexCunt"></param>
        public void Inverse(int IndexCunt)
        {
            Index = IndexCunt - 1 - Index;
        }

        /// <summary>
        /// 获得时间范围内所有的时间线条
        /// </summary>
        /// <param name="start"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="interval"></param>
        /// <param name="lineInterval"></param>
        /// <returns></returns>
        public static List<TimeLine> GetTimeLines(DateTime start, DateTime min, DateTime max, IntervalParameter interval, int lineInterval)
        {
            List<TimeLine> times = new List<TimeLine>();
            DateTime cur = new DateTime(min.Year, min.Month, min.Day, 0, 0, 0);
            while (cur <= max)
            {
                TimeLine line = new TimeLine(start, cur, interval, TimeStyle.Forecast);
                times.Add(line);

                switch (interval.Style)
                {
                    case eInterval.Hour:
                        cur = cur.AddHours(lineInterval);
                        break;
                    case eInterval.Minute:
                        cur = cur.AddMinutes(lineInterval);
                        break;
                }           
            }

            return times;
        }
    }
}
