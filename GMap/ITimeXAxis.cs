using OxyplotEx.Model.Time;
using System.Collections.Generic;

namespace OxyplotEx.GMap
{
    interface ITimeXAxis:IAxis,IGetXLabel, IInverseData
    {
        TimeLinesCollection TimeLines { get; set; }
        List<TimeLine> Grids { get; set; }
        void AddLabel(TimeModel time);
    }
}
