using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsGame1.Model;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.View
{
    class GameView
    {
        private GameModel m_game;
        private SpriteBatch m_spriteBatch;
        private Camera m_camera;


        public GameView(GameModel a_game, SpriteBatch a_spriteBatch, Camera a_camera)
        {
            // TODO: Complete member initialization
            m_game = a_game;
            m_spriteBatch = a_spriteBatch;
            m_camera = a_camera;
        }

        internal void UpdateAndDrawAllExplosions(float a_elapsedTime)
        {
            foreach (ExplosionSystem aExplosion in m_game.getExplosions())
            {
                aExplosion.UpdateExplosion(a_elapsedTime, m_spriteBatch, m_camera);
            }
        }
    }
}
