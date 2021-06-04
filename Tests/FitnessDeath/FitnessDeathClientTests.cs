using EdgyDiscordBotName.Commands;
using EdgyDiscordBotName.Commands.FitnessDeath;
using FitnessDeath.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.FitnessDeath;

namespace FitnessDeaths
{
    [TestClass]
    public class ClientTests
    {
        MySqlConfig config = null;

        [TestInitialize]
        public void TestInit()
        {
            config = new MySqlConfig();
            config.Database = "fitnessdeath";
            config.Password = "toor";
            config.Server = "localhost";
            config.Uid = "root";

            Helpers.TearDownDB(config);

            FitnessDeathClient.BaseUrl = "https://kristof.servebeer.com/fitnessdeath";
        }

        [TestMethod]
        public async Task UserDoesNotExist()
        {
            var response = await FitnessDeathClient.GetUser(1);

            Assert.IsTrue(response.Status == FitnessDeath.Logic.Response.Status.Fail);
        }

        [TestMethod]
        public async Task CreateUserThenPerformActions()
        {
            await FitnessDeathClient.CreateUser(1);
            await FitnessDeathClient.AddDeaths(1, 1);
            await FitnessDeathClient.AddPayment(1, 1);

            var response = await FitnessDeathClient.GetUser(1);

            Assert.IsTrue(response.Status == FitnessDeath.Logic.Response.Status.Success && response.Data.Paid == response.Data.Deaths);
        }

        [TestMethod]
        public async Task CreateTwoUsersGetBoth()
        {
            await FitnessDeathClient.CreateUser(1);
            await FitnessDeathClient.CreateUser(2);

            var response = await FitnessDeathClient.GetAllUsers();

            Assert.IsTrue(response.Status == FitnessDeath.Logic.Response.Status.Success && response.Data.Count == 2);
        }
    }
}
