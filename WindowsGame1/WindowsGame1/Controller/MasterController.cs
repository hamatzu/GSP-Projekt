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
using System.IO;
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
            NextLevel,
        }

        GameState currentGameState = GameState.TitleScreen;
        Texture2D splashTexture;
        private SoundEffect backgroundMusic;
        private SoundEffect titleMusic;
        SoundEffectInstance titleInstance;
        SoundEffectInstance playingInstance;

        SpriteFont font;
        bool countingDown = true;

        Color titleColor;
        private float fadeTime;
        private float maxFadeTime = 1.5f;
        bool gamePaused = false;
        private Texture2D dummyTexture;
        private SoundEffect doorEffect;
        private SoundEffectInstance doorInstance;
        private MenuGUI gui_mainMenu;
        private MenuGUI gui_pauseMenu;
        private MenuGUI gui_gameDoneMenu;

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
            this.IsMouseVisible = false;

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

            titleMusic = Content.Load<SoundEffect>("titleMusic");
            titleInstance = titleMusic.CreateInstance();
            titleInstance.IsLooped = true;

            doorEffect = Content.Load<SoundEffect>("door");
            doorInstance = doorEffect.CreateInstance();
            doorInstance.Pitch = 1f;
            doorInstance.Volume = .5f;

            dummyTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });

            // New Camera
            camera = new View.Camera(graphics.GraphicsDevice.Viewport);

            //Load MenuGUI for mainMenu
            gui_mainMenu = new MenuGUI(spriteBatch, Content, camera.getScreenWidth() / 5, camera.getScreenHeight() - camera.getScreenHeight() / 4);
            gui_mainMenu.addItem("New Game");
            gui_mainMenu.addItem("Exit");


            //Load menuGUI for pauseMenu
            gui_pauseMenu = new MenuGUI(spriteBatch, Content, camera.getScreenWidth() / 2, camera.getScreenHeight() / 3 + 40);
            gui_pauseMenu.addItem("Continue");
            gui_pauseMenu.addItem("Exit to main Menu");

            //Load menuGUI for gameDoneMenu
            gui_gameDoneMenu = new MenuGUI(spriteBatch, Content, camera.getScreenWidth() / 2, camera.getScreenHeight() / 3 + 40);
            gui_gameDoneMenu.addItem("New Game");
            gui_gameDoneMenu.addItem("Exit to main Menu");

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

            if (currentGameState == GameState.TitleScreen)
            {
                // Play theme music
                titleInstance.Play();

                // Update for fading text 
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
                if (newState.GetPressedKeys().Length > 0 && !newState.IsKeyDown(Keys.F9))
                {
                    if (oldState.GetPressedKeys().Length > 0)
                    {
                        currentGameState = GameState.MainMenu;
                    }
                }

                oldState = newState;
            }
            else if (currentGameState == GameState.MainMenu)
            {
                // Check menu input for main menu
                checkMenuInput(gui_mainMenu);
            }
            else if (currentGameState == GameState.GameStarted)
            {
                // Is the Escape key down?
                if (newState.IsKeyDown(Keys.Escape) && game.gameOver == false)
                {
                    if (!oldState.IsKeyDown(Keys.Escape))
                    {
                        if (gamePaused == true)
                        {
                            playingInstance.Play();
                            gamePaused = false;
                        }
                        else if (gamePaused == false)
                        {
                            gui_pauseMenu.setActiveItem(1);
                            playingInstance.Stop();
                            gamePaused = true;
                        }
                    }
                }

                // If the game is paused, check for menu input
                if (gamePaused == true)
                {
                    checkMenuInput(gui_pauseMenu);
                }
                // If the game is gameOver, check for menu input
                else if (game.gameOver)
                {
                    playingInstance.Stop();
                    checkMenuInput(gui_gameDoneMenu);
                }
                // Check for player movement
                else
                {
                    checkPlayerInput();
                }

                // Check if player has reached exit of level
                if (game.getLevel().isExitLevel())
                {
                    doorInstance.Play();
                    playingInstance.Stop();
                    game.getPlayer().setPlayerSpeed(new Vector2(0, 0));
                    if (game.getLevel().finishedLastLevel())
                    {
                        currentGameState = GameState.GameEnded;
                    }
                    else
                    {
                        currentGameState = GameState.NextLevel;
                    }
                }

                // Don't update view when paused or gameOver
                if (gamePaused == false && game.gameOver == false)
                {
                    gameView.UpdateView((float)gameTime.ElapsedGameTime.TotalSeconds);
                    game.UpdateGame((float)gameTime.ElapsedGameTime.TotalSeconds);
                }

            }
            else if (currentGameState == GameState.NextLevel)
            {
                // If all levels are finished
                if (game.getLevel().finishedLastLevel())
                {
                    currentGameState = GameState.GameEnded;
                }
                else if (newState.IsKeyDown(Keys.Enter))
                {
                    doorInstance.Play();

                    // Load next level
                    game.getLevel().nextLevel();

                    // Load current level backgroundMusic
                    if (File.Exists("Content/level" + game.getLevel().getCurrentLevel() + ".xnb") ||
                        File.Exists("Content/level" + game.getLevel().getCurrentLevel() + ".mp3"))
                    {
                        backgroundMusic = Content.Load<SoundEffect>("level" + game.getLevel().getCurrentLevel());
                    }
                    else
                    {
                        backgroundMusic = Content.Load<SoundEffect>("level1");
                    }
                    playingInstance = backgroundMusic.CreateInstance();
                    playingInstance.IsLooped = true;
                    playingInstance.Pan = .5f;

                    playingInstance.Play();

                    gameView.fadeTime = gameView.maxFadeTime;

                    currentGameState = GameState.GameStarted;
                }
            }
            else if (currentGameState == GameState.GameEnded)
            {
                // Check menu input for gameOverMenu
                checkMenuInput(gui_gameDoneMenu);
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
                // Position for splashscreen
                Rectangle destinationRectangle = new Rectangle(0, 0, camera.getScreenWidth(), camera.getScreenHeight());

                spriteBatch.Begin();
                spriteBatch.Draw(splashTexture, destinationRectangle, Color.White);
                spriteBatch.DrawString(font, "(Press any key)", new Vector2(camera.getScreenWidth() - 270, camera.getScreenHeight() - 40), titleColor);
                spriteBatch.End();
            }
            else if (currentGameState == GameState.MainMenu)
            {
                // Position for splashscreen
                Rectangle destinationRectangle = new Rectangle(0, 0, camera.getScreenWidth(), camera.getScreenHeight());

                spriteBatch.Begin();
                spriteBatch.Draw(splashTexture, destinationRectangle, Color.White);
                gui_mainMenu.DrawMenu();
                spriteBatch.End();

            }
            else if (currentGameState == GameState.GameStarted)
            {
                // Update camera
                camera.centerOn(game.getPlayerPosition(),
                                  new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),
                                  new Vector2(Model.Level.LEVEL_WIDTH, Model.Level.LEVEL_HEIGHT));

                camera.setZoom(64);

                gameView.DrawGame(graphics.GraphicsDevice, game.getPlayerPosition(), game.getLevel().getLevelEnemies(), game.getLevel().getLevelGems(), (float)gameTime.ElapsedGameTime.TotalSeconds, Content);

                // Game is Paused
                if (gamePaused)
                {
                    //Black Transparent background 
                    Rectangle destinationRectangle = new Rectangle(0, 0, camera.getScreenWidth(), camera.getScreenHeight());
                    Color pausedColor = new Color(0, 0, 0, 200);

                    spriteBatch.Begin();
                    spriteBatch.Draw(dummyTexture, destinationRectangle, pausedColor);
                    spriteBatch.DrawString(font, "Game Paused", new Vector2(camera.getScreenWidth() / 2 - (int)font.MeasureString("Game Paused").X / 2, camera.getScreenHeight() / 4), Color.White);
                    gui_pauseMenu.DrawMenu();
                    spriteBatch.End();

                }
                // Game Over
                else if (game.gameOver == true)
                {
                    //Black Transparent background 
                    Rectangle destinationRectangle = new Rectangle(0, 0, camera.getScreenWidth(), camera.getScreenHeight());
                    Color gameOverColor = new Color(0, 0, 0, 200);

                    spriteBatch.Begin();
                    spriteBatch.Draw(dummyTexture, destinationRectangle, gameOverColor);
                    spriteBatch.DrawString(font, "Game Over", new Vector2(camera.getScreenWidth() / 2 - (int)font.MeasureString("Game Over").X / 2, camera.getScreenHeight() / 4), Color.White);
                    gui_gameDoneMenu.DrawMenu();
                    spriteBatch.End();
                }
            }
            else if (currentGameState == GameState.NextLevel)
            {
                // Black background
                Rectangle destinationRectangle = new Rectangle(0, 0, camera.getScreenWidth(), camera.getScreenHeight());

                spriteBatch.Begin();
                spriteBatch.Draw(dummyTexture, destinationRectangle, Color.Black);
                spriteBatch.DrawString(font, "Level Completed! - Press \"Enter\" to continue", new Vector2(camera.getScreenWidth() / 2 - font.MeasureString("Level Complete! - Press \"Enter\" to continue").X / 2, camera.getScreenHeight() / 2), Color.White);
                spriteBatch.End();
            }
            else if (currentGameState == GameState.GameEnded)
            {
                // Black Background
                Rectangle destinationRectangle = new Rectangle(0, 0, camera.getScreenWidth(), camera.getScreenHeight());

                spriteBatch.Begin();
                spriteBatch.Draw(dummyTexture, destinationRectangle, Color.Black);
                spriteBatch.DrawString(font, "Congratulations, you have finished all levels!", new Vector2(camera.getScreenWidth() / 2 - font.MeasureString("Congratulations, you have finished all levels!").X / 2, camera.getScreenHeight() / 4), Color.White);
                gui_gameDoneMenu.DrawMenu();
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }


        /*************************************
         * Start new game
         *************************************/
        private void startNewGame()
        {
            titleInstance.Stop();

            game = new Model.Game(camera, spriteBatch);
            gameView = new View.GameView(spriteBatch, camera, game.getLevel(), game.getPlayer(), graphics.GraphicsDevice);

            game.LoadContent(Content);
            gameView.LoadContent(Content);

            backgroundMusic = Content.Load<SoundEffect>("level1");
            playingInstance = backgroundMusic.CreateInstance();
            playingInstance.IsLooped = true;
            playingInstance.Pan = .5f;
            playingInstance.Play();

            currentGameState = GameState.GameStarted;
        }

        /*************************************
         * Check player input
         *************************************/
        private void checkPlayerInput()
        {
            KeyboardState newState = Keyboard.GetState();

            // Is the Left key down?
            if (newState.IsKeyDown(Keys.Left))
            {
                if (game.getPlayer().getCurrentState() != Player.State.Falling)
                {
                    game.getPlayer().setCurrentState(Player.State.Walking);
                }
                game.getPlayer().setCurrentDirection(Player.Direction.Left);
                game.goLeft();
            }

            // Is the Right key down?
            if (newState.IsKeyDown(Keys.Right))
            {
                if (game.getPlayer().getCurrentState() != Player.State.Falling)
                {
                    game.getPlayer().setCurrentState(Player.State.Walking);
                }
                game.getPlayer().setCurrentDirection(Player.Direction.Right);
                game.goRight();
            }

            // Is the Space key down?
            if (newState.IsKeyDown(Keys.Space))
            {
                game.getPlayer().setCurrentState(Player.State.Jumping);
                game.playerJump();
            }
        }

        /*************************************
         * Check menu input
         *************************************/
        private void checkMenuInput(MenuGUI gui_menu)
        {
            KeyboardState newState = Keyboard.GetState();

            // Is Enter key pressed
            if (newState.IsKeyDown(Keys.Enter))
            {
                // If not down last update, key has just been pressed.
                if (!oldState.IsKeyDown(Keys.Enter))
                {
                    if (gui_menu.getActiveItem() == 1)
                    {
                        if (gamePaused == true)
                        {
                            playingInstance.Play();
                            gamePaused = false;
                        }
                        else
                        {
                            startNewGame();
                        }
                    }
                    else if (gui_menu.getActiveItem() == 2)
                    {
                        if (currentGameState == GameState.GameEnded || gamePaused == true)
                        {
                            titleInstance.Play();
                            gamePaused = false;
                            currentGameState = GameState.MainMenu;
                        }
                        else if (currentGameState == GameState.GameStarted && game.gameOver == true)
                        {
                            titleInstance.Play();
                            game.gameOver = false;
                            currentGameState = GameState.MainMenu;
                        }
                        else
                        {
                            this.Exit();
                        }
                    }
                }
            }

            // Is Down key pressed
            if (newState.IsKeyDown(Keys.Down))
            {
                // If not down last update, key has just been pressed.
                if (!oldState.IsKeyDown(Keys.Down))
                {
                    gui_menu.activeItemDown();
                }
            }

            // Is Up key pressed
            if (newState.IsKeyDown(Keys.Up))
            {
                // If not down last update, key has just been pressed.
                if (!oldState.IsKeyDown(Keys.Up))
                {
                    gui_menu.activeItemUp();
                }
            }

            // Set oldState
            oldState = newState;
        }
    }
}