using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ManicFEUP
{
    enum FaceDirection
    {
        Right = 1,
        Left = -1
    }

    class Enemy
    {
        private Sprite sprite;
        private Rectangle localBounds;

        private const float MoveSpeed = 64.0f;
        private const float MaxWaitTime = 0.5f;
        private float waitTime;
        private FaceDirection direction;

        public Vector2 Position;
        public SceneLevel level;
        public Rectangle Bounding
        {
            get
            {
                int left = (int)Math.Round(Position.X - sprite.Origin.X) + localBounds.X;
                int top = (int)Math.Round(Position.Y - sprite.Origin.Y) + localBounds.Y;

                return new Rectangle(left, top, localBounds.Width, localBounds.Height);
            }
        }


        public Enemy(SceneLevel level, Vector2 Pos, string assetName) 
        {
            this.level = level;
            this.Position = Pos;

            LoadContent(assetName);
            direction = FaceDirection.Right;
            sprite.SetAnimLoop(0, 2, 0.2f);
        }

        public void LoadContent(string assetName)
        {
            this.sprite = new Sprite(level.Content.Load<Texture2D>(assetName), 32, 32, 6, new Vector2(16, 32));
            localBounds = new Rectangle(10, 0, 14, 32); //Caixa de colisao
        }

        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Calculate tile position based on the side we are walking towards.
            float posX = Position.X + localBounds.Width / 2 * (int)direction;
            int tileX = (int)Math.Floor(posX / level.tileSet.Width) - (int)direction;
            int tileY = (int)Math.Floor(Position.Y / level.tileSet.Height);

            if (waitTime > 0)
            {
                // Wait for some amount of time.
                waitTime = Math.Max(0.0f, waitTime - (float)gameTime.ElapsedGameTime.TotalSeconds);
                if (waitTime <= 0.0f)
                {
                    // Then turn around.
                    if (direction == FaceDirection.Right)
                    {
                        direction = FaceDirection.Left;
                        sprite.SetAnimLoop(3, 5, 0.2f);
                    }
                    else
                    {
                        direction = FaceDirection.Right;
                        sprite.SetAnimLoop(0, 2, 0.2f);
                    }
                }
            }
            else
            {
                // If we are about to run into a wall or off a cliff, start waiting.
                if (level.GetCollision(tileX + (int)direction, tileY - 1) == TileCollision.Impassable ||
                    level.GetCollision(tileX + (int)direction, tileY) == TileCollision.Passable)
                {
                    waitTime = MaxWaitTime;
                    sprite.SetAnim(false);
                }
                else
                {
                    // Move in the current direction.
                    Vector2 velocity = new Vector2((int)direction * MoveSpeed * elapsed, 0.0f);
                    Position = Position + velocity;
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch, Position);
        }
    }
}
