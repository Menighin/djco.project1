using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;

namespace Game_Test_2 {
    class Platform : Sprite {

        const string PLATFORM_ASSETNAME = "assetlvl";
        const int ENLARGE = GlobalEnvironment.ENLARGE;
        Point frameSize = new Point (8, 8);
        Point currentFrame = new Point(1, 6);
        private int numberPlatforms;


        //Construtor
        public Platform(int x, int y, int n) {
            base.Position = new Vector2(x, y);
            numberPlatforms = n;
        }

        //Override LoadContent
        public void LoadContent(ContentManager theContentManager) {
            base.mSpriteTexture = theContentManager.Load<Texture2D>(PLATFORM_ASSETNAME);
            base.AssetName = PLATFORM_ASSETNAME;
            base.Size = new Rectangle(0, 0, frameSize.X, frameSize.Y);
            base.Rect = new Rectangle((int) Position.X, (int) Position.Y, frameSize.X * ENLARGE * numberPlatforms, frameSize.Y * ENLARGE);
        }

        //Override sprite Draw para desenhar frame
        public override void Draw(SpriteBatch theSpriteBatch) {
            for (int i = 0; i < numberPlatforms; i++) 
                theSpriteBatch.Draw(base.mSpriteTexture, new Vector2 (Position.X + i * frameSize.X * ENLARGE, Position.Y),
                    new Rectangle(currentFrame.X * frameSize.X + currentFrame.X - 1, currentFrame.Y * frameSize.Y + currentFrame.Y - 1,
                        frameSize.X, frameSize.Y),
                    Color.White, 0, Vector2.Zero, ENLARGE, SpriteEffects.None, 0);
        }

        public void Update(GameTime theGameTime) {
            base.Update(theGameTime, Vector2.Zero, Vector2.Zero);
        }

    }
}
