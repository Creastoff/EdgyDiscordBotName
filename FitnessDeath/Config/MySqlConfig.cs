using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessDeath.Config
{
    public class MySqlConfig
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string Uid { get; set; }
        public string Password { get; set; }

        public string GenerateConnectionString()
        {
            return $"server={Server};user={Uid};database={Database};password={Password};";
        }
    }
}
