using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.IO;
using Microsoft.Xna.Framework.Audio;

namespace ManicFEUP
{
    enum LevelStates
    {
        Playing = 0,
        GameOver = 1,
        GameWon = 2
    }
    class SceneLevel : Scene
    {
        private Player player;
        private KeyboardState previousKey;
        protected List<Key> keys = new List<Key>();
        protected List<Shot> shots = new List<Shot>();
        protected List<Enemy> enemies = new List<Enemy>();
        protected List<PlatformFalling> platformsFalling = new List<PlatformFalling>();
        protected List<PlatformFalling> platformsFallingBackup = new List<PlatformFalling>();
        protected Door door;
        protected List<Spike> spikes = new List<Spike>();
        protected SpeedBoots speedBoots;
        protected JumpBoots jumpBoots;
        protected Weapon weapon;
        protected EnergyMeter energyMeter;

        private LevelStates state;
        private Text stateFont = new Text("manicFont", 300, 440);
        
        // Textos
        protected Text noJumps = new Text("manicFont", 35, 400);
        protected Text xToShoot = new Text("manicFont", 35, 440);
        protected Text speedIncreased = new Text("manicFont", 35, 420);
        protected Text energyText = new Text("manicFont", 155, 370);

        // Sons
        protected SoundEffect sndDie;
        protected SoundEffect sndDieLast;
        protected SoundEffect sndItem;

        public TileSet tileSet;
        protected Tile[,] tiles;
        protected Tile[,] tilesBackup;
        protected Vector2 start;
        protected Point exit = InvalidPosition;
        private static readonly Point InvalidPosition = new Point(-1, -1);

        protected const int energy = 600;
        protected int highScore;
        protected int score;
        protected int jumpsLeft = 0;
        protected bool countJump = true;
        protected bool speedBootsOn = false;

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
            energyMeter = new EnergyMeter(this, new Vector2(150, 370), energy);
            backup();
            state = LevelStates.Playing;
            //Load backgrounds
            //Load sounds
        }

        public override void Load() {
            // Loading Fonts
            noJumps.Load(Content);
            xToShoot.Load(Content);
            speedIncreased.Load(Content);
            energyText.Load(Content);
            stateFont.Load(Content);
            sndDie = Content.Load<SoundEffect>("sndDie");
            sndDieLast = Content.Load<SoundEffect>("sndDieLast");
            sndItem = Content.Load<SoundEffect>("sndItem");
        }

