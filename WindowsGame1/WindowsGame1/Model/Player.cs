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

        int playerLifes;
        private Vector2 playerSize = new Vector2(.90f, .95f);
        private FloatRectangle playerBoundingBox;

        State currentState = State.Standing;
        TimeSpan timePerFrame = TimeSpan.FromSeconds((float)1 / 12);
        Point currentFrame = new Point(0, 0);
        float totalElapsed;

        private bool mustDance = false;
        private bool lostALife = false;
        bool allowedJump = true;

        Direction currentDirection;
        private Color playerColor;
        private float maxBlinkingTime = 3f;
        float blinkingTime = 0f;
        State blinkingState = State.NotBlinking;


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
            Stop,
            Blinking,
            NotBlinking,
            Dancing
        }

        public Player()
        {
            playerLifes = 3;
        }

        public void createBoundingBox()
        {
            Vector2 boundingBox = new Vector2(centerBottomPosition.X - playerSize.X / 2, (centerBottomPosition.Y - playerSize.Y));
            playerBoundingBox = FloatRectangle.createFromTopLeft(boundingBox, playerSize);
        }

        internal void Update(float a_elapsedTime)
        {
            Vector2 gravityAcceleration = new Vector2(0.0f, 9.82f);

            //integrate position
            centerBottomPosition = centerBottomPosition + playerSpeed * a_elapsedTime + gravityAcceleration * a_elapsedTime * a_elapsedTime;

            //integrate speed
            playerSpeed = playerSpeed + a_elapsedTime * gravityAcceleration;

            mustDance = false;

            if (currentDirection == Direction.Left)
            {
                currentFrame.Y = 1;
            }
            else
            {
                currentFrame.Y = 0;
            }

            if (blinkingState == State.Blinking)
            {
                if (blinkingTime < maxBlinkingTime)
                {
                    if (blinkingTime < maxBlinkingTime)
                        blinkingTime += a_elapsedTime;

                    playerColor = Color.Red;
                }
                else
                {
                    blinkingTime = 0;
                    playerColor = Color.White;
                    blinkingState = State.NotBlinking;
                }

            }
            else
            { playerColor = Color.White; }

            if (currentState == State.Jumping)
            {
                mustDance = false;
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
            else if (currentState == State.Dancing)
            {
                totalElapsed += a_elapsedTime / 10;
                if (totalElapsed > timePerFrame.TotalSeconds)
                {
                    currentFrame.X = 0;
                    if (currentDirection == Direction.Left)
                    {
                        currentDirection = Direction.Right;
                    }
                    else
                    {
                        currentDirection = Direction.Left;
                    }
                    totalElapsed -= (float)timePerFrame.TotalSeconds;
                }
            }
            else
            {
                currentFrame.X = 0;
            }

            createBoundingBox();
        }

        internal Direction getCurrentDirection()
        {
            return currentDirection;
        }

        internal Color getPlayerColor()
        {
            return playerColor;
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

        internal Vector2 getCenterBottomPosition()
        {
            return centerBottomPosition;
        }

        internal void playerJump()
        {
            if (isAllowedJump())
            {
                currentState = State.Jumping;
                playerSpeed.Y = -6.5f;
            }
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

        internal bool isAllowedJump()
        {
            return allowedJump;
        }

        internal void setAllowedJump(bool jump)
        {
            allowedJump = jump;
        }

        internal int getLifes()
        {
            return playerLifes;
        }

        internal void lostLife()
        {
            playerLifes = playerLifes - 1;
            lostALife = true;
        }

        internal State getBlinkingState()
        {
            return blinkingState;
        }

        internal void setBlinkingState(State state)
        {
            blinkingState = state;
        }

        internal bool mustBoogie()
        {
            return mustDance;
        }

        internal void setMustBoogie(bool dance)
        {
            mustDance = dance;
        }

        internal bool didLostLife()
        {
            return lostALife;
        }

        internal void setLostLife(bool life)
        {
            lostALife = life;
        }

        internal void lifeUp()
        {
            playerLifes++;
        }
    }
}
