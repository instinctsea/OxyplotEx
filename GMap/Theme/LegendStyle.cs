using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OxyplotEx.GMap.Theme
{
    class LegendStyle:StyleBase
    {
        public Color BorderColor { get; set; }
        public Color LabelColor { get; set; }
        public Color TitleColor { get; set; }

        public override StyleBase Clone()
        {
            LegendStyle style = new LegendStyle();

            style.BorderColor = this.BorderColor;
            style.LabelColor = this.LabelColor;
            style.TitleColor = this.TitleColor;

            return style;
        }
    }
}
