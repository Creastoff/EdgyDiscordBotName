using FitnessDeath.Config;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.FitnessDeath
{
    public static class Helpers
    {
        public static void TearDownDB(MySqlConfig config)
        {
            var conn = new MySqlConnection(config.GenerateConnectionString());

            using (conn)
            {
                using(var cmd = new MySqlCommand("DELETE FROM users", conn))
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static string baseUrl = "https://kristof.servebeer.com/fitnessdeath";
    }
}
