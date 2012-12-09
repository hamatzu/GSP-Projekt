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

        private int displacementX = 20;
        private int displacementY = 20;

        private float borderWidth = 2f;

        public Camera(Viewport viewport)
        {
            screenWidth = viewport.Width;
            screenHeight = viewport.Height;

            scaleX = (float)(screenWidth - 2 * displacementX - borderWidth) / (float)Model.Level.LEVEL_WIDTH;
            scaleY = (float)(screenHeight - 2 * displacementY - borderWidth) / (float)Model.Level.LEVEL_HEIGHT;
        }

        public Vector2 convertToView(float model_X, float model_Y)
        {
            return new Vector2(model_X * scaleX + displacementX,
                               model_Y * scaleY + displacementY);
        }

        internal float GetBorderWidth()
        {
            return borderWidth;
        }

        internal int GetDisplacementX()
        {
            return displacementX;
        }

        internal int GetDisplacementY()
        {
            return displacementY;
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
