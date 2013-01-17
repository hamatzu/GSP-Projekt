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
        List<string> menuItems = new List<string>();
        private int m_centerPosX;
        private int m_centerPosY;
        private int m_buttonSeparation = 45;
        public int activeItem;
        private Vector2 position;
        private Rectangle destinationRectangle;

        public MenuGUI(Microsoft.Xna.Framework.Graphics.SpriteBatch a_spriteBatch, ContentManager a_content, int a_centerPosX, int a_centerPosY)
        {
            // TODO: Complete member initialization
            this.m_spriteBatch = a_spriteBatch;
            m_baseFont = a_content.Load<SpriteFont>("VerdanaMenu");
            m_button = a_content.Load<Texture2D>("menu_bg");
            m_centerPosX = a_centerPosX;
            m_centerPosY = a_centerPosY;
            activeItem = 1;
        }

        public void addItem(string item)
        {
            menuItems.Add(item);
        }

        public void DrawMenu()
        {
            for(int i = 0; i < menuItems.Count; i++)
            {
                //Räkna ut bra position för texten
                position = new Vector2(m_centerPosX - HALF_WIDTH + 15, m_centerPosY + m_buttonSeparation * i - HALF_HEIGHT + 7);

                //Räkna ut position för knappen
                destinationRectangle = new Rectangle(m_centerPosX - HALF_WIDTH, m_centerPosY + m_buttonSeparation * i - HALF_HEIGHT, HALF_WIDTH * 2, HALF_HEIGHT * 2);

                string menuItemText = menuItems.ElementAt(i);
                if (activeItem == i + 1)
                {
                    m_spriteBatch.Draw(m_button, destinationRectangle, Color.Purple);
                    m_spriteBatch.DrawString(m_baseFont, menuItemText, position, Color.White);
                }
                else
                {
                    m_spriteBatch.Draw(m_button, destinationRectangle, Color.LightGray);
                    m_spriteBatch.DrawString(m_baseFont, menuItemText, position, Color.LightGray);
                }
            }
        }

        public void setOldState(MouseState a_mouseState)
        {
            m_oldState = a_mouseState;
        }

        internal int getActiveItem()
        {
            return activeItem;
        }

        internal void activeItemDown()
        {
            if (activeItem != menuItems.Count)
            {
                activeItem++;
            }
        }

        internal void activeItemUp()
        {
            if (activeItem != 1)
            {
                activeItem--;
            }
        }

        internal void setActiveItem(int item)
        {
            activeItem = item;
        }
    }
}