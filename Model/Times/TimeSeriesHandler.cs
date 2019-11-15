using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model.Time
{
    /// <summary>
    /// 时间序列处理
    /// </summary>
    public class TimeSeriesHandler
    {
        /// <summary>
        /// 制作时间序列模型
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="interval"></param>
        /// <param name="intervalStyle"></param>
        /// <param name="forecast">预报时效</param>
        /// <param name="timelines"></param>
        /// <param name="startDuration"></param>
        /// <returns></returns>
        public static TimeDataManager MakeTimeModels(DateTime start, DateTime end, IntervalParameter interval, int forecast, out TimeLinesCollection timelines, int startDuration = 0)
        {
            timelines = new TimeLinesCollection();
            TimeDataManager times = new TimeDataManager();

            if (startDuration == 0)
            {
                double index = 0;
                //实况数据时间
                DateTime cur = start;

                while (cur < end)
                {
                    times.Add(new TimeModel(cur, 0, index,true));
                    cur = TimeOperator.TimeAddInterval(cur,interval);
                    index += 1;
                }
                index = GetIndex(start, interval,end);
                times.Add(new TimeModel(end,0, index, true));
                //模式预报时间
                //此处为模式数据处理部分，目前只支持小时间隔
                int duration =startDuration+interval.Value;
                while (duration <= forecast)
                {
                    index = GetIndex(start, interval, end.AddHours(duration));
                    times.Add(new TimeModel(end, duration, index,false));
                    duration += interval.Value;
                }

                //实况
                DateTime live_time = new DateTime(start.Year, start.Month, start.Day, start.Hour, 0, 0);
                while (live_time < end)
                {
                    timelines.Add(new TimeLine(start, live_time, interval, TimeStyle.Live));
                    live_time = TimeOperator.TimeAddInterval(live_time, interval);
                }

                //预报
                DateTime forecast_end = end.AddHours(forecast);
                timelines.Add(new TimeLine(start, end, interval,TimeStyle.Seperator));
                live_time = TimeOperator.TimeAddInterval(end, interval);
                while (live_time <= forecast_end)
                {
                    timelines.Add(new TimeLine(start, live_time, interval, TimeStyle.Forecast));

                    live_time = TimeOperator.TimeAddInterval(live_time, interval);
                }

                int count = (int)Math.Round(index);
                timelines.IndexCount= count;
            }
            else
            {
                int index = 0;
                //模式预报时间
                //此处为模式数据处理部分，目前只支持小时间隔
                int duration = startDuration;
                while (duration <= forecast)
                {
                    times.Add(new TimeModel(end, duration, index++,false));
                    duration += interval.Value;
                }

                DateTime live_time = new DateTime(end.Year, end.Month, end.Day, end.Hour, 0, 0).AddHours(startDuration);

                DateTime forecast_end = end.AddHours(forecast);
                while (live_time <= forecast_end)
                {
                    timelines.Add(new TimeLine(end.AddHours(startDuration), live_time, interval, TimeStyle.Forecast));

                    live_time = TimeOperator.TimeAddInterval(live_time, interval);
                }

                timelines.IndexCount = index;
            }
            return times;
        }

        public static double GetIndex(DateTime start, IntervalParameter interval, DateTime now)
        {
            double index = -1;
            switch (interval.Style)
            { 
                case eInterval.Hour:
                    double total_hour = (now - start).TotalHours;
                    index = total_hour / interval.Value;
                    break;
                case eInterval.Minute:
                    double total_min = (now - start).TotalMinutes;
                    index = total_min / interval.Value;
                    break;
            }

            return index;
        }

        /// <summary>
        /// make all forecast
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="interval"></param>
        /// <param name="forecast"></param>
        /// <returns></returns>
        public static List<TimeDataManager> MakeAllForecast(DateTime start, DateTime end, IntervalParameter interval, int forecast)
        {
            if (start > end)
                throw new Exception("start time must be smaller than or equal end time!");
            if (interval==null ||interval.Value <= 0)
                throw new Exception("interval must be over 0!");
            if (forecast < 0)
                throw new Exception("forecast can't be smaller than 0!");

            List<TimeDataManager> all_forecast = new List<TimeDataManager>();

            //所有预报时间
            DateTime cur = start;
            List<DateTime> times = new List<DateTime>();
            while (cur < end)
            {
                times.Add(cur);
                cur = TimeOperator.TimeAddInterval(cur, interval);
            }

            //预报结束时间
            DateTime forecast_end = end.AddHours(forecast);
            foreach (DateTime time in times)
            {
                TimeDataManager time_datas = new TimeDataManager();
                time_datas.ForecastTime = time;
                //实况
                cur = start;
                int index = 0;
                while (cur < end)
                {
                    time_datas.Add(new TimeModel(cur, 0, index,true));
                    cur = TimeOperator.TimeAddInterval(cur, interval);
                    index++;
                }

                //预报场
                cur = end;
                while (cur <= forecast_end)
                {
                    int duration = (cur - time).Hours;
                    time_datas.Add(new TimeModel(time, duration, index,false));
                    cur = TimeOperator.TimeAddInterval(cur,interval);
                    index++;
                }
                all_forecast.Add(time_datas);
            }

            return all_forecast;
        }

        public static List<DateTime> MakeSeries(DateTime start, DateTime end, IntervalParameter parameter,bool endtostart=true)
        {
            List<DateTime> result = new List<DateTime>();
            if (endtostart)
            {
                IntervalParameter temp = new IntervalParameter();
                temp.Style = parameter.Style;
                temp.Value = parameter.Value * -1;
                DateTime cur = end;
                while (cur >= start)
                {
                    result.Add(cur);
                    cur = TimeOperator.TimeAddInterval(cur, temp);
                }
            }
            else
            {
                DateTime cur = start;
                while (cur <= end)
                {
                    result.Add(cur);
                    cur = TimeOperator.TimeAddInterval(cur, parameter);
                }
            }

            return result;
        }
    }
}
