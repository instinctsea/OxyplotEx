
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.Service
{
    class SeriesServiceProvider
    {
        private Dictionary<object, object> _services;
        public SeriesServiceProvider()
        {
            _services = new Dictionary<object, object>();
        }

        public void RegisterService(Type t,object instance)
        {
            _services[t] = instance;
        }

        public void UnRegisterService(Type t)
        {
            if (_services.ContainsKey(t))
                _services.Remove(t);
        }

        public object this[Type t]
        { 
            get
            {
                return _services[t];
            }
        }
    }
}
