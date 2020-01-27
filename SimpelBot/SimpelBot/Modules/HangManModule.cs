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
        private static RestUserMessage WordInfo;
        private static int lives = 6; 

        [Command("hangman", RunMode = RunMode.Async)]
        [Summary("Hangman minigame minigame")]
        public async Task HangManAsync(string userInput)
        {

            if (userInput.ToLower().Equals("start"))
            {
                await Context.Message.DeleteAsync();
                lives = 6;
                await GenerateWord();
            }
            else
            {
                if (lives <= 0)
                {
                    var returnMsg = await Context.Channel.SendMessageAsync("Oops, you ran out of guesses! Use `!hangman start` to start a new game!");
                    await msg.DeleteAsync();
                    await WordInfo.DeleteAsync();
                    await Task.Delay(5000);
                    await returnMsg.DeleteAsync();
                    return;
                }

                await Context.Message.DeleteAsync();
                userInput = userInput.ToLower();

                if (userInput.Equals(word))
                {
                    await Context.Channel.SendMessageAsync($"{Context.Message.Author.Username} guessed the word! The word was **{word}**");
                    await msg.DeleteAsync();
                    await WordInfo.DeleteAsync();
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
                else
                {
                    lives -= 1;
                }
            }
        }

        private async Task GenerateWord()
        {
            string[] words =
            {
                "analfabeet",
                "nederlands",
                "bioresonantie",
                "elastiek",
                "introductie"
            };
            var rndm = new Random().Next(0, words.Length);
            word = words[rndm];

            var places = "";

            for (var i = 0; i < word.Length; i++)
            {
                places += "-";
            }

            WordInfo = await Context.Channel.SendMessageAsync($"A new hangman game has been created.\nThe word consists out of **{word.Length}** characters.");
            msg = await Context.Channel.SendMessageAsync($"{places}");
        }
    }
}
