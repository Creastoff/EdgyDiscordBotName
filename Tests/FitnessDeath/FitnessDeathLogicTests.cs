using FitnessDeath.Config;
using FitnessDeath.DataAccess;
using FitnessDeath.Logic;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using Tests.FitnessDeath;
using FitnessDeath.Logic.Response;

namespace FitnessDeaths
{
    [TestClass]
    public class LogicTests
    {
        MySqlConfig config = null;
        Database database = null;

        [TestInitialize]
        public void TestInit()
        {
            config = new MySqlConfig();
            config.Database = "fitnessdeath";
            config.Password = "toor";
            config.Server = "localhost";
            config.Uid = "root";

            database = new Database(config);


            Helpers.TearDownDB(config);
        }

        [TestMethod]
        public void PayAmountToTotalNegative()
        {
            var bl = new BusinessLogic(database);
            bl.CreateUser(1);
            var response = bl.AddPayment(1, 1);

            Assert.IsTrue(response.Status == Status.Fail && response.Message == Messages.INVALID_PAYMENT_NEGATIVE);
        }

        [TestMethod]
        public void PayAmountToTotal0ForNonCreatedAccount()
        {
            var bl = new BusinessLogic(database);
            var response = bl.AddPayment(122853490044305413, 1);

            Assert.IsTrue(response.Status == Status.Fail && response.Message == Messages.INVALID_PAYMENT_NONE_OWED);
        }

        [TestMethod]
        public void PayAmountToTotalPositive()
        {
            var bl = new BusinessLogic(database);
            bl.CreateUser(1);
            bl.AddDeaths(1, 1);
            var response = bl.AddPayment(1, 1);

            Assert.IsTrue(response.Status == Status.Success);
        }
    }
}
