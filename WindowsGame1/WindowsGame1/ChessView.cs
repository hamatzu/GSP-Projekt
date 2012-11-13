using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WindowsGame1.View
{
    class ChessView
    {
        Texture2D m_WhiteTexture;
        Texture2D m_BlackTexture;
        Texture2D m_firstTexture;
        Texture2D m_secondTexture;
        Texture2D m_pieceTexture;

        internal void LoadContent(Microsoft.Xna.Framework.Content.ContentManager a_content)
        {
            // Ladda in texturen pad.png från Example2Content
            m_WhiteTexture = a_content.Load<Texture2D>("white");
            m_BlackTexture = a_content.Load<Texture2D>("black");
            m_pieceTexture = a_content.Load<Texture2D>("piece");
        }

        internal void Draw(Model.ChessGame a_game, double a_elapsedTime, Microsoft.Xna.Framework.Graphics.SpriteBatch a_spriteBatch, Viewport a_viewport)
        {
            // om vi vill ha en kant(som i labb 1) eller scrolla behöver vi dessa två. 
            int displacementX = 0;
            int displacementY = 0;

            //Räkna ut spelets skala
            float scaleX = (float)a_viewport.Width / (float)Model.Level.LEVEL_WIDTH;
            float scaleY = (float)a_viewport.Height / (float)Model.Level.LEVEL_HEIGHT;


            int sizeOfTile = 64;
            int borderSize = 64;

            for (int x = 0; x < 8; x++)
            {
                if (x % 2 == 0)
                {
                    m_firstTexture = m_BlackTexture;
                    m_secondTexture = m_WhiteTexture;
                }
                else
                {
                    m_firstTexture = m_WhiteTexture;
                    m_secondTexture = m_BlackTexture;
                }

                for (int y = 0; y < 8; y++)
                {
                    int vx = (int)(borderSize + x * sizeOfTile);
                    int vy = (int)(borderSize + y * sizeOfTile);
                    int vw = 64;
                    int vh = 64;

                    Rectangle destinationRectangle = new Rectangle(vx, vy, vw, vh);
                    Rectangle pieceRectangle = new Rectangle(vx, vy, vw, vh);

                    a_spriteBatch.Begin();
                    if(y % 2 == 0)
                    {
                        a_spriteBatch.Draw(m_firstTexture, destinationRectangle, Color.White);
                    }
                    else
                    {
                        a_spriteBatch.Draw(m_secondTexture, destinationRectangle, Color.White);
                    }

                    if (x == 3 && y == 0)
                    {
                        a_spriteBatch.Draw(m_pieceTexture, pieceRectangle, Color.White);
                    }

                    a_spriteBatch.End();
                }
            }
        }
    }
}
