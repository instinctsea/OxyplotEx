
using OxyplotEx.Model.Styles;
/*
 * Copyright (c) 2014-2015 jiayongqiang. All rights reserved";
 * CLR version: 4.0.30319.18444"
 * File name:   ElementRenderTypeMapping.cs "
 * Date:        2015/8/10 17:28:06 
 * Author :  jiayongqiang Developer
 * Description: 
	
 * History:  created by jiayongqiang 2015/8/10 17:28:06
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model
{
    public class ElementRenderTypeMapping
    {
        private Dictionary<string,eRenderType> _element_axis_mappings = new Dictionary<string,eRenderType>();

        public ElementRenderTypeMapping()
        {
            _element_axis_mappings.Add(ElementNames.Cloud, eRenderType.Axis);
            _element_axis_mappings.Add(ElementNames.Default, eRenderType.Series);
            _element_axis_mappings.Add(ElementNames.Td, eRenderType.Series);
            _element_axis_mappings.Add(ElementNames.Temperature, eRenderType.Series);
            _element_axis_mappings.Add(ElementNames.Wind, eRenderType.Axis);
            _element_axis_mappings.Add(ElementNames.Var3Pressure, eRenderType.Axis);
            _element_axis_mappings.Add(ElementNames.Visible, eRenderType.Axis);

            _element_axis_mappings.Add(ElementNames.Pressure, eRenderType.Series);
            _element_axis_mappings.Add(ElementNames.Rain, eRenderType.Series); 
        }

        public bool Contains(string elementName)
        {
            return _element_axis_mappings.ContainsKey(elementName);
        }

        public eRenderType this[string name]
        {
            get
            {
                eRenderType render_type;
                _element_axis_mappings.TryGetValue(name, out render_type);
                return render_type;
            }
        }

        public void Clear()
        {
            _element_axis_mappings.Clear();
        }
    }
}
