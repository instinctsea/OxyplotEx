using OxyplotEx.Model.Styles;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace OxyplotEx.GMap.Theme
{
    public class AxisTheme:ThemeBase,ICloneable
    {
        public AxisTheme()
        {
            AxisStyle light = new AxisStyle() { TitleColor = Color.FromArgb(84, 84, 101), LabelColor = Color.Black, LineColor = Color.FromArgb(148, 148, 148),LiveColor=Color.FromArgb(253,218,30),ForecastColor=Color.FromArgb(240,128,128),GridColor=Color.FromArgb(213,213,213),SepatorColor=Color.FromArgb(240,128,128) };
            AxisStyle dark = new AxisStyle() { TitleColor = Color.White, LabelColor = Color.White, LineColor = Color.White, LiveColor = Color.FromArgb(253, 218, 30), ForecastColor = Color.FromArgb(240, 128, 128), GridColor = Color.FromArgb(213, 213, 213), SepatorColor = Color.FromArgb(240, 128, 128) };

            AddStyle(eThemeMode.Light, light);
            AddStyle(eThemeMode.Dark, dark);
        }

        public object Clone()
        {
            AxisTheme theme = new AxisTheme();

            foreach (KeyValuePair<eThemeMode, StyleBase> pair in _styles)
            {
                theme.AddStyle(pair.Key, pair.Value.Clone());
            }

            return theme;
        }
    }
}
