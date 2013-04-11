using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ManicFEUP
{
    class SceneMenu : Scene
    {
        private Texture2D background;
        private Vector2 bgPos;

        public SceneMenu(GameServiceContainer Services) : base(Services)
        {

        }

        public override void Load() {
            this.background = Content.Load<Texture2D>("spr/sprMenu");
            bgPos = new Vector2(0, 0);

            try
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(Content.Load<Song>("snd/sndMusic"));
            }
            catch { }
        }

        public override bool Update(GameTime gameTime, KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Space))
                return false;

            return true;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, bgPos, Color.White);
        }
    }
}
