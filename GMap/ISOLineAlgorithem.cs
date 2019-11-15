using OxyplotEx.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace OxyplotEx.GMap
{
    class ISOLineAlgorithem
    {
        [DllImport("micapsBaseA.dll", EntryPoint = "SanJiao", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern float* SanJiao(float[] xx, float[] yy, float[] zz, float[] anaVals, int jj, int autoAna, float startV, float fJianju, int lineNum);
        [DllImport("micapsBaseA.dll", EntryPoint = "freecPointer", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void freecPointer(float* anaValue);

        LineStringCollection _lines = new LineStringCollection();
        public ISOLineAlgorithem(float interval = 0.2f)
        {
            Interval = interval;
        }

        public float Interval { get; set; }

        public LineStringCollection LineStrings
        {
            get { return _lines; }
        }

        unsafe public void Analysis(VPointCollection pts)
        {
            _lines.Clear();
            if (pts == null || pts.Count == 0)
                return;

            //analysis values
            float min = pts.GetMinValue();
            float max = pts.GetMaxValue();
            if (min == max)
                Interval = 1;
            if (Interval == 0)
                Interval = 1;

            if ((min < max && Interval < 0)||(min > max && Interval > 0))
                Interval = -Interval;

            min = GetNearestValue(min, Interval, false);
            max = GetNearestValue(max, Interval, true);

            float cur = min;
            List<float> analysis_values = new List<float>();
            while (cur <= max)
            {
                analysis_values.Add(cur);
                cur += Interval;
            }

            float[] x_array, y_array, v_array,analysis_array=null;
            pts.Split(out x_array, out y_array, out v_array);

            float* result = null;
            try
            {
                result = SanJiao(x_array, y_array, v_array, analysis_array, pts.Count, 1, min, Interval, 2);
                   
                if(result==null)
                    return;

                int analysis_value_count = (int)result[0];
                int index = 1;
                if (analysis_value_count <= 0)
                    return;

                for (; index <= analysis_value_count;)
                {
                    int point_count = (int)result[index++];
                    float analysis_value = result[index++];
                    LineString line = new LineString(analysis_value);

                    for (int i = 0; i < point_count; i++)
                    {
                        float x = result[index++];
                        float y = result[index++];
                        if (y <= Helper.InvalidData)
                            line.AddPoint(new System.Drawing.PointF(x, y));
                    }

                    if (line.Points.Count > 0)
                        line.AddVPoint(line.Points[0]);

                    _lines.Add(line);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (result != null)
                    freecPointer(result);
            }
        }

        public static float GetNearestValue(float value, float interval, bool moreThan = true)
        {
            if (moreThan)
            {
                if (value % interval == 0)
                {
                    return value;
                }
                else
                {
                    return value + Math.Abs(value % interval);
                }
            }
            else
            {
                if (value % interval == 0)
                    return value;
                else
                {
                    return value - Math.Abs(value % interval);
                }
            }
        }
    }
}
