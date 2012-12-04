using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Model
{
    class BallSimulation
    {
        Ball ball;

        public BallSimulation()
        {
            ball = new Ball();
        }

        internal void Update(float elapsedTimeSeconds)
        {
            //If X greater than 0 going right, if X smaller than 0 going left
            //If Y greater than 0 going down, if Y smaller than 0 going up
                bool directionX = ball.getBallVelocity().X > 0;
                bool directionY = ball.getBallVelocity().Y > 0;

            //Check collision with right wall
                if (ball.getBallCenterPosition().X > Model.Level.LEVEL_WIDTH - ball.getBallRadius() && directionX == true)
                {
                    Console.WriteLine("Hit right wall!");
                    ball.setBallVelocityX(-ball.getBallVelocity().X);
                }

            //Check collision with left wall
                if (ball.getBallCenterPosition().X < 0 + ball.getBallRadius() && directionX == false)
                {
                    Console.WriteLine("Hit left wall!");
                    ball.setBallVelocityX(Math.Abs(ball.getBallVelocity().X));
                }

            //Check collision with top wall
                if (ball.getBallCenterPosition().Y < 0 + ball.getBallRadius() && directionY == false)
                {
                    Console.WriteLine("Hit top wall!");
                    ball.setBallVelocityY(ball.getBallVelocity().Y - (ball.getBallVelocity().Y * 2));
                }

            //Check collision with bottom wall
                if (ball.getBallCenterPosition().Y > Model.Level.LEVEL_HEIGHT - ball.getBallRadius() && directionY == true)
                {
                    Console.WriteLine("Hit bottom wall!");
                    ball.setBallVelocityY(-ball.getBallVelocity().Y);
                }

            //Calculate center position and move the ball depending on collision
                ball.setBallCenterPosition(ball.getBallCenterPosition() + ball.getBallVelocity() * elapsedTimeSeconds);
        }

        internal Ball getBall()
        {
            return ball;
        }
    }
}
