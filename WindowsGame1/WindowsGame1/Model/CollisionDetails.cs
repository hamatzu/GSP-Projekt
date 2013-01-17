using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGame1.Model
{
    class CollisionDetails
    {
        public Vector2 _speedAfterCollision;
        public Vector2 _positionAfterCollision;
        public bool _hasCollidedWithGround;

        public CollisionDetails(Vector2 a_oldPos, Vector2 a_velocity)
        {
            _positionAfterCollision = a_oldPos;
            _speedAfterCollision = a_velocity;
        }

        public bool hasCollidedWithGround()
        {
            return _hasCollidedWithGround;
        }

        internal Vector2 positionAfterCollision()
        {
            return _positionAfterCollision;
        }
        internal Vector2 speedAfterCollision()
        {
            return _speedAfterCollision;
        }

        internal void setPositionAfterCollision(Vector2 slidingPosition)
        {
            _positionAfterCollision = slidingPosition;
        }

        internal void setHasCollidedWithGround(bool collide)
        {
            _hasCollidedWithGround = collide;
        }

        internal void setSpeedAfterCollision(Vector2 speed)
        {
            _speedAfterCollision = speed;
        }
    }
}
