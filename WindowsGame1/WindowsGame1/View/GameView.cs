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
        Song backgroundMusic;
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
        private Texture2D enemyTexture;
        private Player player;
        private Texture2D heartTexture;

        public GameView(SpriteBatch batch, Camera camera, Level lvl, Player a_player)
        {
            startNewGame(batch, camera, lvl, a_player);
        }

        public void startNewGame(SpriteBatch batch, Camera camera, Level lvl, Player a_player)
        {
            player = a_player;
            level = lvl;
            fadeTime = maxFadeTime;
            m_camera = camera;
            spriteBatch = batch;
        }

        internal void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            backgroundTexture = content.Load<Texture2D>("background");
            playerTexture = content.Load<Texture2D>("playersheet");
            enemyTexture = content.Load<Texture2D>("ghost");
            tileTexture = content.Load<Texture2D>("tile");
            fontTexture = content.Load<SpriteFont>("fontTexture");
            heartTexture = content.Load<Texture2D>("heart");
        }

        internal void UpdateView(float a_elapsedTime)
        {
            
            if (fadeTime > 0)
                fadeTime -= a_elapsedTime;

            float a = fadeTime / maxFadeTime;
            levelColor = new Color(a, a, a, a);

            colorChanger.Update(a_elapsedTime);
        }


        public void DrawLevel(GraphicsDevice a_graphicsDevice, Vector2 a_playerPosition, List<Enemy> enemies)
        {
            Vector2 viewportSize = new Vector2(a_graphicsDevice.Viewport.Width, a_graphicsDevice.Viewport.Height);
            float scale = m_camera.getScale();

            a_graphicsDevice.Clear(Microsoft.Xna.Framework.Color.LightSteelBlue);

            //draw all images
            spriteBatch.Begin();


            //Background
            Rectangle destinationRectangle = new Rectangle(0, 0, m_camera.getScreenWidth(), m_camera.getScreenHeight());
            spriteBatch.Draw(backgroundTexture, destinationRectangle, colorChanger.CurrentColor);



            //draw level
            for (int x = 0; x < Model.Level.LEVEL_WIDTH; x++)
            {
                for (int y = 0; y < Model.Level.LEVEL_HEIGHT; y++)
                {
                    Vector2 viewPos = m_camera.getViewPosition(x, y, viewportSize);
                    DrawTile(viewPos.X, viewPos.Y, scale, level.levelTiles[x, y]);
                }
            }

            //draw player on top of level
            Vector2 playerViewPos = m_camera.getViewPosition(a_playerPosition.X, a_playerPosition.Y, viewportSize);
            DrawPlayerAt(playerViewPos, scale, player);

            DrawEnemies(enemies, viewportSize);

            if(fadeTime > 0)
                spriteBatch.DrawString(fontTexture, level.getLevelName(), new Vector2(m_camera.getScreenWidth() / 2 - (int)fontTexture.MeasureString(level.getLevelName()).X / 2, m_camera.getScreenHeight() / 2), levelColor);

            DrawHud();

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
            Rectangle sourceRectangle = new Rectangle(27 * textureIndex, 44 * textureRowIndex, 27, 44);
            
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

            if(a_tile.isTrap() && a_tile.isWalkedOn())
            {
                a_tile.setWalkedOn(false);
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

        private void DrawEnemies(List<Enemy> enemies, Vector2 viewportSize)
        {
            float scale = m_camera.getScale();

            foreach (Enemy aEnemy in enemies)
            {
                Vector2 enemyPosition = m_camera.getViewPosition(aEnemy.getCenterBottomPosition().X, aEnemy.getCenterBottomPosition().Y, viewportSize);
                int textureIndex = aEnemy.getCurrentFrame().X;
                int textureRowIndex = aEnemy.getCurrentFrame().Y;

                //Get the source rectangle (pixels on the texture) for the tile type 
                Rectangle sourceRectangle = new Rectangle(60 * textureIndex, 60 * textureRowIndex, 60, 60);

                //Create rectangle and draw it, note the transformation of the position
                Rectangle destRect = new Rectangle((int)(enemyPosition.X - scale / 2.0f), (int)(enemyPosition.Y - scale), (int)scale, (int)scale);

                spriteBatch.Draw(enemyTexture, destRect, sourceRectangle, Color.White);
            }
   
        }


        //internal void DrawLevel(Model.Game a_game, float a_elapsedTime, SpriteBatch a_spriteBatch, Camera a_camera)
        //{
        //    int playerWidth = (int)a_game.getPlayer().getPlayerWidth() * (int)a_camera.getScaleX();
        //    int playerHeight = (int)a_game.getPlayer().getPlayerHeight() * (int)a_camera.getScaleY();

        //    float scale = a_camera.getScale();

        //    Vector2 playerViewTopLeft = a_camera.convertToView(a_game.getPlayer().getCenterBottomPosition().X - (a_game.getPlayer().getPlayerWidth() / 2),
        //                                                         a_game.getPlayer().getCenterBottomPosition().Y - a_game.getPlayer().getPlayerHeight() +.05f);
           
        //    Rectangle playerDestRectangle = new Rectangle((int)playerViewTopLeft.X, (int)playerViewTopLeft.Y, (int)scale, (int)scale);

        //    a_spriteBatch.Begin();

        //    for (int x = 0; x < Model.Level.LEVEL_WIDTH; x++)
        //    {
        //        for (int y = 0; y < Model.Level.LEVEL_HEIGHT; y++)
        //        {
        //            Vector2 viewPos = a_camera.convertToView(x, y);
        //            float tileWidth = Model.Level.TILE_WIDTH * a_camera.getScaleX();
        //            float tileHeight = Model.Level.TILE_HEIGHT * a_camera.getScaleY();

        //            int textureIndex = a_game.getLevel().levelTiles[x, y].isBlocked() ? 0 : 1;

        //            //Get the source rectangle (pixels on the texture) for the tile type 
        //            Rectangle sourceRectangle = new Rectangle(textureTileSize * textureIndex, 0, textureTileSize, textureTileSize);

        //            //Destination rectangle in windows coordinates only scaling
        //            Rectangle destRect = new Rectangle((int)viewPos.X, (int)viewPos.Y, (int)tileWidth, (int)tileHeight);

                    
        //            a_spriteBatch.Draw(tileTexture, destRect, sourceRectangle, Color.White);
        //        }
        //    }

        //    a_spriteBatch.Draw(playerTexture, playerDestRectangle, Color.White);

        //    a_spriteBatch.End();

        //}

    
    }
}
