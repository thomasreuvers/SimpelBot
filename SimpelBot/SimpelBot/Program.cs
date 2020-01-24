using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using SimpelBot.Models;

namespace SimpelBot
{
    internal class Program
    {
        private DiscordSocketClient _client;

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();

            // Log clients message to console
            _client.Log += Log;

            await _client.LoginAsync(TokenType.Bot, LoadConfig().ClientSecret);
            await _client.StartAsync();

            var cmdHandler = new CommandHandler(_client, new CommandService(), LoadConfig().ClientPrefix);
            await cmdHandler.InstallCommandsAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }




        /*
         * This method will load up the bot config and populate the ConfigModel class
         */
        public static ConfigModel LoadConfig()
        {
            using var file = File.OpenText(@"../../../Resources/Config/SimpelBot.json");

            var serializer = new JsonSerializer();
            return (ConfigModel)serializer.Deserialize(file, typeof(ConfigModel));
        }
    }
}
