using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace WindowsGame1.View
{
    class ChessView
    {
        Texture2D m_WhiteTexture;
        Texture2D m_BlackTexture;
        Texture2D m_firstTexture;
        Texture2D m_secondTexture;
        Texture2D m_pieceTexture;
        SpriteFont font;


        internal void LoadContent(Microsoft.Xna.Framework.Content.ContentManager a_content, GraphicsDeviceManager a_graphics)
        {
            // Ladda in texturen pad.png från Example2Content
            m_WhiteTexture = a_content.Load<Texture2D>("white");
            m_BlackTexture = a_content.Load<Texture2D>("black");
            m_pieceTexture = a_content.Load<Texture2D>("piece");
            font = a_content.Load<SpriteFont>("verdana");
        }

        internal void Draw(Model.ChessGame a_game, double a_elapsedTime, Microsoft.Xna.Framework.Graphics.SpriteBatch a_spriteBatch, Viewport a_viewport, String a_playerTurn)
        {


            Model.Camera cam = new Model.Camera(a_viewport);

            cam.setPlayer(a_playerTurn);

            

            a_spriteBatch.Begin();
            a_spriteBatch.DrawString(font, "Current player: " + cam.getPlayer() + "(Press space to switch player)", new Vector2(0, 0), Color.Bisque);
            for (int x = 0; x < Model.Level.LEVEL_WIDTH; x++)
            {
                if (x % 2 == 0)
                {
                    m_firstTexture = m_WhiteTexture;
                    m_secondTexture = m_BlackTexture;
                }
                else
                {
                    m_firstTexture = m_BlackTexture;
                    m_secondTexture = m_WhiteTexture;
                }

                for (int y = 0; y < Model.Level.LEVEL_HEIGHT; y++)
                {

                    Console.WriteLine(x + ", " + y);
                    Vector2 coordinates = cam.convertToVisual(x, y);

                    int tileWidth = (int)(Model.Level.TILE_WIDTH * cam.getScaleX());
                    int tileHeight = (int)(Model.Level.TILE_HEIGHT * cam.getScaleY());
                    
                    Rectangle chessTile = new Rectangle((int)coordinates.X, (int)coordinates.Y, tileWidth, tileHeight);

                    
                    if(y % 2 == 0)
                    {
                        a_spriteBatch.Draw(m_firstTexture, chessTile, Color.White);
                    }
                    else
                    {
                        a_spriteBatch.Draw(m_secondTexture, chessTile, Color.White);
                        
                    }

                    if (x == 4 && y == 2)
                    {                        
                        a_spriteBatch.Draw(m_pieceTexture, chessTile, null, Color.White);
                    }
                }
            }
            a_spriteBatch.End();
        }
    }
}
