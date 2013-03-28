using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace ManicFEUP
{
    /// <summary>
    /// This is the main type for the game
    /// </summary>
    public class GameCore : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private KeyboardState keyboardState;

        private int screenWidth;
        private int screenHeight;

        private Scene level;

        public GameCore()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            screenWidth = graphics.PreferredBackBufferWidth;
            screenHeight = graphics.PreferredBackBufferHeight;
        }

        /// <summary>
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // Load Fonts
            // Load Textures
            // Load Sounds

            // Load the level.
            using (Stream fileStream = TitleContainer.OpenStream("Content/Level1.txt"))
                level = new SceneLevel(Services, fileStream);
        }

        /// <summary>
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            HandleInput();      // Handle Input
            level.Update(gameTime, keyboardState);  // Update Level
            base.Update(gameTime);
        }

        /// <summary>
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);  // Clear background to black
            spriteBatch.Begin();
            level.Draw(gameTime, spriteBatch);  // Draw Level
            spriteBatch.End();
            base.Draw(gameTime);
        }


        private void HandleInput()
        {
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();
        }
    }
}
