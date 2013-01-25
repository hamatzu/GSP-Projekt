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


        private Vector2 ball_centerposition;
        private float ball_width = Level.LEVEL_WIDTH / 20;
        private float ball_radius = (Level.LEVEL_WIDTH / 20) / 2;

        private Vector2 ball_velocity;
        private bool isHit = false; 

        public Ball(int randomSeed)
        {
            Random r = new Random(randomSeed);
            int value_1 = r.Next(1, 10);
            int value_2 = r.Next(1, 10);
            ball_velocity = new Vector2(value_1, value_2);
            ball_centerposition = new Vector2(Level.LEVEL_WIDTH / value_1, Level.LEVEL_HEIGHT / value_2);
        }

        internal bool isDead()
        {
            return isHit;
        }

        internal void setHit(bool hit)
        {
            isHit = hit;
        }

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
