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
            NextLevel,
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
        private Texture2D dummyTexture;
        private int mainActiveItem = 1;
        private SoundEffect doorEffect;
        private SoundEffectInstance doorInstance;

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
            doorInstance.Volume = 1f;

            dummyTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });

            //Load content in MenuGUI
            m_gui = new MenuGUI(spriteBatch, Content);

            camera = new View.Camera(graphics.GraphicsDevice.Viewport);

            int menuX = camera.getScreenWidth() / 5;
            int menuY = camera.getScreenHeight() - camera.getScreenHeight() / 4;

            Mouse.SetPosition(menuX, menuY);

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


            if (currentGameState == GameState.NextLevel)
            {
                if(newState.IsKeyDown(Keys.Enter))
                {
                    doorInstance.Play();
                    game.getLevel().nextLevel();

                    backgroundMusic = Content.Load<SoundEffect>("level" + game.getLevel().getCurrentLevel());
                    playingInstance = backgroundMusic.CreateInstance();
                    playingInstance.IsLooped = true;
                    playingInstance.Pan = .5f;

                    playingInstance.Play();

                    gameView.fadeTime = gameView.maxFadeTime;

                    currentGameState = GameState.GameStarted;
                }
            }
            else if (currentGameState == GameState.MainMenu)
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
                    if (oldState.GetPressedKeys().Length > 0)
                    {
                        currentGameState = GameState.MainMenu;
                    }
                }

                oldState = newState;
            }
            else if (currentGameState == GameState.GameStarted)
            {

                if (game.getLevel().isExitLevel())
                {
                    doorInstance.Play();
                    playingInstance.Stop();
                    currentGameState = GameState.NextLevel;
                }

                if (isEditingLevel)
                {
                    MouseState mouse = Mouse.GetState();
                    Vector2 mousePos = new Vector2(mouse.X / camera.getScale() + camera.getModelTopLeftPosition().X, mouse.Y / camera.getScale() + camera.getModelTopLeftPosition().Y);

                    Vector2 mouseSize = new Vector2(.10f, .10f);
                    FloatRectangle mouseRect = FloatRectangle.createFromTopLeft(mousePos, mouseSize);


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
                        isEditingLevel = true;
                    }
                }

                // Is the game paused
                if (newState.IsKeyDown(Keys.Escape) || newState.IsKeyDown(Keys.P))
                {
                    if (!oldState.IsKeyDown(Keys.Escape) || !oldState.IsKeyDown(Keys.P))
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
                }

                if (gamePaused == false && game.gameOver == false)
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

            if (currentGameState == GameState.NextLevel)
            {
                //Räkna ut position för knappen
                Rectangle destinationRectangle = new Rectangle(0, 0, camera.getScreenWidth(), camera.getScreenHeight());

                spriteBatch.Begin();
                spriteBatch.Draw(dummyTexture, destinationRectangle, Color.Black);
                spriteBatch.DrawString(font, "Level Completed! - Press \"Enter\" to continue", new Vector2(camera.getScreenWidth() / 2 - font.MeasureString("Level Complete! - Press \"Enter\" to continue").X / 2, camera.getScreenHeight() / 2), Color.White);
                spriteBatch.End();
            }
            else if (currentGameState == GameState.TitleScreen)
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
                KeyboardState newState = Keyboard.GetState();
                MouseState newMouseState = Mouse.GetState();

                //Räkna ut position för knappen
                Rectangle destinationRectangle = new Rectangle(0, 0, camera.getScreenWidth(), camera.getScreenHeight());

                spriteBatch.Begin();
                spriteBatch.Draw(splashTexture, destinationRectangle, Color.White);   
                spriteBatch.End();


                int menuX = camera.getScreenWidth() / 5;
                int menuY = camera.getScreenHeight() - camera.getScreenHeight() / 4;
                int buttonSeparation = 45;

                if (newState.IsKeyDown(Keys.Down))
                {
                    // If not down last update, key has just been pressed.
                    if (!oldState.IsKeyDown(Keys.Down))
                    {
                        Mouse.SetPosition(menuX, menuY + buttonSeparation);
                        mainActiveItem = 2;
                    }
                }
                else if (newState.IsKeyDown(Keys.Up))
                {
                    // If not down last update, key has just been pressed.
                    if (!oldState.IsKeyDown(Keys.Up))
                    {
                        Mouse.SetPosition(menuX, menuY);
                        mainActiveItem = 1;
                    }
                }
                else if (newState.IsKeyDown(Keys.Enter))
                {
                    // If not down last update, key has just been pressed.
                    if (!oldState.IsKeyDown(Keys.Enter))
                    {
                        if (mainActiveItem == 1)
                        {
                            play = true;
                            Console.WriteLine("New Game");

                            game = new Model.Game(camera);
                            gameView = new View.GameView(spriteBatch, camera, game.getLevel(), game.getPlayer(), graphics.GraphicsDevice);

                            game.LoadContent(Content);
                            gameView.LoadContent(Content);

                            Mouse.SetPosition(camera.getScreenWidth() / 2, camera.getScreenWidth() / 3 - 40);

                            backgroundMusic = Content.Load<SoundEffect>("level1");
                            playingInstance = backgroundMusic.CreateInstance();
                            playingInstance.IsLooped = true;
                            playingInstance.Pan = .5f;

                            currentGameState = GameState.GameStarted;

                        }

                        if (mainActiveItem == 2)
                        {
                            this.Exit();
                        }
                    }
                }

                if (m_gui.DoButton(Mouse.GetState(), "New game", menuX, menuY))
                {
                    play = true;
                    gamePaused = false;
                    Console.WriteLine("New Game");

                    game = new Model.Game(camera);
                    gameView = new View.GameView(spriteBatch, camera, game.getLevel(), game.getPlayer(), graphics.GraphicsDevice);

                    game.LoadContent(Content);
                    gameView.LoadContent(Content);

                    backgroundMusic = Content.Load<SoundEffect>("level1");
                    playingInstance = backgroundMusic.CreateInstance();
                    playingInstance.IsLooped = true;
                    playingInstance.Pan = .5f;

                    currentGameState = GameState.GameStarted;
                }

                if (m_gui.DoButton(Mouse.GetState(), "Exit", menuX, menuY += buttonSeparation))
                {
                    this.Exit();
                }

                oldState = newState;
                m_gui.setOldState(Mouse.GetState());

            }
            else if (currentGameState == GameState.GameStarted)
            {
                KeyboardState newState = Keyboard.GetState();

                //update camera
                camera.centerOn(game.getPlayerPosition(),
                                  new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),
                                  new Vector2(Model.Level.LEVEL_WIDTH, Model.Level.LEVEL_HEIGHT));

                camera.setZoom(64);

                gameView.DrawLevel(graphics.GraphicsDevice, game.getPlayerPosition(), game.getLevel().getLevelEnemies(), game.getLevel().getLevelGems(), (float)gameTime.TotalGameTime.TotalSeconds, Content);

                if (gamePaused)
                {

                    //Räkna ut position för knappen
                    Rectangle destinationRectangle = new Rectangle(0, 0, camera.getScreenWidth(), camera.getScreenHeight());
                    Color pausedColor = new Color(0, 0, 0, 200);

                    spriteBatch.Begin();
                    spriteBatch.Draw(dummyTexture, destinationRectangle, pausedColor);
                    spriteBatch.DrawString(font, "Game Paused", new Vector2(camera.getScreenWidth() / 2 - (int)font.MeasureString("Game Paused").X / 2, camera.getScreenHeight() / 4), Color.White);
                    spriteBatch.End();

                    int menuX = camera.getScreenWidth() / 2;
                    int menuY = camera.getScreenHeight() / 3 + 40;
                    int buttonSeparation = 45;


                    if (newState.IsKeyDown(Keys.Down))
                    {
                            Console.WriteLine("Down");
                            Mouse.SetPosition(menuX, menuY + buttonSeparation);
                            mainActiveItem = 2;
                    }
                    else if (newState.IsKeyDown(Keys.Up))
                    {
                            Console.WriteLine("Up");
                            Mouse.SetPosition(menuX, menuY);
                            mainActiveItem = 1;
                    }
                    else if (newState.IsKeyDown(Keys.Enter))
                    {
                        if (mainActiveItem == 1)
                        {
                            playingInstance.Play();
                            gamePaused = false;
                        }

                        if (mainActiveItem == 2)
                        {
                            titleInstance.Play();
                            play = false;
                            gamePaused = false;
                            Mouse.SetPosition(camera.getScreenWidth() / 5, camera.getScreenHeight() - camera.getScreenHeight() / 4);
                            mainActiveItem = 1;
                            currentGameState = GameState.MainMenu;
                        }
                    }


                    if (m_gui.DoButton(Mouse.GetState(), "Continue", menuX, menuY))
                    {
                        playingInstance.Play();
                        gamePaused = false;
                    }
                    if (m_gui.DoButton(Mouse.GetState(), "Exit", menuX, menuY += buttonSeparation))
                    {
                        gamePaused = false;
                        Mouse.SetPosition(camera.getScreenWidth() / 5, camera.getScreenHeight() - camera.getScreenHeight() / 4);
                        mainActiveItem = 1;
                        currentGameState = GameState.MainMenu;
                    }

                    m_gui.setOldState(Mouse.GetState());
                }

                if (game.gameOver == true)
                {
                    playingInstance.Stop();

                    Rectangle destinationRectangle = new Rectangle(0, 0, camera.getScreenWidth(), camera.getScreenHeight());
                    Color gameOverColor = new Color(0, 0, 0, 200);

                    spriteBatch.Begin();
                    spriteBatch.Draw(dummyTexture, destinationRectangle, gameOverColor);
                    spriteBatch.DrawString(font, "Game Over", new Vector2(camera.getScreenWidth() / 2 - (int)font.MeasureString("Game Over").X / 2, camera.getScreenHeight() / 4), Color.White);
                    spriteBatch.End();

                    int menuX = camera.getScreenWidth() / 2;
                    int menuY = camera.getScreenHeight() / 3 + 40;
                    int buttonSeparation = 45;

                    if (newState.IsKeyDown(Keys.Down))
                    {
                        Console.WriteLine("Down");
                        Mouse.SetPosition(menuX, menuY + buttonSeparation);
                        mainActiveItem = 2;
                    }
                    else if (newState.IsKeyDown(Keys.Up))
                    {
                        Console.WriteLine("Up");
                        Mouse.SetPosition(menuX, menuY);
                        mainActiveItem = 1;
                    }
                    else if (newState.IsKeyDown(Keys.Enter))
                    {
                        if (mainActiveItem == 1)
                        {
                            play = true;
                            Console.WriteLine("New Game");
                            gamePaused = false;

                            game = new Model.Game(camera);
                            gameView = new View.GameView(spriteBatch, camera, game.getLevel(), game.getPlayer(), graphics.GraphicsDevice);

                            game.LoadContent(Content);
                            gameView.LoadContent(Content);

                            backgroundMusic = Content.Load<SoundEffect>("level1");
                            playingInstance = backgroundMusic.CreateInstance();
                            playingInstance.IsLooped = true;
                            playingInstance.Pan = .5f;  

                            currentGameState = GameState.GameStarted;
                        }

                        if (mainActiveItem == 2)
                        {
                            titleInstance.Play();
                            play = false;
                            Mouse.SetPosition(camera.getScreenWidth() / 5, camera.getScreenHeight() - camera.getScreenHeight() / 4);
                            mainActiveItem = 1;
                            gamePaused = false;
                            currentGameState = GameState.MainMenu;
                        }
                    }

                    if (m_gui.DoButton(Mouse.GetState(), "New Game", menuX, menuY))
                    {
                        play = true;
                        Console.WriteLine("New Game");
                        gamePaused = false;

                        game = new Model.Game(camera);
                        gameView = new View.GameView(spriteBatch, camera, game.getLevel(), game.getPlayer(), graphics.GraphicsDevice);

                        game.LoadContent(Content);
                        gameView.LoadContent(Content);

                        currentGameState = GameState.GameStarted;
                    }
                    if (m_gui.DoButton(Mouse.GetState(), "Exit", menuX, menuY += buttonSeparation))
                    {
                        this.Exit();
                    }



                    //titleInstance.Play();
                    //currentGameState = GameState.MainMenu;
                }
            }

            base.Draw(gameTime);
        }
    }
}
