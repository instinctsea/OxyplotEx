using OxyplotEx.Model.Styles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model
{
   public class ElementConfiguration:ICloneable
   {
        public ElementConfiguration(string id)
        {
            OffSet = new PointF(0, 0);
            Id = id;
            this.Visible = true;
            this.LineStyle = eLineStyle.Solid;
            this.LineWidth = 3;
        }

       public string Id { get; private set; }
       private string _ElementName;

       public string ElementName
       {
           get { return _ElementName; }
           set { _ElementName = value; }
       }

       private Color _ElementColor;

       public Color ElementColor
       {
           get { return _ElementColor; }
           set { _ElementColor = value; }
       }

       private int _ElementField;

       public int ElementField
       {
           get { return _ElementField; }
           set { _ElementField = value; }
       }

       private int _ElementStyle;

       public int ElementStyle
       {
           get { return _ElementStyle; }
           set { _ElementStyle = value; }
       }
       private bool _Visible;

       public bool Visible
       {
           get { return _Visible; }
           set { _Visible = value; }
       }

       public string Unit
       {
           get;
           set;
       }

        public PointF OffSet
        {
            get;set;
        }

        public string AxisKey
        {
            get;set;
        }

        public int LineWidth
        {
            get;set;
        }

        public eLineStyle LineStyle
        {
            get;set;
        }
        public string Type { get; set; }
        public object Clone()
        {
            ElementConfiguration element = new ElementConfiguration(this.Id);
            element.AxisKey = this.AxisKey;
            element.ElementColor = this.ElementColor;
            element.ElementField = this.ElementField;
            element.ElementName = this.ElementName;
            element.ElementStyle = this.ElementStyle;
            element.LineStyle = this.LineStyle;
            element.LineWidth = this.LineWidth;
            element.OffSet = this.OffSet;
            element.Unit = this.Unit;
            element.Visible = this.Visible;
            element.Type = this.Type;
            return element;
        }
    }
}
