using Discord.Commands;
using EdgyDiscordBotName.Commands.MemeGenerator;
using EdgyDiscordBotName.Commands.MemeGenerator.Model;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace EdgyDiscordBotName.Commands
{
    [Group("mg")]
    public class MemeGeneratorModule : ModuleBase<SocketCommandContext>
    {
        private string _imgFlipUsername;
        private string _imgFlipPassword;

        public MemeGeneratorModule()
        {
            _imgFlipUsername = ConfigurationManager.AppSettings["ImgFlipUsername"];
            _imgFlipPassword = ConfigurationManager.AppSettings["ImgFlipPassword"];
        }

        [Command("get")]
        [Summary("Gets The Available Memes")]
        public async Task GetAsync()
        {
            var templates = await ImgFlipClient.GetTemplates();
            string message = $"@{Context.Message.Author} ";

            if (templates.Success)
            {
                foreach(var meme in templates.Data.Memes)
                {
                    message += $"{Environment.NewLine}{meme.Name}:{meme.Id}";
                }
            }
            else
            {
                message += $"Something went fucksy wucksie. @Kristof sucks dick.";
            }

            if(message.Length > 2000)
            {
                message = message.Substring(0, 2000);
            }

            await Context.Channel.SendMessageAsync(message);
        }

        [Command("make")]
        [Summary("Creates memes")]
        public async Task MakeAsync(
            [Summary("The id of the meme")] int id,
            [Summary("Quotation enclose, space delimited list of captions.")] params string[] captions)
        {
            if(captions.Count() != 2)
            {
                await Context.Channel.SendMessageAsync($"Give 2 texts only for now ya dingus." + string.Join(",", captions.ToList()));
                return;
            }

            var parameters = new MemeParameters(id, _imgFlipUsername, _imgFlipPassword);
            parameters.Text0 = captions[0];
            parameters.Text1 = captions[1];

            var response = await ImgFlipClient.CreateMeme(parameters);
            await Context.Channel.SendMessageAsync($"@{Context.Message.Author} {response.Data.Url}");
        }
    }
}
