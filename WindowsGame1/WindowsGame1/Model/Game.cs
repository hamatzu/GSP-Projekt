using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using WindowsGame1.View;
using Microsoft.Xna.Framework.Graphics;

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
        SoundEffect enemyDead;
        SoundEffectInstance enemyDeadInstance;
        private Camera m_camera;
        private SoundEffect lifeUp;
        private SoundEffectInstance lifeInstance;
        private bool blockFound = false;
        private SpriteBatch m_spriteBatch;

        public Game(Camera a_camera, SpriteBatch a_spriteBatch)
        {
            m_camera = a_camera;
            m_spriteBatch = a_spriteBatch;
            player = new Player();
            level = new Level(player, a_camera);
            player.createBoundingBox();
        }

        internal void LoadContent(Microsoft.Xna.Framework.Content.ContentManager a_content)
        {
            enemyHit = a_content.Load<SoundEffect>("enemyHit");
            enemyHitInstance = enemyHit.CreateInstance();

            enemyDead = a_content.Load<SoundEffect>("pop");
            enemyDeadInstance = enemyDead.CreateInstance();

            lifeUp = a_content.Load<SoundEffect>("heartPop");
            lifeInstance = lifeUp.CreateInstance();
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

            //Check if player intersect with a gem
            checkGemCollision();


            //Game over
            if (player.getLifes() == 0)
            {
                gameOver = true;
            }

            //If player goes outside bottom of map
            if (player.getCenterBottomPosition().Y > Model.Level.LEVEL_HEIGHT)
            {

                blockFound = false;
                player.setPlayerPosition(new Vector2(player.getCenterBottomPosition().X, 13));

                int playerX = (int)player.getCenterBottomPosition().X;
                int playerY = 15;

                //Check for closest tile that is blocked for respawn
                for (int x = playerX; x >= 0; x--)
                {
                    for (int y = 0; y < playerY; y++)
                    {
                        if (level.levelTiles[x, y].isBlocked())
                        {
                            player.setPlayerPosition(new Vector2(x + .6f, y - .3f));
                            blockFound = true;
                            break;
                        }
                    }

                    if (blockFound)
                        break;
                }

                if (player.getBlinkingState() != Player.State.Blinking)
                {
                    player.setBlinkingState(Player.State.Blinking);
                    player.lostLife();
                }
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

            //If player is not hurt i.e not in blinking state check collision with enemy
            if (player.getBlinkingState() == Player.State.NotBlinking)
            {
                checkEnemyCollision(a_elapsedTimeSeconds);
            }

            //Set correct player state
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

                if(aEnemy.getEnemyType() != Enemy.EnemyType.Ghost)
                {
                    if (didCollide(newPos, aEnemy.getEnemySize()))
                    {
                        CollisionDetails details = getCollisionDetails(oldPos, newPos, aEnemy.getEnemySize(), speed);
                        hasCollidedWithGround = details.hasCollidedWithGround();

                        //set the new speed and position after collision
                        aEnemy.setEnemyPosition(details.positionAfterCollision());
                        aEnemy.setEnemySpeed(details.speedAfterCollision());
                    }
                }

                if (aEnemy.getEnemyType() == Enemy.EnemyType.Ghost)
                {
                    Vector2 enemyPosition = aEnemy.getCenterBottomPosition();
                    enemyPosition.Y += aEnemy.getEnemySize().Y / 2;
                    Vector2 playerPosition = player.getCenterBottomPosition();
                    playerPosition.Y = playerPosition.Y + 1;
                    Vector2 enemySpeed = aEnemy.getEnemySpeed();
                    
                    //Check if player is in sight for pursue 
                    if (enemyPosition.X + 3 >= playerPosition.X &&
                        enemyPosition.X - 3 <= playerPosition.X &&
                        enemyPosition.Y + 3 >= playerPosition.Y &&
                        enemyPosition.Y - 3 <= playerPosition.Y)
                    {

                        //If X greater than 0 going right, if X smaller than 0 going left
                        //If Y greater than 0 going down, if Y smaller than 0 going up
                        bool directionX = enemySpeed.X > 0;
                        bool directionY = enemySpeed.Y > 0;

                        if (enemyPosition.X < playerPosition.X && directionX == false)
                        {
                            //Console.WriteLine("go right");
                            aEnemy.setEnemySpeed(new Vector2(1 , enemySpeed.Y));
                        }
                        
                        if (enemyPosition.X > playerPosition.X && directionX == true)
                        {
                            //Console.WriteLine("go left");
                            aEnemy.setEnemySpeed(new Vector2(-1, enemySpeed.Y));
                        }

                        if (enemyPosition.Y < playerPosition.Y && directionY == false)
                        {
                            //Console.WriteLine("go down");
                            aEnemy.setEnemySpeed(new Vector2(enemySpeed.X, .8f));
                        }
                        
                        if (enemyPosition.Y > playerPosition.Y && directionY == true)
                        {
                            //Console.WriteLine("go up");
                            aEnemy.setEnemySpeed(new Vector2(enemySpeed.X,  -.8f));
                        }

                        //Console.WriteLine(enemySpeed);
                    }
                }
            }
        }

                        
        private void checkEnemyCollision(float a_elapsedTime)
        {
            // Loop through level enemies to check collision
            for (int i = 0; i < level.getLevelEnemies().Count; i++)
            {
                Enemy aEnemy = level.getLevelEnemies().ElementAt(i);

                // Kill enemy if jumping on top of it
                if (aEnemy.getEnemyTopBoundingBox().isIntersectingTop(player.getPlayerBoundingBox()) && aEnemy.getEnemyType() == Enemy.EnemyType.Rasta)
                {
                    enemyDeadInstance.Play();
                    player.setPlayerSpeed(new Vector2(player.getPlayerSpeed().X, -3f));
                    aEnemy.setDead();
                    level.getLevelEnemies().RemoveAt(i); 
                }
                //Check collision between enemy and player
                else if (aEnemy.getEnemyBoundingBox().isIntersecting(player.getPlayerBoundingBox()))
                {
                    //Lose life
                    player.lostLife();

                    if (aEnemy.getEnemyType() != Enemy.EnemyType.Ghost)
                    {
                        //Animate player when hurt
                        if (player.getCurrentDirection() == Player.Direction.Right)
                        {
                            player.setPlayerSpeed(new Vector2(-5.5f, player.getPlayerSpeed().Y));
                        }

                        if (player.getCurrentDirection() == Player.Direction.Left)
                        {
                            player.setPlayerSpeed(new Vector2(5.5f, player.getPlayerSpeed().Y));
                        }

                        //Set cooldown
                        player.setPlayerPosition(new Vector2(player.getCenterBottomPosition().X, player.getCenterBottomPosition().Y - .5f));
                    }

                    player.setBlinkingState(Player.State.Blinking);
                    enemyHitInstance.Play();
                }
            }
        }

        private void checkGemCollision()
        {
            for (int i = 0; i < level.getLevelGems().Count; i++)
            {
                Gem aGem = level.getLevelGems().ElementAt(i);
                if (aGem.getGemBoundingBox().isIntersecting(player.getPlayerBoundingBox()) && aGem.getGemType() == Gem.GemType.Life)
                {
                    lifeInstance.Play();
                    player.lifeUp();
                    level.getLevelGems().RemoveAt(i);
                }
            }
        }


        private bool didCollide(Vector2 a_centerBottom, Vector2 a_size)
        {
            FloatRectangle occupiedArea = FloatRectangle.createFromCenterBottom(a_centerBottom, a_size);
            if (level.IsCollidingAt(occupiedArea, a_centerBottom))
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
            a_velocity.Y = 0; //no bounce
            
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
