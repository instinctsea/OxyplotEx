using OxyplotEx.Model.Styles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OxyplotEx.GMap.Theme
{
    class LegendTheme:ThemeBase
    {
        public LegendTheme()
        {
            LegendStyle light = new LegendStyle { BorderColor = Color.FromArgb(175, 175, 175), LabelColor = Color.Black ,TitleColor=Color.Black};
            LegendStyle dark = new LegendStyle { BorderColor = Color.White, LabelColor = Color.White ,TitleColor=Color.White};

            AddStyle(eThemeMode.Light, light);
            AddStyle(eThemeMode.Dark, dark);
        }
    }
}
