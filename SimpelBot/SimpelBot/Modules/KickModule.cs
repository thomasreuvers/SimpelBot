using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace SimpelBot.Modules
{
    public class KickModule : ModuleBase<SocketCommandContext>
    {
        [Command("kick")]
        [Summary("kicks the specified user")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task KickAsync(SocketUser user, [Remainder]string reason = "No reason specified")
        {
            await Context.Message.DeleteAsync();

            if (!user.Equals(null))
            {
                if (user.IsBot)
                {
                    var msg = await Context.Channel.SendMessageAsync("I cannot kick myself... :thinking:");
                    await Task.Delay(4000);
                    await msg.DeleteAsync();
                    return;
                }

                if (user == Context.User)
                {
                    var msg = await Context.Channel.SendMessageAsync("You cannot kick yourself! :man_facepalming:");
                    await Task.Delay(4000);
                    await msg.DeleteAsync();
                    return;
                }

                if (((IGuildUser)user).GuildPermissions.Administrator)
                {
                    var msg = await Context.Channel.SendMessageAsync("HEY! You cannot kick an administrator! :point_up:");
                    await Task.Delay(4000);
                    await msg.DeleteAsync();
                    return;
                }


                var embed = new EmbedBuilder
                {
                    Author = new EmbedAuthorBuilder().WithIconUrl(Context.Client.CurrentUser.GetAvatarUrl()).WithName(Context.Client.CurrentUser.Username),
                    Timestamp = DateTimeOffset.UtcNow,
                    Title = "Kick",
                    Color = Color.Blue,
                    Fields = new List<EmbedFieldBuilder>
                    {
                        new EmbedFieldBuilder().WithName("Kicked user:").WithValue($"{user.Mention}"),
                        new EmbedFieldBuilder().WithName("Kicked by:").WithValue($"{Context.Message.Author.Mention}"),
                        new EmbedFieldBuilder().WithName("Reason:").WithValue($"{reason}")
                    }

                };

                await Context.Channel.SendMessageAsync(embed: embed.Build());

                // TODO: Kick user logic
            }

        }
    }
}
