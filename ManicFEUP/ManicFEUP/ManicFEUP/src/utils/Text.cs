using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ManicFEUP {
    class Text {
        private SpriteFont font;
        private Vector2 position;
        private String fontName;

        public Text(String font, int x = 0, int y = 0) {
            this.fontName = font;
            this.position = new Vector2(x, y);
        }

        public void Load(ContentManager content) {
            font = content.Load<SpriteFont>(fontName);
        }

        public void Draw(SpriteBatch spriteBatch, String text, Color color) {
            spriteBatch.DrawString(font, text, position, color);
        }

    
    }
}
