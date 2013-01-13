using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WindowsGame1.View
{
    class SplitterParticle
    {

        public static Vector2 GRAVITY = new Vector2(0f, 10f);
        public static float MAX_LIFE = 3.0f;
        public static float DELAY_MAX = 1.0f;
        public static float MIN_SPEED = .1f;
        public static float MAX_SPEED = 3f;
        private static Random rand = new Random();

        //State
        public float particleLife = 0;
        public float particleDelay = 10f;
        public Vector2 particlePosition;
        public Vector2 particleVelocity;

        public SplitterParticle(Vector2 position, int random_seed)
        {
            Respawn(position, random_seed);
            particleLife = 0;
        }

        private void Respawn(Vector2 position, int random_seed)
        {
            particleDelay = 0;
            particleLife = MAX_LIFE;
            particlePosition = position;
            particleVelocity = getRandomVelocity(random_seed);
        }

        private Vector2 getRandomVelocity(int a_randomSeed)
        {
            //skapa en random utifrån seed
            Random rand = new Random(a_randomSeed);

            //Random values for X and Y
            float x = (float)rand.NextDouble() - 0.5f;
            float y = (float)rand.NextDouble() - 0.5f;


            //Normalize vector
            Vector2 ret = new Vector2(x, y);
            ret.Normalize();

            //slumpa hastighet mellan 0.1
            float speed = (float)rand.NextDouble();

            //Speed between MIN_SPEED and MAX_SPEED
            ret = ret * (MIN_SPEED + speed * (MAX_SPEED - MIN_SPEED));

            return ret;
        }


        public bool IsAlive()
        {
            return particleLife > 0;
        }


        internal void Update(float a_elapsedTime, Vector2 respawnPosition, int random_seed)
        {
            //Decrease life
            particleLife -= a_elapsedTime;

            //Check if particle is dead
            if (particleLife < 0.0f)
            {
                //If not alive delay respanw if any
                if (particleDelay > 0)
                {
                    particleDelay -= a_elapsedTime;
                    return;
                }
                Respawn(respawnPosition, random_seed);
            }


            //Calculate new velocity and position
            particleVelocity = particleVelocity + GRAVITY * a_elapsedTime;
            particlePosition = particlePosition + particleVelocity * a_elapsedTime;

        }

        internal Vector2 getParticlePostion()
        {
            return particlePosition;
        }
    }
}