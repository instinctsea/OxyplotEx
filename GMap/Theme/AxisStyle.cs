using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OxyplotEx.GMap.Theme
{
    class AxisStyle:StyleBase
    {
        public AxisStyle()
        {
            TitleColor = Color.White;
            LabelColor = Color.White;
            LineColor = Color.White;
            LiveColor = Color.FromArgb(253, 218, 30);
            ForecastColor = Color.FromArgb(240, 128, 128);
            GridColor = Color.FromArgb(213, 213, 213);
            SepatorColor = Color.FromArgb(240, 128, 128);
        }

        public Color TitleColor { get; set; }
        public Color LineColor { get; set; }
        public Color LabelColor { get; set; }
        public Color GridColor { get; set; }
        public Color LiveColor { get; set; }
        public Color ForecastColor { get; set; }
        public Color SepatorColor { get; set; }

        public override StyleBase Clone()
        {
            AxisStyle style = new AxisStyle();

            style.TitleColor = this.TitleColor;
            style.LineColor = this.LineColor;
            style.LabelColor = this.LabelColor;
            style.GridColor = this.GridColor;
            style.LiveColor = this.LiveColor;
            style.ForecastColor = this.ForecastColor;
            style.SepatorColor = this.SepatorColor;

            return style;
        }
    }
}
