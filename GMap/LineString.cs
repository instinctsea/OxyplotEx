using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OxyplotEx.GMap
{
    class LineString
    {
        List<PointF> _pts = new List<PointF>();
        List<PointF> _v_pts = new List<PointF>();
        public LineString(float value)
        {
            Value = value;
        }

        public float Value
        {
            get; set;
        }

        public void AddPoint(PointF pt)
        {
            _pts.Add(pt);
        }

        public void AddVPoint(PointF pt)
        {
            _v_pts.Add(pt);
        }

        public List<PointF> Points { get { return _pts; } }

        public List<PointF> VPoints { get { return _v_pts; } }
    }
}
