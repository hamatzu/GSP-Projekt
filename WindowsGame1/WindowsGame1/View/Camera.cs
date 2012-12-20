using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WindowsGame1.View
{
    class Camera
    {
        private float scaleX;
        private float scaleY;

        private Vector2 modelCenterPosition = new Vector2(0, 0);
        private float scale = 32.0f;

        private int displacementX = 0;
        private int displacementY = 0;

        private int screenHeight;
        private int screenWidth;

        Vector2 modelTopLeftPosition = new Vector2(0,0);

        public Camera(Viewport viewPort)
        {
            screenWidth = viewPort.Width;
            screenHeight = viewPort.Height;
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

        internal float getScaleX()
        {
            return scaleX;
        }

        internal float getScaleY()
        {
            return scaleY;
        }

        internal int getScreenWidth()
        {
            return screenWidth;
        }

        internal int getScreenHeight()
        {
            return screenHeight;
        }

        internal float getScale()
        {
            return scale;
        }

        internal Vector2 getViewPosition(float x, float y, Vector2 a_viewPortSize)
        {
            Vector2 modelPosition = new Vector2(x, y);

            Vector2 modelViewPortSize = new Vector2(a_viewPortSize.X / scale, a_viewPortSize.Y / scale);

            //get model top left position
            modelTopLeftPosition = modelCenterPosition - modelViewPortSize / 2.0f;

            return (modelPosition - modelTopLeftPosition  )* scale;
        }

        internal Vector2 getModelTopLeftPosition()
        {
            return modelTopLeftPosition;
        }

        internal void setZoom(float a_scale)
        {
            scale = a_scale;
        }


        internal void centerOn(Vector2 a_newCenterPosition, Vector2 a_viewPortSize, Vector2 a_levelSize)
        {
            modelCenterPosition = a_newCenterPosition;

            Vector2 modelViewPortSize = new Vector2(a_viewPortSize.X / scale, a_viewPortSize.Y / scale);

            //check left
            if (modelCenterPosition.X < modelViewPortSize.X / 2.0f)
            {
                modelCenterPosition.X = modelViewPortSize.X / 2.0f;
            }

            //check bottom
            if (modelCenterPosition.Y > a_levelSize.Y - modelViewPortSize.Y / 2.0f)
            {
                modelCenterPosition.Y = a_levelSize.Y - modelViewPortSize.Y / 2.0f;
            }

            //check top
            if (modelCenterPosition.Y < modelViewPortSize.Y / 2.0f)
            {
                modelCenterPosition.Y = modelViewPortSize.Y / 2.0f;
            }
        }

        internal void setGameScale()
        {
            //Räkna ut spelets skala
            scaleX = (float)screenWidth / (float)Model.Level.LEVEL_WIDTH;
            scaleY = (float)screenHeight / (float)Model.Level.LEVEL_HEIGHT;
        }
    }
}
