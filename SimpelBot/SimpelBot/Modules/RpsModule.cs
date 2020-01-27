using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace SimpelBot.Modules
{
    public class RpsModule : ModuleBase<SocketCommandContext>
    {
        [Command("rps")]
        [Summary("Rock Paper Scissors minigame")]
        public async Task RpsAsync(string userInput)
        {
            await Context.Message.DeleteAsync();

            var rndm = new Random().Next(0, 3);
            string[] rps = {"rock", "paper", "scissors"};
            userInput = userInput.ToLower();

            var botInput = rps[rndm];

            if (rps.Contains(userInput.ToLower()))
            {
                if (botInput.Equals(userInput))
                {
                    await Context.Channel.SendMessageAsync("That's a draw!");
                }
                else if (botInput.Equals(rps[0]) && userInput.Equals(rps[2]) ||
                         botInput.Equals(rps[1]) && userInput.Equals(rps[0]) ||
                         botInput.Equals(rps[2]) && userInput.Equals(rps[1]))
                {
                    await Context.Channel.SendMessageAsync($"I had {botInput}, that means I win!");
                }
                else if (botInput.Equals(rps[0]) && userInput.Equals(rps[1]) ||
                         botInput.Equals(rps[1]) && userInput.Equals(rps[2]) ||
                         botInput.Equals(rps[2]) && userInput.Equals(rps[0]))
                {
                    await Context.Channel.SendMessageAsync($"OOF! I had {botInput}, that means you win!");
                }
            }
            else
            {
                var msg = await Context.Channel.SendMessageAsync("Hi there, this is Rock Paper Scissors.");
                await Task.Delay(4000);
                await msg.DeleteAsync();
            }
        }
    }
}
