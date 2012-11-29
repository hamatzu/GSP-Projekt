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
        public Vector2 ball_centerposition = new Vector2(2, 5);
        public float ball_radius = 0.5f;
        private float ball_width = Level.LEVEL_WIDTH / 10;


        private Vector2 ball_velocity = new Vector2(4f, 1.3f);



        internal void Update(float elapsedTimeSeconds, Viewport a_viewport)
        {
            //If X greater than 0 going right, if X smaller than 0 going left
            //If Y greater than 0 going down, if Y smaller than 0 going up
            bool directionX = ball_velocity.X > 0;
            bool directionY = ball_velocity.Y > 0;

            if (ball_centerposition.X > Model.Level.LEVEL_WIDTH - ball_radius * 2 && directionX == true)
            {
                Console.WriteLine("Hit right wall!");
                ball_velocity.X = -ball_velocity.X;
            }

            if (ball_centerposition.X < 0 + ball_radius && directionX == false)
            {
                Console.WriteLine("Hit left wall!");
                ball_velocity.X = Math.Abs(ball_velocity.X);
            }

            if (ball_centerposition.Y < 0 + ball_radius && directionY == false)
            {
                Console.WriteLine("Hit top wall!");
                ball_velocity.Y = ball_velocity.Y - (ball_velocity.Y * 2);
            }

            if (ball_centerposition.Y > Model.Level.LEVEL_HEIGHT - ball_radius * 2 && directionY == true)
            {
                Console.WriteLine("Hit bottom wall!");
                ball_velocity.Y = -(ball_velocity.Y) ;
            }

            ball_centerposition = ball_centerposition + ball_velocity * elapsedTimeSeconds;

        }

        internal float getBallWidth()
        {
            return ball_width;
        }

    }
}
