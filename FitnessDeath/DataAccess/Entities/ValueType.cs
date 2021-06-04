using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessDeath.DataAccess.Entities
{
    public class ValueType : IDatabaseEntity
    {
       public long Number = 0;

        public void Populate(DataRow row)
        {
            if(row[0] != null)
            {
                Number = (Int64)row[0];
            }
        }
    }
}