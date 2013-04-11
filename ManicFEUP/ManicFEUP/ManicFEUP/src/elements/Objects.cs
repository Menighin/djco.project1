using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace ManicFEUP
{
    class Key
    {
        private Sprite sprite;
        private Vector2 Position;
        private bool active;

        public Scene level;
        public Rectangle Bounding
        {
            get
            {
                return new Rectangle((int)Math.Round(Position.X),
                                     (int)Math.Round(Position.Y), 16, 16);
            }
        }
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public Key(Scene level, Vector2 position)
        {
            this.level = level;
            this.Position = position;
            this.active = true;

            LoadContent();
            sprite.SetAnimLoop(0, 3, 0.2f);
        }

        public void LoadContent()
        {
            this.sprite = new Sprite(level.Content.Load<Texture2D>("spr/sprKey"), 16, 16, 4, new Vector2(0, 0));
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch, Position);
        }
    }

    class Door
    {
        private Sprite sprite;
        private Vector2 Position;
        private bool active;

        public Scene level;
        public Rectangle Bounding
        {
            get
            {
                return new Rectangle((int)Math.Round(Position.X),
                                     (int)Math.Round(Position.Y), 32, 32);
            }
        }
        public bool Active { get { return active; } }

        public Door(Scene level, Vector2 position)
        {
            this.level = level;
            this.Position = position;
            this.active = false;

            LoadContent();
        }

        public void SetActive(bool active)
        {
            this.active = active;

            if (active)
                sprite.SetAnimLoop(0, 1, 0.2f);
            else
                sprite.SetAnim(false);
        }

        public void LoadContent()
        {
            this.sprite = new Sprite(level.Content.Load<Texture2D>("spr/sprDoor"), 32, 32, 2, new Vector2(0, 0));
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch, Position);
        }
    }

    class Spike
    {
        private Sprite sprite;
        private Vector2 Position;
        private int spikeType;

        public Scene level;
        public Rectangle Bounding
        {
            get
            {
                return new Rectangle((int)Math.Round(Position.X),
                                     (int)Math.Round(Position.Y), 16, 16);
            }
        }

        public Spike(Scene level, Vector2 position, int type)
        {
            this.level = level;
            this.Position = position;
            this.spikeType = type;

            LoadContent();
        }

        public void LoadContent()
        {
            this.sprite = new Sprite(level.Content.Load<Texture2D>("spr/sprSpikes"), 16, 16, 4, new Vector2(0, 0));
            this.sprite.SetAnimLoop(spikeType, spikeType, 0.0f);
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch, Position);
        }
    }

    class SpeedBoots {
        private Sprite sprite;
        private Vector2 Position;
        private Boolean visible;

        public SceneLevel level;
        public Rectangle Bounding {
            get {
                return new Rectangle((int)Math.Round(Position.X),
                                     (int)Math.Round(Position.Y), 16, 16);
            }
        }

        public SpeedBoots(SceneLevel level, Vector2 position) {
            this.level = level;
            this.Position = position;
            visible = true;
            
            LoadContent();
            sprite.SetAnimLoop(0, 0, 0.2f);
        }

        public void LoadContent() {
            this.sprite = new Sprite(level.Content.Load<Texture2D>("spr/sprObjects"), 16, 16, 3, new Vector2(0, 0));
        }

        public bool Update(GameTime gameTime, Player player) {
            if (this.visible && this.Bounding.Intersects(player.Bounding)) {
                visible = false;
                player.Speed *= 1.35f;
                player.Acceleration *= 1.35f;
                return true;
            }
            return false;
        }

        public void Reset(Player player)
        {
            if (!visible)
            {
                player.Speed /= 1.35f;
                player.Acceleration /= 1.35f;
                visible = true;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            if (visible) 
                sprite.Draw(gameTime, spriteBatch, Position);
        }

        public void DrawHUDCopy(GameTime gameTime, SpriteBatch spriteBatch, int x = 0, int y = 0) {
            sprite.Draw(gameTime, spriteBatch, new Vector2(x, y));
        }
    }

    class JumpBoots {
        private Sprite sprite;
        private Vector2 Position;
        private Boolean visible;

        public SceneLevel level;
        public Rectangle Bounding {
            get {
                return new Rectangle((int)Math.Round(Position.X),
                                     (int)Math.Round(Position.Y), 16, 16);
            }
        }

        public JumpBoots(SceneLevel level, Vector2 position) {
            this.level = level;
            this.Position = position;
            visible = true;

            LoadContent();
            sprite.SetAnimLoop(2, 2, 0.2f);
        }


        public void LoadContent() {
            this.sprite = new Sprite(level.Content.Load<Texture2D>("spr/sprObjects"), 16, 16, 3, new Vector2(0, 0));
        }

        public Boolean Update(GameTime gameTime, Player player) {
            if (visible && Bounding.Intersects(player.Bounding)) {
                visible = false;
                player.Jump *= 1.8f;
                return true;
            }
            return false;
        }

        public void reset(Player player) {
            if (!visible)
            {
                player.Jump /= 1.8f;
                visible = true;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            if (visible)
                sprite.Draw(gameTime, spriteBatch, Position);
        }

        public void DrawHUDCopy(GameTime gameTime, SpriteBatch spriteBatch, int x = 0, int y = 0) {
            sprite.Draw(gameTime, spriteBatch, new Vector2(x, y));
        }
    }

    class Weapon {
        private Sprite sprite;
        private Vector2 Position;
        private Boolean visible;

        public SceneLevel level;
        public Rectangle Bounding {
            get {
                return new Rectangle((int)Math.Round(Position.X),
                                     (int)Math.Round(Position.Y), 16, 16);
            }
        }

        public Weapon(SceneLevel level, Vector2 position) {
            this.level = level;
            this.Position = position;
            visible = true;

            LoadContent();
            sprite.SetAnimLoop(1, 1, 0.2f);
        }


        public void LoadContent() {
            this.sprite = new Sprite(level.Content.Load<Texture2D>("spr/sprObjects"), 16, 16, 3, new Vector2(0, 0));
        }

        public void Update(GameTime gameTime, Player player, SoundEffect sndItem) {
            if (visible && Bounding.Intersects(player.Bounding)) {
                visible = false;
                player.Weapon = true;
                sndItem.Play();
            }
        }

        public void Reset(Player player)
        {
            visible = true;
            player.Weapon = false;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            if (visible)
                sprite.Draw(gameTime, spriteBatch, Position);
        }

        public void DrawHUDCopy(GameTime gameTime, SpriteBatch spriteBatch, int x = 0, int y = 0) {
            sprite.Draw(gameTime, spriteBatch, new Vector2(x, y));
        }
    }
}
