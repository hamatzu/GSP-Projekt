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
        private List<BallView> allBallViews = new List<BallView>();



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

        internal void LoadContent(Microsoft.Xna.Framework.Content.ContentManager a_content, GraphicsDevice a_graphicsDevice)
        {
            for (int i = 0; i < m_game.getBallSimulations().Count; i++)
            {
                BallView ballView = new BallView(m_game.getBallSimulations().ElementAt(i));
                ballView.LoadContent(a_content, a_graphicsDevice);
                allBallViews.Add(ballView);
            }
        }


        internal void DrawGame(SpriteBatch a_spriteBatch, Camera a_camera)
        {
            foreach (BallView aView in allBallViews)
            {
                aView.Draw(a_spriteBatch, a_camera);
            }
        }
    }
}
