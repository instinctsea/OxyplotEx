using CMA.MICAPS.ReactNative.Charts;
using DevComponents.DotNetBar;
using Module.MICAPSDataChart.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Module.MICAPSDataChart.View;

namespace Module.MICAPSDataChart.GMap
{
    class EChartMapView :EChart, IMap
    {
        public EChartMapView():base()
        {
            this.IsLegendVisible = true;
        }

        event EventHandler<SeriesVisibleChangedEventArgs> IMap.SeriesVisibleChanged
        {
            add { }
            remove { }
        }

        List<IAxis> _axes = new List<IAxis>();
        List<ISeries> _serieses = new List<ISeries>();
        public object Content => this;

        public void AddAxis(IAxis axis)
        {
            if (FindAxis(axis.Name) != null)
                throw new Exception(axis.Name + " is already exist!");
            _axes.Add(axis);
            EChartAxis echart_axis = axis as EChartAxis;
            base.AddAxis(echart_axis);
        }

        public void AddPoint(PointModel pm)
        {
            ISeries se = FindSeries(pm.Name);
            se.AddPoint(pm);
            base.UpdateSeries(se as EChartSeries);
        }

        public void AddRange(IEnumerable<PointModel> pms)
        {
            foreach (PointModel pt in pms)
                this.AddPoint(pt);
        }

        public void AddSeries(ISeries series)
        {
            _serieses.Add(series);
            base.AddSeries(series as EChartSeries);
        }

        public new void Clear()
        {
            _serieses.Clear();
            _axes.Clear();
            base.Clear();
        }

        public new void ClearAxes()
        {
            _axes.Clear();
            base.ClearAxes();
        }

        public void ClearSeries()
        {
            _serieses.Clear();
            base.ClearSerieses();
        }

        public Image CopyImage()
        {
            Bitmap bitmap = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bitmap, new Rectangle(0, 0, this.Width, this.Height));
            return bitmap;
        }

        public new IAxis FindAxis(string name)
        {
            return _axes.Find(axis => axis.Name == name);
        }

        public new ISeries FindSeries(string id)
        {
            return _serieses.Find(se => se.Id == id);
        }

        public ISeries FindSeries(Predicate<ISeries> match)
        {
            return _serieses.Find(match);
        }

        public IEnumerable<IAxis> GetAllAxes()
        {
            return _axes;
        }

        public IEnumerable<ISeries> GetAllSeries()
        {
            return _serieses;
        }

        public void InitlizeLegend()
        {
            
        }

        public void InverseData()
        {
            //
            foreach (IAxis axis in _axes)
            {
                if (axis is IInverseData)
                {
                    ((IInverseData)axis).InverseData();
                }
            }

            foreach (ISeries se in _serieses)
            {
                if (se is IInverseData)
                {
                    ((IInverseData)se).InverseData();
                }
            }
        }

        public void Perform()
        {
            
        }

        public void Prefer()
        {
            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            this.RefreshView();
        }

        public new void RefreshView()
        {
            RefreshTheme();
            base.RefreshView();
        }

        void RefreshTheme()
        {
            base.RefreshTheme(StartUp.Current.GetService<ThemeModesManager>().CurrentReactNativeMode);
        }

        public void RemoveSeries(string id)
        {
            for (int i = 0; i < _serieses.Count; i++)
            {
                if (_serieses[i].Id == id) {
                    _serieses.Remove(_serieses[i]);
                    break;
                }
            }
            base.DeleteSeries(id);
        }

        public void RemoveSeries(ISeries se)
        {
            RemoveSeries(se.Id);
        }

        public void ResetAllAxes()
        {
            
        }

        public void SetGroupVisible(string id, bool visible)
        {
            
        }

        public new void SetTitle(string title)
        {
            base.SetTitle(title);
        }

        public void SortGroupSeries(Action<IEnumerable<ISeries>> sortgroup)
        {
            
        }
    }
}
