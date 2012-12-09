using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGame1.Model
{
    class Game
    {
        //Spelets beståndsdelar
        private Player player = new Player();
        private Level level = new Level();
        private bool hasCollidedWithGround;

        internal void UpdateGame(float a_elapsedTimeSeconds)
        {
            float timeStep = 0.001f;
            if (a_elapsedTimeSeconds > 0)
            {
                int numIterations = (int)(timeStep / a_elapsedTimeSeconds);

                for (int i = 0; i < numIterations; i++)
                {
                    UpdatePlayer(timeStep);
                }

                float timeLeft = a_elapsedTimeSeconds - timeStep * numIterations;
                UpdatePlayer(timeLeft);
            }

        }

        private void UpdatePlayer(float a_elapsedTimeSeconds)
        {
            //Get the old position
            Vector2 oldPos = player.getCenterBottomPosition();

            //Get the new position
            player.Update(a_elapsedTimeSeconds);

            Vector2 newPos = player.getCenterBottomPosition();

            //Collide
            hasCollidedWithGround = false;
            Vector2 speed = player.getPlayerSpeed();

            if (didCollide(newPos, player.getPlayerSize()))
            {
                CollisionDetails details = getCollisionDetails(oldPos, newPos, player.getPlayerSize(), speed);
                hasCollidedWithGround = details.hasCollidedWithGround();

                //set the new speed and position after collision
                player.setPlayerPosition(details.positionAfterCollision());
                player.setPlayerSpeed(details.speedAfterCollision());
            }
        }

        private bool didCollide(Vector2 a_centerBottom, Vector2 a_size)
        {
            FloatRectangle occupiedArea = FloatRectangle.createFromCenterBottom(a_centerBottom, a_size);
            if (level.IsCollidingAt(occupiedArea))
            {
                return true;
            }
            return false;
        }

        private CollisionDetails getCollisionDetails(Vector2 a_oldPos, Vector2 a_newPosition, Vector2 a_size, Vector2 a_velocity)
        {
            CollisionDetails details = new CollisionDetails(a_oldPos, a_velocity);

            Vector2 slidingXPosition = new Vector2(a_newPosition.X, a_oldPos.Y); //Y movement ignored
            Vector2 slidingYPosition = new Vector2(a_oldPos.X, a_newPosition.Y); //X movement ignored

            if (didCollide(slidingXPosition, a_size) == false)
            {
                return doOnlyXMovement(ref a_velocity, details, ref slidingXPosition);
            }
            else if (didCollide(slidingYPosition, a_size) == false)
            {

                return doOnlyYMovement(ref a_velocity, details, ref slidingYPosition);
            }
            else
            {
                return doStandStill(details, a_velocity);
            }

        }

        private static CollisionDetails doStandStill(CollisionDetails details, Vector2 a_velocity)
        {
            if (a_velocity.Y > 0)
            {
                details.setHasCollidedWithGround(true);
            }
            details.setSpeedAfterCollision(new Vector2(0, 0));
            return details;
        }

        private static CollisionDetails doOnlyYMovement(ref Vector2 a_velocity, CollisionDetails details, ref Vector2 slidingYPosition)
        {
            a_velocity.X *= -0.5f; //bounce from wall
            details.setSpeedAfterCollision(a_velocity);
            details.setPositionAfterCollision(slidingYPosition);
            return details;
        }

        private static CollisionDetails doOnlyXMovement(ref Vector2 a_velocity, CollisionDetails details, ref Vector2 slidingXPosition)
        {
            details.setPositionAfterCollision(slidingXPosition);
            //did we slide on ground?
            if (a_velocity.Y > 0)
            {
                details.setHasCollidedWithGround(true);
            }

            details.setSpeedAfterCollision(doSetSpeedOnVerticalCollision(a_velocity));
            return details;
        }

        private static Vector2 doSetSpeedOnVerticalCollision(Vector2 a_velocity)
        {
            //did we collide with ground?
            if (a_velocity.Y > 0)
            {
                a_velocity.Y = 0; //no bounce
            }
            else
            {
                //collide with roof
                a_velocity.Y *= -1.0f;
            }

            a_velocity.X *= 0.10f;

            return a_velocity;
        }

        internal bool canJump()
        {
            return hasCollidedWithGround;
        }

        internal void playerJump()
        {
            if (canJump())
            {
                player.playerJump();
            }
            
        }


        internal Level getLevel()
        {
            return level;
        }

        internal Vector2 getPlayerPosition()
        {
            return player.getCenterBottomPosition();
        }

        internal float getPlayerSpeed()
        {
            return player.getPlayerSpeed().Length();
        }

        internal void goLeft()
        {
            player.setPlayerSpeed(new Vector2(-5.0f, player.getPlayerSpeed().Y));
        }

        internal void goRight()
        {
            player.setPlayerSpeed(new Vector2(5.0f, player.getPlayerSpeed().Y));
        }

        internal Player getPlayer()
        {
            return player;
        }
    }
}
