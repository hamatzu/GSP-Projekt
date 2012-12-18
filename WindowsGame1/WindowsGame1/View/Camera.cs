using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.View
{
    class Camera
    {
        private int screenWidth;
        private int screenHeight;

        private float scaleX;
        private float scaleY;

        private int displacementX = 0;
        private int displacementY = 0;

        public Camera(Viewport viewport)
        {
            screenWidth = viewport.Width;
            screenHeight = viewport.Height;

            scaleX = (float)screenWidth / (float)Model.Level.LEVEL_WIDTH;
            scaleY = (float)screenHeight / (float)Model.Level.LEVEL_HEIGHT;
        }

        public Vector2 convertToView(float model_X, float model_Y)
        {
            return new Vector2(model_X * scaleX + displacementX,
                               model_Y * scaleY + displacementY);
        }

        public Vector2 convertToModel(float view_X, float view_Y)
        {
            float model_X = (view_X - displacementX) / scaleX;
            float model_Y = (view_Y - displacementY) / scaleY;

            return new Vector2(model_X, model_Y);
        }

        internal float GetScaleX()
        {
            return scaleX;
        }

        internal float GetScaleY()
        {
            return scaleY;
        }

        internal float GetScreenWidth()
        {
            return screenWidth;
        }

        internal float GetScreenHeight()
        {
            return screenHeight;
        }
    }
}