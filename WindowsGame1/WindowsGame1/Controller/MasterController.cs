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
using WindowsGame1.Controller;


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
        KeyboardState oldState;
        MenuGUI m_gui;

        View.Camera camera;
        Model.Game game;
        View.GameView gameView;


        enum GameState
        {
            TitleScreen = 0,
            MainMenu,
            GameStarted,
            GamePaused,
            GameEnded,
        }

        GameState currentGameState = GameState.TitleScreen;
        Texture2D splashTexture;
        private SoundEffect backgroundMusic;
        private SoundEffect titleMusic;
        SoundEffectInstance titleInstance;
        SoundEffectInstance playingInstance;


        private bool play = true;
        SpriteFont font;
        bool countingDown = true;

        Color titleColor;
        private float fadeTime;
        private float maxFadeTime = 1.5f;
        private bool isEditingLevel;
        private MouseState m_oldState;
        bool gamePaused = false;

        public MasterController()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
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

            fadeTime = maxFadeTime;
            this.Window.Title = "In Search of The Disco";
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

            // TODO: use this.Content to load your game content here
            splashTexture = this.Content.Load<Texture2D>("splash");
            font = Content.Load<SpriteFont>("fontTexture");

            backgroundMusic = Content.Load<SoundEffect>("discoBackground");
            playingInstance = backgroundMusic.CreateInstance();
            playingInstance.IsLooped = true;
            playingInstance.Pan = .5f;

            titleMusic = Content.Load<SoundEffect>("titleMusic");
            titleInstance = titleMusic.CreateInstance();
            titleInstance.IsLooped = true;

            //Load content in MenuGUI
            m_gui = new MenuGUI(spriteBatch, Content);

            camera = new View.Camera(graphics.GraphicsDevice.Viewport);

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
            KeyboardState newState = Keyboard.GetState();


            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();


            if (currentGameState == GameState.MainMenu)
            {
                if (play == true)
                {
                    titleInstance.Play();
                    play = false;
                }
            }
            else if (currentGameState == GameState.TitleScreen)
            {
                if (play == true)
                {
                    titleInstance.Play();
                    play = false;
                }

                if (fadeTime < 0)
                    countingDown = false;

                if (fadeTime >= maxFadeTime)
                    countingDown = true;

                if (countingDown == true)
                    fadeTime -= (float)gameTime.ElapsedGameTime.TotalSeconds * 2;

                if (countingDown == false)
                    fadeTime += (float)gameTime.ElapsedGameTime.TotalSeconds * 2;

                float a = fadeTime / maxFadeTime;
                titleColor = new Color(a, a, a, a);

                //If any keyed pressed
                if (newState.GetPressedKeys().Length > 0)
                {
                    currentGameState = GameState.MainMenu;
                }  
            }
            else if(currentGameState == GameState.GameStarted)
            {
                if (game.gameOver == true)
                {
                    Console.WriteLine("Game Over");
                    currentGameState = GameState.MainMenu;
                }

                if(game.getLevel().isExitLevel())
                {
                    play = true;
                    playingInstance.Stop();
                    game.getLevel().nextLevel();
                    gameView.fadeTime = gameView.maxFadeTime;
                }

                if (isEditingLevel)
                {
                    MouseState mouse = Mouse.GetState();
                    Vector2 mousePos = new Vector2(mouse.X / camera.getScale() + camera.getModelTopLeftPosition().X, mouse.Y / camera.getScale() + camera.getModelTopLeftPosition().Y);

                    Vector2 mouseSize = new Vector2(.10f, .10f);
                    FloatRectangle mouseRect = FloatRectangle.createFromTopLeft(mousePos, mouseSize);

                    Console.WriteLine(camera.getModelTopLeftPosition().X);

                    if (mouse.LeftButton == ButtonState.Released && m_oldState.LeftButton == ButtonState.Pressed)
                    {
                        game.getLevel().ToggleTile(mouseRect);
                    }

                    m_oldState = mouse;
                }

                if (play == true)
                {
                    titleInstance.Stop();
                    playingInstance.Play();
                    play = false;
                }

                // Is the Left key down?
                if (newState.IsKeyDown(Keys.Left))
                {
                    // If not down last update, key has just been pressed.
                    if (!oldState.IsKeyDown(Keys.Left))
                    {
                        if (game.getPlayer().getCurrentState() != Player.State.Falling)
                        {
                            game.getPlayer().setCurrentState(Player.State.Walking);
                        }
                        game.getPlayer().setCurrentDirection(Player.Direction.Left);
                        game.goLeft();
                    }
                }

                if (newState.IsKeyDown(Keys.Right))
                {
                    // If not down last update, key has just been pressed.
                    if (!oldState.IsKeyDown(Keys.Right))
                    {
                        if (game.getPlayer().getCurrentState() != Player.State.Falling)
                        {
                            game.getPlayer().setCurrentState(Player.State.Walking);
                        }
                        game.getPlayer().setCurrentDirection(Player.Direction.Right);
                        game.goRight();
                    }
                }

                if (newState.IsKeyDown(Keys.Space))
                {
                    // If not down last update, key has just been pressed.
                    if (!oldState.IsKeyDown(Keys.Space))
                    {
                        game.getPlayer().setCurrentState(Player.State.Jumping);
                        game.playerJump();
                    }
                }

                // Is the F1 key down?
                if (newState.IsKeyDown(Keys.F1))
                {
                    // If not down last update, key has just been pressed.
                    if (!oldState.IsKeyDown(Keys.F1))
                    {
                        Console.WriteLine("editing");
                        isEditingLevel = true;
                    }
                }

                // Is the game paused
                if (newState.IsKeyDown(Keys.Escape) || newState.IsKeyDown(Keys.P))
                {
                    Console.WriteLine("pause");
                    if (gamePaused == true)
                    {
                        playingInstance.Play();
                        gamePaused = false;
                    }
                    else if (gamePaused == false)
                    {
                        playingInstance.Stop();
                        gamePaused = true;
                    }
                }
                Console.WriteLine(gamePaused);

                if (gamePaused == false)
                {
                    gameView.UpdateView((float)gameTime.ElapsedGameTime.TotalSeconds);
                    game.UpdateGame((float)gameTime.ElapsedGameTime.TotalSeconds);
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);


            if (currentGameState == GameState.TitleScreen)
            {
                
                //Räkna ut position för knappen
                Rectangle destinationRectangle = new Rectangle(0, 0, camera.getScreenWidth(), camera.getScreenHeight());

                spriteBatch.Begin();
                spriteBatch.Draw(splashTexture, destinationRectangle, Color.White);
                spriteBatch.DrawString(font, "(Press any key)", new Vector2(camera.getScreenWidth() - 270, camera.getScreenHeight() - 40), titleColor);
                spriteBatch.End();

            }
            else if (currentGameState == GameState.MainMenu)
            {
                //Räkna ut position för knappen
                Rectangle destinationRectangle = new Rectangle(0, 0, camera.getScreenWidth(), camera.getScreenHeight());

                spriteBatch.Begin();
                spriteBatch.Draw(splashTexture, destinationRectangle, Color.White);
                spriteBatch.End();

                int menuX = camera.getScreenWidth() / 5;
                int menuY = camera.getScreenHeight() - camera.getScreenHeight() / 4;
                int buttonSeparation = 45;
                if (m_gui.DoButton(Mouse.GetState(), "New game", menuX, menuY))
                {
                    play = true;
                    Console.WriteLine("New Game");

                    game = new Model.Game(camera);
                    gameView = new View.GameView(spriteBatch, camera, game.getLevel(), game.getPlayer());

                    game.LoadContent(Content);
                    gameView.LoadContent(Content);

                    currentGameState = GameState.GameStarted;
                }
                if (m_gui.DoButton(Mouse.GetState(), "Exit", menuX, menuY += buttonSeparation))
                {
                    this.Exit();
                }

                m_gui.setOldState(Mouse.GetState());

            }
            else if (currentGameState == GameState.GameStarted)
            {
                //update camera
                camera.centerOn(game.getPlayerPosition(),
                                  new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),
                                  new Vector2(Model.Level.LEVEL_WIDTH, Model.Level.LEVEL_HEIGHT));

                camera.setZoom(64);

                gameView.DrawLevel(graphics.GraphicsDevice, game.getPlayerPosition(), game.getLevel().getLevelEnemies());
            }

            base.Draw(gameTime);
        }
    }
}
