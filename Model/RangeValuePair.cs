
using System;

namespace OxyplotEx.Model
{
    public struct RangeValuePair
    {
        private double _min;
        private double _max;
        public RangeValuePair(double min,double max)
        { 
            this._min=min;
            this._max=max;
        }

        public double Minimum
        {
            get
            {
                return _min;
            }
            set
            {
                _min = value;
            }
        }

        public double Maximum
        {
            get
            {
                return _max;
            }
            set
            {
                _max = value;
            }
        }

        public void SetMaxRange(double min, double max)
        {
            if (this.Minimum > min)
                this.Minimum = min;

            if (this.Maximum < max)
                this.Maximum = max;
        }

        public void SetMaxRange(RangeValuePair range)
        {
            SetMaxRange(range.Minimum, range.Maximum);
        }

        /// <summary>
        /// 尝试获取范围percent百分位上下的范围
        /// </summary>
        /// <param name="percent"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public bool TryGetLargerRange(float percent,out RangeValuePair range)
        {
            range = new RangeValuePair();
            if(Maximum<=Minimum)
                return false;

            double sub_value=Maximum-Minimum;
            double percent_value=sub_value*percent;

            range.Minimum=Math.Floor(this.Minimum-percent_value);
            range.Maximum=Math.Ceiling(this.Maximum+percent_value);

            return true;
        }
    }
}
