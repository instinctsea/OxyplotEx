using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OxyplotEx.GMap.Theme
{
    public class LineSeriesStyle:StyleBase
    {
        public LineSeriesStyle()
        {

        }
        public LineSeriesStyle(Color color)
        {
            LineColor = color;
            AverageColor = color;
            AlarmColor = color;
        }
        public Color LineColor { get; set; }
        public Color AverageColor { get; set; }
        public Color AlarmColor { get; set; }

        public override StyleBase Clone()
        {
            LineSeriesStyle style = new LineSeriesStyle();

            style.LineColor = this.LineColor;
            style.AverageColor = this.AverageColor;
            style.AlarmColor = this.AlarmColor;

            return style;
        }
    }
}
