using CMA.MICAPS.GMap.Layers;
using CMA.MICAPS.GMap.Presenter;
using CMA.MICAPS.Infrastructures;
using CMA.MICAPS.IO;
using CMA.MICAPS.Resolver.Time;
using Module.MICAPSDataChart.GMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module.MICAPSDataChart.View;
using Module.MICAPSDataChart.Model.Web;

namespace Module.MICAPSDataChart.Service
{
    class GetDataManager
    {
        //private IMapManager _mapManager;
        private List<Type4DataModel> _dataForWeb = new List<Type4DataModel>();
        public bool IsOverLay { get; set; }

        public GetDataManager()
        {
            //_mapManager = ServiceLocator.Current.GetInstance<IMapManager>();
        }
        public List<Type4DataModel> GetData()
        {
            
            return _dataForWeb;
        }
        public List<Type4DataModel> GetDataForOverLay()
        {
            return _dataForWeb;
        }
        public List<Type4DataModel> UpdateTime(string startDate, string endDate, string hour)
        {
            //new 会导致重新new presenter 数据会重置
            //new TimeLineWebView().OnTimeChanged(startDate, endDate, hour);

            return _dataForWeb;
        }
        public List<Type4DataModel> UpdateForecast(string forecastHour)
        {
            //new TimeLineWebView().ForecastHourChanged(int.Parse(forecastHour));

            return _dataForWeb;
        }


        public void ReorganizeData(TimeDataManager times, List<PointModel> pts )
        {
            IsOverLay = false;
            if(!IsOverLay)
                _dataForWeb.Clear();
            for(int i = 0; i < times.Count; i++)
            {
                bool isFind = false;
                var model = new Type4DataModel();
                model.DateTime = times[i].CompsiteTime.ToString("MM-dd HH");
                model.Station = pts.Count>0? pts[0].Name:"";
                foreach (var pt in pts)
                {
                    if(pt.Index == i)
                    {
                        model.DataValue = pt.Value;
                        isFind = true;
                        break;
                    }
                }
                if (!isFind)
                {
                    model.DataValue = "-1";
                }
                _dataForWeb.Add(model);
            }
        }
        
        


    }
}