        public override bool Update(GameTime gameTime, KeyboardState keyboardState)
        {
            if (state != LevelStates.Playing)
            {
                if (keyboardState.IsKeyDown(Keys.Enter))
                    return false;

                return true;
            }

            if (keyboardState.IsKeyDown(Keys.R))
                this.reset();

            player.Update(gameTime, keyboardState, shots);
            UpdateKeys(gameTime);
            UpdateEnemies(gameTime);
            UpdateSpikes(gameTime);
            if (speedBoots.Update(gameTime, player)) {
                speedBootsOn = true;
                sndItem.Play();
            }
            UpdateJumps(gameTime, keyboardState);
            weapon.Update(gameTime, player, sndItem);
            previousKey = keyboardState;
            UpdateShots(gameTime);
            for (int i = 0; i < platformsFalling.Count; i++)
                if (platformsFalling[i].Update(gameTime, player)) {
                    tiles[(int)platformsFalling[i].X / tileSet.Width, (int)platformsFalling[i].Y / tileSet.Height] = new Tile(TileCollision.Passable); //Apaga do tileset
                    platformsFalling[i].IsActive = false;
                }

            energyMeter.Update(gameTime, player);

            if (door.Active && door.Bounding.Intersects(player.Bounding))
            {
                state = LevelStates.GameWon;
            }

            if (!player.IsAlive)
            {
                player.Lifes--;
                if (player.Lifes == 0) {
                    state = LevelStates.GameOver;
                    sndDieLast.Play();
                }
                else sndDie.Play();
                this.reset();
            }

            return true;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawTiles(spriteBatch);

            foreach (Spike spike in spikes)
                spike.Draw(gameTime, spriteBatch);

            foreach (Key key in keys)
                if(key.Active) key.Draw(gameTime, spriteBatch);

            player.Draw(gameTime, spriteBatch);

            foreach (Enemy enemy in enemies)
                if(enemy.IsAlive) enemy.Draw(gameTime, spriteBatch);
            speedBoots.Draw(gameTime, spriteBatch);
            jumpBoots.Draw(gameTime, spriteBatch);
            weapon.Draw(gameTime, spriteBatch);
            foreach (Shot shot in shots)
                shot.Draw(gameTime, spriteBatch);
            foreach (PlatformFalling platform in platformsFalling)
                if( platform.IsActive) platform.Draw(gameTime, spriteBatch);

            door.Draw(gameTime, spriteBatch);

            DrawHUD(gameTime, spriteBatch);
            
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

        private void DrawHUD(GameTime gameTime, SpriteBatch spriteBatch) {
            noJumps.Draw(spriteBatch, "x " + ((jumpsLeft - 1 >= 0) ? jumpsLeft - 1 : 0), Color.White);
            
            
            if(speedBootsOn) speedIncreased.Draw(spriteBatch, "Speed increased", Color.White);
            if (player.Weapon) xToShoot.Draw(spriteBatch, "Press \"X\" to shoot", Color.White);

            jumpBoots.DrawHUDCopy(gameTime, spriteBatch, 10, 400);
            
            if(speedBootsOn) speedBoots.DrawHUDCopy(gameTime, spriteBatch, 10, 420);
            if(player.Weapon) weapon.DrawHUDCopy(gameTime, spriteBatch, 10, 440);

            energyMeter.Draw(gameTime, spriteBatch);
            energyText.Draw(spriteBatch, "Energy", Color.White);

            if (state != LevelStates.Playing)
            {
                switch (state)
                {
                    case LevelStates.GameOver:
                        stateFont.Draw(spriteBatch, "Game Over. Press Enter to Continue", Color.White);
                        break;

                    case LevelStates.GameWon:
                        stateFont.Draw(spriteBatch, "You Won the Game! Press Enter to Continue", Color.White);
                        break;
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
            tilesBackup = new Tile[width, lines.Count];

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
                case '~': return LoadFallingPlatform(x, y); //return new Tile(0, 7, TileCollision.Platform);    // Platform block     
                case ':': return new Tile(0, 8, TileCollision.Passable);    // Passable block 
                case '-': return new Tile(0, 7, TileCollision.Platform);  // Floating platform
                case '0': return LoadSpikeTile(x, y, 0);
                case '1': return LoadSpikeTile(x, y, 1);
                case '2': return LoadSpikeTile(x, y, 2);
                case '3': return LoadSpikeTile(x, y, 3);
                case 'P': return LoadStartTile(x, y); // Player 1 start point  
                case 'X': return LoadExitTile(x, y);    // Door
                case 'K': return LoadKeyTile(x, y); // Key
                case 'B': return LoadSpeedBootsTile(x, y); //Speed Boots
                case 'J': return LoadJumpBootsTile(x, y); //Jump Boots
                case 'E': return LoadEnemyTile(x, y, "sprEnemy", 3);   // Various enemies
                case 'M': return LoadEnemyTile(x, y, "sprBoss", 5);
                case 'W': return LoadWeaponTile(x, y);  //Weapon
                // Unknown tile type character
                default: throw new NotSupportedException(String.Format("Unsupported tile type character '{0}' at position {1}, {2}.", tileType, x, y));
            }
        }

        private Tile LoadStartTile(int x, int y)
        {
            if (player != null)
                throw new NotSupportedException("A level may only have one starting point.");
            start = new Vector2( x*tileSet.Width + tileSet.Width/2, y*tileSet.Height + tileSet.Height);
            player = new Player(this, start, Content);
            return new Tile(TileCollision.Passable);
        }

        private Tile LoadExitTile(int x, int y)
        {
            if (exit != InvalidPosition)
                throw new NotSupportedException("A level may only have one exit.");

            exit = new Point(x * tileSet.Width, y * tileSet.Height);
            door = new Door(this, new Vector2(exit.X, exit.Y) );
            return new Tile(TileCollision.Passable);
        }

        private Tile LoadEnemyTile(int x, int y, string assetName, int life)
        {
            Vector2 position = RectUtils.GetBottomCenter(GetBounds(x, y));
            enemies.Add(new Enemy(this, position, assetName, life));
            return new Tile(TileCollision.Passable);
        }

        private Tile LoadKeyTile(int x, int y)
        {
            Point position = new Point(x * tileSet.Width, y*tileSet.Height);
            keys.Add(new Key(this, new Vector2(position.X, position.Y)));
            return new Tile(TileCollision.Passable);
        }

        private Tile LoadSpeedBootsTile(int x, int y) {
            Point position = new Point(x * tileSet.Width, y * tileSet.Height);
            speedBoots = new SpeedBoots(this, new Vector2(position.X, position.Y));
            return new Tile(TileCollision.Passable);
        }

        private Tile LoadJumpBootsTile(int x, int y) {
            Vector2 position = new Vector2(x * tileSet.Width, y * tileSet.Height);
            jumpBoots = new JumpBoots(this, position);
            return new Tile(TileCollision.Passable);
        }

        private Tile LoadWeaponTile(int x, int y) {
            Vector2 position = new Vector2(x * tileSet.Width, y * tileSet.Height);
            weapon = new Weapon(this, position);
            return new Tile(TileCollision.Passable);
        }

        private Tile LoadFallingPlatform(int x, int y) {
            Vector2 position = new Vector2(x * tileSet.Width, y * tileSet.Height);
            platformsFalling.Add(new PlatformFalling(this, position));
            return new Tile(TileCollision.Platform);
        }

        private Tile LoadSpikeTile(int x, int y, int type)
        {
            Vector2 position = new Vector2(x * tileSet.Width, y * tileSet.Height);
            spikes.Add(new Spike(this, position, type));
            return new Tile(TileCollision.Passable);
        }

        #endregion
       
        private void UpdateKeys(GameTime gameTime)
        {
            for (int i = 0; i < keys.Count; ++i)
            {
                Key key = keys[i];
                //key.Update(gameTime);
                if (key.Active && key.Bounding.Intersects(player.Bounding))
                {
                    sndItem.Play();
                    player.keyNumber++;
                    keys[i].Active = false;
                    //OnGemCollected(gem, Player);

                    if (keys.Count == player.keyNumber)
                        door.SetActive(true);
                }
            }
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            foreach (Enemy enemy in enemies)
            {
                if (enemy.IsAlive)
                {
                    enemy.Update(gameTime, player);

                    // Touching an enemy instantly kills the player
                    if (enemy.Bounding.Intersects(player.Bounding))
                    {
                        //    OnPlayerKilled(enemy);
                        player.IsAlive = false;
                    }
                    // Kill the player
                }
            }
        }

        private void UpdateSpikes(GameTime gameTime)
        {
            for (int i = 0; i < spikes.Count; ++i)
            {
                Spike spike = spikes[i];
                //key.Update(gameTime);
                if (spike.Bounding.Intersects(player.Bounding))
                {
                    player.IsAlive = false;
                }
            }
        }

        private void UpdateJumps(GameTime gameTime, KeyboardState keyboardState) {
            if (jumpBoots.Update(gameTime, player)) {
                sndItem.Play();
                jumpsLeft = 11;
            }

            if (jumpsLeft > 0 && keyboardState != previousKey && countJump && keyboardState.IsKeyDown(Keys.Space)) {
                countJump = false;
                jumpsLeft--;
                if (jumpsLeft == 0)
                    jumpBoots.reset(player);
            }
            if (player.onGround && !keyboardState.IsKeyDown(Keys.Space)) countJump = true;
        }

        private void UpdateShots (GameTime gameTime) {
            for (int i = 0; i < shots.Count; i++)
                if (shots[i].Update(gameTime, enemies))
                    shots.Remove(shots[i--]);
        }

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

        // Funções para retornar ao estado inicial apos o player morrer
        private void backup() {
            tilesBackup = tiles;
            platformsFallingBackup = platformsFalling;
        }

        public void reset() {
            tiles = tilesBackup;
            platformsFalling = platformsFallingBackup;

            player.Reset(start);
            weapon.Reset(player);
            jumpBoots.reset(player);
            jumpsLeft = 0;
            speedBoots.Reset(player);
            energyMeter.Reset();
            speedBootsOn = false;

            foreach (Enemy enemy in enemies)
                enemy.Reset();
            foreach (Key key in keys)
                key.Active = true;
            foreach (PlatformFalling platform in platformsFalling)
            {
                platform.Reset();
                tiles[(int)platform.X / tileSet.Width, (int)platform.Y / tileSet.Height] = new Tile(TileCollision.Platform); //Apaga do tileset
            }
            door.SetActive(false);
        }

    }
}