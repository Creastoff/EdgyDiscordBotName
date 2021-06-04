using FitnessDeath.Logic.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessDeath.DataAccess.Entities
{
    public class User : IDatabaseEntity, IBusinessLogicEntity
    {
        public long UserId { get; set; }
        public long Deaths { get; set; }
        public long Paid { get; set; }

        public void Populate(DataRow row)
        {
            UserId = (Int64)row[0];
            Deaths = (Int32)row[1];
            Paid = (Int32)row[2];
        }
    }
}