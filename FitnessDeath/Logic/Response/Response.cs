using FitnessDeath.DataAccess.Entities;
using FitnessDeath.Logic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessDeath.Logic.Response
{
    public class Response<T>
    {
        public Status Status { get; set; } = Status.Success;
        public T Data { get; set; }
        public string Message { get; set; }

        public Response()
        {
            Status = Status.Success;
        }
    }
}