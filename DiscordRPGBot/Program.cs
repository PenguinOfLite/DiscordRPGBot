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
                    activeAdventure.Move(DIRECTION.UP);
                    message.Channel.SendMessageAsync(Program.activeAdventure.DisplayTiles());
                    break;
                case "s":
                    activeAdventure.Move(DIRECTION.DOWN);
                    message.Channel.SendMessageAsync(Program.activeAdventure.DisplayTiles());
                    break;
                case "w":
                    activeAdventure.Move(DIRECTION.LEFT);
                    message.Channel.SendMessageAsync(Program.activeAdventure.DisplayTiles());
                    break;
                case "e":
                    activeAdventure.Move(DIRECTION.RIGHT);
                    message.Channel.SendMessageAsync(Program.activeAdventure.DisplayTiles());
                    break;
                case "xd": message.Channel.SendMessageAsync("XDLMAO420");
                    break;
                default: break;
            }






            return Task.CompletedTask;
            
        }

        public static void No()
        {
            lastMessage.Channel.SendMessageAsync("NO! I WON'T! :rage:");
        }
    }

    public class Adventure
    {
        const int size = 5;
        Tile[,] tiles;
        Tile activeTile;
        Vector2 position;
        
        public Vector2 Position
        {
            get { return position; }
            set {
                position = value;
                activeTile = tiles[(int)position.X, (int)position.Y];
            }
        }
        List<Tile> acccessibleTiles;
        public Adventure()
        {
            tiles = new Tile[size, size];  //x,y
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    tiles[i, j] = Tile.GenerateTile();
                }
            }
            activeTile = tiles[2,2];
            position = new Vector2(2,2);
            activeTile.visited = true;

        }

        public void Move(DIRECTION direction)
        {
            Tile destinationTile = Tile.GenerateTile(); //csak hogy elinduljon
            if (direction == DIRECTION.UP)
            {
                if (position.Y < size - 1)
                {
                    destinationTile = tiles[(int)position.X, (int)position.Y + 1];
                    Position += new Vector2(0, 1);
                }
                else { Program.No(); }

            }
            else if (direction == DIRECTION.LEFT)
            {
                if (Position.X > 0)
                {
                    destinationTile = tiles[(int)position.X - 1, (int)position.Y];
                    Position += new Vector2(-1, 0);
                }
                else { Program.No(); }
            }
            else if (direction == DIRECTION.DOWN)
            {
                if (position.Y > 0)
                {
                    destinationTile = tiles[(int)position.X, (int)position.Y + -1];
                    Position += new Vector2(0, -1);
                }
                else { Program.No(); }
            }
            else if (direction == DIRECTION.RIGHT)
            {
                if (Position.X < size-1)
                {
                    destinationTile = tiles[(int)position.X + 1, (int)position.Y];
                    Position += new Vector2(1, 0);
                }
                else { Program.No(); }
            
            }
            destinationTile.visited = true;
        }



        List<Tile> GetAccessibleTiles()
        {
            List<Tile> yes = new List<Tile>();

           


            return yes;
        }

        public string DisplayTiles()
        {
            string s = "";
            string tile;
            for (int j = size-1; j >= 0; j--)
            {
                for (int i = 0; i <size; i++)
                {
                    if (tiles[i, j] == activeTile)
                    {
                        tile = ":flushed:";
                    }
                    else if (tiles[i, j].visited == false) { tile = ":black_large_square:"; }
                    else
                    {
                        switch (tiles[i, j].type.ToString())
                        {
                            case "XD":
                                tile = ":homes:";
                                break;
                            case "FOREST":

                                tile = ":evergreen_tree:";
                                break;
                            case "FIELD":
                                tile = ":green_circle:";
                                break;
                            default:
                                tile = "[]";
                                break;

                        }
                    }
                    
                    s += tile;
                }
                s += "\n";
            }
            return s+"\nCurrent tile: " + activeTile.type.ToString();
        }
    }

    public class Tile
    {
        public string name;
        public string description;
        public TILETYPES type;
        public bool[] routes = new bool[4]; //N, W, S, E
        public bool visited;

        public Tile(string n,string d, TILETYPES t)
        {
            name = n;
            description = d;
            type = t;
            visited = false;


            for (int i = 0; i < 4; i++)
            {
                Random r = new Random();
                routes[i] = Convert.ToBoolean(r.Next(0,2));
            }
            
        }

        public static Tile GenerateTile()
        {
            Random r = new Random();
            TILETYPES[] types = { TILETYPES.FIELD, TILETYPES.FOREST, TILETYPES.XD };
            return new Tile("","",types[r.Next(0,3)]);
        }
        
        public void OnFirstEnter()
        { 
            
        }

        public void OnEnter()
        { 
            
        }
        
    }

    public enum TILETYPES
    { 
        FOREST, FIELD, XD
       
    }

    public enum DIRECTION
    { 
        UP, LEFT, DOWN, RIGHT
    }
        

}

