using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1.Model
{
    class Game
    {
        //Spelets beståndsdelar
        public Player player = new Player();
        public Level level = new Level();

        internal void UpdateSimulation(float a_elapsedTimeSeconds)
        {
            player.Update(a_elapsedTimeSeconds);
        }

        internal Player getPlayer()
        {
            return player;
        }
    }
}
