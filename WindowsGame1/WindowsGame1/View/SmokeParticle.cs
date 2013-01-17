using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WindowsGame1.View
{
    class SmokeParticle
    {

        public static Vector2 GRAVITY = new Vector2(0, -0.1f);
        public static float MAX_LIFE = 2f;
        public static float DELAY_MAX = 3f;
        public static float MIN_SPEED = .1f;
        public static float MAX_SPEED = .2f;
        private static Random rand = new Random();

        //State
        public float particleLife;
        public float particleDelay = 2f;
        public float particleTimeLived = 0f;
        public Vector2 particlePosition = new Vector2(0, 0);
        public Vector2 particleVelocity = new Vector2(0, 0);
        public float minScale = .5f;
        public float maxScale = 2f;
        public float particleScale = 1f;
        private float lifePercent;
        private float particleRotation = 1f;
        float elapsedTime;

        public SmokeParticle(Vector2 position, int random_seed)
        {
            elapsedTime = 0;
            particleLife = MAX_LIFE;
            particleTimeLived = 0;
            particlePosition = new Vector2(0, 0);
            particlePosition = position;
            particleScale = 1f;
            particleRotation = 1f;
            particleVelocity = new Vector2(0, 0);
            particleVelocity = getRandomVelocity(random_seed);
        }


        private Vector2 getRandomVelocity(int a_randomSeed)
        {
            //Random seed
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
                elapsedTime += .0011f;
                //Decrease life
                particleLife -= elapsedTime;

                particleTimeLived += elapsedTime;
                lifePercent = particleTimeLived / MAX_LIFE;
                particleScale = minScale + lifePercent * maxScale;
                particleRotation += elapsedTime / 5;

                //v1 = v0 + a *t
                particleVelocity = particleVelocity + GRAVITY * elapsedTime;

                //s1 = s0 + var * t
                particlePosition = particlePosition + particleVelocity * elapsedTime;
            

        }

        internal float getScale()
        {
            return particleScale;
        }

        internal Vector2 getParticlePostion()
        {
            return particlePosition;
        }

        internal float getParticleLife()
        {
            return particleLife;
        }

        internal float GetVisibility()
        {
            return particleLife / MAX_LIFE;
        }

        internal float getRotation()
        {
            return particleRotation;
        }
    }
}