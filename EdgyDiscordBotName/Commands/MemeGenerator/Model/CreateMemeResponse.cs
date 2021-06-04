using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgyDiscordBotName.Commands.MemeGenerator.Model
{
    public class CreateMemeResponse
    {
        public bool Success { get; set; }
        public CreateMemeResponseData Data { get; set; }
        public string Error_Message { get; set; }

        public class CreateMemeResponseData
        {
            public string Url { get; set; }
            public string Page_Url { get; set; }
        }
    }
}
