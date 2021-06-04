using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessDeath.DataAccess.Entities
{
    public interface IDatabaseEntity
    {
        void Populate(DataRow row);
    }
}
