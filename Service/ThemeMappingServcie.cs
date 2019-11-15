using OxyplotEx.Model;
using OxyplotEx.Model.Styles;
using System.Collections.Generic;

namespace OxyplotEx.Service
{
    class ThemeMappingServcie
    {
        private Dictionary<string, eThemeMode> _key_theme_map = new Dictionary<string, eThemeMode>();
        private Dictionary<eThemeMode, string> _theme_key_mode = new Dictionary<eThemeMode, string>();

        public ThemeMappingServcie()
        {
            _key_theme_map.Add("light", eThemeMode.Light);
            _key_theme_map.Add("dark", eThemeMode.Dark);

            _theme_key_mode.Add(eThemeMode.Dark, "dark");
            _theme_key_mode.Add(eThemeMode.Light, "light");
        }

        public eThemeMode this[string key]
        {
            get
            {
                eThemeMode mode;
                _key_theme_map.TryGetValue(key, out mode);
                return mode;
            }
        }

        public string this[eThemeMode key]
        {
            get
            {
                string mode;
                _theme_key_mode.TryGetValue(key, out mode);
                return mode;
            }
        }
    }
}
