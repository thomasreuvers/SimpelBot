using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace SimpelBot.Modules
{
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        [Summary("helps the user")]
        public async Task HelpAsync()
        {
            await Context.Message.DeleteAsync();
            await Context.User.SendMessageAsync($"Hi there {Context.User},\n Help text here");
        }
    }
}
