using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model
{
    class FeatureText
    {
        public FeatureText(string text, ScreenPoint sp, OxySize size,ScreenPoint sourcePosition)
        {
            Text = text;
            Position = sp;
            Size = size;
            this.SourcePosition = sourcePosition;
        }

        public string Text;
        public ScreenPoint Position;
        public ScreenPoint SourcePosition;
        public OxySize Size;
    }
}
