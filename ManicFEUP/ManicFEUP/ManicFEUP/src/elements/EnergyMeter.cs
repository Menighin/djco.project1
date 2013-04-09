using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ManicFEUP {
    class EnergyMeter {
        private Texture2D sprMeter;
        private Texture2D[] rects;
        private Vector2 Position;
        private const float TimeInterval = 0.1f;

        public SceneLevel level;
        public Rectangle life;
        public int FullLife;

        public float timeInterval = TimeInterval;

        public EnergyMeter(SceneLevel level, Vector2 position, int life) {
            this.level = level;
            this.Position = position;
            this.life = new Rectangle((int)position.X, (int)position.Y, life, 20);
            this.FullLife = life;
            rects = new Texture2D[4];

            LoadContent();
        }

        public void LoadContent() {
            sprMeter = level.Content.Load<Texture2D>("sprEnergyMeter");
            rects[0] = level.Content.Load<Texture2D>("sprEnergyMeter");
            rects[1] = level.Content.Load<Texture2D>("sprEnergyMeter");
            rects[2] = level.Content.Load<Texture2D>("sprEnergyMeter");
            rects[3] = level.Content.Load<Texture2D>("sprEnergyMeter");
        }

        public void Update(GameTime gameTime, Player player) {
            timeInterval -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeInterval < 0) {
                life.Width -= 1;
                timeInterval = TimeInterval;
                if (life.Width <= 0) {
                    player.IsAlive = false;
                    life.Width = FullLife;
                }
            }
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.Draw(sprMeter, life, Color.LightYellow);
            spriteBatch.Draw(rects[0], new Rectangle((int)Position.X, (int)Position.Y, FullLife, 3), Color.Blue);
            spriteBatch.Draw(rects[1], new Rectangle((int)Position.X, (int)Position.Y + 20, FullLife, 3), Color.Blue);
            spriteBatch.Draw(rects[2], new Rectangle((int)Position.X + FullLife, (int)Position.Y, 3, 23), Color.Blue);
            spriteBatch.Draw(rects[3], new Rectangle((int)Position.X, (int)Position.Y, 3, 23), Color.Blue);

        }

        public float X {
            get { return Position.X; }
        }

        public float Y {
            get { return Position.Y; }
        }
    }
}
