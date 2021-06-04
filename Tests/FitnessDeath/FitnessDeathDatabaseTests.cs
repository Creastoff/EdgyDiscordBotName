using FitnessDeath.Config;
using FitnessDeath.DataAccess;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using Tests.FitnessDeath;

namespace FitnessDeaths
{
    [TestClass]
    public class DatabaseTests
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
        }

        [TestMethod]
        public void CanInitialiseConnection()
        {
            var db = new Database(config);
        }

        [TestMethod]
        public void CreateUserSuccess()
        {
            var db = new Database(config);
            db.CreateUser(1);
        }

        [TestMethod]
        [ExpectedException(typeof(MySqlException))]
        public void CreateUserFail()
        {
            var db = new Database(config);
            db.CreateUser(1);
            db.CreateUser(1);
        }

        [TestMethod]
        public void UserDoesNotExist()
        {
            var db = new Database(config);

            Assert.IsFalse(db.DoesUserExist(1));
        }

        [TestMethod]
        public void UserExists()
        {
            var db = new Database(config);
            db.CreateUser(1);

            Assert.IsTrue(db.DoesUserExist(1));
        }

        [TestMethod]
        public void AddDeaths()
        {
            var db = new Database(config);
            db.CreateUser(1);

            var user = db.GetUser(1);
            Assert.IsTrue(user.Deaths == 0);

            db.AddDeaths(1, 5);
            user = db.GetUser(1);
            Assert.IsTrue(user.Deaths == 5);
        }

        [TestMethod]
        public void AddPayment()
        {
            var db = new Database(config);
            db.CreateUser(1);

            var user = db.GetUser(1);
            Assert.IsTrue(user.Deaths == 0 && user.Paid == 0);

            db.AddPayment(1, 5);
            user = db.GetUser(1);
            Assert.IsTrue(user.Paid == 5);
        }

        [TestMethod]
        public void GetAllUsers()
        {
            var db = new Database(config);
            db.CreateUser(1);
            db.CreateUser(2);

            var user = db.GetAllUsers();

            Assert.IsTrue(user.Count == 2);
        }
    }
}
