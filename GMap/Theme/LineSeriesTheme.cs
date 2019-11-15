using OxyplotEx.Model.Styles;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace OxyplotEx.GMap.Theme
{
    public class LineSeriesTheme:ThemeBase,ICloneable
    {
        public LineSeriesTheme()
        {
            LineSeriesStyle light = new LineSeriesStyle() { LineColor= Color.FromArgb(66,116,175),AverageColor= Color.Black,AlarmColor= Color.Blue};
            LineSeriesStyle dark = new LineSeriesStyle() { LineColor = Color.Pink, AverageColor = Color.White, AlarmColor = Color.Blue };

            AddStyle(eThemeMode.Light, light);
            AddStyle(eThemeMode.Dark, dark);
        }

        public static LineSeriesTheme FromColor(Color color)
        {
            LineSeriesTheme theme = new LineSeriesTheme();
            List<eThemeMode> modes = new List<eThemeMode>();
            modes.Add(eThemeMode.Dark);
            modes.Add(eThemeMode.Light);

            foreach (eThemeMode mode in modes)
            {
                LineSeriesStyle style = theme.GetStyle(mode) as LineSeriesStyle;
                style.LineColor = color;
                style.AlarmColor = color;
                style.AverageColor = color;
            }

            return theme;
        }

        public object Clone()
        {
            LineSeriesTheme theme = new LineSeriesTheme();
            StyleBase light = this.GetStyle(eThemeMode.Light).Clone();
            StyleBase dark = this.GetStyle(eThemeMode.Dark).Clone();

            theme.AddStyle(eThemeMode.Light, light);
            theme.AddStyle(eThemeMode.Dark, dark);
            return theme;
        }
    }
}
