using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgyDiscordBotName.Core
{
    public class Config
    {
        public string EnvironmentVariableName { get; set; }
        public EnvironmentVariableTarget EnvironmentTargetName { get; set; }

        public Config()
        {
            EnvironmentVariableName = ConfigurationManager.AppSettings["ApiKeyEnvironmentVariableName"];

            var variableTargetName = ConfigurationManager.AppSettings["EnvironmentTarget"];
            EnvironmentTargetName = (EnvironmentVariableTarget)Enum.Parse(typeof(EnvironmentVariableTarget), variableTargetName, true);
        }

        public string GetToken()
        {
#nullable enable
            string? token = null;
#nullable disable

            token = Environment.GetEnvironmentVariable(EnvironmentVariableName, EnvironmentTargetName);

            if (token == null)
            {
                throw new Exception("Unable to get token.");
            }

            return token;
        }
    }
}
