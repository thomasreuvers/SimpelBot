using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.VisualBasic;

namespace SimpelBot.Modules
{
    public class ClearModule : ModuleBase<SocketCommandContext>
    {
        [Command("clear")]
        [Summary("helps the user")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task ClearAsync(int amount)
        {
            if (amount < 0)
            {
                var msg = await Context.Channel.SendMessageAsync("You cannot clear a negative amount of messages!");
                await Task.Delay(3000);
                await msg.DeleteAsync();
            }
            else
            {
                var messages = await Context.Channel.GetMessagesAsync(amount + 1).FlattenAsync();

                foreach (var message in messages)
                {
                    await Context.Channel.DeleteMessageAsync(message);
                }

                const int delay = 5000;
                var amountText = amount + 1 > 1 ? "messages" : "message";

                var msg = await Context.Channel.SendMessageAsync($"Purge completed, deleted **{amount}** {amountText}. _This message will be deleted in {delay / 1000} seconds._");
                await Task.Delay(delay);
                await msg.DeleteAsync();
            }
        }
    }
}
