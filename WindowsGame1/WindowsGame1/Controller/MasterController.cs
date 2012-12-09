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
using WindowsGame1.Model;


namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MasterController : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager m_graphics;
        SpriteBatch m_spriteBatch;
        View.ChessView m_view = new View.ChessView();
        Model.ChessGame m_game = new Model.ChessGame();
        Camera m_camera;

        KeyboardState oldState;

        public MasterController()
        {
            m_graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";


            //Set screen width and height
            m_graphics.IsFullScreen = false;
            m_graphics.PreferredBackBufferWidth = 320;
            m_graphics.PreferredBackBufferHeight = 240;

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

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            m_spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            m_view.LoadContent(Content, m_graphics);
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

            // TODO: Add your update logic here
            UpdateInput();

            base.Update(gameTime);
        }

        private void UpdateInput()
        {
            KeyboardState newState = Keyboard.GetState();
            m_camera = new Camera(m_graphics.GraphicsDevice.Viewport);

            // Is the SPACE key down?
            if (newState.IsKeyDown(Keys.Space))
            {
                // If not down last update, key has just been pressed.
                if (!oldState.IsKeyDown(Keys.Space))
                {
                    if (m_game.getPlayerTurn() == ChessGame.PlayerTurn.White)
                    {
                        m_game.setPlayerTurn(ChessGame.PlayerTurn.Black);
                    }
                    else if (m_game.getPlayerTurn() == ChessGame.PlayerTurn.Black)
                    {
                        m_game.setPlayerTurn(ChessGame.PlayerTurn.White);
                    }
                }
            }

            // Is the Enter key down?
            if (newState.IsKeyDown(Keys.Enter))
            {
                // If not down last update, key has just been pressed.
                if (!oldState.IsKeyDown(Keys.Enter))
                {
                    if (m_graphics.PreferredBackBufferWidth == 640)
                    {
                        m_graphics.PreferredBackBufferWidth = 320;
                        m_graphics.PreferredBackBufferHeight = 240;
                        m_graphics.ApplyChanges();
                    }
                    else
                    {
                        m_graphics.PreferredBackBufferWidth = 640;
                        m_graphics.PreferredBackBufferHeight = 640;
                        m_graphics.ApplyChanges();
                    }
                }
            }
            
            // Update saved state.
            oldState = newState;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime a_gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            m_view.Draw(m_spriteBatch, m_camera, m_game);
                
            base.Draw(a_gameTime);
        }
    }
}
