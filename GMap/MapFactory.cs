using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.GMap
{
    class MapFactory
    {
        internal static IMap Create(MapMode mode)
        {
            IMap map = null;
            switch (mode)
            {
                case MapMode.EChart:
                    //EChartMapView echart = new EChartMapView();
                    //echart.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                    //echart.Show();
                    //map = echart;
                    break;
                case MapMode.OxyPlot:
                    map = new MapView();
                    break;
                default:
                    map = new MapView();
                    break;
            }

            return map;
        }

        internal enum MapMode
        {
            OxyPlot,
            EChart
        }
    }
}
