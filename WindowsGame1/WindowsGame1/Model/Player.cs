using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WindowsGame1.Model
{
    class Player
    {
        Vector2 centerBottomPosition = new Vector2(5.0f, 5);
        Vector2 playerSpeed = new Vector2(0, 0f);

        private float playerWidth = 1;
        private float playerHeight = 1;
        private Vector2 playerSize = new Vector2(.95f, .95f);
        private FloatRectangle playerBoundingBox;

        State currentState = State.Standing;
        TimeSpan timePerFrame = TimeSpan.FromSeconds((float)1 / 14);
        Point currentFrame = new Point(0, 0);
        float totalElapsed;

        Direction currentDirection;

        public enum Direction
        {
            Left,
            Right
        }

        public enum State
        {
            Standing,
            Walking,
            Jumping,
            Falling,
            Stop
        }

        internal void Update(float a_elapsedTime)
        {
            Vector2 gravityAcceleration = new Vector2(0.0f, 9.82f);

            //integrate position
            centerBottomPosition = centerBottomPosition + playerSpeed * a_elapsedTime + gravityAcceleration * a_elapsedTime * a_elapsedTime;

            //integrate speed
            playerSpeed = playerSpeed + a_elapsedTime * gravityAcceleration;
           

            if (currentDirection == Direction.Left)
            {
                currentFrame.Y = 1;
            }
            else
            {
                currentFrame.Y = 0;
            }

            if(currentState == State.Jumping)
            {
                currentFrame.X = 7;
            }
            else if (currentState == State.Falling)
            {
                currentFrame.X = 8;
            }
            else if (currentState == State.Walking)
            {
                totalElapsed += a_elapsedTime;
                if (totalElapsed > timePerFrame.TotalSeconds)
                {
                    currentFrame.X++;
                    if (currentFrame.X >= 6)
                    {
                        currentFrame.X = 0;
                    }
                    totalElapsed -= (float)timePerFrame.TotalSeconds;
                }
            }
            else if(currentState == State.Falling)
            {
                currentFrame.X = 7;
            }
            else
            {
                currentFrame.X = 0;
            }


        }

        internal Vector2 getPlayerSpeed()
        {
            return playerSpeed;
        }

        internal Vector2 getPlayerSize()
        {
            return playerSize;
        }

        internal State getCurrentState()
        {
            return currentState;
        }

        internal FloatRectangle getPlayerBoundingBox()
        {
            return playerBoundingBox;
        }

        internal float getPlayerHeight()
        {
            return playerHeight;
        }

        internal float getPlayerWidth()
        {
            return playerWidth;
        }

        internal Vector2 getCenterBottomPosition()
        {
            return centerBottomPosition;
        }

        internal void playerJump()
        {
            currentState = State.Jumping;
            playerSpeed.Y = -7f;
        }

        internal void stop()
        {
            currentState = State.Standing;
            playerSpeed.X = 0f;
            playerSpeed.Y = 0f;
        }

        internal void setPlayerPosition(Vector2 a_pos)
        {
            centerBottomPosition = a_pos;
        }

        internal void setPlayerSpeed(Vector2 a_speed)
        {
            playerSpeed = a_speed;
        }

        internal Point getCurrentFrame()
        {
            return currentFrame;
        }

        internal void setCurrentState(State state)
        {
            currentState = state;
        }
        internal void setCurrentDirection(Direction direction)
        {
            currentDirection = direction;
        }
    }
}
