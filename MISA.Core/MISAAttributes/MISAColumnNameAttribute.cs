using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.MISAAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MISAColumnNameAttribute : Attribute
    {
        public string ColumnName { get; }

        public MISAColumnNameAttribute(string columnName)
        {
            ColumnName = columnName;
        }
    }
}
