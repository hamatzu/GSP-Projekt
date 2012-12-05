using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WindowsGame1.View
{
    class GameView
    {
        Texture2D playerTexture;
        Texture2D tileTexture;

        private int textureTileSize = 32;

        internal void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            playerTexture = content.Load<Texture2D>("player");
            tileTexture = content.Load<Texture2D>("tile");
        }

        internal void DrawPlayer(Model.Game a_game, float a_elapsedTime, SpriteBatch a_spriteBatch, Camera a_camera)
        {
            int playerWidth = (int)a_game.getPlayer().getPlayerWidth() * (int)a_camera.getScaleX();
            int playerHeight = (int)a_game.getPlayer().getPlayerHeight() * (int)a_camera.getScaleY();



            Vector2 playerViewTopLeft = a_camera.convertToView(a_game.getPlayer().getCenterBottomPosition().X - (a_game.getPlayer().getPlayerWidth()),
                                                                 a_game.getPlayer().getCenterBottomPosition().Y - a_game.getPlayer().getPlayerHeight());
           
            Rectangle playerDestRectangle = new Rectangle((int)playerViewTopLeft.X, (int)playerViewTopLeft.Y, playerWidth, playerHeight);

            a_spriteBatch.Begin();

            for (int x = 0; x < Model.Level.LEVEL_WIDTH; x++)
            {
                for (int y = 0; y < Model.Level.LEVEL_HEIGHT; y++)
                {
                    Vector2 viewPos = a_camera.convertToView(x, y);
                    float tileWidth = Model.Level.TILE_WIDTH * a_camera.getScaleX();
                    float tileHeight = Model.Level.TILE_HEIGHT * a_camera.getScaleY();

                    int textureIndex = a_game.level.levelTiles[x, y].isBlocked() ? 0 : 1;

                    //Get the source rectangle (pixels on the texture) for the tile type 
                    Rectangle sourceRectangle = new Rectangle(textureTileSize * textureIndex, 0, textureTileSize, textureTileSize);

                    //Destination rectangle in windows coordinates only scaling
                    Rectangle destRect = new Rectangle((int)viewPos.X, (int)viewPos.Y, (int)tileWidth, (int)tileHeight);


                    a_spriteBatch.Draw(tileTexture, destRect, sourceRectangle, Color.White);
                }
            }

            a_spriteBatch.Draw(playerTexture, playerDestRectangle, Color.White);

            a_spriteBatch.End();

        }

    }
}
