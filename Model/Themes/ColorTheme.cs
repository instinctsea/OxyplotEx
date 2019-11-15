using OxyplotEx.Model.Styles;
using System.Collections.Generic;
using System.Drawing;

namespace OxyplotEx.Model.Themes
{
    class ColorTheme:ITheme
    {
        Dictionary<eThemeMode, Color> _colors = new Dictionary<eThemeMode, Color>();
        public ColorTheme()
        {
            SetThemeColor(eThemeMode.Dark, Color.SkyBlue);
            SetThemeColor(eThemeMode.Light, Color.Black);
        }

        public void SetThemeColor(eThemeMode themeMode, Color color)
        {
            _colors[themeMode] = color;
        }

        public Color GetThemeColor(eThemeMode themeMode)
        {
            Color color;
            _colors.TryGetValue(themeMode, out color);
            return color;
        }
    }
}
