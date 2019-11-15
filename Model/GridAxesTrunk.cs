using Module.MICAPSDataChart.Model.Themes;
using Module.MICAPSDataChart.Model.CoordinateAxises;
using OxyPlot;
using OxyPlot.Axes;
/*
 * Copyright (c) 2014-2015 jiayongqiang. All rights reserved";
 * CLR version: 4.0.30319.18444"
 * File name:   GridAxesTrunk.cs "
 * Date:        2015/8/11 19:34:02 
 * Author :  jiayongqiang Developer
 * Description: 
	
 * History:  created by jiayongqiang 2015/8/11 19:34:02
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module.MICAPSDataChart.Model
{
    class GridAxesTrunk
    {
        AxisManager _axis_manager = new AxisManager();
        public GridAxesTrunk(TimeSeriesSetting setting)
        {
            CoordinateXYAxis temperature_axis = new CoordinateXYAxis();
            temperature_axis.Id = AxisName.Temperature;
            temperature_axis.SeriesBingdingKey = AxisName.Temperature;
            temperature_axis.Position = AxisPosition.Left;
            temperature_axis.ExtraGridlines = new Double[1];
            temperature_axis.ExtraGridlines[0] = 0;
            temperature_axis.MajorGridlineStyle = LineStyle.Solid;
            temperature_axis.MinorGridlineStyle = LineStyle.Dot;
            temperature_axis.TicklineColor = OxyColors.SkyBlue;
            temperature_axis.Maximum = 50;
            temperature_axis.Minimum = -30;
            temperature_axis.TextColor = OxyColors.SkyBlue;
            temperature_axis.TitleColor = OxyColors.SkyBlue;
            temperature_axis.IsZoomEnabled = false;
            temperature_axis.ExtraGridlineColor = OxyColor.FromArgb(255, 96, 96, 96);
            temperature_axis.MajorGridlineColor = OxyColor.FromArgb(255, 96, 96, 96);
            temperature_axis.MinorGridlineColor = OxyColor.FromArgb(255, 96, 96, 96);
            ColorTheme temperature_theme = new ColorTheme();
            temperature_axis.Theme = temperature_theme;

            CategoryAxisX cloud_axis = new CategoryAxisX();
            cloud_axis.Id = AxisName.Cloud;
            cloud_axis.SeriesBingdingKey = AxisName.Cloud;
            cloud_axis.TicklineColor = OxyColors.SkyBlue;
            cloud_axis.IsZoomEnabled = false;
            //cloud_axis.AxisTickToLabelDistance = 45;
            cloud_axis.Position = AxisPosition.Bottom;
            cloud_axis.MinorStep = 1;
            cloud_axis.MajorGridlineStyle = LineStyle.Solid;
            cloud_axis.MinorGridlineStyle = LineStyle.Dot;
            cloud_axis.TextColor = OxyColors.SkyBlue;
            cloud_axis.ExtraGridlineColor = OxyColor.FromArgb(255, 96, 96, 96);
            cloud_axis.MajorGridlineColor = OxyColor.FromArgb(255, 96, 96, 96);
            cloud_axis.MinorGridlineColor = OxyColor.FromArgb(255, 96, 96, 96);

            DateTime temp = setting.StartTime;
            while (temp <= setting.EndTime)
            {
                cloud_axis.Labels.Add(temp.ToString("dd-HH"));
                temp = temp.AddHours(24);
            }
            int interval = setting.TimeInterval.Value;
            while (interval <= setting.ForecastTime)
            {
                temp = setting.EndTime.AddHours(interval);
                cloud_axis.Labels.Add(temp.ToString("dd-HH"));
                interval = interval + setting.TimeInterval.Value;
            }
            _axis_manager.Add(temperature_axis);
            _axis_manager.Add(cloud_axis);
        }

        public AxisManager Axes
        {
            get { return _axis_manager; }
        }
    }
}
