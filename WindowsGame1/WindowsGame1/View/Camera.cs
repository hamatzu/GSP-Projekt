using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WindowsGame1.Model
{
    class Camera
    {
        float visualX;
        float visualY;
        int screenWidth;
        int screenHeight;
        float scaleX;
        float scaleY;
        int borderSize;

        public Camera(Viewport a_viewport)
        {
            screenWidth = a_viewport.Width;
            screenHeight = a_viewport.Height;

            borderSize = 40;

            scaleX = ((float)screenWidth - 2 * borderSize) / (float)Model.Level.LEVEL_WIDTH;
            scaleY = ((float)screenHeight - 2 * borderSize) / (float)Model.Level.LEVEL_HEIGHT;

        }


        public Vector2 convertToWhiteVisual(float a_logicX, float a_logicY)
        {
            visualX = borderSize + (a_logicX * scaleX);
            visualY = borderSize + (a_logicY * scaleY);

            Vector2 m_visualCoordinates = new Vector2(visualX, visualY);
            return m_visualCoordinates;
        }

        public Vector2 convertToBlackVisual(float a_logicX, float a_logicY)
        {
            visualX = borderSize + (((Model.Level.LEVEL_WIDTH - 1) - a_logicX) * scaleX);
            visualY = borderSize + (((Model.Level.LEVEL_HEIGHT - 1) - a_logicY) * scaleY);

            Vector2 m_visualCoordinates = new Vector2(visualX, visualY);
            return m_visualCoordinates;
        }

        internal float getScaleX()
        {
            return scaleX;
        }

        internal float getScaleY()
        {
            return scaleY;
        }

        internal int getScreenHeight()
        {
            return screenHeight;
        }
    }
}
