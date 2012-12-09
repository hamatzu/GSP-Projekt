using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using WindowsGame1.Model;

namespace WindowsGame1.View
{
    class GameView
    {
        Texture2D playerTexture;
        Texture2D tileTexture;

        private int textureTileSize = 64;
        private SpriteBatch spriteBatch;

        public GameView(SpriteBatch batch)
        {
            spriteBatch = batch;
        }

        internal void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            playerTexture = content.Load<Texture2D>("playersheet");
            tileTexture = content.Load<Texture2D>("tile");
        }

        public void DrawLevel(GraphicsDevice a_graphicsDevice, Model.Level a_level, Camera a_camera, Vector2 a_playerPosition, Player a_player)
        {
            Vector2 viewportSize = new Vector2(a_graphicsDevice.Viewport.Width, a_graphicsDevice.Viewport.Height);
            float scale = a_camera.getScale();

            a_graphicsDevice.Clear(Microsoft.Xna.Framework.Color.Green);

            //draw all images
            spriteBatch.Begin();

            //draw level
            for (int x = 0; x < Model.Level.LEVEL_WIDTH; x++)
            {
                for (int y = 0; y < Model.Level.LEVEL_HEIGHT; y++)
                {
                    Vector2 viewPos = a_camera.getViewPosition(x, y, viewportSize);
                    DrawTile(viewPos.X, viewPos.Y, scale, a_level.levelTiles[x, y]);
                }
            }

            //draw player on top of level
            Vector2 playerViewPos = a_camera.getViewPosition(a_playerPosition.X, a_playerPosition.Y, viewportSize);
            DrawPlayerAt(playerViewPos, scale, a_player);
            spriteBatch.End();
        }



        private void DrawPlayerAt(Microsoft.Xna.Framework.Vector2 a_viewBottomCenterPosition, float a_scale, Player a_player)
        {
            int textureIndex = a_player.getCurrentFrame().X;
            int textureRowIndex = a_player.getCurrentFrame().Y;


            //Get the source rectangle (pixels on the texture) for the tile type 
            Rectangle sourceRectangle = new Rectangle(27 * textureIndex, 44 * textureRowIndex, 27, 44);
            
            //Create rectangle and draw it, note the transformation of the position
            Rectangle destRect = new Rectangle((int)(a_viewBottomCenterPosition.X - a_scale / 2.0f), (int)(a_viewBottomCenterPosition.Y - a_scale), (int)a_scale, (int)a_scale);

            spriteBatch.Draw(playerTexture, destRect, sourceRectangle, Color.White);
        }

        private void DrawTile(float a_x, float a_y, float a_scale, Model.Tile a_tile)
        {
            int textureIndex = a_tile.isBlocked() ? 0 : 1;

            //Get the source rectangle (pixels on the texture) for the tile type 
            Rectangle sourceRectangle = new Rectangle(textureTileSize * textureIndex, 0, textureTileSize, textureTileSize);

            //Destination rectangle in windows coordinates only scaling
            Rectangle destRect = new Rectangle((int)a_x, (int)a_y, (int)a_scale, (int)a_scale);


            spriteBatch.Draw(tileTexture, destRect, sourceRectangle, Color.White);
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
