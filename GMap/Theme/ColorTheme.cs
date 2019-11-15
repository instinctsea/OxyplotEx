using OxyplotEx.Model.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.GMap.Theme
{
    public class ColorTheme:ThemeBase
    {
        public ColorTheme()
        {
            ColorStyle light = new ColorStyle();
            ColorStyle dark = new ColorStyle();

            AddStyle(eThemeMode.Light,light);
            AddStyle(eThemeMode.Dark, dark);
        }
    }
}
