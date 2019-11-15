using OxyplotEx.GMap;
using OxyplotEx.Model.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model
{
    public class LineStyleDescriptor
    {
        public LineStyleDescriptor(eLineStyle style, string desc)
        {
            LineStyle = style;
            Descriptor = desc;
        }
        public eLineStyle LineStyle;
        public string Descriptor;
        public override string ToString()
        {
            return Descriptor;
        }
    }

    public class PointLineStyleDesc
    {
        public PointLineStyleDesc(ePointLineStyle2 style, string desc)
        {
            this.Style = style;
            this.Desc = desc;
        }

        public ePointLineStyle2 Style { get; set; }

        public string Desc { get; set; }

        public override string ToString()
        {
            return Desc;
        }
    }
}
