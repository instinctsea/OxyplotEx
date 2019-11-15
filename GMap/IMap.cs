using System;
using System.Collections.Generic;
using System.Drawing;

namespace OxyplotEx.GMap
{
    public interface IMap
    {
        int SeriesCount { get; }
        object Content { get; }
        void Clear();
        void ClearSeries();
        void ClearAxes();
        void ResetAllAxes();
        void RefreshView();
        void Perform();
        void Prefer();
        void SetTitle(string title);
        void InitlizeLegend();
        IAxis FindAxis(string name);
        event EventHandler<SeriesVisibleChangedEventArgs> SeriesVisibleChanged;
        ISeries FindSeries(string id);
        ISeries FindSeries(Predicate<ISeries> match);
        IEnumerable<ISeries> GetAllSeries();
        IEnumerable<IAxis> GetAllAxes();
        void AddPoint(PointModel pm);
        void AddRange(IEnumerable<PointModel> pms);
        void AddAxis(IAxis axis);
        void AddSeries(ISeries series);
        void SetGroupVisible(string id,bool visible);
        void InverseData();
        void RemoveSeries(string id);
        void RemoveSeries(ISeries se);
        void SortGroupSeries(Action<IEnumerable<ISeries>> sortgroup);
        Image CopyImage();
    }
}
