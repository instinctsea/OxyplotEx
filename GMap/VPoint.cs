using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module.MICAPSDataChart.GMap
{
    struct VPoint
    {
        public VPoint(float x, float y, float v)
        {
            this.X = x;
            this.Y = y;
            this.V = v;
        }
        public float X;
        public float Y;
        public float V;
    }
}
