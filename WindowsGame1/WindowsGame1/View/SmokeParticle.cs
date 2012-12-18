using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGame1.View
{
    class SmokeParticle : Particle
    {
        public SmokeParticle(Vector2 position, int random_seed) : base(position, random_seed)
        {
            GRAVITY = new Vector2(0, -0.1f);
            MAX_LIFE = 10f;
            MIN_SPEED = .1f;
            MAX_SPEED = .2f;
            MAX_SCALE = 15f;
            MIN_SCALE = .1f;
        }

        public override void Update(float a_elapsedTime, Vector2 respawnPosition, int random_seed)
        {
            //Increase time lived
            particleTimeLived += a_elapsedTime;

            particleLifePercent = particleTimeLived / MAX_LIFE;

            particleScale = MIN_SCALE + particleLifePercent * MAX_SCALE;
            particleRotation += a_elapsedTime / 5;

            base.Update(a_elapsedTime, respawnPosition, random_seed);
        }
    }
}
