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

        public static Vector2 GRAVITY = new Vector2(1f, -1.3f);
        public static float MAX_LIFE = 5.0f;
        public static float DELAY_MAX = 1.0f;
        public static float MIN_SPEED = 0.1f;
        public static float MAX_SPEED = 0.5f;
        private static Random rand = new Random();

        //State
        public float particleLife = 0;
        public float particleDelay = 10f;
        public Vector2 particlePosition;  
        public Vector2 particleSpeed;

        public SplitterParticle(Vector2 position, int random_seed)
        {
            Respawn(position, random_seed);
            particleLife = 0;
        }

        private void Respawn(Vector2 position, int random_seed)
        {
            Random r = new Random(random_seed);

            particleDelay = 0;
            particleLife = MAX_LIFE;
            particlePosition = position;
            particleSpeed = getRandomSpeed(random_seed);
        }

        private Vector2 getRandomSpeed(int a_randomSeed)
        {
            //skapa en random utifrån seed
            Random rand = new Random(a_randomSeed);

            //x och yield får värden mellan -1 och 1
            float x = (float)(rand.NextDouble() * 2.0 - 1.0);
            float y = (float)(rand.NextDouble() * 2.0 - 1.0);

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


        internal Vector2 getParticlePostion()
        {
            return particlePosition;
        }

        internal void Update(float a_elapsedTime, Vector2 respawnPosition, int random_seed)
        {
            //dra bort liv
            particleLife -= a_elapsedTime;

            //har partikeln dött?
            if (particleLife < 0.0f)
            {
                //Om vi inte längre lever väntar vi m_delay innan vi respawnar
                if (particleDelay > 0)
                {
                    particleDelay -= a_elapsedTime;
                    return;
                }
                Respawn(respawnPosition, random_seed);
            }

            //Vector2 randomDirection = new Vector2((float)rand.NextDouble() - 0.5f, (float)rand.NextDouble() - 0.5f);
            //randomDirection.Normalize();
            //randomDirection = randomDirection * ((float)rand.NextDouble() * MAX_SPEED);

            //GRAVITY = randomDirection;

            //v1 = v0 + a *t
            //particleSpeed = particleSpeed + GRAVITY * a_elapsedTime;


            //s1 = s0 + var * t
            particlePosition = particlePosition + particleSpeed * a_elapsedTime;
        }
    }
}
