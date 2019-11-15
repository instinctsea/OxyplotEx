using Module.MICAPSDataChart.GMap;
using Module.MICAPSDataChart.Model.DataSeries;
/*
 * Copyright (c) 2014-2015 jiayongqiang. All rights reserved";
 * CLR version: 4.0.30319.18444"
 * File name:   SeriesFactory.cs "
 * Date:        2015/8/5 18:21:51 
 * Author :  jiayongqiang Developer
 * Description: 
	
 * History:  created by jiayongqiang 2015/8/5 18:21:51
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module.MICAPSDataChart.Model
{
    public static class SeriesFactory
    {
        public static ISeriesTag CreateSeries(int seriesStyle)
        {
            ISeriesTag series = null;
            switch (seriesStyle)
            {
                case 1: series = new Model.DataSeries.PointLineSeries(); break;
                case 2: series = new ColumnDataSeries(); break;
                case 3: break;
            }

            return series;
        }
    }
}
