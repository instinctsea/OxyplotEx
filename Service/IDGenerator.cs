using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.Service
{
    public static class IDGenerator
    {
        public static string CreateID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
