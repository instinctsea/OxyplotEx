using Module.MICAPSDataChart.Model.Themes;
using Module.MICAPSDataChart.Model.DataSeries;
using OxyPlot;
using OxyPlot.Axes;
/*
 * Copyright (c) 2014-2015 jiayongqiang. All rights reserved";
 * CLR version: 4.0.30319.18444"
 * File name:   CoordinateAxis.cs "
 * Date:        2015/8/5 11:23:30 
 * Author :  jiayongqiang Developer
 * Description: 
	
 * History:  created by jiayongqiang 2015/8/5 11:23:30
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module.MICAPSDataChart.Model.CoordinateAxises
{
    class CoordinateXYAxis:LinearAxis,IAxis, IValueRange
    {
        public CoordinateXYAxis()
        {

        }

        public ITheme Theme
        {
            get;
            set;
        }

        public string Id
        {
            get;
            set;
        }

        public string SeriesBingdingKey
        {
            get
            {
                return base.Key;
            }
            set
            {
                base.Key = value;
            }
        }

        public double ValueMaximum
        {
            get
            {
                return this.Maximum;
            }
            set
            {
                this.Maximum = value ;
            }
        }

        public double ValueMinimum
        {
            get
            {
                return this.Minimum;
            }
            set
            {
                this.Minimum = value;
            }
        }

        protected override string FormatValueOverride(double x)
        {
            string value = string.Empty;
            if (x == 0)
                value = "0";
            else
            {
                if (x == (int)x)
                    value = x.ToString("f0");
                else
                    value = x.ToString("f1");
            }
            return value;
        }

        public override void Render(OxyPlot.IRenderContext rc, PlotModel model1, AxisLayer axisLayer, int pass)
        {
            PlotModel model = this.PlotModel;
            if (Theme != null)
            {
                OxyColor color = Convertor.ConvertColorToOxyColor(Theme.GetThemeColor());
                TicklineColor = color;
                TextColor = color;
                TitleColor = color;
            }
            base.Render(rc, model1,axisLayer, pass);
        }
    }
}
