using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGame1.Model
{
    class FloatRectangle
    {
        Vector2 m_topLeft;
        Vector2 m_bottomRight;

        public FloatRectangle(Vector2 topLeft, Vector2 bottomRight)
        {
            m_topLeft = topLeft;
            m_bottomRight = bottomRight;
        }

        public static FloatRectangle createFromTopLeft(Vector2 a_topLeft, Vector2 a_size)
        {
            Vector2 topLeft = a_topLeft;
            Vector2 bottomRight = a_topLeft + a_size;

            return new FloatRectangle(topLeft, bottomRight);
        }

        public static FloatRectangle createFromCenterBottom(Vector2 a_centerBottom, Vector2 a_size)
        {
            Vector2 topLeft = new Vector2(a_centerBottom.X - a_size.X / 2.0f, a_centerBottom.Y - a_size.Y);
            Vector2 bottomRight = new Vector2(a_centerBottom.X + a_size.X / 2.0f, a_centerBottom.Y);

            return new FloatRectangle(topLeft, bottomRight);
        }

        public float Right
        {
            get { return m_bottomRight.X; }
        }

        public float Bottom
        {
            get { return m_bottomRight.Y; }
        }

        public float Left
        {
            get { return m_topLeft.X; }
        }

        public float Top
        {
            get { return m_topLeft.Y; }
        }

        internal bool isIntersecting(FloatRectangle other)
        {
            if (Right < other.Left)
                return false;
            if (Bottom < other.Top)
                return false;
            if (Left > other.Right)
                return false;
            if (Top > other.Bottom)
                return false;

            return true;
        }

        internal bool isIntersectingTop(FloatRectangle other)
        {
            if (Right < other.Left)
                return false;
            if (Bottom < other.Top)
                return false;
            if (Left > other.Right)
                return false;
            if (Top > other.Bottom)
                return false;

            return true;
        }
    }
}
