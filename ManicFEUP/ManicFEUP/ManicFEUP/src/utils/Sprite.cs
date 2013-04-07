using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ManicFEUP
{
    class Sprite
    {
        private Texture2D texture;  // Textura com as frames do sprite
        private Vector2 origin;     // vetor de origem do frame
        private int frameWidth;     // largura do frame
        private int frameHeight;    // altura do frame
        private int frameNumber;    // Numero toral de frames da texture
        private int frameIndex;     // Frame atual
        private float frameTime;    // Tempo de cada frame
        private float time;         // tempo para calcular animacao

        private bool animate;       
        private int firstFrame;
        private int lastFrame;

        public Vector2 Origin
        {
            get { return origin; }
        }
        public int Number
        { 
            get { return frameNumber; } 
        }
        public int Index
        {
            get { return frameIndex; }
            set { frameIndex = value; }
        }
        public float Time
        {
            get { return frameTime; }
            set { frameTime = value; }
        }

        public Sprite(Texture2D texture, int width, int height, int number, Vector2 origin)
        {
            this.texture = texture;
            frameWidth = width;
            frameHeight = height;
            frameNumber = number;
            this.origin = origin;
            frameIndex = 0;
            frameTime = 0.0f;
            time = 0.0f;
            animate = false;
            firstFrame = 0;
            lastFrame = number - 1;
        }

        public void ResetAnim()
        {
            this.frameIndex = this.firstFrame;
        }

        public void SetAnim(bool anim)
        {
            frameIndex = firstFrame;
            animate = anim;
        }

        public void SetAnimLoop(int firstFrame, int lastFrame, float frameTime)
        {
            this.firstFrame = firstFrame;
            this.lastFrame = lastFrame;
            this.frameTime = frameTime;

            if (frameIndex < firstFrame || frameIndex > lastFrame)
                frameIndex = firstFrame;

            if (lastFrame > firstFrame)
                animate = true;
            else
                animate = false;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, Color? c = null)
        {
            if (animate)
            {
                //Calcular o tempo da animacao
                time += (float)gameTime.ElapsedGameTime.TotalSeconds;
                while (time > frameTime)
                {
                    time -= frameTime;
                    // Avanca para o proximo frame
                    frameIndex++;
                    if (frameIndex > lastFrame)
                        frameIndex = firstFrame;
                }
            }

            Color color = c.HasValue ? c.Value : Color.White;

            Rectangle rect = new Rectangle(frameIndex * frameWidth, 0, frameWidth, frameHeight);
            spriteBatch.Draw(texture, position - origin, rect, color);

        }

    }
}
