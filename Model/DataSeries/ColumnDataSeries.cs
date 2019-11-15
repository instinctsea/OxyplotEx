using Module.MICAPSDataChart.Model.CoordinateAxises;
using Module.MICAPSDataChart.Model.Styles;
using OxyPlot;
using OxyPlot.Series;
/*
 * Copyright (c) 2014-2015 jiayongqiang. All rights reserved";
 * CLR version: 4.0.30319.18444"
 * File name:   ColumnDataSeries.cs "
 * Date:        2015/8/5 18:23:52 
 * Author :  jiayongqiang Developer
 * Description: 
	
 * History:  created by jiayongqiang 2015/8/5 18:23:52
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Module.MICAPSDataChart.Model.Styles;

namespace Module.MICAPSDataChart.Model.DataSeries
{
    class ColumnDataSeries:ColumnSeries,ISeriesTag,IXYAxisBinding,ISeqDataScheduler,ISeriesStyle,IValueRange,IReverseData
    {
        private Dictionary<int, int> _seqs = new Dictionary<int, int>();
        public ColumnDataSeries():base()
        {
            ColumnWidth = 0.3;
            StrokeThickness = 1;
            FillColor = OxyColors.Yellow;
            ResetVerticalValueRange();
        }
        public string Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int SumSeq
        {
            get;
            set;
        }

        public string XAxisBingdingKey
        {
            get { return base.XAxisKey; }
            set 
            { 
                base.XAxisKey = value;
            }
        }

        public string YAxisBingdingKey
        {
            get { return base.YAxisKey; }
            set
            {
                base.YAxisKey = value;
            }
        }

        public void AddSeqData(SeqData data)
        {
            double value;
            double.TryParse(data.Y,out value);
            int seq = value == Helper.NoData ? -1 : data.Seq;
            value = value == Helper.NoData ? double.NaN : value;
            //SumSeq++;
            if (!_seqs.ContainsKey(seq))
            {
                _seqs[seq] = seq;
                base.Items.Add(new ColumnItem(value == Helper.NoData ? double.NaN : value, value == Helper.NoData ? -1 : data.Seq));
                UpdateVerticalValueRange(data);
            }
            
        }

        public void AddRange(IEnumerable<SeqData> datas)
        {
            foreach (SeqData data in datas)
            {
                this.AddSeqData(data);
            }
        }

        public System.Drawing.Color MainColor
        {
            get
            {
                if (Theme != null)
                    this.FillColor = Convertor.ConvertColorToOxyColor(Theme.GetThemeColor());

                return Convertor.ConvertOxyColorToColor(this.FillColor);
            }
            set
            {
                this.FillColor = Convertor.ConvertColorToOxyColor(value);
                if (Theme != null)
                {
                    Theme.SetThemeColor(value);
                }
            }
        }

        public bool Visible
        {
            get
            {
                return base.IsVisible;
            }
            set
            {
                base.IsVisible = value;
            }
        }

        public string SeriesTitle
        {
            get
            {
                return base.Title;
            }
            set
            {
                base.Title = value;
            }
        }

        public double ValueMaximum
        {
            get;
            set;
        }

        public double ValueMinimum
        {
            get;
            set;
        }
        protected new void UpdateValidData()
        {
            this.ValidItems = new List<BarItemBase>();
            this.ValidItemsIndexInversion = new Dictionary<int, int>();
            //var count = this.GetCategoryAxis();
            try
            {
                this.GetCategoryAxis();
            }
            catch (Exception)
            {

                return;
            }
      
            var categories = this.GetCategoryAxis().Labels.Count;
            var valueAxis = this.GetValueAxis();

            int i = 0;
            foreach (var item in this.GetItems())
            {
                var barSeriesItem = item as BarItemBase;

                int category_index = item.CategoryIndex;
                if (barSeriesItem != null &&category_index>=0&& category_index < categories
                    && valueAxis.IsValidValue(barSeriesItem.Value))
                {
                    this.ValidItemsIndexInversion.Add(this.ValidItems.Count, i);
                    this.ValidItems.Add(barSeriesItem);
                }

                i++;
            }
        }
        public override void Render(IRenderContext rc,PlotModel model1)
        {
            PlotModel model = this.PlotModel;
            if (Theme != null)
            {
                OxyColor color = Convertor.ConvertColorToOxyColor(Theme.GetThemeColor());
                this.FillColor = color;
            }
            //base.Render(rc, model);
            UpdateValidData();
            this.ActualBarRectangles = new List<OxyRect>();

            if (this.ValidItems == null || this.ValidItems.Count == 0)
            {
                return;
            }

            var clippingRect = this.GetClippingRect();
            var categoryAxis = this.GetCategoryAxis();

            var actualBarWidth = this.GetActualBarWidth();
            var stackIndex = this.IsStacked ? categoryAxis.GetStackIndex(this.StackGroup) : 0;
            for (var i = 0; i < this.ValidItems.Count; i++)
            {
                var item = this.ValidItems[i];
                var categoryIndex = this.ValidItems[i].CategoryIndex;
                if (categoryIndex < 0)
                    continue;
                var value = item.Value;

                // Get base- and topValue
                var baseValue = double.NaN;
                if (this.IsStacked)
                {
                    baseValue = categoryAxis.GetCurrentBaseValue(stackIndex, categoryIndex, value < 0);
                }

                if (double.IsNaN(baseValue))
                {
                    baseValue = this.BaseValue;
                }

                var topValue = this.IsStacked ? baseValue + value : value;

                // Calculate offset
                double categoryValue;
                if (this.IsStacked)
                {
                    categoryValue = categoryAxis.GetCategoryValue(categoryIndex, stackIndex, actualBarWidth);
                }
                else
                {
                    categoryValue = categoryIndex - 0.5 + categoryAxis.GetCurrentBarOffset(categoryIndex);
                }

                if (this.IsStacked)
                {
                    categoryAxis.SetCurrentBaseValue(stackIndex, categoryIndex, value < 0, topValue);
                }

                var rect = this.GetRectangle(baseValue, topValue, categoryValue, categoryValue + actualBarWidth);
                this.ActualBarRectangles.Add(rect);

                this.RenderItem(rc, clippingRect, topValue, categoryValue, actualBarWidth, item, rect);

                if (this.LabelFormatString != null)
                {
                    this.RenderLabel(rc, clippingRect, rect, value, i);
                }

                if (!this.IsStacked)
                {
                    categoryAxis.IncreaseCurrentBarOffset(categoryIndex, actualBarWidth);
                }
            }
        }

        private void UpdateVerticalValueRange(SeqData data)
        {
            double y;
            if (double.TryParse(data.Y, out y) && y != Helper.NoData)
            {
                if (y > ValueMaximum)
                    ValueMaximum = y;
                if (y < ValueMinimum)
                    ValueMinimum = y;
            }
        }

        private void ResetVerticalValueRange()
        {
            ValueMaximum = double.MinValue;
            ValueMinimum = 0;
        }

        protected override double GetActualBarWidth()
        {
            return ColumnWidth;
        }

        public void ReverseData()
        {
            List<ColumnItem> items = new List<ColumnItem>();
            foreach (ColumnItem item in Items)
            {
                int seq = (int)item.CategoryIndex;
                seq = seq < 0 ? seq : SumSeq - 1 - seq;
                items.Add(new ColumnItem(item.Value,seq));
            }

            Items.Clear();
            Items.AddRange(items);
        }


        public void Clear()
        {
            this.Items.Clear();
        }


        public MICAPSDataChart.Model.Themes.ITheme Theme
        {
            get;
            set;
        }

        public int LineWidth
        {
            get;set;
        }

        public eLineStyle SeriesLineStyle
        {
            get;set;
        }
    }
}
