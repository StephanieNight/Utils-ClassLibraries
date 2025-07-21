using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension
{
    public static class DateOnlyExtension
    {        
        public static DateOnly? GetNullableDate(this DateOnly date)
        {
            return date > DateOnly.MinValue ? date : null;
        }
    }
}
