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

        public static Vector2 GRAVITY = new Vector2(0, .2f);
        public static float MAX_LIFE = 2.0f;
        public static float DELAY_MAX = 1.0f;
        public static float MIN_SPEED = .1f;
        public static float MAX_SPEED = .5f;
        private static Random rand = new Random();

        //State
        public float particleLife = 0;
        public float particleDelay = 10f;
        public Vector2 particlePosition = new Vector2(0, 0);
        public Vector2 particleVelocity = new Vector2(0, 0);
        private float elapsedTime;

        public SplitterParticle(Vector2 position, int random_seed)
        {
            elapsedTime = 0;
            particleDelay = 0;
            particleLife = MAX_LIFE;
            particlePosition = new Vector2(0, 0);
            particlePosition = position;
            particleVelocity = new Vector2(0, 0);
            particleVelocity = getRandomVelocity(random_seed);
        }

        private Vector2 getRandomVelocity(int a_randomSeed)
        {
            //Random from seed
            Random rand = new Random(a_randomSeed);

            //Random values for X and Y
            float x = (float)rand.NextDouble() - 0.5f;
            float y = (float)rand.NextDouble() - 0.5f;


            //Normalize vector
            Vector2 ret = new Vector2(x, y);
            ret.Normalize();

            //Random speed
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
            //Check if particle is dead
            if (particleLife < 0.0f)
            {
                elapsedTime = 0;
                return;
            }

            elapsedTime += .0009f;
            particleLife -= elapsedTime;

            //v1 = v0 + a *t
            particleVelocity = particleVelocity + GRAVITY * elapsedTime;

            //s1 = s0 + var * t
            particlePosition = particlePosition + particleVelocity * elapsedTime;
        }

        internal Vector2 getParticlePostion()
        {
            return particlePosition;
        }

        internal float getVisibility()
        {
            return particleLife / MAX_LIFE;
        }
    }
}