using OxyplotEx.Model.Styles;
using System.Collections.Generic;

namespace OxyplotEx.GMap.Theme
{
    public abstract  class ThemeBase
    {
        protected Dictionary<eThemeMode, StyleBase> _styles = new Dictionary<eThemeMode, StyleBase>();
        public virtual void AddStyle(eThemeMode mode, StyleBase style)
        {
            _styles[mode] = style;
        }

        public virtual StyleBase GetStyle(eThemeMode mode)
        {
            StyleBase style;
            _styles.TryGetValue(mode,out style);

            return style;
        }
    }
}
