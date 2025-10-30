using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.MISAAttributes
{
    public class MISATableNameAttribute : Attribute
    {
        public string TableName { get; set; }
        public MISATableNameAttribute(string tableName)
        {
            TableName = tableName;
        }
    }
}
