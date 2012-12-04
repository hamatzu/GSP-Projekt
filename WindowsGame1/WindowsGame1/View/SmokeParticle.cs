﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WindowsGame1.View
{
    class SmokeParticle
    {

        public static Vector2 GRAVITY = new Vector2(0, -0.4f);
        public static float MAX_LIFE = 30f;
        public static float DELAY_MAX = 3f;
        public static float MIN_SPEED = .1f;
        public static float MAX_SPEED = .2f;
        private static Random rand = new Random();

        //State
        public float particleLife = 20f;
        public float particleDelay = 2f;
        public float particleTimeLived = 0f;
        public Vector2 particlePosition;
        public Vector2 particleVelocity;
        public float minScale = 65f;
        public float maxScale = 150f;
        public float particleScale = 75f;
        private float lifePercent;

        public SmokeParticle(Vector2 position, int random_seed)
        {
            Respawn(position, random_seed);
            particleLife = 0;
            particleTimeLived = 0;
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

            //x och yield får värden mellan -1 och 1
            float x = (float)rand.NextDouble() * 2 - 1f;
            float y = (float)rand.NextDouble() * 2 - 1f;

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


        internal void Update(float a_elapsedTime, Vector2 respawnPosition, int random_seed)
        {
            //Decrease life
            particleLife -= a_elapsedTime;

            //Check if particle is dead
            if (particleLife < 0.0f)
            {
                //If not alive delay respawn if any
                if (particleDelay > 0)
                {
                    particleDelay -= a_elapsedTime;
                    return;
                }

                Respawn(respawnPosition, random_seed);
            }

            particleTimeLived += a_elapsedTime;
            lifePercent = particleTimeLived / MAX_LIFE;
            particleScale = minScale + lifePercent * maxScale;
           
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

        internal float getParticleTL()
        {
            return particleTimeLived;
        }

        internal float GetVisibility()
        {
            return particleLife / MAX_LIFE;
        }
    }
}
