using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Test_2 {

    class Sprite {

        public string AssetName;
        public Rectangle Size;
        private float mScale = 1.0f;

        public Vector2 Position = new Vector2(0, 0);    //The current position of the Sprite
        private Texture2D mSpriteTexture;               //The texture object used when drawing the sprite


        //Load the texture for the sprite using the Content Pipeline
        public void LoadContent(ContentManager theContentManager, string theAssetName) {
            mSpriteTexture = theContentManager.Load<Texture2D>(theAssetName);
            AssetName = theAssetName;
            Size = new Rectangle(0, 0, (int)(mSpriteTexture.Width * Scale), (int)(mSpriteTexture.Height * Scale));
        }

        //Draw the sprite to the screen
        public void Draw(SpriteBatch theSpriteBatch) {
            theSpriteBatch.Draw(mSpriteTexture, Position, new Rectangle(0, 0, mSpriteTexture.Width, mSpriteTexture.Height), Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }

        //Updaet
        public void Update(GameTime theGameTime, Vector2 theSpeed, Vector2 theDirection) {
            Position += theDirection * theSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
        }

        public float Scale {
            get { return mScale; }
            
            set {
                mScale = value;
                //Recalculate the Size of the Sprite with the new scale
                Size = new Rectangle(0, 0, (int)(mSpriteTexture.Width * Scale), (int)(mSpriteTexture.Height * Scale));
            }
        }

    }

}
