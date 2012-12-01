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
        string playerTurn = "white";
        float visualX;
        float visualY;
        int screenWidth;
        int screenHeight;
        float scaleX;
        float scaleY;
        int borderSize;
        float drawAreaX;
        float drawAreaY;

        public Camera(Viewport a_viewport)
        {
            screenWidth = a_viewport.Width;
            screenHeight = a_viewport.Height;

            borderSize = 40;

            scaleX = ((float)screenWidth - 2 * borderSize) / (float)Model.Level.LEVEL_WIDTH;
            scaleY = ((float)screenHeight - 2 * borderSize) / (float)Model.Level.LEVEL_HEIGHT;

            drawAreaX = ((float)screenWidth - 2 * borderSize);
            drawAreaY = ((float)screenHeight - 2 * borderSize);

        }

        internal void setPlayer(string a_playerTurn)
        {
            playerTurn = a_playerTurn;
        }

        internal String getPlayer()
        {
            return playerTurn;
        }

        public Vector2 convertToVisual(float a_logicX, float a_logicY)
        {
            visualX = borderSize + (a_logicX * scaleX);
            visualY = borderSize + (a_logicY * scaleY);

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
    }
}
