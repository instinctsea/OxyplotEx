using Module.MICAPSDataChart.Model;
using Module.MICAPSDataChart.Model.Themes;
using OxyPlot;
using OxyPlot.Axes;
/*
 * Copyright (c) 2014-2015 jiayongqiang. All rights reserved";
 * CLR version: 4.0.30319.18444"
 * File name:   CategoryAxis.cs "
 * Date:        2015/8/12 11:17:36 
 * Author :  jiayongqiang Developer
 * Description: 
	
 * History:  created by jiayongqiang 2015/8/12 11:17:36
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module.MICAPSDataChart.Model.CoordinateAxises
{
    class CategoryAxisX:CategoryAxis,IAxis,IReverseData
    {
        public CategoryAxisX()
        {
            Theme = new ColorTheme();
        }
        public string Id
        {
            get;
            set;
        }

        public ITheme Theme
        {
            get;
            set;
        }

        public string SeriesBingdingKey
        {
            get { return base.Key; }
            set
            {
                base.Key = value;
            }
        }

        public void ReverseData()
        {
            this.Labels.Reverse();
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
            //base.Render(rc, model, axisLayer, pass);
            IList<IList<ScreenPoint>> points = new List<IList<ScreenPoint>>();
            FeatureTextIntersector intersector = new FeatureTextIntersector(FeatureTextIntersector.SortStyle.Horizontal, 3);
            for (int i = 0; i < this.Labels.Count; i++)
            {
                string label = this.Labels[i];
                OxySize text_size = rc.MeasureText(label, this.ActualFont, this.ActualFontSize, this.ActualFontWeight);

                double x = Transform(i);
                if (x < model.PlotArea.Left || x > model.PlotArea.Right)
                    continue;

                double y = model.PlotArea.Bottom;
                double y2 = y + 5;

                IList<ScreenPoint> sps = new List<ScreenPoint>();
                sps.Add(new ScreenPoint(x, y));
                sps.Add(new ScreenPoint(x, model.PlotArea.Top));

                points.Add(sps);
                intersector.Add(new FeatureText(label, new ScreenPoint(x, y2), text_size));

                // rc.DrawText(new ScreenPoint(x, y + 7), label, TextColor, this.ActualFont, this.ActualFontSize, this.ActualFontWeight, this.Angle, HorizontalAlignment.Left, VerticalAlignment.Middle);
            }

            List<FeatureText> fearures = intersector.DiscaredIntersection();
            if (fearures != null)
            {
                foreach (FeatureText ft in fearures)
                {
                    rc.DrawText(ft.Position, ft.Text, this.TextColor, this.ActualFont, this.ActualFontSize, this.ActualFontWeight, this.Angle, HorizontalAlignment.Center);
                }
            }

            foreach (IList<ScreenPoint> line in points)
            {
                rc.DrawLine(line, this.ExtraGridlineColor, 1, LineStyle.Dash.GetDashArray());
            }
        }
    }
}
