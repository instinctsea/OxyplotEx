using System.Collections.Generic;
using System.Text;
using static OxyplotEx.GMap.MapFactory;

namespace OxyplotEx.GMap
{
    public class SeriesFactory
    {
        public enum SeriesMode
        {
            Line=1,
            Column=2,
            Area=3,
            Symbol=4,
            MultySymbol=6,
            IsoLine=7
        }
        public static int Line = 1;
        public static int Column = 2;
        public static int Area = 3;
        public static int Symbol = 4;

        static List<int> s_number_series = new List<int>();
        static List<int> s_font_series = new List<int>();
        static List<int> s_level_series = new List<int>();
        static Dictionary<int, string> s_series_names = new Dictionary<int, string>();
        static SeriesFactory()
        {
            s_number_series.AddRange(new int[] {1,2,3,5,7});
            s_font_series.AddRange(new int[] { 4,6});
            s_level_series.AddRange(new int[] { 6, 7 });
            s_series_names[1] = "折线图";
            s_series_names[2]= "柱状图";
            s_series_names[3] = "面积图";
            s_series_names[4] = "天气符号";
            s_series_names[6]= "多层天气符号";
            s_series_names[7] = "等值线";
        }

        public static string GetSeriesName(int seriesStyle)
        {
            string name=null;

            s_series_names.TryGetValue(seriesStyle, out name);
            return name;
        }

        public static bool NeedDashLimit(int seriesStyle)
        {
            return seriesStyle == 7;
        }

        public static bool IsFontSeries(int seriesStyle)
        {
            return s_font_series.Contains(seriesStyle);
        }

        public static string GetSeriesName(IEnumerable<int> seriesStyles)
        {
            StringBuilder buffer = new StringBuilder();
            foreach (int style in seriesStyles)
            {
                buffer.Append(GetSeriesName(style));
                buffer.Append("\\");
            }
            return buffer.ToString().TrimEnd('\\');
        }


        internal static ISeries CreateSeries(SeriesMode style, MapMode mode,string id)
        {
            ISeries se = null;
            switch (mode)
            {
                case MapMode.EChart:
                    se= CreateEChartSeries(style, id);
                    break;
                case MapMode.OxyPlot:
                    se=CreateOxyPlotSeries(style, id);
                    break;
            }

            return se;
        }

        internal static ISeries CreateOxyPlotSeries(SeriesMode style,string id)
        {
            ISeries se = null;
            switch (style)
            {
                case SeriesMode.Line:
                    se = new PointLineSeries();
                    se.Id = id;
                    break;
                case SeriesMode.Column:
                    break;
                case SeriesMode.Symbol:
                    break;
            }

            return se;
        }

        internal static ISeries CreateEChartSeries(SeriesMode style, string id)
        {
            ISeries se = null;
            switch (style)
            {
                //case SeriesMode.Line:
                //    se = new EChartLineSeriesEx(id);
                //    break;
                case SeriesMode.Column:
                    break;
                case SeriesMode.Symbol:
                    break;
            }

            return se;
        }

        public static ISeries CreateSeries(int seriesStyle)
        {
            ISeries series = null;
            switch (seriesStyle)
            {
                case 1: series = new PointLineSeries(); break;
                case 2: series = new PointColumnSeries(); break;
                case 3: series = new PointAreaSeries(); break;
                //case 4:
                //    FontFamily family = SeriesServiceLocator.Current.GetInstance<DataManager>().TryGetFontFamily(element.FontFamily);
                //    if (family != null)
                //        series = new FontLabelSeries(family, element.FontSize);
                //    break;
                case 5:
                    series = new ValuePairPointLineSeries();
                    break;
                //case 6:
                //    family = SeriesServiceLocator.Current.GetInstance<DataManager>().TryGetFontFamily(element.FontFamily);
                //    if (family != null)
                //        series = new FontLabelValuePairSeries(family, element.FontSize);
                //    break;
                case 7:
                    series = new ISOLineSeries();
                    break;
            }

            return series;
        }        
    }
}
