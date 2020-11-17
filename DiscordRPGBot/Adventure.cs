using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace DiscordRPGBot
{   //NO DISCORD METHODS HERE
    public class Adventure
    {
        const int size = 5;
        Tile[,] tiles;
        Tile activeTile;
        Vector2 position; 
        List<Tile> acccessibleTiles;

        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                activeTile = tiles[(int)position.X, (int)position.Y];
            }
        }
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
            activeTile = tiles[2, 2];
            position = new Vector2(2, 2);
            activeTile.visited = true;
            Tile.Behaviours = TileBehaviourHelper.GetTileBehaviours();
        }

        public string Move(DIRECTION direction)
        {
            Tile destinationTile = Tile.GenerateTile(); //csak hogy elinduljon
            if (direction == DIRECTION.UP)
            {
                if (position.Y < size - 1 )
                {
                    destinationTile = tiles[(int)position.X, (int)position.Y + 1];
                    Position += new Vector2(0, 1);
                }
                else {return Program.No(); }

            }
            else if (direction == DIRECTION.LEFT)
            {
                if (Position.X > 0)
                {
                    destinationTile = tiles[(int)position.X - 1, (int)position.Y];
                    Position += new Vector2(-1, 0);
                }
                else { return Program.No(); }
            }
            else if (direction == DIRECTION.DOWN)
            {
                if (position.Y > 0)
                {
                    destinationTile = tiles[(int)position.X, (int)position.Y + -1];
                    Position += new Vector2(0, -1);
                }
                else { return Program.No(); }
            }
            else if (direction == DIRECTION.RIGHT)
            {
                if (Position.X < size - 1)
                {
                    destinationTile = tiles[(int)position.X + 1, (int)position.Y];
                    Position += new Vector2(1, 0);
                }
                else { return Program.No(); }

            }

            string x = destinationTile.OnEnter();
            destinationTile.visited = true;
            return x;
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
            for (int j = size - 1; j >= 0; j--)
            {
                for (int i = 0; i < size; i++)
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
                            case "VILLAGE":
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
            return s + "\nCurrent tile: " + activeTile.type.ToString();
        }

        bool TestForRoutes(DIRECTION direction, Tile destionationTile)
        {
            return activeTile.directions.Contains(direction) && destionationTile.directions.Contains(direction); 
        }
    }

    public class Tile 
    {
        public string name;             // public fields = bad practice 
        public string description;      // use private fields + properties, or more commonly, auto-properties
        public TILETYPES type;
        public List<DIRECTION> directions = new List<DIRECTION>();
        public bool visited;
        public static Queue<ITileBehaviour> Behaviours { get; set; } //use auto-properties wherever possible
        public Tile(string n, string d, TILETYPES t)
        {
            name = n;
            description = d;
            type = t;
            visited = false;
             

            for (int i = 0; i < 4; i++)
            {
                Random r = new Random();
                
            }

        }

        public static Tile GenerateTile()
        {
            Random r = new Random();
            TILETYPES[] types = { TILETYPES.FIELD, TILETYPES.FOREST, TILETYPES.VILLAGE };
            return new Tile("", "This is a test tile. You have nothing to do here, you can go wherever you want", types[r.Next(0, 3)]);
        }

        // I would move this up to the adventure class, it's the adventure's job to decide what happens when the player steps on  a tile
        // (the tile class shouldn't know about how it's being used from the outside, it only represents a tile and holds its data)
        string OnFirstEnter()
        {
            TileBehaviour();
            return ReadTile();
        }

        public string OnEnter()
        {
            if (!visited)return OnFirstEnter();
            else 
            {
                return $"You entered {type.ToString().ToLower()}.";
            }
        }

        public string ReadTile()
        {
            return $"You are in a(n) {type.ToString().ToLower()}! \n{description}";
        }

        public void TileBehaviour()
        {
            try
            {
                Behaviours.Dequeue().Play();
            }
            catch (InvalidOperationException)
            {
                // the queue was empty, generate a random behaviour or handle it somehow
                throw;      // delete this line obviously 
            }
        }
    }

    public enum TILETYPES
    {
        FOREST, FIELD, VILLAGE

    }

    public enum DIRECTION
    {
        UP, LEFT, DOWN, RIGHT
    }

    
}
