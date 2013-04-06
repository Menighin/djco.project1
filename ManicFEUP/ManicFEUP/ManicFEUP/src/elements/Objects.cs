using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ManicFEUP
{
    class Key
    {
        private Sprite sprite;
        private Vector2 Position;

        public SceneLevel level;
        public Rectangle Bounding
        {
            get
            {
                return new Rectangle((int)Math.Round(Position.X),
                                     (int)Math.Round(Position.Y), 16, 16);
            }
        }

        public Key(SceneLevel level, Vector2 position)
        {
            this.level = level;
            this.Position = position;

            LoadContent();
            sprite.SetAnimLoop(0, 3, 0.2f);
        }

        public void LoadContent()
        {
            this.sprite = new Sprite(level.Content.Load<Texture2D>("sprKey"), 16, 16, 4, new Vector2(0, 0));
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
        private Vector2 position;

        public Door()
        {
        }

        public void LoadContent()
        {
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }
    }

    class RoseBush
    {
        private Sprite sprite;
        private Vector2 position;

        public RoseBush()
        {
        }

        public void LoadContent()
        {
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
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
            sprite.SetAnimLoop(2, 2, 0.2f);
        }

        public void LoadContent() {
            this.sprite = new Sprite(level.Content.Load<Texture2D>("temporary"), 16, 16, 4, new Vector2(0, 0));
        }

        public bool Update(GameTime gameTime, Player player) {
            if (this.visible && this.Bounding.Intersects(player.Bounding)) {
                visible = false;
                player.Speed *= 2;
                player.Acceleration *= 1.50f;
                return true;
            }
            return false;
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
            sprite.SetAnimLoop(3, 3, 0.2f);
        }


        public void LoadContent() {
            this.sprite = new Sprite(level.Content.Load<Texture2D>("temporary"), 16, 16, 4, new Vector2(0, 0));
        }

        public Boolean Update(GameTime gameTime, Player player) {
            if (visible && Bounding.Intersects(player.Bounding)) {
                visible = false;
                player.Jump *= 2;
                return true;
            }
            return false;
        }

        public void reset(Player player) {
            player.Jump /= 2;
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
            sprite.SetAnimLoop(0, 1, 0.2f);
        }


        public void LoadContent() {
            this.sprite = new Sprite(level.Content.Load<Texture2D>("temporary"), 16, 16, 4, new Vector2(0, 0));
        }

        public void Update(GameTime gameTime, Player player) {
            if (visible && Bounding.Intersects(player.Bounding)) {
                visible = false;
                player.Weapon = true;
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
}
