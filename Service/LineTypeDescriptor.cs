using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.Service
{
    public class LineTypeDescriptor
    {
        public LineTypeDescriptor(string name, int lineType)
        {
            this.Name = name;
            this.LineType = lineType;
        }

        public int LineType;
        public string Name;

        public override string ToString()
        {
            return Name;
        }
    }
}
