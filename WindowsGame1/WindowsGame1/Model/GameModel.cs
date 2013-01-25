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
        List<BallSimulation> allBallSimulations = new List<BallSimulation>();
        List<ExplosionSystem> allExplosions = new List<ExplosionSystem>();
        int nrOfBalls = 20;

        public GameModel()
        {
            for (int i = 0; i < nrOfBalls; i++)
            {
                allBallSimulations.Add(new BallSimulation(i));
            }
        }

        internal void addExplosion(ExplosionSystem explosion)
        {
            allExplosions.Add(explosion);
        }

        internal List<ExplosionSystem> getExplosions()
        {
            return allExplosions;
        }

        internal List<BallSimulation> getBallSimulations()
        {
            return allBallSimulations;
        }

        internal void UpdateGame(float a_elapsedTime)
        {
            foreach (BallSimulation aBallSimulation in allBallSimulations)
            {
                aBallSimulation.Update(a_elapsedTime);
            }
        }

        internal void checkBallHit(Vector2 a_modelPosition)
        {
            for (int z = 0; z < allBallSimulations.Count; z++)
            {
                float distance = (allBallSimulations.ElementAt(z).getBall().getBallCenterPosition() - a_modelPosition).Length();
                if(distance < 1.0f)
                {
                    allBallSimulations.ElementAt(z).getBall().setHit(true);
                }
            }
        }
    }
}
