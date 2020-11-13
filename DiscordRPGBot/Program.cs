using System;
using Discord;
using Discord.WebSocket;
using System.CodeDom;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Discord.API;
using Discord.Commands;
using Discord.Webhook;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.Numerics;

namespace DiscordRPGBot
{
    class Program
    {
        public static Adventure activeAdventure = new Adventure();
        public static SocketMessage lastMessage;
        public static void Main(string[] args)
        => new Program().MainAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _client.MessageReceived += CommandHandler;
            _client.Log += Log;

            
            var token = File.ReadAllText("token.txt.txt");



            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private Task CommandHandler(SocketMessage message)
        {
            
            //variables
            string command = "";
            int lengthOfCommand = -1;
            lastMessage = message;
            //filtering messages begin here
            if (!message.Content.StartsWith("!")) //This is your prefix
                return Task.CompletedTask;

            if (message.Author.IsBot) //This ignores all commands from bots
                return Task.CompletedTask;

            if (message.Content.Contains(" "))
                lengthOfCommand = message.Content.IndexOf(' ');
            else
                lengthOfCommand = message.Content.Length;


            command = message.Content.Substring(1, lengthOfCommand - 1).ToLower();

            //Commands begin here
            Console.WriteLine(command);
            switch (command)
            {
                case "start":
                    Program.activeAdventure = new Adventure();
                    message.Channel.SendMessageAsync(Program.activeAdventure.DisplayTiles());
                    break;
                case "displaytiles":
                    message.Channel.SendMessageAsync(Program.activeAdventure.DisplayTiles());
                    break;
                case "n":
                    message.Channel.SendMessageAsync(activeAdventure.Move(DIRECTION.UP));
                    
                    break;
                case "s":
                    message.Channel.SendMessageAsync(activeAdventure.Move(DIRECTION.DOWN));
                    
                    break;
                case "w":
                    message.Channel.SendMessageAsync(activeAdventure.Move(DIRECTION.LEFT));
                    
                    break;
                case "e":
                    message.Channel.SendMessageAsync(activeAdventure.Move(DIRECTION.RIGHT));
                   ;
                    break;
                case "xd": message.Channel.SendMessageAsync("XDLMAO420");
                    break;
                default: break;
            }






            return Task.CompletedTask;
            
        }

        public static string No()
        {
           return "NO! I WON'T! :rage:";
        }
    }

    
        

}

