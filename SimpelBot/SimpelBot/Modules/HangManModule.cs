using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.VisualBasic;

namespace SimpelBot.Modules
{
    public class HangManModule : ModuleBase<SocketCommandContext>
    {
        private static string word;
        private static RestUserMessage msg;
        [Command("hangman")]
        [Summary("Hangman minigame minigame")]
        public async Task HangManAsync(string userInput)
        {

            if (userInput.ToLower().Equals("start"))
            {
                await GenerateWord();
            }
            else
            {
                await Context.Message.DeleteAsync();
                userInput = userInput.ToLower();

                if (userInput.Equals(word))
                {
                    await Context.Channel.SendMessageAsync($"{Context.Message.Author.Username} guessed the word! The word was **{word}**");
                    await msg.DeleteAsync();
                    return;
                }

                if (word.Contains(userInput.First()))
                {
                    var listOfIndexes = new List<int>();
                    var exists = new StringBuilder(msg.Content);

                    for (var i = 0; i < word.Length; i++)
                    {
                        if (word[i].Equals(userInput.First()))
                        {
                            listOfIndexes.Add(i);
                        }
                    }

                    foreach (var occurance in listOfIndexes)
                    {
                        // exists = exists.Replace(occurance.ToString(), $"{userInput.First().ToString()}");
                        exists = exists.Remove(occurance, 1).Insert(occurance, userInput.First());
                    }


                    await msg.DeleteAsync();

                    msg = await Context.Channel.SendMessageAsync(exists.ToString());
                }
            }
        }

        public async Task GenerateWord()
        {
            string[] words =
            {
                "analfabeet",
                "nederlands",
                "bioresonantie",
                "elastiek",
                "introductie"
            };
            var rndm = new Random().Next(0, 5);
            word = words[rndm];

            var places = "";

            for (var i = 0; i < word.Length; i++)
            {
                places += "-";
            }

            await Context.Channel.SendMessageAsync($"A new hangman game has been created.\nThe word consists out of **{word.Length}** characters.");
            msg = await Context.Channel.SendMessageAsync($"{places}");
        }
    }
}
