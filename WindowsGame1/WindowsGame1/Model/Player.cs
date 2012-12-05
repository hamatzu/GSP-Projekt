using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGame1.Model
{
    class Player
    {
        Vector2 centerBottomPosition = new Vector2(5, 5);
        Vector2 playerSpeed = new Vector2(0, 0f);

        private float playerWidth = 1;
        private float playerHeight = 1;


        internal void Update(float a_elapsedTime)
        {
            Vector2 gravityAcceleration = new Vector2(0.0f, 0f);

            //integrate position
            centerBottomPosition = centerBottomPosition + playerSpeed * a_elapsedTime + gravityAcceleration * a_elapsedTime * a_elapsedTime;

            //integrate speed
            playerSpeed = playerSpeed  * gravityAcceleration;

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

        internal void goLeft()
        {
            Console.WriteLine("go left");
            playerSpeed.X -= 5f;
        }

        internal void goRight()
        {
            Console.WriteLine("go right");
            playerSpeed.X += 5f;
        }

        internal void stop()
        {
            playerSpeed.X = 0f;
        }
        
        
    }
}
