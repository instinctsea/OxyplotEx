using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module.MICAPSDataChart.GMap.EChartGMap
{
    static class ThemeConvertor
    {
        static string CreateName()
        {
            return Guid.NewGuid().ToString();
        }

        internal static CMA.MICAPS.ReactNative.Styles.AxisStyle ConvertToAxisStyle(Theme.AxisStyle style)
        {
            CMA.MICAPS.ReactNative.Styles.AxisStyle result = new CMA.MICAPS.ReactNative.Styles.AxisStyle();
            result.Name = CreateName();
            result.MainColor = style.LabelColor;

            return result;
        }

        internal static CMA.MICAPS.ReactNative.Styles.LineStyle ConvertToLineStyle(Theme.LineSeriesStyle style)
        {
            CMA.MICAPS.ReactNative.Styles.LineStyle result = new CMA.MICAPS.ReactNative.Styles.LineStyle();
            result.Name = CreateName();
            result.MainColor = style.LineColor;

            return result;
        }

        internal static CMA.MICAPS.ReactNative.Themes.AxisTheme ConvertToAxisTheme(Theme.AxisTheme theme)
        {
            CMA.MICAPS.ReactNative.Themes.AxisTheme result = new CMA.MICAPS.ReactNative.Themes.AxisTheme();

            foreach (CMA.MICAPS.Styles.ThemeColorMode mode in Enum.GetValues(typeof(CMA.MICAPS.Styles.ThemeColorMode)))
            {
                result.SetStyle(ConvertToThemeColorMode(mode),ConvertToAxisStyle(theme.GetStyle(mode) as Theme.AxisStyle));
            }

            return result;
        }

        internal static CMA.MICAPS.ReactNative.Themes.LineTheme ConvertToLineTheme(Theme.LineSeriesTheme theme)
        {
            CMA.MICAPS.ReactNative.Themes.LineTheme result = new CMA.MICAPS.ReactNative.Themes.LineTheme();

            foreach (CMA.MICAPS.Styles.ThemeColorMode mode in Enum.GetValues(typeof(CMA.MICAPS.Styles.ThemeColorMode)))
            {
                result.SetStyle(ConvertToThemeColorMode(mode), ConvertToLineStyle(theme.GetStyle(mode) as Theme.LineSeriesStyle));
            }

            return result;
        }

        internal static CMA.MICAPS.ReactNative.ThemeMode ConvertToThemeColorMode(CMA.MICAPS.Styles.ThemeColorMode mode)
        {
            switch (mode)
            {
                case CMA.MICAPS.Styles.ThemeColorMode.Dark:
                    return CMA.MICAPS.ReactNative.ThemeMode.Dark;
                case CMA.MICAPS.Styles.ThemeColorMode.Light:
                    return CMA.MICAPS.ReactNative.ThemeMode.Light;
                default:
                    return CMA.MICAPS.ReactNative.ThemeMode.Dark;
            }
        }
    }
}
