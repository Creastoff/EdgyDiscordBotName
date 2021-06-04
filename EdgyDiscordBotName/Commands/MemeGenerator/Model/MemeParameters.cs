using System.Collections.Generic;

namespace EdgyDiscordBotName.Commands.MemeGenerator.Model
{
    public class MemeParameters
    {
        public MemeParameters(int id, string username, string password)
        {
            Template_Id = id;
            Username = username;
            Password = password;
        }

        public int Template_Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Text0 { get; set; }
        public string Text1 { get; set; }
        public List<Box> Boxes { get; set; }

        public class Box
        {
            public Box(string text)
            {
                Text = text;
            }

            public string Text { get; set; }
        }
    }
}
