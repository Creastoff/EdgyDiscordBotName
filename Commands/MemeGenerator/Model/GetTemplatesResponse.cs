using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgyDiscordBotName.Commands.MemeGenerator.Model
{
    public class GetTemplatesResponse
    {
        public bool Success { get; set; }
        public GetTemplatesResponseData Data { get; set; }

        public class GetTemplatesResponseData
        {
            public List<MemeTemplate> Memes { get; set; }

            public class MemeTemplate
            {
                public string Id { get; set; }
                public string Name { get; set; }
                public string Url { get; set; }
                public int Width { get; set; }
                public int Height { get; set; }
                public int Box_Count { get; set; }
            }
        }
    }
}
