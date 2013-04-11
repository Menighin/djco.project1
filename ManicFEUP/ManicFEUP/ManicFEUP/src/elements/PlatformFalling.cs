using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ManicFEUP {
    class PlatformFalling {
        private Sprite sprite;
        private Vector2 Position;
        private int frame;
        private float timeFrame;
        private const float TimeToFall = 0.2f;
        private bool isActive;

        private const float speed = 3.0f;

        public SceneLevel level;
        public Rectangle Bounding {
            get {
                return new Rectangle((int)Math.Round(Position.X),
                                     (int)Math.Round(Position.Y), 16, 16);
            }
        }

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public PlatformFalling(SceneLevel level, Vector2 position) {
            this.level = level;
            this.Position = position;

            LoadContent();
            Reset();
        }

        public void LoadContent() {
            this.sprite = new Sprite(level.Content.Load<Texture2D>("spr/sprPlatformFalling"), 16, 14, 4, new Vector2(0, 0));
        }

        public void Reset()
        {
            isActive = true;
            frame = 0;
            timeFrame = TimeToFall;
            sprite.SetAnimLoop(frame, frame, 0.2f);
        }

        public bool Update(GameTime gameTime, Player player) {
            if (isActive && isStandingOnTop(player)) {
                if (timeFrame < 0 && frame == 3) return true;
                if ((timeFrame < 0 || frame == 0) && frame < 3) {
                    frame++;
                    timeFrame = TimeToFall;
                }
                timeFrame -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            return false;
        }

        private bool isStandingOnTop(Player player) {
            Rectangle bound = player.Bounding;
            return player.onGround && bound.Bottom >= Bounding.Top && bound.Bottom < Bounding.Bottom && (
                (bound.Right < Bounding.Right && bound.Right > Bounding.Left) || 
                (bound.Left > Bounding.Left && bound.Left < Bounding.Right));
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            sprite.SetAnimLoop(frame, frame, 1f);
            sprite.Draw(gameTime, spriteBatch, Position);
        }

        public float X {
            get { return Position.X; }
        }

        public float Y {
            get { return Position.Y; }
        }

    }
}
