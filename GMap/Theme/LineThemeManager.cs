using OxyplotEx.Model.Styles;
using System.Collections.Generic;
using System.Drawing;

namespace OxyplotEx.GMap.Theme
{
    public sealed class LineThemeManager
    {
        List<LineSeriesTheme> _themes = new List<LineSeriesTheme>();
        private int _index = 0;
        public LineThemeManager()
        {
            Load();
        }

        public bool IsLoaded
        {
            get; private set;
        }

        public void Load()
        {
            if (IsLoaded)
                return;

            LineSeriesTheme theme1 = new LineSeriesTheme();
            LineSeriesTheme theme2 = new LineSeriesTheme();
            LineSeriesTheme theme3 = new LineSeriesTheme();
            LineSeriesTheme theme4 = new LineSeriesTheme();
            LineSeriesTheme theme5 = new LineSeriesTheme();
            LineSeriesTheme theme6 = new LineSeriesTheme();
            LineSeriesTheme theme7 = new LineSeriesTheme();

            LineSeriesStyle light1 = new LineSeriesStyle() { LineColor = Color.FromArgb(66, 116, 175), AverageColor = Color.Black, AlarmColor = Color.Blue };
            LineSeriesStyle light2 = new LineSeriesStyle() { LineColor = Color.FromArgb(143, 90, 188), AverageColor = Color.Black, AlarmColor = Color.Blue };
            LineSeriesStyle light3 = new LineSeriesStyle() { LineColor = Color.FromArgb(21, 138, 236), AverageColor = Color.Black, AlarmColor = Color.Blue };
            LineSeriesStyle light4 = new LineSeriesStyle() { LineColor = Color.FromArgb(41, 166, 83), AverageColor = Color.Black, AlarmColor = Color.Blue };
            LineSeriesStyle light5 = new LineSeriesStyle() { LineColor = Color.FromArgb(204, 88, 66), AverageColor = Color.Black, AlarmColor = Color.Blue };
            LineSeriesStyle light6 = new LineSeriesStyle() { LineColor = Color.FromArgb(215, 160, 41), AverageColor = Color.Black, AlarmColor = Color.Blue };
            LineSeriesStyle light7 = new LineSeriesStyle() { LineColor = Color.FromArgb(162, 188, 122), AverageColor = Color.Black, AlarmColor = Color.Blue };

            LineSeriesStyle dark1 = new LineSeriesStyle() { LineColor = Color.DeepPink, AverageColor = Color.White, AlarmColor = Color.Yellow };
            LineSeriesStyle dark2 = new LineSeriesStyle() { LineColor = Color.FromArgb(143,90,188), AverageColor = Color.White, AlarmColor = Color.Yellow };
            LineSeriesStyle dark3 = new LineSeriesStyle() { LineColor = Color.FromArgb(21,138,236), AverageColor = Color.White, AlarmColor = Color.Yellow };
            LineSeriesStyle dark4 = new LineSeriesStyle() { LineColor = Color.FromArgb(41,166,83), AverageColor = Color.White, AlarmColor = Color.Yellow };
            LineSeriesStyle dark5 = new LineSeriesStyle() { LineColor = Color.FromArgb(204,88,66), AverageColor = Color.White, AlarmColor = Color.Yellow };
            LineSeriesStyle dark6 = new LineSeriesStyle() { LineColor = Color.FromArgb(215,160,41), AverageColor = Color.White, AlarmColor = Color.Yellow };
            LineSeriesStyle dark7 = new LineSeriesStyle() { LineColor = Color.FromArgb(0,120,39), AverageColor = Color.White, AlarmColor = Color.Yellow };

            theme1.AddStyle(eThemeMode.Light, light1);
            theme2.AddStyle(eThemeMode.Light, light2);
            theme3.AddStyle(eThemeMode.Light, light3);
            theme4.AddStyle(eThemeMode.Light, light4);
            theme5.AddStyle(eThemeMode.Light, light5);
            theme6.AddStyle(eThemeMode.Light, light6);
            theme7.AddStyle(eThemeMode.Light, light7);

            theme1.AddStyle(eThemeMode.Dark, dark1);
            theme2.AddStyle(eThemeMode.Dark, dark2);
            theme3.AddStyle(eThemeMode.Dark, dark3);
            theme4.AddStyle(eThemeMode.Dark, dark4);
            theme5.AddStyle(eThemeMode.Dark, dark5);
            theme6.AddStyle(eThemeMode.Dark, dark6);
            theme7.AddStyle(eThemeMode.Dark, dark7);

            _themes.Add(theme1);
            _themes.Add(theme2);
            _themes.Add(theme3);
            _themes.Add(theme4);
            _themes.Add(theme5);
            _themes.Add(theme6);
            _themes.Add(theme7);

            IsLoaded = true;
        }

        public LineSeriesTheme GetTheme()
        {
            if (_themes.Count == 0)
                return new LineSeriesTheme();

            if (_index >= _themes.Count)
                _index = _index %_themes.Count;

            return _themes[_index++];
        }
    }
}
