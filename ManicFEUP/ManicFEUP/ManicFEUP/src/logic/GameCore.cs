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

            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load Textures
            // Load Sounds

            LoadLevel(0);   //Load the menu
        }

        protected void LoadLevel(int levelNumber)
        {
            switch (levelNumber)
            {
                case 0:
                    level = new SceneMenu(Services);
                    level.Load();
                    break;
                case 1:
                    using (Stream fileStream = TitleContainer.OpenStream("Content/Level1.txt"))
                        level = new SceneLevel(Services, fileStream);
                    level.Load();
                    break;
            }


        }

        void Window_ClientSizeChanged(object sender, EventArgs e) {
           
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            HandleInput();      // Handle Input
            if (!level.Update(gameTime, keyboardState))  // Update Level
            {
                if (level is SceneMenu)
                    LoadLevel(1);
                else
                    LoadLevel(0);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(new Color (0.1f, 0.1f, 0.1f));  // Clear background to black
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
