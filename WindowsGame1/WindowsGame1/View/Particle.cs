using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WindowsGame1.View
{
    class Particle
    {

        public Vector2 GRAVITY;
        public static float MAX_LIFE;
        public static float MIN_SPEED;
        public static float MAX_SPEED;
        public static float MAX_SCALE;
        public static float MIN_SCALE;
        public static Random rand = new Random();

        //State
        public float particleLife;
        public float particleTimeLived;
        public Vector2 particlePosition;
        public Vector2 particleVelocity;
        public float particleScale;
        public float particleLifePercent;
        public float particleRotation;


        public Particle(Vector2 position, int random_seed)
        {
            particleLife = MAX_LIFE;
            particleTimeLived = 0;
            particlePosition = position;
            particleVelocity = getRandomVelocity(random_seed);
        }

        //private void Respawn(Vector2 position, int random_seed)
        //{
        //    particleLife = MAX_LIFE;
        //    particleTimeLived = 0;
        //    particlePosition = position;
        //    particleVelocity = getRandomVelocity(random_seed);
        //}


        private Vector2 getRandomVelocity(int a_randomSeed)
        {
            //skapa en random utifrån seed
            Random rand = new Random(a_randomSeed);

            //x och yield får värden mellan -1 och 1
            float x = (float)rand.NextDouble() - 0.5f;
            float y = (float)rand.NextDouble() - 0.5f;

            //skapa och normalisera en vektor
            Vector2 ret = new Vector2(x, y);
            ret.Normalize(); // Vektorn får längden 1.

            //slumpa hastighet mellan 0.1
            float speed = (float)rand.NextDouble();

            //hastighet mellan MIN_SPEED och MAX_SPEED
            ret = ret * (MIN_SPEED + speed * (MAX_SPEED - MIN_SPEED));

            return ret;
        }


        public bool IsAlive()
        {
            return particleLife > 0;
        }


        public virtual void Update(float a_elapsedTime, Vector2 respawnPosition, int random_seed)
        {
            //Decrease life
            particleLife -= a_elapsedTime;

            //v1 = v0 + a *t
            particleVelocity = particleVelocity + GRAVITY * a_elapsedTime;

            //s1 = s0 + var * t
            particlePosition = particlePosition + particleVelocity * a_elapsedTime;

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