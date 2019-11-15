using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model.Time
{
    /// <summary>
    /// 时间计算器
    /// </summary>
    public static class TimeOperator
    {
        /// <summary>
        /// 时间加上间隔
        /// </summary>
        /// <param name="time"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static DateTime TimeAddInterval(DateTime time, IntervalParameter interval)
        {
            DateTime ret = time;
            switch (interval.Style)
            {
                case eInterval.Hour:
                    ret = time.AddHours(interval.Value);
                    break;
                case eInterval.Minute:
                    ret = time.AddMinutes(interval.Value);
                    break;
            }

            return ret;
        }

        /// <summary>
        /// 时间加上间隔
        /// </summary>
        /// <param name="time"></param>
        /// <param name="interval"></param>
        /// <param name="intervalType"></param>
        /// <returns></returns>
        public static DateTime TimeAddInterval(DateTime time,int interval,eInterval intervalType)
        {
            DateTime ret = time;
            switch (intervalType)
            {
                case eInterval.Hour:
                    ret = time.AddHours(interval);
                    break;
                case eInterval.Minute:
                    ret = time.AddMinutes(interval);
                    break;
            }

            return ret;
        }

        /// <summary>
        /// 时间加上间隔
        /// </summary>
        /// <param name="time"></param>
        /// <param name="interval"></param>
        /// <param name="intervalType"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static DateTime TimeAddInterval(DateTime time, double index,double interval, eInterval intervalType)
        {
            DateTime ret = time;
            switch (intervalType)
            {
                case eInterval.Hour:
                    ret = time.AddHours(index*interval);
                    break;
                case eInterval.Minute:
                    ret = time.AddMinutes(interval*interval);
                    break;
            }

            return ret;
        }

        /// <summary>
        /// 时间加上间隔
        /// </summary>
        /// <param name="time"></param>
        /// <param name="interval"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static DateTime TimeAddInterval(DateTime time, double index, IntervalParameter interval)
        {
            return TimeAddInterval(time, index, interval.Value, interval.Style);
        }
    }
}
