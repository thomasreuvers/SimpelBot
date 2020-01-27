using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace SimpelBot.Modules
{
    public class MuteModule : ModuleBase<SocketCommandContext>
    {
        [Command("mute", RunMode = RunMode.Async)]
        [Summary("Mutes the user for x seconds")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task MuteAsync(SocketUser user, int seconds)
        {
            var userToMute = user;
            var muteRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "[MUTED]");

            await Context.Message.DeleteAsync();

            if (!userToMute.Equals(null))
            {
                if (seconds.Equals(null) || seconds < 1)
                {
                    var msg = await Context.Channel.SendMessageAsync("Amount of seconds cannot be less than 1");
                    await Task.Delay(4000);
                    await msg.DeleteAsync();
                    return;
                }

                if (user.IsBot)
                {
                    var msg = await Context.Channel.SendMessageAsync("I cannot mute myself... :mute:");
                    await Task.Delay(4000);
                    await msg.DeleteAsync();
                    return;
                }

                if (user == Context.User)
                {
                    var msg = await Context.Channel.SendMessageAsync("You cannot mute yourself! :man_facepalming:");
                    await Task.Delay(4000);
                    await msg.DeleteAsync();
                    return;
                }

                if (((IGuildUser)user).GuildPermissions.Administrator)
                {
                    var msg = await Context.Channel.SendMessageAsync("HEY! You cannot mute an administrator! :point_up:");
                    await Task.Delay(4000);
                    await msg.DeleteAsync();
                    return;
                }


                if (muteRole != null)
                {
                    foreach (var channel in Context.Guild.Channels)
                    {
                        if (((IGuildChannel) channel).GetPermissionOverwrite(muteRole).HasValue) { continue; }

                        await ((IGuildChannel) channel).AddPermissionOverwriteAsync(muteRole,
                            new OverwritePermissions(sendMessages: PermValue.Deny, useExternalEmojis: PermValue.Deny,
                                useVoiceActivation: PermValue.Deny, speak: PermValue.Deny, addReactions: PermValue.Deny));
                    }

                    await ((IGuildUser) userToMute).AddRoleAsync(muteRole);

                    var msg = await Context.Channel.SendMessageAsync($"Muted: {userToMute.Username} for {seconds / 1000} seconds");
                    await Task.Delay(4000);
                    await msg.DeleteAsync();

                    await Task.Delay(seconds);
                    await ((IGuildUser) userToMute).RemoveRoleAsync(muteRole);

                    msg = await Context.Channel.SendMessageAsync($"Unmuted {userToMute.Username}!");
                    await Task.Delay(4000);
                    await msg.DeleteAsync();
                }
                else
                {
                    await Context.Guild.CreateRoleAsync("[MUTED]", new GuildPermissions(viewChannel: true, readMessageHistory: true), Color.DarkGrey);
                    muteRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "[MUTED]");


                    foreach (var channel in Context.Guild.Channels)
                    {
                        if (((IGuildChannel)channel).GetPermissionOverwrite(muteRole).HasValue) { continue; }

                        await ((IGuildChannel)channel).AddPermissionOverwriteAsync(muteRole,
                            new OverwritePermissions(sendMessages: PermValue.Deny, useExternalEmojis: PermValue.Deny,
                                useVoiceActivation: PermValue.Deny, speak: PermValue.Deny, addReactions: PermValue.Deny));
                    }

                    await ((IGuildUser) userToMute).AddRoleAsync(muteRole);

                    var msg = await Context.Channel.SendMessageAsync($"Muted: {userToMute.Username} for {seconds / 1000} seconds");
                    await Task.Delay(4000);
                    await msg.DeleteAsync();

                    
                    ((IGuildUser)userToMute).RemoveRoleAsync(muteRole).Wait(seconds);

                    msg = await Context.Channel.SendMessageAsync($"Unmuted {userToMute.Username}!");
                    await Task.Delay(4000);
                    await msg.DeleteAsync();
                }

            }
            else
            {
                var msg = await Context.Message.Channel.SendMessageAsync("This user does not exist");
                await Task.Delay(4000);
                await msg.DeleteAsync();
            }
        }
    }
}
