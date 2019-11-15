
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using OxyplotEx.Model.Styles;
using OxyplotEx.Model.Themes;

namespace OxyplotEx.Model.Styles
{
    class PointLineStyle:ILineSeriesStyle
    {
        public PointLineStyle()
        {
            MarkerFillColor = Color.White;
            MarkerStrokeColor = Color.ForestGreen;
            MarkerStyle = eMarkerStyle.Circle;
            LineWidth = 3;
            XDataField = "Date";
            YDataField = "Value";
            this.LineSmooth = false;
        }

        public System.Drawing.Color MarkerFillColor
        {
            get;
            set;
        }

        public System.Drawing.Color MarkerStrokeColor
        {
            get;
            set;
        }

        public eMarkerStyle MarkerStyle
        {
            get;
            set;
        }

        public int LineWidth
        {
            get;
            set;
        }

        public string XDataField
        {
            get;
            set;
        }

        public string YDataField
        {
            get;
            set;
        }

        public string SeriesTitle
        {
            get;
            set;
        }

        public void Update(ILineSeriesStyle lineStyle)
        {
            this.LineWidth = lineStyle.LineWidth;
            this.MarkerStyle = lineStyle.MarkerStyle;
            this.SeriesTitle = lineStyle.SeriesTitle;
            this.XDataField = lineStyle.XDataField;
            this.YDataField = lineStyle.YDataField;
            this.LineSmooth = lineStyle.LineSmooth;
        }

        public Color MainColor
        {
            get;
            set;
        }

        public bool Visible
        {
            get;
            set;
        }

        public bool LineSmooth
        {
            get;
            set;
        }


        public ITheme Theme
        {
            get;
            set;
        }

        public eLineStyle SeriesLineStyle
        {
            get;set;
        }
    }
}
