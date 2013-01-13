using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using WindowsGame1.Model;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1.View
{
    class GameView
    {
        Texture2D playerTexture;
        Texture2D tileTexture;
        ColorChanger colorChanger = new ColorChanger(
                                        .4f, //Time between color changes
                                        true, //Is the color changer active?
                                        Color.Red, Color.Orange, Color.Yellow, Color.Green, //All the colors we want to cycle through
                                            Color.Blue, Color.Indigo, Color.Violet);

        private int textureTileSize = 64;
        private SpriteBatch spriteBatch;
        private Texture2D backgroundTexture;
        Camera m_camera;
        private SpriteFont fontTexture;

        public float fadeTime;
        public float maxFadeTime = 3f;
        private Color levelColor;
        private Level level;
        private Player player;
        private Texture2D heartTexture;
        private Texture2D crowdTexture;
        private SpriteFont inGameFont;
        private Texture2D dummyTexture;
        private GraphicsDevice m_graphicsDevice;
        private float backgroundX = 0;
        private Texture2D enemyTextureHipHopper;
        private Texture2D enemyTextureGhost;
        private Texture2D enemyTextureRasta;
        private SplitterSystem enemyDeadExplosion = new SplitterSystem();
        private Texture2D gemHeartTexture;

        public GameView(SpriteBatch batch, Camera camera, Level lvl, Player a_player, GraphicsDevice a_graphicsDevice)
        {
            startNewGame(batch, camera, lvl, a_player, a_graphicsDevice);
        }

        public void startNewGame(SpriteBatch batch, Camera camera, Level lvl, Player a_player, GraphicsDevice a_graphicsDevice)
        {
            player = a_player;
            level = lvl;
            fadeTime = maxFadeTime;
            m_camera = camera;
            m_graphicsDevice = a_graphicsDevice;
            spriteBatch = batch;
        }

        internal void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            backgroundTexture = content.Load<Texture2D>("background");
            crowdTexture = content.Load<Texture2D>("backgroundCrowd");
            playerTexture = content.Load<Texture2D>("playersheet");

            enemyTextureHipHopper = content.Load<Texture2D>("hiphopper");
            enemyTextureRasta = content.Load<Texture2D>("rastasheet");
            enemyTextureGhost = content.Load<Texture2D>("ghost");

            tileTexture = content.Load<Texture2D>("tile");
            fontTexture = content.Load<SpriteFont>("fontTexture");
            heartTexture = content.Load<Texture2D>("heart");
            gemHeartTexture = content.Load<Texture2D>("gemHeart");

            inGameFont = content.Load<SpriteFont>("VerdanaInGame");
            enemyDeadExplosion.LoadContent(content);

            dummyTexture = new Texture2D(m_graphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });
        }

        internal void UpdateView(float a_elapsedTime)
        {
            
            if (fadeTime > 0)
                fadeTime -= a_elapsedTime;

            float a = fadeTime / maxFadeTime;
            levelColor = new Color(a, a, a, a);


            //Scroll background
            if (player.getCurrentState() == Player.State.Walking)
            {
                if(player.getCurrentDirection() == Player.Direction.Right)
                { backgroundX -= 1; }

                if(player.getCurrentDirection() == Player.Direction.Left)
                { backgroundX += 1; }

            }

            //If the background has gone too far
            if (backgroundX <= -crowdTexture.Width + m_camera.getScreenWidth() ||
                backgroundX >= crowdTexture.Width + m_camera.getScreenWidth())
            {
                //Reset the offset
                backgroundX = 0;
            }

            colorChanger.Update(a_elapsedTime);
        }


        public void DrawLevel(GraphicsDevice a_graphicsDevice, Vector2 a_playerPosition, List<Enemy> enemies, List<Gem> gems, float a_elapsedTime, Microsoft.Xna.Framework.Content.ContentManager a_content)
        {
            Vector2 viewportSize = new Vector2(a_graphicsDevice.Viewport.Width, a_graphicsDevice.Viewport.Height);
            float scale = m_camera.getScale();

            //draw all images
            spriteBatch.Begin();

            //Background
            Rectangle destinationRectangle = new Rectangle(0, 0, m_camera.getScreenWidth(), m_camera.getScreenHeight());


            Rectangle destinationRectangle2 = new Rectangle((int)backgroundX, 0, crowdTexture.Width, crowdTexture.Height);

            spriteBatch.Draw(backgroundTexture, destinationRectangle, Color.White);
            spriteBatch.Draw(crowdTexture, destinationRectangle2, Color.White);

            Vector2 topLeft = m_camera.getModelTopLeftPosition();
            Vector2 topRight = new Vector2(topLeft.X + 13, topLeft.Y);

            if (topRight.X > Model.Level.LEVEL_WIDTH)
                topRight.X = Model.Level.LEVEL_WIDTH;


            //draw level
            for (int x = (int)topLeft.X; x < topRight.X; x++)
            {
                for (int y = 0; y < Model.Level.LEVEL_HEIGHT; y++)
                {
                    Vector2 viewPos = m_camera.getViewPosition(x, y, viewportSize);
                    Vector2 topRightPos = m_camera.getViewPosition(topRight.X, topRight.Y, viewportSize);
                    Vector2 topLeftPos = m_camera.getViewPosition(topLeft.X, topLeft.Y, viewportSize);

                    if (viewPos.X >= topLeftPos.X - 64 && viewPos.X <= topRightPos.X)
                    {
                        //TODO: Check if tile is on screen
                        DrawTile(viewPos.X, viewPos.Y, scale, level.levelTiles[x, y]);
                    }
                }
            }

            //draw player on top of level
            Vector2 playerViewPos = m_camera.getViewPosition(a_playerPosition.X, a_playerPosition.Y, viewportSize);
            DrawPlayerAt(playerViewPos, scale, player);

            
            DrawEnemies(enemies, viewportSize, a_elapsedTime, a_content);

            DrawGems(gems, viewportSize, a_elapsedTime, a_content);

            if(fadeTime > 0)
                spriteBatch.DrawString(fontTexture, level.getLevelName(), new Vector2(m_camera.getScreenWidth() / 2 - (int)fontTexture.MeasureString(level.getLevelName()).X / 2, m_camera.getScreenHeight() / 2), levelColor);

            DrawHud();


            if (player.mustBoogie())
            {
                player.setCurrentState(Player.State.Dancing);
                String boogieText = "I feel a sudden urge to... dance!";
                spriteBatch.Draw(dummyTexture, new Rectangle(((int)playerViewPos.X + playerTexture.Width / 8) - 5, (int)playerViewPos.Y - playerTexture.Height - 5, (int)inGameFont.MeasureString(boogieText).X + 8, (int)inGameFont.MeasureString(boogieText).Y + 8), Color.Black);
                spriteBatch.DrawString(inGameFont, boogieText, new Vector2(playerViewPos.X + playerTexture.Width / 8, playerViewPos.Y - playerTexture.Height), Color.White);
            }

            if (player.getBlinkingState() == Player.State.Blinking)
            {
                String boogieText = "Ouch!";
                spriteBatch.Draw(dummyTexture, new Rectangle(((int)playerViewPos.X + playerTexture.Width / 8) - 5, (int)playerViewPos.Y - playerTexture.Height - 5, (int)inGameFont.MeasureString(boogieText).X + 8, (int)inGameFont.MeasureString(boogieText).Y + 8), Color.Black);
                spriteBatch.DrawString(inGameFont, boogieText, new Vector2(playerViewPos.X + playerTexture.Width / 8, playerViewPos.Y - playerTexture.Height), Color.White);
            }

            spriteBatch.End();
        }

        private void DrawHud()
        {
            //Create rectangle and draw it, note the transformation of the position
            Rectangle destRect = new Rectangle(10, 10, 32, 32);
            spriteBatch.Draw(heartTexture, destRect, Color.White);
            spriteBatch.DrawString(fontTexture, "x " + player.getLifes(), new Vector2(52, 10), Color.White);
        }

        private void DrawPlayerAt(Microsoft.Xna.Framework.Vector2 a_viewBottomCenterPosition, float a_scale, Player a_player)
        {
            int textureIndex = a_player.getCurrentFrame().X;
            int textureRowIndex = a_player.getCurrentFrame().Y;


            //Get the source rectangle (pixels on the texture) for the tile type 
            Rectangle sourceRectangle = new Rectangle(28 * textureIndex, 45 * textureRowIndex, 28, 44);
            
            //Create rectangle and draw it, note the transformation of the position
            Rectangle destRect = new Rectangle((int)(a_viewBottomCenterPosition.X - a_scale / 2.0f), (int)(a_viewBottomCenterPosition.Y - a_scale), (int)a_scale, (int)a_scale);

            spriteBatch.Draw(playerTexture, destRect, sourceRectangle, player.getPlayerColor());
        }

        private void DrawTile(float a_x, float a_y, float a_scale, Model.Tile a_tile)
        {
            int textureIndex = 0;
            Color tileColor;

            if (a_tile.isExit())
                textureIndex = 2;

            if(a_tile.isBlocked())
                textureIndex = 1;

            if (a_tile.isTrap())
            {
                textureIndex = 1;
            }

            if (a_tile.isTrap() && a_tile.isWalkedOn())
            {
                a_tile.setWalkedOn(false);
            }

            if(a_tile.isTrap())
            {
                tileColor = colorChanger.CurrentColor;
            }
            else
            {
                tileColor = Color.White;
            }

            //Get the source rectangle (pixels on the texture) for the tile type 
            Rectangle sourceRectangle = new Rectangle(textureTileSize * textureIndex, 0, textureTileSize, textureTileSize);

            //Destination rectangle in windows coordinates only scaling
            Rectangle destRect = new Rectangle((int)a_x, (int)a_y, (int)a_scale, (int)a_scale);

            spriteBatch.Draw(tileTexture, destRect, sourceRectangle, tileColor);
        }

        private void DrawEnemies(List<Enemy> enemies, Vector2 viewportSize, float a_elapsedTime, Microsoft.Xna.Framework.Content.ContentManager a_content)
        {
            float scale = m_camera.getScale();

            foreach (Enemy aEnemy in enemies)
            {
                Vector2 enemyPosition = m_camera.getViewPosition(aEnemy.getCenterBottomPosition().X, aEnemy.getCenterBottomPosition().Y, viewportSize);
                int textureIndex = aEnemy.getCurrentFrame().X;
                int textureRowIndex = aEnemy.getCurrentFrame().Y;



                //Create rectangle and draw it, note the transformation of the position
                Rectangle destRect = new Rectangle((int)(enemyPosition.X - scale / 2.0f), (int)(enemyPosition.Y - scale), (int)scale, (int)scale);


                if (aEnemy.getEnemyType() == Enemy.EnemyType.Rasta)
                {
                    //Get the source rectangle (pixels on the texture) for the tile type 
                    Rectangle sourceRectangle = new Rectangle(40 * textureIndex, 50 * textureRowIndex, 40, 50);

                    spriteBatch.Draw(enemyTextureRasta, destRect, sourceRectangle, Color.White);
                }

                if (aEnemy.getEnemyType() == Enemy.EnemyType.Ghost)
                {
                    //Get the source rectangle (pixels on the texture) for the tile type 
                    Rectangle sourceRectangle = new Rectangle(60 * textureIndex, 60 * textureRowIndex, 60, 60);

                    spriteBatch.Draw(enemyTextureGhost, destRect, sourceRectangle, Color.White);
                }

                if (aEnemy.getEnemyType() == Enemy.EnemyType.HipHopper)
                {
                    //Get the source rectangle (pixels on the texture) for the tile type 
                    Rectangle sourceRectangle = new Rectangle(45 * textureIndex, 44 * textureRowIndex, 45, 44);

                    spriteBatch.Draw(enemyTextureHipHopper, destRect, sourceRectangle, Color.White);
                }


                if (aEnemy.isDead())
                {
                    enemyDeadExplosion.initializeParticles(aEnemy.getCenterBottomPosition());
                    enemyDeadExplosion.UpdateAndDraw(a_elapsedTime, spriteBatch, m_camera, aEnemy.getCenterBottomPosition());
                }
            }
        }

        private void DrawGems(List<Gem> gems, Vector2 viewportSize, float a_elapsedTime, Microsoft.Xna.Framework.Content.ContentManager a_content)
        {
            float scale = m_camera.getScale();

            foreach (Gem aGem in gems)
            {
                Vector2 gemPosition = m_camera.getViewPosition(aGem.getCenterBottomPosition().X, aGem.getCenterBottomPosition().Y, viewportSize);

                //Create rectangle and draw it, note the transformation of the position
                Rectangle destRect = new Rectangle((int)(gemPosition.X), (int)(gemPosition.Y), (int)scale, (int)scale);

                //Get the source rectangle (pixels on the texture) for the tile type 
                Rectangle sourceRectangle = new Rectangle(0, 0, 32, 32);

                if (aGem.getGemType() == Gem.GemType.Life)
                {
                    spriteBatch.Draw(gemHeartTexture, destRect, sourceRectangle, Color.White);
                }

            }
        }
    }
}
