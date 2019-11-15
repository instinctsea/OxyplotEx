using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model.TimeSeries
{
    public class DataDescriptorCollection
    {
        public DataDescriptorCollection(string id,bool isModel)
        {
            Datas = new Collection<DataDescriptor>();
            this.IsModel = isModel;
        }

        /// <summary>
        /// 是否是模式数据
        /// </summary>
        public bool IsModel
        {
            get;
            private set;
        }

        /// <summary>
        /// 数据集合
        /// </summary>
        public Collection<DataDescriptor> Datas
        {
            get;
            private set;
        }
    }
}
