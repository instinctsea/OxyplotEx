using System;
using System.Collections.Generic;

namespace OxyplotEx.Model.Time
{
    /// <summary>
    /// 间隔参数
    /// </summary>
    public class IntervalParameter:IEquatable<IntervalParameter>,IComparer<IntervalParameter>,IComparable<IntervalParameter>
    {
        static Dictionary<eInterval, string> s_interval_desc_map = new Dictionary<eInterval, string>();
        static IntervalParameter()
        {
            s_interval_desc_map[eInterval.Minute] ="min";
            s_interval_desc_map[eInterval.Hour] = "hour";
        }
        /// <summary>
        /// ctr. no parameter
        /// </summary>
        public IntervalParameter()
        {

        }

        /// <summary>
        /// ctr.
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="intervalStyle"></param>
        public IntervalParameter(int interval, eInterval intervalStyle)
        {
            this.Value = interval;
            this.Style = intervalStyle;
        }
        /// <summary>
        /// 间隔类型
        /// </summary>
        public eInterval Style { get; set; }

        private int _value = 1;
        /// <summary>
        /// 间隔数值
        /// </summary>
        public int Value
        {
            get { return _value; }
            set
            {
                _value = value;
                if (value == 0)
                    throw new Exception("interval value could't be 0!");
            }
        }

        /// <summary>
        /// tostring
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Value.ToString()+s_interval_desc_map[Style];
        }

        /// <summary>
        /// equal
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IntervalParameter other)
        {
            if (other == null)
                return false;

            return this.Value == other.Value && this.Style == other.Style;
        }

        /// <summary>
        /// 比较器
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(IntervalParameter x, IntervalParameter y)
        {
            if (x == null && y == null)
                return 0;
            else if (x != null && y == null)
                return 1;
            else if (x == null && y != null)
                return -1;
            else
            {
                if ((x.Style == eInterval.Hour && y.Style == eInterval.Hour) ||
                    (x.Style == eInterval.Minute && y.Style == eInterval.Minute))
                {
                    if (x.Value > y.Value)
                        return 1;
                    else if (x.Value == y.Value)
                        return 0;
                    else
                        return -1;
                }
                else if (x.Style == eInterval.Hour && y.Style == eInterval.Minute)
                    return 1;
                else
                    return -1;
            }


        }

        /// <summary>
        /// 比较器，用来排序
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(IntervalParameter other)
        {
            if (other == null)
                return 1;
            else
            {
                if ((Style == eInterval.Hour && other.Style == eInterval.Hour) ||
                    (Style == eInterval.Minute && other.Style == eInterval.Minute))
                {
                    if (Value > other.Value)
                        return 1;
                    else if (Value == other.Value)
                        return 0;
                    else
                        return -1;
                }
                else if (Style == eInterval.Hour && other.Style == eInterval.Minute)
                    return 1;
                else
                    return -1;
            }
        }
    }
}
