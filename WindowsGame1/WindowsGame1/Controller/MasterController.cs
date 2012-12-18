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

        Texture2D smokeTexture;

        View.SmokeSystem smokeSystem;
        View.SmokeSystem smokeSystem2;
        private View.Camera m_camera;
        private View.SmokeSystem smokeSystem3;

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

            smokeTexture = Content.Load<Texture2D>("smoke");

            smokeSystem = new View.SmokeSystem(new Vector2(4, 4), smokeTexture, 1f);
            smokeSystem2 = new View.SmokeSystem(new Vector2(6, 8), smokeTexture, 5f);
            smokeSystem3 = new View.SmokeSystem(new Vector2(2, 9), smokeTexture, 9f);
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

            //if(Mouse.GetState().LeftButton == ButtonState.Pressed)
            //{
            //    Console.WriteLine("Pressed");
            //    smokeSystem = new View.SmokeSystem(m_camera.convertToModel(Mouse.GetState().X, Mouse.GetState().Y), smokeTexture);
            //}

            // TODO: Add your update logic here

            //splitterSystem.UpdateAndDraw((float)gameTime.ElapsedGameTime.TotalSeconds, spriteBatch, new View.Camera(graphics.GraphicsDevice.Viewport));

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            if (smokeSystem == null)
                return;
            // TODO: Add your drawing code here
            smokeSystem.UpdateAndDraw((float)gameTime.ElapsedGameTime.TotalSeconds, spriteBatch, m_camera);
            smokeSystem2.UpdateAndDraw((float)gameTime.ElapsedGameTime.TotalSeconds, spriteBatch, m_camera);
            smokeSystem3.UpdateAndDraw((float)gameTime.ElapsedGameTime.TotalSeconds, spriteBatch, m_camera);


            base.Draw(gameTime);
        }
    }
}
