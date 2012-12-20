using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using WindowsGame1.View;

namespace WindowsGame1.Model
{
    class Game
    {
        //Spelets beståndsdelar
        public Player player;
        public Level level;

        private bool hasCollidedWithGround;
        public bool gameOver = false;
        SoundEffect enemyHit;
        SoundEffectInstance enemyHitInstance;

        public Game(Camera a_camera)
        {
            player = new Player();
            level = new Level(player, a_camera);
            player.createBoundingBox();
        }

        internal void LoadContent(Microsoft.Xna.Framework.Content.ContentManager a_content)
        {
            enemyHit = a_content.Load<SoundEffect>("enemyHit");
            enemyHitInstance = enemyHit.CreateInstance();
        }

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
                UpdateEnemies(timeLeft);
                UpdatePlayer(timeLeft);
            }

            if (player.getCenterBottomPosition().Y > Model.Level.LEVEL_HEIGHT)
            {
                player.lostLife();
                player.setPlayerPosition(new Vector2(player.getCenterBottomPosition().X - 4f, player.getCenterBottomPosition().Y - 2f));
            }

            if (player.getLifes() == 0)
            {
                gameOver = true;
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

            if(player.getBlinkingState() == Player.State.NotBlinking)
                checkEnemyCollision();


            if (!hasCollidedWithGround)
            {
                player.setCurrentState(Player.State.Falling);
            }
            else
            {
                player.setCurrentState(Player.State.Standing);
            }
        }

        private void UpdateEnemies(float a_elapsedTimeSeconds)
        {
            foreach (Enemy aEnemy in level.getLevelEnemies())
            {
                //Get the old position
                Vector2 oldPos = aEnemy.getCenterBottomPosition();

                //Get the new position
                aEnemy.Update(a_elapsedTimeSeconds);

                Vector2 newPos = aEnemy.getCenterBottomPosition();

                //Collide
                hasCollidedWithGround = false;
                Vector2 speed = aEnemy.getEnemySpeed();

                if (didCollide(newPos, aEnemy.getEnemySize()))
                {
                    CollisionDetails details = getCollisionDetails(oldPos, newPos, aEnemy.getEnemySize(), speed);
                    hasCollidedWithGround = details.hasCollidedWithGround();

                    //set the new speed and position after collision
                    aEnemy.setEnemyPosition(details.positionAfterCollision());
                    aEnemy.setEnemySpeed(details.speedAfterCollision());
                }
                
            }

        }

        private void checkEnemyCollision()
        {
            for (int i = 0; i < level.getLevelEnemies().Count; i++)
            {
                Enemy aEnemy = level.getLevelEnemies().ElementAt(i);
                if (aEnemy.getEnemyBoundingBox().isIntersecting(player.getPlayerBoundingBox()))
                {
                    player.lostLife();
                    
                    
                    if (player.getCurrentDirection() == Player.Direction.Right)
                    {
                        player.setPlayerSpeed(new Vector2(-5.5f, player.getPlayerSpeed().Y));
                    }

                    if (player.getCurrentDirection() == Player.Direction.Left)
                    {
                        player.setPlayerSpeed(new Vector2(5.5f, player.getPlayerSpeed().Y));
                    }

                    player.setPlayerPosition(new Vector2(player.getCenterBottomPosition().X, player.getCenterBottomPosition().Y - 1f));

                    player.setBlinkingState(Player.State.Blinking);
                    enemyHitInstance.Play();
                }
            }
        }


                        

        private bool didCollide(Vector2 a_centerBottom, Vector2 a_size)
        {
            FloatRectangle occupiedArea = FloatRectangle.createFromCenterBottom(a_centerBottom, a_size);
            if (level.IsCollidingAt(occupiedArea))
            {
                return true;
            }

            if (occupiedArea.Left < 0)
                return true;

            if (occupiedArea.Right > Model.Level.LEVEL_WIDTH)
                return true;

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
            player.setPlayerSpeed(new Vector2(-2.5f, player.getPlayerSpeed().Y));
        }

        internal void goRight()
        {
            player.setPlayerSpeed(new Vector2(2.5f, player.getPlayerSpeed().Y));
        }

        internal Player getPlayer()
        {
            return player;
        }

    }
}
