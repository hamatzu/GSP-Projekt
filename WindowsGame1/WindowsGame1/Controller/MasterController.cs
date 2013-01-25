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
using WindowsGame1.View;


//I am adding some comment code to test GIT!

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MasterController : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private View.Camera m_camera;
        private View.ExplosionSystem explosionSystem;
        private ExplosionSystem explosionSystem2;
        private ExplosionSystem explosionSystem3;
        private double currentDuration;
        private double totalDuration = 3;
        private double totalDuration2 = 6;
        
        private double currentDuration2;

        public MasterController()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";


            graphics.PreferredBackBufferWidth = 640;
            graphics.PreferredBackBufferHeight = 640;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            m_camera = new View.Camera(graphics.GraphicsDevice.Viewport);
            this.IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            explosionSystem = new ExplosionSystem(new Vector2(4, 4));
            explosionSystem2 = new ExplosionSystem(new Vector2(6, 6));
            explosionSystem3 = new ExplosionSystem(new Vector2(3, 5));
            explosionSystem.LoadContent(Content);
            explosionSystem2.LoadContent(Content);
            explosionSystem3.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            if (explosionSystem == null)
                return;

            spriteBatch.Begin();
            explosionSystem.UpdateExplosion((float)gameTime.ElapsedGameTime.TotalSeconds, spriteBatch, m_camera);

            currentDuration += gameTime.ElapsedGameTime.TotalSeconds;


            if (currentDuration > totalDuration)
            {
                explosionSystem2.UpdateExplosion((float)gameTime.ElapsedGameTime.TotalSeconds, spriteBatch, m_camera);
                currentDuration = totalDuration;
            }

            currentDuration2 += gameTime.ElapsedGameTime.TotalSeconds;

            if (currentDuration2 > totalDuration2)
            {
                explosionSystem3.UpdateExplosion((float)gameTime.ElapsedGameTime.TotalSeconds, spriteBatch, m_camera);
                currentDuration2 = totalDuration2;
            }
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
