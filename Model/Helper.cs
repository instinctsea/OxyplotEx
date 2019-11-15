using OxyPlot;
using OxyplotEx.GMap;
using OxyplotEx.GMap.Theme;
using OxyplotEx.Model.Styles;
using OxyplotEx.Model.TimeSeries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace OxyplotEx.Model
{
    public static class Helper
    {
        public const double InvalidData = 9999;
        public const double NoData = 9999;
        public const string InvalidDataStr = "9999";
        public const string InvalidDataStr2 = "99999";
        public static Color DarkBackColor = Color.FromArgb(45, 45, 48);
        public static Color LightBackColor = Color.White;

        public static LineSeriesTheme CreateTheme()
        {
            LineSeriesTheme theme = new LineSeriesTheme();
            var style= theme.GetStyle(eThemeMode.Dark) as LineSeriesStyle;
            style.LineColor = DarkBackColor;
            style.AverageColor = DarkBackColor;

            var light = theme.GetStyle(eThemeMode.Light) as LineSeriesStyle;
            light.LineColor = LightBackColor;

            return theme;
        }

        public static void GetAndSetLegendTheme(IMap map, LineSeriesTheme theme)
        {
            IEnumerable<ISeries> serieses = map.GetAllSeries();
            if (serieses == null)
                return;

            foreach (ISeries se in serieses)
            {
                if (se is LegendSeries)
                {
                    se.Theme = theme;
                }
            }
        }

        public static Color GetBackColorByThemeMode(eThemeMode mode)
        {
            switch (mode)
            {
                case eThemeMode.Dark:
                    return DarkBackColor;
                case eThemeMode.Light:
                    return LightBackColor;
                default:
                    return DarkBackColor;
            }
        }


       

        public static OxyColor ConvertColorToOxyColor(Color color)
        {
            return OxyColor.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static Color ConvertOxyColorToColor(OxyColor color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static eLineStyle GetLineStyle(string style)
        {
            if (string.IsNullOrEmpty(style))
                return eLineStyle.None;

            eLineStyle line_style;
            switch (style.ToLower())
            {
                case "solid":
                    line_style=eLineStyle.Solid;
                    break;
                case "dash":
                    line_style= eLineStyle.Dash;
                    break;
                case "dot":
                    line_style = eLineStyle.Dot;
                    break;
                case "dashdot":
                    line_style = eLineStyle.DashDot;
                    break;
                case "dashdashdot":
                    line_style = eLineStyle.DashDashDot;
                    break;
                case "dashdotdot":
                    line_style = eLineStyle.DashDotDot;
                    break;
                case "dashdashdotdot":
                    line_style = eLineStyle.DashDashDotDot;
                    break;
                default:
                    line_style = eLineStyle.None;
                    break;
            }

            return line_style;
        }

        public static ePointLineStyle2 GetLineStyle2(string style)
        {
            if (string.IsNullOrEmpty(style))
                return ePointLineStyle2.SmoothSolid;

            ePointLineStyle2 line_style;
            switch (style.ToLower())
            {
                case "solid":
                case "polygonsolid":
                    line_style = ePointLineStyle2.PolygonSolid;
                    break;
                case "dash":
                case "polygondash":
                    line_style = ePointLineStyle2.PolygonDash;
                    break;
                case "smoothsolid":
                    line_style = ePointLineStyle2.SmoothSolid;
                    break;
                case "smoothdash":
                    line_style = ePointLineStyle2.SmoothDash;
                    break;
                case "cloumn":
                    line_style = ePointLineStyle2.Cloumn;
                    break;
                default:
                    line_style = ePointLineStyle2.PolygonSolid;
                    break;
            }

            return line_style;
        }

        public static Color ColorFromString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return Color.White;

            string[] rgba = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (rgba.Length == 4)
            {
                byte a = ConvertStringToByte(rgba[0]);
                byte r = ConvertStringToByte(rgba[1]);
                byte g = ConvertStringToByte(rgba[2]);
                byte b = ConvertStringToByte(rgba[3]);
                return Color.FromArgb(a, r, g, b);
            }
            else
            {
                return Color.White;
            }
        }

        public static string ColorToString(Color color)
        {
            return string.Format("{0},{1},{2},{3}", color.A, color.R, color.G, color.B);
        }

        internal static FigureType ConvertStringToFigureType(string figure)
        {
            FigureType figure_type = FigureType.None;
            if (string.IsNullOrEmpty(figure))
                return figure_type;
            switch (figure.ToLower())
            {
                case "datagroup":
                    figure_type = FigureType.DataGroup;
                    break;
                case "data":
                    figure_type = FigureType.Data;
                    break;
                default:
                    figure_type = FigureType.None;
                    break;
            }

            return figure_type;
        }

        public static byte ConvertStringToByte(string str)
        {
            byte ret;
            if (byte.TryParse(str, out ret))
                return ret;

            return (byte)255;
        }
    }
}
