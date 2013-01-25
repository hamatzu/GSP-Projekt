﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGame1.View
{
    class SplitterParticle : Particle
    {
        public SplitterParticle(Vector2 position, int random_seed)
            : base(position, random_seed)
        {
            GRAVITY = new Vector2(0, 1f);
            MAX_LIFE = 2f;
            MIN_SPEED = 1f;
            MAX_SPEED = 1.5f;
            particleScale = .15f;
        }
    }
}