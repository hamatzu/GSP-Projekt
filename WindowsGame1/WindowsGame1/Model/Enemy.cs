using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGame1.Model
{
    class Enemy
    {
        Vector2 centerBottomPosition;
        Vector2 enemySpeed = new Vector2(0, 0f);
        private Vector2 enemySize = new Vector2(.95f, .95f);

        TimeSpan timePerFrame = TimeSpan.FromSeconds((float)1 / 6);
        Point currentFrame = new Point(0, 0);
        float totalElapsed;

        private float enemyWidth = 1;
        private float enemyHeight = 1;

        float distance;
        float oldDistance;
        Direction currentDirection;
        FloatRectangle enemyBoundingBox;
        private EnemyType enemyType;
        private FloatRectangle enemyTopBoundingBox;


        public enum EnemyType
        {
            Ghost,
            HipHopper,
            Rasta,
            HardRocker
        }

        public enum Direction
        {
            Left,
            Right
        }

        public Enemy(Vector2 enemyBottomPosition, float a_scale)
        {
            distance = 20f * a_scale;
            oldDistance = distance;
            centerBottomPosition = enemyBottomPosition;
        }

        public void createBoundingBox()
        {
            Vector2 boundingBox = new Vector2(centerBottomPosition.X - enemySize.X / 2, (centerBottomPosition.Y - enemySize.Y));
            enemyBoundingBox = FloatRectangle.createFromTopLeft(boundingBox, enemySize);
        }
        public void createTopBoundingBox()
        {
            Vector2 boundingBox = new Vector2((centerBottomPosition.X - enemySize.X / 2) + .1f, (centerBottomPosition.Y - enemySize.Y - .1f));
            Vector2 topSize = new Vector2(.8f, .1f);
            enemyTopBoundingBox = FloatRectangle.createFromTopLeft(boundingBox, topSize);
        }

        public FloatRectangle getEnemyBoundingBox()
        {
            return enemyBoundingBox;
        }

        internal void Update(float a_elapsedTime)
        {

           Vector2 gravityAcceleration = new Vector2(0.0f, 9.82f);

            if (enemyType == EnemyType.HipHopper)
            {
                totalElapsed += a_elapsedTime;
                if (totalElapsed > timePerFrame.TotalSeconds)
                {
                    currentFrame.X++;
                    if (currentFrame.X >= 4)
                    {
                        currentFrame.X = 0;
                    }
                    totalElapsed -= (float)timePerFrame.TotalSeconds;
                }
            }

            if (enemyType == EnemyType.Rasta)
            {
                if (distance <= 0)
                {
                    currentDirection = Direction.Right;
                }
                else if (distance >= oldDistance)
                {
                    currentDirection = Direction.Left;
                    currentFrame.Y = 1;
                }

                if (currentDirection == Direction.Right)
                {
                    distance += 1f;
                    setEnemySpeed(new Vector2(1.5f, getEnemySpeed().Y));
                    currentFrame.Y = 0;
                }
                else
                {
                    distance -= 1f;
                    setEnemySpeed(new Vector2(-1.5f, getEnemySpeed().Y));
                }

                totalElapsed += a_elapsedTime;
                if (totalElapsed > timePerFrame.TotalSeconds)
                {
                    currentFrame.X++;
                    if (currentFrame.X >= 2)
                    {
                        currentFrame.X = 0;
                    }
                    totalElapsed -= (float)timePerFrame.TotalSeconds;
                }

            }

            //integrate position
            centerBottomPosition = centerBottomPosition + enemySpeed * a_elapsedTime + gravityAcceleration * a_elapsedTime * a_elapsedTime;

            //integrate speed
            enemySpeed = enemySpeed + a_elapsedTime * gravityAcceleration;
            
            createBoundingBox();
            createTopBoundingBox();
        }

        internal Vector2 getCenterBottomPosition()
        {
            return centerBottomPosition;
        }

        internal Point getCurrentFrame()
        {
            return currentFrame;
        }

        internal Vector2 getEnemySpeed()
        {
            return enemySpeed;
        }

        internal Vector2 getEnemySize()
        {
            return enemySize;
        }

        internal void setEnemyPosition(Vector2 a_pos)
        {
            centerBottomPosition = a_pos;
        }

        internal void setEnemySpeed(Vector2 a_speed)
        {
            enemySpeed = a_speed;
        }

        internal void setEnemyType(EnemyType type)
        {
            enemyType = type;
        }

        internal EnemyType getEnemyType()
        {
            return enemyType;
        }

        internal FloatRectangle getEnemyTopBoundingBox()
        {
            return enemyTopBoundingBox;
        }
    }
}
