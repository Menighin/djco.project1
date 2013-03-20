using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Game_Test_2 {

    class Negritude : Sprite {

        //Constantes
        const string NEGRITUDE_ASSETNAME = "negritude";
        const int START_POSITION_X = 0;
        const int START_POSITION_Y = 0;
        const int NEGRITUDE_SPEED = 160;
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;
        const float GRAVITY = 9.8f;

        //Estados representando ações do herói
        enum State {
            Null,
            Walking,
            Jumping,
            Falling
        }

        //Propriedades do herói
        State mCurrentState = State.Walking;
        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;
        Vector2 mStartingPosition = Vector2.Zero;

        KeyboardState mPreviousKeyboardState;

        //Carrega sprite
        public void LoadContent(ContentManager theContentManager) {
            Position = new Vector2(GlobalEnvironment.ScreenWidth / 2, START_POSITION_Y);
            base.LoadContent(theContentManager, NEGRITUDE_ASSETNAME);
        }

        //Update das posições dos objetos
        public void Update(GameTime theGameTime) {
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();

            UpdateFall(aCurrentKeyboardState);
            UpdateMovement(aCurrentKeyboardState);
            UpdateJump(aCurrentKeyboardState);

            mPreviousKeyboardState = aCurrentKeyboardState;

            base.Update(theGameTime, mSpeed, mDirection);
        }

        //Detecta teclas para movimentos laterais
        private void UpdateMovement(KeyboardState aCurrentKeyboardState) {
            if (mCurrentState == State.Walking) {
                mSpeed = Vector2.Zero;
                mDirection = Vector2.Zero;

                if (aCurrentKeyboardState.IsKeyDown(Keys.Left) == true && this.Position.X > 0) {
                    mSpeed.X = NEGRITUDE_SPEED;
                    mDirection.X = MOVE_LEFT;
                }
                else if (aCurrentKeyboardState.IsKeyDown(Keys.Right) == true && this.Position.X < GlobalEnvironment.ScreenWidth - Size.Width) {
                    mSpeed.X = NEGRITUDE_SPEED;
                    mDirection.X = MOVE_RIGHT;
                }
            }
        }

        //Detecta pulo
        private void UpdateJump(KeyboardState aCurrentKeyboardState) {
            if (mCurrentState == State.Walking && aCurrentKeyboardState.IsKeyDown(Keys.Space) == true && mPreviousKeyboardState.IsKeyDown(Keys.Space) == false) {
                Jump();
            }

            if (mCurrentState == State.Jumping) {
                
                mSpeed.Y -= GRAVITY;

                //De pulo passa a cair
                if (mSpeed.Y <= 0) {
                    Vector2 speedTemp = mSpeed; 
                    Fall();
                    mSpeed.X = speedTemp.X;
                    mSpeed.Y = speedTemp.Y;
                }

                if (aCurrentKeyboardState.IsKeyDown(Keys.Left) == true && this.Position.X > 0) {
                    mSpeed.X = NEGRITUDE_SPEED;
                    mDirection.X = MOVE_LEFT;
                }
                else if (aCurrentKeyboardState.IsKeyDown(Keys.Right) == true && this.Position.X < GlobalEnvironment.ScreenWidth - Size.Width) {
                    mSpeed.X = NEGRITUDE_SPEED;
                    mDirection.X = MOVE_RIGHT;
                }

                if (this.Position.X <= -1) {
                    mSpeed.X = 0;
                    this.Position.X = 0;
                }
                else if (this.Position.X >= GlobalEnvironment.ScreenWidth - Size.Width + 1) {
                    mSpeed.X = 0;
                    this.Position.X = GlobalEnvironment.ScreenWidth - Size.Width;
                }

                if (Position.Y > mStartingPosition.Y) {
                    Position.Y = mStartingPosition.Y;
                    mCurrentState = State.Walking;
                    mDirection = Vector2.Zero;
                }
            }
        }

        //Inicia pulo
        private void Jump() {
            if (mCurrentState != State.Jumping) {
                mCurrentState = State.Jumping;
                mStartingPosition = Position;
                mDirection.Y = MOVE_UP;
                mSpeed = new Vector2(NEGRITUDE_SPEED, 2*NEGRITUDE_SPEED);
            }
        }

        //Detecta queda
        private void UpdateFall(KeyboardState aCurrentKeyboardState) {
            if (mCurrentState == State.Walking && !GroundCollision()) {
                Fall();
            }

            if (mCurrentState == State.Falling) {
                mSpeed.Y += GRAVITY;

                if (aCurrentKeyboardState.IsKeyDown(Keys.Left) == true && this.Position.X > 0) {
                    mSpeed.X = NEGRITUDE_SPEED;
                    mDirection.X = MOVE_LEFT;
                }
                else if (aCurrentKeyboardState.IsKeyDown(Keys.Right) == true && this.Position.X < GlobalEnvironment.ScreenWidth - Size.Width) {
                    mSpeed.X = NEGRITUDE_SPEED;
                    mDirection.X = MOVE_RIGHT;
                }

                if (this.Position.X <= -1) {
                    mSpeed.X = 0;
                    this.Position.X = 0;
                }
                else if (this.Position.X >= GlobalEnvironment.ScreenWidth - Size.Width + 1) {
                    mSpeed.X = 0;
                    this.Position.X = GlobalEnvironment.ScreenWidth - Size.Width;
                }

                if (GroundCollision() || PlatformCollision() == State.Walking)
                    mCurrentState = State.Walking;
            }
        }

        //Começa a cair
        private void Fall() {
            if (mCurrentState != State.Falling) {
                mCurrentState = State.Falling;
                mDirection.Y = MOVE_DOWN;
                mSpeed = new Vector2(NEGRITUDE_SPEED, NEGRITUDE_SPEED);
            }
        }

        //Detecta colisao com limite inferior da tela
        private bool GroundCollision() {
            if (this.Position.Y + this.Size.Height < GlobalEnvironment.ScreenHeight) 
                return false;

            this.Position.Y = GlobalEnvironment.ScreenHeight - this.Size.Height;
            return true;
        }

        //Detecta colisão com plataformas
        private State PlatformCollision() {
            List<Platform> platform = GlobalEnvironment.getPlatformList();

            foreach (Platform p in platform) {
                if (this.Rect.Intersects(p.Rect)) {
                    if (this.Rect.Bottom < p.Rect.Bottom) {
                        this.Position.Y = p.Rect.Y - this.Rect.Height + 1;
                        return State.Walking;
                    }
                    /*else if (this.Rect.Top > p.Rect.Top)
                        this.Position.Y = p.Rect.Y + p.Rect.Height;
                    else if (this.Rect.Right > p.Rect.Right && this.Rect.Left < p.Rect.Right) {
                        this.Position.X = p.Rect.Right + 1;
                        this.mSpeed.X = 0;
                        return State.Falling;
                    }
                    else if (this.Rect.Left < p.Rect.Left && this.Rect.Right > p.Rect.Left) {
                        this.Position.X = p.Rect.Left - 1 - this.Rect.Width;
                        this.mSpeed.X = 0;
                        return State.Falling;
                    }*/

                    //return State.Walking;
                }
            }

            return State.Null;
        }

    }

}
