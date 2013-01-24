using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsGame1.View;

namespace WindowsGame1.Model
{
    class GameModel
    {
        BallSimulation m_ballSimulation;
        List<ExplosionSystem> allExplosions = new List<ExplosionSystem>();

        public GameModel()
        {
            m_ballSimulation = new BallSimulation();
        }

        internal void addExplosion(ExplosionSystem explosion)
        {
            allExplosions.Add(explosion);
        }


        internal List<ExplosionSystem> getExplosions()
        {
            return allExplosions;
        }
    }
}
