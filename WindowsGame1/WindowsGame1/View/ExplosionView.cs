using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WindowsGame1.View
{
    class ExplosionView
    {
        private SpriteBatch m_spriteBatch;
        private Texture2D m_explosionTexture;
        private float timeElapsed;
        private Vector2 numFrames = new Vector2(4f, 6f);
        private float numberOfFrames = 24;
        private float maxTime = 1f;
        private float frameX;
        private float frameY;
        private int textureTileSize = 128;
        private float timeDelay = 2f;

        public ExplosionView(Microsoft.Xna.Framework.Graphics.SpriteBatch a_spriteBatch)
        {
            m_spriteBatch = a_spriteBatch;
        }

        internal void Update(float a_elapsedTime)
        {

            if (timeElapsed > maxTime + timeDelay)
            {
                timeElapsed = 0f;
            }

            timeElapsed += a_elapsedTime;
            float percentAnimated = timeElapsed / maxTime;
            int frame = (int)(percentAnimated * numberOfFrames);
            frameX = frame % numFrames.X;
            frameY = frame / numFrames.X;
            
        }

        internal void Draw()
        {
            m_spriteBatch.Begin();

            Rectangle sourceRectangle = new Rectangle(textureTileSize * (int)frameX, textureTileSize * (int)frameY, textureTileSize, textureTileSize);

            Rectangle destRect = new Rectangle(130, 130, textureTileSize, textureTileSize);


            m_spriteBatch.Draw(m_explosionTexture, destRect, sourceRectangle, Color.White);

            m_spriteBatch.End();
        }

        internal void LoadContent(Microsoft.Xna.Framework.Content.ContentManager a_content)
        {
            m_explosionTexture = a_content.Load<Texture2D>("explosion");
        }
    }
}
