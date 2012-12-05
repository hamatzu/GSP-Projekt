﻿using System;
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
        SpriteFont font2;


        internal void LoadContent(Microsoft.Xna.Framework.Content.ContentManager a_content, GraphicsDeviceManager a_graphics)
        {
            // Load textures
            m_WhiteTexture = a_content.Load<Texture2D>("white");
            m_BlackTexture = a_content.Load<Texture2D>("black");
            m_pieceTexture = a_content.Load<Texture2D>("piece");
            font = a_content.Load<SpriteFont>("verdana");
            font2 = a_content.Load<SpriteFont>("verdana2");
        }

        internal void Draw(double a_elapsedTime, Microsoft.Xna.Framework.Graphics.SpriteBatch a_spriteBatch, Viewport a_viewport, String a_playerTurn)
        {
            //Iniate camera
            Model.Camera cam = new Model.Camera(a_viewport);

            //Set player turn
            cam.setPlayer(a_playerTurn);


            //Draw player text
            a_spriteBatch.Begin();
            a_spriteBatch.DrawString(font, "Current player: " + cam.getPlayer(), new Vector2(40, 6), Color.White);
            a_spriteBatch.DrawString(font2, "Switch player: Space \nSwitch resolution: Enter", new Vector2(40, a_viewport.Height-35), Color.Black);

            //Draw chess board, loop out tiles 
            for (int x = 0; x < Model.Level.LEVEL_WIDTH; x++)
            {
                //Solve chess board pattern
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
                    Vector2 coordinates = cam.convertToVisual(x, y);
                    if (cam.getPlayer() == "black")
                    {
                        coordinates = cam.convertToVisual(((Model.Level.LEVEL_WIDTH - 1) - x), ((Model.Level.LEVEL_WIDTH - 1) - y));
                    }

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

                    if (x == 1 && y == 1)
                    {                        
                        a_spriteBatch.Draw(m_pieceTexture, chessTile, null, Color.White);
                    }
                }
            }
            a_spriteBatch.End();
        }
    }
}