using OxyplotEx.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.GMap
{
    class VPointCollection:IEnumerable<ValuePairPointModel>
    {
        List<ValuePairPointModel> _pts = new List<ValuePairPointModel>();
        public VPointCollection()
        {
            
        }       

        public int Count
        {
            get { return _pts.Count; }
        }

        public ValuePairPointModel this[int index]
        {
            get { return _pts[index]; }
        }

        public float GetMaxValue()
        {
            float max = float.MinValue;
            for (int i = 0; i < _pts.Count; i++)
            {
                double value;
                if(Double.TryParse(_pts[i].Value,out value))
                    if (max < value)
                        max = (float)value;
            }

            return max;
        }

        public float GetMinValue()
        {
            float min = float.MaxValue;
            for (int i = 0; i < _pts.Count; i++)
            {
                double value;
                if (Double.TryParse(_pts[i].Value, out value))
                {
                    if (min > value)
                        min = (float)value;
                }
            }

            return min;
        }

        public void Add(ValuePairPointModel pt)
        {
            _pts.Add(pt);
        }

        public void Split(out float[] xArray, out float[] yArray, out float[] vArray)
        {
            xArray = null;
            yArray = null;
            vArray = null;
            if (Count == 0)
                return;

            xArray = new float[Count];
            yArray = new float[Count];
            vArray = new float[Count];
            for (int i = 0; i < Count; i++)
            {
                xArray[i] = (float)this[i].Index;
                yArray[i] = (float)this[i].Y;
                double value;
                if (double.TryParse(this[i].Value, out value))
                {
                    vArray[i] = (float)value;
                }
                else
                    vArray[i] = (float)Helper.InvalidData;
            }
        }

        public void Clear()
        {
            _pts.Clear();
        }

        public IEnumerator<ValuePairPointModel> GetEnumerator()
        {
            return _pts.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _pts.GetEnumerator();
        }
    }
}
