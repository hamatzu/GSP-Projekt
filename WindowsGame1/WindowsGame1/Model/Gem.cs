using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGame1.Model
{
    class Gem
    {
        Vector2 centerBottomPosition;
        private Vector2 gemSize = new Vector2(.95f, .95f);
        Vector2 gemSpeed = new Vector2(0, 0f);

        TimeSpan timePerFrame = TimeSpan.FromSeconds((float)1 / 6);
        Point currentFrame = new Point(0, 0);

        FloatRectangle gemBoundingBox;
        private GemType currentGemType;

        public enum GemType
        {
            Life,
            PowerUp
        }

        public Gem(Vector2 gemBottomPosition)
        {
            centerBottomPosition = gemBottomPosition;
            createGemBoundingBox();
        }

        public void createGemBoundingBox()
        {
            Vector2 boundingBox = new Vector2(centerBottomPosition.X, (centerBottomPosition.Y));
            gemBoundingBox = FloatRectangle.createFromTopLeft(boundingBox, gemSize);
        }

        internal void setGemType(GemType gemType)
        {
            currentGemType = gemType;
        }

        internal GemType getGemType()
        {
            return currentGemType;
        }


        internal Vector2 getCenterBottomPosition()
        {
            return centerBottomPosition;
        }

        internal Vector2 getGemSize()
        {
            return gemSize;
        }

        internal Vector2 getGemSpeed()
        {
            return gemSpeed;
        }

        internal void setGemPosition(Vector2 a_pos)
        {
            centerBottomPosition = a_pos;
        }

        internal void setGemSpeed(Vector2 a_speed)
        {
            gemSpeed = a_speed;
        }

        internal FloatRectangle getGemBoundingBox()
        {
            return gemBoundingBox;
        }
    }
}
