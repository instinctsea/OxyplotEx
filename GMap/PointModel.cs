using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.GMap
{
    public class PointModel
    {
        public PointModel(string name,double index,string value,int indexcount)
        {
            this.Name = name;
            this.Index = index;
            this.Value = value;
            this.OriginValue = value;
            this.IndexCount = indexcount;
        }

        public string Name
        {
            get;set;
        }

        public double Index
        {
            get;set;
        }

        public string Value
        {
            get;set;
        }

        public string OriginValue
        {
            get;set;
        }

        public int IndexCount
        {
            get;set;
        }

        public virtual void InverseIndex()
        {
            Index = IndexCount - 1 - Index;
        }

        public static int SortAsc(PointModel l, PointModel r)
        {
            if (l.Index > r.Index)
                return 1;
            else if (l.Index < r.Index)
                return -1;
            return 0;
        }
    }
}
