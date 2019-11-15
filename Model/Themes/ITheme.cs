using OxyplotEx.Model.Styles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model.Themes
{
    public interface ITheme
    {
        Color GetThemeColor(eThemeMode eThemeMode);
        void SetThemeColor(eThemeMode eThemeMode, Color color);
    }
}
