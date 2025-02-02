using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager.Model
{
   public  class DbDescription
    {
        public string SchemaName { get; set; }
        public string ColumnName { get; set; }
        public string TableName { get; set; }
        public string ExtendedProperty { get; set; }
        public string ExtendedPropertyValue { get; set; }
    }
}
