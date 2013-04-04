using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ManicFEUP
{
    abstract class Scene // Classe abstrata para os leveis
    {
        protected int width;
        protected int height;

        public ContentManager Content;

        public Scene(IServiceProvider serviceProvider)
        {
            Content = new ContentManager(serviceProvider, "Content");
        }

        public abstract void Update(GameTime gameTime, KeyboardState keyboardState);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

    }
}
