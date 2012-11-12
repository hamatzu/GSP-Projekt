using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace DrawBox
{
    class BoxView
    {
        private SpriteBatch m_spriteBatch;
        private Texture2D m_boxTexture;


        public BoxView(GraphicsDevice graphicsDevice, ContentManager content)
        {
            m_spriteBatch = new SpriteBatch(graphicsDevice);

            m_boxTexture = content.Load<Texture2D>("bubbles");

        }


        internal void drawBox()
        {

            Rectangle destrect = new Rectangle(10, 10, 128, 128);
            

            m_spriteBatch.Begin();
            m_spriteBatch.Draw(m_boxTexture, destrect, Color.White);
            m_spriteBatch.End();
        }

        internal void boxClicked()
        {
            Rectangle destrect = new Rectangle(100, 100, 128, 128);


            m_spriteBatch.Begin();
            m_spriteBatch.Draw(m_boxTexture, destrect, Color.White);
            m_spriteBatch.End();
        }

    }
}
