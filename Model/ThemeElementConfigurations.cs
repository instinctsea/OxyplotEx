using OxyplotEx.Model.Styles;
using System.Collections.Generic;

namespace OxyplotEx.Model
{
    public class ThemeElementConfigurations
    {
        public ThemeElementConfigurations(string name)
        {
            this.Name = name;
        }

        public string Name
        {
            get;set;
        }

        protected Dictionary<eThemeMode, ElementConfiguration> _styles = new Dictionary<eThemeMode, ElementConfiguration>();
        public virtual void AddStyle(eThemeMode mode, ElementConfiguration style)
        {
            _styles[mode] = style;
        }

        public virtual ElementConfiguration GetStyle(eThemeMode mode)
        {
            ElementConfiguration style;
            _styles.TryGetValue(mode, out style);

            return style;
        }
    }
}
