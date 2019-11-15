using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model.TimeSeries
{
    public class DataDescriptor
    {
        public DataDescriptor(string id, string name, int dataColumn)
        {
            this.Id = id;
            this.Name = name;
            this.DataColumn = dataColumn;
            this.PairDataColumn = -1;
            this.LineType = GMap.SeriesFactory.Line;
            this.FontSize = 14;
            this.IsNumber = true;
        }

        public string Id { get; private set; }
        public string Name { get; set; }
        public int DataColumn { get; set; }
        public int LineType { get; set; }
        public string FontFamily { get; set; }
        public bool IsNumber { get; set; }
        public Point Offset { get; set; }
        public int PairDataColumn { get; set; }
        public float FontSize { get; set; }
        public bool HasCouple
        {
            get { return PairDataColumn >= 0; }
        }

        public DataDescriptorCollection Parent
        {
            get;
            internal set;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
