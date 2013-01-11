using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace WindowsGame1.Controller
{
    class MenuGUI
    {
        private static int HALF_WIDTH = 140;
        private static int HALF_HEIGHT = 20;

        private MouseState m_oldState;
        private Microsoft.Xna.Framework.Graphics.SpriteBatch m_spriteBatch;
        public SpriteFont m_baseFont;
        public Texture2D m_button;

        public MenuGUI(Microsoft.Xna.Framework.Graphics.SpriteBatch a_spriteBatch, ContentManager a_content)
        {
            // TODO: Complete member initialization
            this.m_spriteBatch = a_spriteBatch;
            m_baseFont = a_content.Load<SpriteFont>("VerdanaMenu");
            m_button = a_content.Load<Texture2D>("menu_bg");

        }


        internal bool DoButton(MouseState a_mouseState, string a_text, int a_centerPosX, int a_centerPosY)
        {
            bool mouseOver = false;
            bool wasClicked = false;

            //Hantera indata och samla på sig information
            //om musen är över knappen 
            if ((a_centerPosX - a_mouseState.X) * (a_centerPosX - a_mouseState.X) < HALF_WIDTH * HALF_WIDTH &&
                (a_centerPosY - a_mouseState.Y) * (a_centerPosY - a_mouseState.Y) < HALF_HEIGHT * HALF_HEIGHT)
            {
                mouseOver = true;

                //om vi klickar
                if (a_mouseState.LeftButton == ButtonState.Released && m_oldState.LeftButton == ButtonState.Pressed)
                {
                    wasClicked = true;
                }
            }



            //Räkna ut bra position för texten
            Vector2 position = new Vector2(a_centerPosX - HALF_WIDTH + 15, a_centerPosY - HALF_HEIGHT + 7);

            //Räkna ut position för knappen
            Rectangle destinationRectangle = new Rectangle(a_centerPosX - HALF_WIDTH, a_centerPosY - HALF_HEIGHT, HALF_WIDTH * 2, HALF_HEIGHT * 2);

            //Generera utdata
            m_spriteBatch.Begin();


            //beroende på tillstånd rita ut knappen
            if (mouseOver)
            {

                //klick
                if (a_mouseState.LeftButton == ButtonState.Pressed)
                {
                    m_spriteBatch.Draw(m_button, destinationRectangle, Color.Purple);
                    m_spriteBatch.DrawString(m_baseFont, a_text, position, Color.Purple);
                }
                else
                {
                    m_spriteBatch.Draw(m_button, destinationRectangle, Color.Purple);
                    m_spriteBatch.DrawString(m_baseFont, a_text, position, Color.White);
                }
            }
            else
            {
                m_spriteBatch.Draw(m_button, destinationRectangle, Color.LightGray);
                m_spriteBatch.DrawString(m_baseFont, a_text, position, Color.LightGray);
            }

            m_spriteBatch.End();


            return wasClicked;
        }


        public void setOldState(MouseState a_mouseState)
        {
            m_oldState = a_mouseState;
        }
    }
}