using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Game_Test_2 {

    class Negritude : Sprite {

        const string NEGRITUDE_ASSETNAME = "negritude";
        const int START_POSITION_X = 125;
        const int START_POSITION_Y = 245;
        const int NEGRITUDE_SPEED = 160;
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;
        const float GRAVITY = 9.8f;

        enum State {
            Walking,
            Jumping
        }

        State mCurrentState = State.Walking;
        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;
        Vector2 mStartingPosition = Vector2.Zero;

        KeyboardState mPreviousKeyboardState;

        public void LoadContent(ContentManager theContentManager) {
            Position = new Vector2(START_POSITION_X, START_POSITION_Y);
            base.LoadContent(theContentManager, NEGRITUDE_ASSETNAME);
        }

        public void Update(GameTime theGameTime) {
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();

            UpdateMovement(aCurrentKeyboardState);
            UpdateJump(aCurrentKeyboardState);
            GravityActs();

            mPreviousKeyboardState = aCurrentKeyboardState;

            base.Update(theGameTime, mSpeed, mDirection);
        }

        private void UpdateMovement(KeyboardState aCurrentKeyboardState) {
            if (mCurrentState == State.Walking) {
                mSpeed = Vector2.Zero;
                mDirection = Vector2.Zero;

                if (aCurrentKeyboardState.IsKeyDown(Keys.Left) == true) {
                    mSpeed.X = NEGRITUDE_SPEED;
                    mDirection.X = MOVE_LEFT;
                }
                else if (aCurrentKeyboardState.IsKeyDown(Keys.Right) == true) {
                    mSpeed.X = NEGRITUDE_SPEED;
                    mDirection.X = MOVE_RIGHT;
                }
            }
        }

        private void UpdateJump(KeyboardState aCurrentKeyboardState) {
            if (mCurrentState == State.Walking && aCurrentKeyboardState.IsKeyDown(Keys.Space) == true && mPreviousKeyboardState.IsKeyDown(Keys.Space) == false) {
                Jump();
            }

            if (mCurrentState == State.Jumping) {

                mSpeed.Y -= GRAVITY;

                if (aCurrentKeyboardState.IsKeyDown(Keys.Left) == true) {
                    mSpeed.X = NEGRITUDE_SPEED;
                    mDirection.X = MOVE_LEFT;
                }
                else if (aCurrentKeyboardState.IsKeyDown(Keys.Right) == true) {
                    mSpeed.X = NEGRITUDE_SPEED;
                    mDirection.X = MOVE_RIGHT;
                }

                if (Position.Y > mStartingPosition.Y) {
                    Position.Y = mStartingPosition.Y;
                    mCurrentState = State.Walking;
                    mDirection = Vector2.Zero;
                }
            }
        }

        private void Jump() {
            if (mCurrentState != State.Jumping) {
                mCurrentState = State.Jumping;
                mStartingPosition = Position;
                mDirection.Y = MOVE_UP;
                mSpeed = new Vector2(NEGRITUDE_SPEED, 2*NEGRITUDE_SPEED);
            }
        }


        private void GravityActs() {
            if (mCurrentState == State.Walking) {

            }
        }

    }

}
