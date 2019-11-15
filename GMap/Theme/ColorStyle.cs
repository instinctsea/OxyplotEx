using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OxyplotEx.GMap.Theme
{
    class ColorStyle : StyleBase
    {
        public ColorStyle()
        {
            this.Color = Color.Red;
        }

        public Color Color
        {
            get;set;
        }

        public override StyleBase Clone()
        {
            ColorStyle style = new ColorStyle();
            style.Color = this.Color;
            return style;
        }
    }
}
