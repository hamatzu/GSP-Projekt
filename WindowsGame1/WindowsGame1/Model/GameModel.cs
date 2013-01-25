using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsGame1.View;
using Microsoft.Xna.Framework;

namespace WindowsGame1.Model
{
    class GameModel
    {
        BallSimulation m_ballSimulation;
        List<Vector2> allExplosions = new List<Vector2>();

        public GameModel()
        {
            m_ballSimulation = new BallSimulation();
        }

        internal void addExplosion(Vector2 modelPos)
        {
            allExplosions.Add(modelPos);
        }


        internal List<Vector2> getExplosions()
        {
            return allExplosions;
        }
    }
}
