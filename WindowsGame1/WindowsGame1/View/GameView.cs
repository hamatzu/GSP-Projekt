using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsGame1.Model;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WindowsGame1.View
{
    class GameView
    {
        private GameModel m_game;
        private SpriteBatch m_spriteBatch;
        private Camera m_camera;
        List<ExplosionSystem> allExplosions = new List<ExplosionSystem>();


        public GameView(SpriteBatch a_spriteBatch, Camera a_camera)
        {
            m_spriteBatch = a_spriteBatch;
            m_camera = a_camera;
        }

        internal void UpdateAndDrawAllExplosions(float a_elapsedTime)
        {
            foreach (ExplosionSystem aExplosion in allExplosions)
            {
                aExplosion.UpdateExplosion(a_elapsedTime, m_spriteBatch, m_camera);
            }
        }

        internal void addExplosion(Vector2 a_modelPosition, Microsoft.Xna.Framework.Content.ContentManager a_content)
        {
            ExplosionSystem explosion = new ExplosionSystem(a_modelPosition);
            explosion.LoadContent(a_content);
            allExplosions.Add(explosion);
        }
    }
}
