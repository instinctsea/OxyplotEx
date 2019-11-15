
using OxyplotEx.Model.Styles;
using OxyplotEx.Model.Themes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OxyplotEx.Service
{
    class ColorScheduler
    {
        List<ITheme> _colors = new List<ITheme>();
        private int _color_index = 0;
        public ColorScheduler()
        {
            ColorTheme th1 = new ColorTheme();
            th1.SetThemeColor(eThemeMode.Dark, Color.SkyBlue);
            th1.SetThemeColor(eThemeMode.Light, Color.Blue);

            ColorTheme th2 = new ColorTheme();
            th2.SetThemeColor(eThemeMode.Dark, Color.Red);
            th2.SetThemeColor(eThemeMode.Light, Color.Red);

            ColorTheme th3 = new ColorTheme();
            th3.SetThemeColor(eThemeMode.Dark, Color.YellowGreen);
            th3.SetThemeColor(eThemeMode.Light, Color.Green);

            ColorTheme th4 = new ColorTheme();
            th4.SetThemeColor(eThemeMode.Dark, Color.Tomato);
            th4.SetThemeColor(eThemeMode.Light, Color.Black);
            _colors.AddRange(new ITheme[] { th1,th2,th3,th4 });
        } 

        public ITheme SrandTheme()
        {
            if (_color_index >= _colors.Count)
                _color_index = 0;

            return _colors[_color_index++];
        }
    }
}
