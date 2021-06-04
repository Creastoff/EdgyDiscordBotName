using FitnessDeath.Config;
using FitnessDeath.DataAccess;
using FitnessDeath.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessDeath.Controllers
{
    [ApiController]
    [Route("")]
    public class FitnessDeathController : ControllerBase
    {
        private readonly IOptions<MySqlConfig> _appSettings;
        private Database _database;
        private BusinessLogic _businessLogic;

        public FitnessDeathController(IOptions<MySqlConfig> appSettings)
        {
            _appSettings = appSettings;
            _database = new Database(_appSettings.Value);
            _businessLogic = new BusinessLogic(_database);
        }

        [HttpGet]
        public string Get(ulong uid)
        {
            return JsonConvert.SerializeObject(_businessLogic.GetUser(uid));
        }

        [HttpGet]
        [Route("/AllUsers")]
        public string GetAllUsers()
        {
            return JsonConvert.SerializeObject(_businessLogic.GetAllUsers());
        }

        [HttpGet]
        [Route("/DoesUserExist")]
        public string DoesUserExist(ulong uid)
        {
            return JsonConvert.SerializeObject(_businessLogic.DoesUserExist(uid));
        }

        [HttpGet]
        [Route("/CreateUser")]
        public string CreateUser(ulong uid)
        {
            return JsonConvert.SerializeObject(_businessLogic.CreateUser(uid));
        }

        [HttpGet]
        [Route("/AddPayment")]
        public string AddPayment(ulong uid, int amount)
        {
            return JsonConvert.SerializeObject(_businessLogic.AddPayment(uid, amount));
        }

        [HttpGet]
        [Route("/AddDeaths")]
        public string AddDeaths(ulong uid, int amount)
        {
            return JsonConvert.SerializeObject(_businessLogic.AddDeaths(uid, amount));
        }
    }
}
