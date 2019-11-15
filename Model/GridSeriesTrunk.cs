using Module.MICAPSDataChart.Model.DataSeries;
using Module.MICAPSDataChart.Model.Styles;
using Module.MICAPSDataChart.Service;
/*
 * Copyright (c) 2014-2015 jiayongqiang. All rights reserved";
 * CLR version: 4.0.30319.18444"
 * File name:   GridSeriesTrunk.cs "
 * Date:        2015/8/11 19:23:40 
 * Author :  jiayongqiang Developer
 * Description: 
	
 * History:  created by jiayongqiang 2015/8/11 19:23:40
*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Module.MICAPSDataChart.Model
{
    public class GridSeriesTrunk
    {
        private SeriesManager _series_manager = new SeriesManager();
        public GridSeriesTrunk(string stationId,string stationName,bool smooth)
        {
            var axis_binding_mapping = Service.SeriesServiceLocator.Current.GetInstance<ElementAxisBingdingKeyMapping>();
            var color_scheduler = Service.SeriesServiceLocator.Current.GetInstance<ColorScheduler>();
            ISeriesTag series = SeriesFactory.CreateSeries(1);
            if (series == null)
                return;
            series.Id = stationId;
            series.Name =string.IsNullOrEmpty(stationName)? stationId:stationName;
            if (series is IXYAxisBinding)
            {
                //((IXYAxisBinding)series).XAxisBingdingKey = ;
                ((IXYAxisBinding)series).YAxisBingdingKey ="temper";
            }
            if (series is ISeriesStyle)
            {
                //((ISeriesStyle)series).MainColor = 
                ((ISeriesStyle)series).Theme = color_scheduler.SrandTheme();
                ((ISeriesStyle)series).SeriesTitle = string.IsNullOrEmpty(stationName) ? stationId : stationName;
            }
            if (series is ILineSeriesStyle)
            {             
                ((ILineSeriesStyle)series).LineSmooth = smooth;
            }
            _series_manager.Add(series);
        }

        public SeriesManager Series
        {
            get { return _series_manager; }
        }
    }
}
