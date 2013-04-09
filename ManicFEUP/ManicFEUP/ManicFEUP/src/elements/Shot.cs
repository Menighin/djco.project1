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

        private const float speed = 5.0f;
        private Side direction;

        public SceneLevel level;
        public Rectangle Bounding {
            get {
                return new Rectangle((int)Math.Round(Position.X),
                                     (int)Math.Round(Position.Y), 16, 16);
            }
        }

        public Shot(SceneLevel level, Vector2 position, int side) {
            this.level = level;
            this.Position = position;

            if (side >= 0) direction = Side.Right;
            else direction = Side.Left;

            LoadContent();
            sprite.SetAnimLoop(0, 3, 0.2f);
        }

        public void LoadContent() {
            this.sprite = new Sprite(level.Content.Load<Texture2D>("shot"), 16, 16, 4, new Vector2(0, 0));
        }

        public bool Update(GameTime gameTime, List<Enemy> enemies) {
            if (this.direction == Side.Left)
                this.Position.X -= speed;
            else if (this.direction == Side.Right)
                this.Position.X += speed;

            return HandleCollision(enemies);
        }

        private bool HandleCollision(List<Enemy> enemies) {

            for (int i = 0; i < enemies.Count; i++)
                if (Bounding.Intersects(enemies[i].Bounding)) {
                    enemies[i].Life--;
                    enemies[i].BeenHit = true;
                    if (enemies[i].Life == 0) enemies[i].IsAlive = false;
                    return true;
                }

            Rectangle bounds = Bounding;
            int leftTile = (int)Math.Floor((float)bounds.Left / level.tileSet.Width);
            int rightTile = (int)Math.Ceiling(((float)bounds.Right / level.tileSet.Width)) -1;
            int topTile = (int)Math.Floor((float)bounds.Top / level.tileSet.Height);
            int bottomTile = (int)Math.Ceiling(((float)bounds.Bottom / level.tileSet.Height)) -1;

            for (int y = topTile; y <= bottomTile; ++y)
            {
                for (int x = leftTile; x <= rightTile; ++x)
                {
                    TileCollision collision = level.GetCollision(x, y);
                    if (collision == TileCollision.Impassable) // Ignore platforms.
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
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
