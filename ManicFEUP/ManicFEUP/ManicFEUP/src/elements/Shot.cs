using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ManicFEUP {
    class Shot {
        private Sprite sprite;
        private Vector2 Position;

        enum Side { Right, Left };

        private const float speed = 3.0f;

        public SceneLevel level;
        public Rectangle Bounding {
            get {
                return new Rectangle((int)Math.Round(Position.X),
                                     (int)Math.Round(Position.Y), 16, 16);
            }
        }

        public Shot(SceneLevel level, Vector2 position) {
            this.level = level;
            this.Position = position;

            LoadContent();
            sprite.SetAnimLoop(0, 3, 0.2f);
        }

        public void LoadContent() {
            this.sprite = new Sprite(level.Content.Load<Texture2D>("sprKey"), 16, 16, 4, new Vector2(0, 0));
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState) {
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            sprite.Draw(gameTime, spriteBatch, Position);
        }
    }
}
