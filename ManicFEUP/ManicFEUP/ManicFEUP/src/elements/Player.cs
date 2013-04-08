using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ManicFEUP
{
    enum Face
    {
        Right = 0,      //Frame 0 do sprite = player parado para a direita
        Left = 3        //Frame 3 do sprite = plauer parado para a esquerda
    }

    class Player 
    {
        // Constants for controling horizontal movement
        private float MoveAcceleration = 9500.0f;            // 13000.0f;
        private float MaxMoveSpeed = 1000.0f;                 //1750.0f;
        private const float GroundDragFactor = 0.48f;               //0.48f;
        private const float AirDragFactor = 0.49f;                  //0.58f;
        // Constants for controlling vertical movement
        private float MaxJumpTime = 0.35f;                    // 0.35f;
        private const float JumpLaunchVelocity = -1400.0f;        //-3500.0f;
        private const float GravityAcceleration = 2000.0f;          //3400.0f;
        private const float MaxFallSpeed = 500.0f;                  //550.0f;
        private const float JumpControlPower = 0.14f;               //0.14f
        private const int MaxShots = 3;

        private Vector2 Position;
        private Vector2 startPosition;
        private Vector2 velocity;
        private Sprite sprite;
        private Sprite HUDsprite;
        private bool isAlive;
        private int lifes = 3;
        private bool isOnGround;
        private float movement;
        private bool isJumping;
        private bool wasJumping;
        private float jumpTime;
        private bool canShoot;
        private int side;
        private float shootInterval;

        private SceneLevel level;
        private int previousBottom;

        private Sprite sprMask;         //Sprite mascara de colisao
        private Rectangle localBounds;  //Caixa de colisao
        public Rectangle Bounding
        {
            get
            {
                int left = (int)Math.Round(Position.X - sprite.Origin.X) + localBounds.X;
                int top = (int)Math.Round(Position.Y - sprite.Origin.Y) + localBounds.Y;

                return new Rectangle(left, top, localBounds.Width, localBounds.Height);
            }
        }

        public Player(SceneLevel level, Vector2 pos) 
        {
            this.level = level;
            this.Position = pos;
            this.startPosition = pos;
            this.canShoot = false;
            isAlive = true;

            Reset(Position);
            LoadContent();
        }

        public void Reset(Vector2 position)
        {
            this.Position = position;
            this.velocity = Vector2.Zero;
            this.isAlive = true;
            this.isJumping = false;
        }

        public void LoadContent()
        {
            this.sprite = new Sprite(level.Content.Load<Texture2D>("sprPlayer"), 32, 32, 6, new Vector2(16,31));
            this.sprMask = new Sprite(level.Content.Load<Texture2D>("sprPlayerMask"), 32, 32, 1, new Vector2(16, 31));
            this.HUDsprite = new Sprite(level.Content.Load<Texture2D>("sprPlayer"), 32, 32, 6, new Vector2(16, 31));

            localBounds = new Rectangle(10, 1, 12, 31); //Caixa de colisao
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, List<Shot> shots)
        {
            GetInput(gameTime, keyboardState, shots);    //Get input
            ApplyPhysics(gameTime);     //Apply Physics

            if (movement > 0)
                sprite.SetAnimLoop(0, 2, 0.2f);
            else if (movement < 0)
                sprite.SetAnimLoop(3, 5, 0.2f);
            else
                sprite.SetAnim(false);

            // Clear input.
            movement = 0.0f;
            isJumping = false;
            HUDsprite.SetAnimLoop(0, 0, 0.2f);

            // If is dead
            if (!isAlive) {
                Position = startPosition;
                lifes--;
                isAlive = true;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch, Position);
            //sprMask.Draw(gameTime, spriteBatch, Position);

            // Draw hud
            for (int i = 0; i < lifes; i++)
                HUDsprite.Draw(gameTime, spriteBatch, new Vector2(20 + i * 25, 380));
        }

        private void GetInput(GameTime gameTime, KeyboardState keyboardState, List<Shot> shots)
        {
            // Ignore small movements to prevent running in place.
            if (Math.Abs(movement) < 0.5f)
                movement = 0.0f;

            // If any digital horizontal movement input is found, override the analog movement.
            if ( keyboardState.IsKeyDown(Keys.Left) )
            {
                movement = -1.0f;
                side = -1;
            }
            else if (keyboardState.IsKeyDown(Keys.Right) )
            {
                movement = 1.0f;
                side = 1;
            }

            // Check if the player wants to jump.
            isJumping = keyboardState.IsKeyDown(Keys.Space);

            // Player atirando
            shootInterval -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (canShoot && keyboardState.IsKeyDown(Keys.X)) {
                if (shots.Count < MaxShots) {
                    if (shootInterval < 0 || shots.Count == 0) {
                        if (side >= 0)
                            shots.Add(new Shot(level, new Vector2(Position.X, Position.Y - 20), side));
                        else
                            shots.Add(new Shot(level, new Vector2(Position.X - 10, Position.Y - 20), side));
                        shootInterval = 0.2f;
                    }
                }
            }
            

        }

        public void ApplyPhysics(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 previousPosition = Position;

            // Base velocity is a combination of horizontal movement control and
            // acceleration downward due to gravity.
            velocity.X += movement * MoveAcceleration * elapsed;
            velocity.Y = MathHelper.Clamp(velocity.Y + GravityAcceleration * elapsed, -MaxFallSpeed, MaxFallSpeed);

            velocity.Y = DoJump(velocity.Y, gameTime);

            // Apply pseudo-drag horizontally.
            if (isOnGround)
                velocity.X *= GroundDragFactor;
            else
                velocity.X *= AirDragFactor;

            // Prevent the player from running faster than his top speed.            
            velocity.X = MathHelper.Clamp(velocity.X, -MaxMoveSpeed, MaxMoveSpeed);

            // Apply velocity.
            Position += velocity * elapsed;
            Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));

            // If the player is now colliding with the level, separate them.
            HandleCollisions();

            // If the collision stopped us from moving, reset the velocity to zero.
            if (Position.X == previousPosition.X)
                velocity.X = 0;

            if (Position.Y == previousPosition.Y)
                velocity.Y = 0;
        }

        private float DoJump(float velocityY, GameTime gameTime)
        {
            // If the player wants to jump
            if (isJumping)
            {
                // Begin or continue a jump
                if ((!wasJumping && isOnGround) || jumpTime > 0.0f)
                {
                    //if (jumpTime == 0.0f)
                        //jumpSound.Play();

                    jumpTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    //sprite.PlayAnimation(jumpAnimation);
                }

                // If we are in the ascent of the jump
                if (0.0f < jumpTime && jumpTime <= MaxJumpTime)
                {
                    // Fully override the vertical velocity with a power curve that gives players more control over the top of the jump
                    velocityY = JumpLaunchVelocity * (1.0f - (float)Math.Pow(jumpTime / MaxJumpTime, JumpControlPower));
                }
                else
                {
                    // Reached the apex of the jump
                    jumpTime = 0.0f;
                }
            }
            else
            {
                // Continues not jumping or cancels a jump in progress
                jumpTime = 0.0f;
            }
            wasJumping = isJumping;

            return velocityY;
        }

        private void HandleCollisions()
        {
            // Get the player's bounding rectangle and find neighboring tiles.
            Rectangle bounds = Bounding;
            int leftTile = (int)Math.Floor((float)bounds.Left / level.tileSet.Width);
            int rightTile = (int)Math.Ceiling(((float)bounds.Right / level.tileSet.Width)) -1;
            int topTile = (int)Math.Floor((float)bounds.Top / level.tileSet.Height);
            int bottomTile = (int)Math.Ceiling(((float)bounds.Bottom / level.tileSet.Height)) -1;

            // Reset flag to search for ground collision.
            isOnGround = false;

            // For each potentially colliding tile,
            for (int y = topTile; y <= bottomTile; ++y)
            {
                for (int x = leftTile; x <= rightTile; ++x)
                {
                    // If this tile is collidable,
                    TileCollision collision = level.GetCollision(x, y);
                    if (collision != TileCollision.Passable) {
                        // Determine collision depth (with direction) and magnitude.
                        Rectangle tileBounds = level.GetBounds(x, y);
                        Vector2 depth = RectUtils.GetIntersectionDepth(bounds, tileBounds);
                        if (depth != Vector2.Zero)
                        {
                            float absDepthX = Math.Abs(depth.X);
                            float absDepthY = Math.Abs(depth.Y);

                            // Resolve the collision along the shallow axis.
                            if (absDepthY < absDepthX || collision == TileCollision.Platform)
                            {
                                // If we crossed the top of a tile, we are on the ground.
                                if (previousBottom <= tileBounds.Top)
                                    isOnGround = true;

                                // Check if hit the roof
                                if (depth.Y > 0)
                                    jumpTime = MaxJumpTime;

                                // Ignore platforms, unless we are on the ground.
                                if (collision == TileCollision.Impassable || isOnGround)
                                {
                                    // Resolve the collision along the Y axis.
                                    Position = new Vector2(Position.X, Position.Y + depth.Y);
                                    
                                    // Perform further collisions with the new bounds.
                                    bounds = Bounding;
                                }
                            }
                            else if (collision == TileCollision.Impassable) // Ignore platforms.
                            {
                                // Resolve the collision along the X axis.
                                Position = new Vector2(Position.X + depth.X, Position.Y);

                                // Perform further collisions with the new bounds.
                                bounds = Bounding;
                            }
                        }
                    }
                }
            }

            // Save the new bounds bottom.
            previousBottom = bounds.Bottom;
        }

        public float Speed {
            get { return MaxMoveSpeed; }
            set { MaxMoveSpeed = value; }
        }

        public float Acceleration {
            get { return MoveAcceleration; }
            set { MoveAcceleration = value; }
        }

        public float Jump {
            get { return MaxJumpTime; }
            set { MaxJumpTime = value; }
        }

        public bool onGround {
            get { return isOnGround; }
        }

        public bool Weapon {
            get { return canShoot; }
            set { canShoot = value; }
        }

        public bool IsAlive {
            get { return isAlive; }
            set { isAlive = value; }
        }
    }
}
