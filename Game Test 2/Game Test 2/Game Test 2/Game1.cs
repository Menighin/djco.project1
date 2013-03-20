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

namespace Game_Test_2 {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Negritude negritude;
        Platform p1;

        //Construtor
        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        //Método que inicializa
        protected override void Initialize() {
            GlobalEnvironment.ScreenHeight = graphics.PreferredBackBufferHeight;
            GlobalEnvironment.ScreenWidth = graphics.PreferredBackBufferWidth;
            negritude = new Negritude();

            GlobalEnvironment.addPlatform(new Platform((int)GlobalEnvironment.ScreenWidth / 2 - 400, (int)GlobalEnvironment.ScreenHeight - 160));
            GlobalEnvironment.addPlatform(new Platform ((int) GlobalEnvironment.ScreenWidth / 2, (int) GlobalEnvironment.ScreenHeight - 80));
            
            
            base.Initialize();
        }

        //Faz os loads dos recursos
        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            negritude.LoadContent(this.Content);
            //p1.LoadContent(this.Content);
            foreach (Platform p in GlobalEnvironment.getPlatformList()) {
                p.LoadContent(this.Content);
            }
        }

        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        //Faz update na cena
        protected override void Update(GameTime gameTime) {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            negritude.Update(gameTime);
            foreach (Platform p in GlobalEnvironment.getPlatformList()) {
                p.Update(gameTime);
            }

            base.Update(gameTime);
        }

        //Desenha
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            negritude.Draw(this.spriteBatch);
            foreach (Platform p in GlobalEnvironment.getPlatformList()) {
                p.Draw(this.spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
