using Discord.Commands;
using EdgyDiscordBotName.Commands.FitnessDeath;
using System;
using System.Configuration;
using System.Threading.Tasks;
using FitnessDeath.Logic.Response;
using System.Linq;
using Discord.WebSocket;
using System.Collections.Generic;

namespace EdgyDiscordBotName.Commands
{
    [Group("fd")]
    public class FitnessDeathModule : ModuleBase<SocketCommandContext>
    {
        public FitnessDeathModule()
        {
            FitnessDeathClient.BaseUrl = ConfigurationManager.AppSettings["FitnessDeathBaseUrl"];
        }

        [Command("")]
        [Summary("Adds deaths to this user.")]
        public async Task AddAsync(params string[] input)
        {
            string message = $"@{Context.Message.Author} ";
            int parsed;

            if(!String.IsNullOrEmpty(input[0]) && Int32.TryParse(input[0], System.Globalization.NumberStyles.Any, null, out parsed))
            {
                var response = await FitnessDeathClient.AddDeaths(Context.User.Id, parsed);

                if(response.Status == Status.Success)
                {
                    message += $"Added {parsed} death{Pluralise(parsed)} to your suffering.";
                }
                else
                {
                    message += HandleAPIFailure(response.Status, response.Message);
                }
            }
            else if(!String.IsNullOrEmpty(input[0]))
            {
                switch (input[0])
                {
                    case "pay":
                        message = await PayAsync(input);
                        break;
                    case "get":
                        message = await GetAsync(input, false);
                        break;
                    case "bal":
                        message = await GetAsync(input, true);
                        break;
                    case "getall":
                        message = await GetAllAsync();
                        break;
                    default:
                        message = " Invalid command.";
                        break;
                }
            }
            else
            {
                message += "Specify a number of deaths to add to your balance.";
            }

            await Context.Channel.SendMessageAsync(message, isTTS: false);
        }

        [Summary("Pays off exercise from this user.")]
        public async Task<string> PayAsync(string[] numPaid)
        {
            string message = $"@{Context.Message.Author} ";
            int parsed;

            if (!String.IsNullOrEmpty(numPaid[1]) && Int32.TryParse(numPaid[1], System.Globalization.NumberStyles.Any, null, out parsed))
            {
                var response = await FitnessDeathClient.AddPayment(Context.User.Id, parsed);

                if (response.Status == Status.Success)
                {
                    message += $"Paid {parsed} exercise point{Pluralise(parsed)} off your suffering.";
                }
                else
                {
                    message += HandleAPIFailure(response.Status, response.Message);
                }
            }
            else
            {
                message += "Specify a number of exercise points.";
            }

            return message;
        }

        [Summary("Gets a user's balance.")]
        public async Task<string> GetAsync(string[] username, bool self)
        {
            string message = $"@{Context.Message.Author} ";
            ulong userId = 0;

            if(self)
            {
                userId = Context.Message.Author.Id;
            }
            else if (!String.IsNullOrEmpty(username[1]))
            {
                if (username[1].StartsWith("@"))
                {
                    userId = Context.Guild.Users.Where(u => u.Username == username[1]).FirstOrDefault().Id;
                }
                else
                {
                    message += "Include the '@' symbol on the username";
                }
            }
            else
            {
                message += "Specify a valid username prefixed with '@'";
            }

            if (userId != 0)
            {
                var response = await FitnessDeathClient.GetUser(userId);

                if (response.Status == Status.Success)
                {
                    long deaths = response.Data.Deaths;
                    long paid = response.Data.Paid;
                    long total = deaths - paid;
                    message += $"{username[0]}'s owes {total} exercise point{Pluralise(total)}. {username[0]}'s balance is {deaths} death{Pluralise(deaths)} & {paid} death{Pluralise(paid)}.";
                }
                else
                {
                    message += HandleAPIFailure(response.Status, response.Message);
                }
            }
            else
            {
                message += "Couldn't find user 🤷‍";
            }

            return message;
        }

        [Summary("Pays off exercise from this user.")]
        public async Task<string> GetAllAsync()
        {
            string message = $"@{Context.Message.Author} ";
            int parsed;

            var response = await FitnessDeathClient.GetAllUsers();

            var idUserNames = new Dictionary<long, string>();
            long balance = 7;
            long largestDeaths = 6;
            long largestPaid = 4;

            foreach (var user in response.Data)
            {
                var userName = Context.Guild.GetUser((ulong)user.UserId).Username;
                idUserNames.Add(user.UserId, userName);

                var tempLength = user.Deaths - user.Paid;
                if (tempLength.ToString().Length > balance) balance = tempLength;
                if (user.Deaths.ToString().Length > largestDeaths) largestDeaths = user.Deaths;
                if (user.Paid.ToString().Length > largestPaid) largestPaid = user.Paid;
            }

            var temp = idUserNames.Values.Aggregate("", (max, cur) => (max.Length > cur.Length) ? max : cur).Length;
            var longestUsername = (temp > 8 ? temp : 8);
           

            string usernameHeader = new string('-', longestUsername);
            string balanceHeader = new string('-', balance.ToString().Length);
            string deathsHeader = new string('-', largestDeaths.ToString().Length);
            string paidHeader = new string('-', largestPaid.ToString().Length);

            message += $"+{usernameHeader}+{balanceHeader}+{deathsHeader}+{paidHeader}+\n";
            message += $"|Username|Balance|Deaths|Paid|\n";
            message += $"+{usernameHeader}+{balanceHeader}+{deathsHeader}+{paidHeader}+\n";

            foreach(var user in response.Data)
            {
                var username = idUserNames.Where(u => u.Key == user.UserId).First().Value;

                string balPadding = new string(' ', (int)((user.Deaths.ToString().Length - user.Paid.ToString().Length) - balanceHeader.ToString().Length));
                string deathPadding = new string(' ', (int)(user.Deaths.ToString().Length - deathsHeader.ToString().Length));
                string paidPadding = new string(' ', (int)(user.Deaths.ToString().Length - paidHeader.ToString().Length));


                message += $"|{username}|{balPadding}{user.Deaths-user.Paid}|{deathPadding}{user.Deaths}|{paidPadding}{user.Paid}|\n";
            }


            message += $"+{usernameHeader}+{balanceHeader}+{deathsHeader}+{paidHeader}+";

            return message;
        }

        private string Pluralise(int num)
        {
            return num == 1 ? "" : "s";
        }

        private string Pluralise(long num)
        {
            return num == 1 ? "" : "s";
        }

        private string HandleAPIFailure(Status status, string message)
        {
            if(status == Status.Fail)
            {
                return message;
            }
            else
            {
                return "Shit's broken.";
            }
        }
    }
}
