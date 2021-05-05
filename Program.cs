using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Configuration;
using System.Threading.Tasks;
using EdgyDiscordBotName.Core;

namespace EdgyDiscordBotName
{
    public class Program
    {
        public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        private Config _config;
        private Logger _logger;
        private DiscordSocketClient _client;
        private CommandHandler _commandHandler;

        public async Task MainAsync()
        {
            try
            {
                //Initialise objects
                _config = new Config();
                _logger = new Logger();
                _client = new DiscordSocketClient();
                _commandHandler = new CommandHandler(_client);
                await _commandHandler.InstallCommandsAsync();

                //Bind event handlers
                _client.Log += _logger.Log;
                _client.MessageReceived += _commandHandler.HandleCommandAsync;

                //Sign in
                await _client.LoginAsync(TokenType.Bot, _config.GetToken());
                await _client.StartAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Issue when starting:");
                Console.Write(ex);
            }
            await Task.Delay(-1);
        }
    }
}
