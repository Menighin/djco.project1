using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;

namespace Game_Test_2 {
    class Platform : Sprite {

        const string PLATFORM_ASSETNAME = "platform";

        //Construtor
        public Platform(int x, int y) {
            base.Position = new Vector2(x, y);
        }

        public void LoadContent(ContentManager theContentManager) {
            base.LoadContent(theContentManager, PLATFORM_ASSETNAME);
        }

        public void Update(GameTime theGameTime) {
            base.Update(theGameTime, Vector2.Zero, Vector2.Zero);
        }

    }
}
