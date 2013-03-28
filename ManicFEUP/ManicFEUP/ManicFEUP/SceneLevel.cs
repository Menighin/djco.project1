using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace ManicFEUP
{
    class SceneLevel : Scene
    {
        private Player player;
        private List<Key> keys = new List<Key>();
        protected List<Enemy> enemies = new List<Enemy>();
        protected Door door;

        public TileSet tileSet;
        protected Tile[,] tiles;
        protected Vector2 start;
        protected Point exit = InvalidPosition;
        private static readonly Point InvalidPosition = new Point(-1, -1);

        protected int energy;
        protected int highScore;
        protected int score;

        public int Width
        {
            get { return tiles.GetLength(0); }
        }

        public int Height
        {
            get { return tiles.GetLength(1); }
        }

        public SceneLevel(GameServiceContainer Services, Stream fileStream) : base(Services)
        {
            LoadTiles("tileset", fileStream);  // Load TileSet and Tiles
            //Load backgrounds
            //Load sounds
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            player.Update(gameTime, keyboardState);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawTiles(spriteBatch);

            foreach (Key key in keys)
                key.Draw(gameTime, spriteBatch);

            player.Draw(gameTime, spriteBatch);

            //foreach (Enemy enemy in enemies)
            //    enemy.Draw(gameTime, spriteBatch);
        }

        private void DrawTiles(SpriteBatch spriteBatch)
        {
            // For each tile position
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    if (tiles[x, y].visible)
                    {
                        Vector2 position = new Vector2(x, y) * tileSet.Size;
                        Rectangle rect = new Rectangle(tiles[x, y].tileX * 16, tiles[x, y].tileY * 16, 16, 16);
                        spriteBatch.Draw(tileSet.Texture, position, rect, Color.White);
                    }
                        
                }
            }
        }

        #region LoadingScene
        private void LoadTiles(string tileSetName, Stream fileStream)
        {
            // Load TileSet
            tileSet = new TileSet(Content.Load<Texture2D>(tileSetName), 16, 16);
            // Load the level and ensure all of the lines are the same length.
            int width;
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string line = reader.ReadLine();
                width = line.Length;
                while (line != null)
                {
                    lines.Add(line);
                    if (line.Length != width)
                        throw new Exception(String.Format("The length of line {0} is different from all preceeding lines.", lines.Count));
                    line = reader.ReadLine();
                }
            }
            // Allocate the tile grid.
            tiles = new Tile[width, lines.Count];

            this.width = width;
            this.height = lines.Count;
            // Loop over every tile position,
            for (int y = 0; y < this.height; ++y)
            {
                for (int x = 0; x < this.width; ++x)
                {
                    // to load each tile.
                    char tileType = lines[y][x];
                    tiles[x, y] = LoadTile(tileType, x, y);
                }
            }

            // Verify that the level has a beginning and an end.
            if (player == null)
                throw new NotSupportedException("A level must have a starting point.");
            //if (exit == InvalidPosition)
            //    throw new NotSupportedException("A level must have an exit.");

        }

        private Tile LoadTile(char tileType, int x, int y)
        {
            switch (tileType)
            {
                case '.': return new Tile(TileCollision.Passable);    // Blank space
                case '#': return new Tile(0, 6, TileCollision.Impassable);  // Impassable block
                case '~': return new Tile(0, 7, TileCollision.Platform);    // Platform block     
                case ':': return new Tile(0, 8, TileCollision.Passable);    // Passable block 
                case '-': return new Tile(1, 7, TileCollision.Platform);  // Floating platform
                case '1': return LoadStartTile(x, y); // Player 1 start point  
                case 'X': return LoadExitTile(x, y);    // Door
                case 'K': return LoadKeyTile(x, y); // Key
                case 'E': return LoadEnemyTile(x, y, "MonsterA");   // Various enemies
                // Unknown tile type character
                default: throw new NotSupportedException(String.Format("Unsupported tile type character '{0}' at position {1}, {2}.", tileType, x, y));
            }
        }

        private Tile LoadStartTile(int x, int y)
        {
            if (player != null)
                throw new NotSupportedException("A level may only have one starting point.");
            player = new Player(this, new Vector2( x*tileSet.Width + tileSet.Width/2, y*tileSet.Height + tileSet.Height) );
            return new Tile(TileCollision.Passable);
        }

        private Tile LoadExitTile(int x, int y)
        {
            if (exit != InvalidPosition)
                throw new NotSupportedException("A level may only have one exit.");

            //exit = GetBounds(x, y).Center;
            //door = new Door();
            return new Tile(TileCollision.Passable);
        }

        private Tile LoadEnemyTile(int x, int y, string spriteSet)
        {
            //Vector2 position = RectangleExtensions.GetBottomCenter(GetBounds(x, y));
            //enemies.Add(new Enemy(this, position, spriteSet));
            return new Tile(TileCollision.Passable);
        }


        private Tile LoadKeyTile(int x, int y)
        {
            //Point position = GetBounds(x, y).Center;
            //keys.Add(new Key(this, new Vector2(position.X, position.Y)));
            return new Tile(TileCollision.Passable);
        }
        #endregion

        public Rectangle GetBounds(int x, int y)
        {
            return new Rectangle(x * tileSet.Width, y * tileSet.Height, tileSet.Width, tileSet.Height);
        }

        public TileCollision GetCollision(int x, int y)
        {
            // Prevent escaping past the level ends.
            if (x < 0 || x >= Width)
                return TileCollision.Impassable;
            // Allow jumping past the level top and falling through the bottom.
            if (y < 0 || y >= Height)
                return TileCollision.Passable;

            return tiles[x, y].Collision;
        }
    }
}
