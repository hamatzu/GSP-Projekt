using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Model
{
    class Ball
    {
        private Vector2 ball_centerposition = new Vector2(Level.LEVEL_WIDTH / 2, Level.LEVEL_HEIGHT / 2);
        private float ball_width = Level.LEVEL_WIDTH / 10;
        private float ball_radius = (Level.LEVEL_WIDTH / 10) / 2;

        private Vector2 ball_velocity = new Vector2(0.5f, 0.6f);

        internal float getBallWidth()
        {
            return ball_width;
        }

        internal float getBallRadius()
        {
            return ball_radius;
        }

        internal Vector2 getBallCenterPosition()
        {
            return ball_centerposition;
        }

        internal void setBallCenterPosition(Vector2 centerPosition)
        {
            ball_centerposition = centerPosition;
        }

        internal Vector2 getBallVelocity()
        {
            return ball_velocity;
        }

        internal void setBallVelocityX(float velocity)
        {
            ball_velocity.X = velocity;
        }

        internal void setBallVelocityY(float velocity)
        {
            ball_velocity.Y = velocity;
        }
    }
}
