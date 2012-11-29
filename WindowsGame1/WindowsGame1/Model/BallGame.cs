using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Model
{
    class BallGame
    {
        public Ball ball = new Ball();

        internal void UpdateSimulation(float elapsedTimeSeconds, Viewport a_viewport)
        {
           ball.Update(elapsedTimeSeconds, a_viewport);
        }
    }
}
