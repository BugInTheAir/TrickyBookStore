using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrickyBookStore.Services.Utils
{
    public static class DoubleExtensions
    {
        public static double Round(this double value, int places)
        {
            long factor = (long)Math.Pow(10, places);
            value = value * factor;
            double tmp = Math.Round(value);
            return (double)tmp / factor;
        }
    }
}
