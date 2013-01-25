using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using WindowsGame1.Model;

namespace WindowsGame1.View
{
    class BallView
    {
        //Ball texture
        Texture2D ballTexture;
        Texture2D whiteTexture;
        Texture2D blackTexture;
        Texture2D lineTexture;
        BallSimulation m_ballSimulation;
        private Ball m_ball;

        public BallView(BallSimulation a_ballSimulation)
        {
            m_ballSimulation = a_ballSimulation;
            m_ball = m_ballSimulation.getBall();
        }
        

        internal void LoadContent(Microsoft.Xna.Framework.Content.ContentManager a_content, GraphicsDevice a_graphicdevice)
        {
            ballTexture = a_content.Load<Texture2D>("ball");
            whiteTexture = a_content.Load<Texture2D>("white");
            blackTexture = a_content.Load<Texture2D>("black");

            lineTexture = new Texture2D(a_graphicdevice, 1, 1, false, SurfaceFormat.Color);
            lineTexture.SetData(new[] { Color.White });

        }

        internal void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Camera camera)
        {

            //Get ball properties
                float ballRadius = m_ballSimulation.getBall().getBallRadius();
                float ballWidth = m_ballSimulation.getBall().getBallWidth();
                Vector2 ballCenterPosition = m_ballSimulation.getBall().getBallCenterPosition();


            //Calculate ball to draw
                float viewBallWidth = (ballWidth * camera.GetScaleX());
                float viewBallHeight = (ballWidth  * camera.GetScaleX());

                Vector2 ballviewTopLeft = camera.convertToView(ballCenterPosition.X - ballWidth / 2,
                                                               ballCenterPosition.Y - ballWidth / 2);

                Rectangle ballDestinationRectangle = new Rectangle((int)ballviewTopLeft.X, (int)ballviewTopLeft.Y, (int)viewBallWidth, (int)viewBallHeight);


                spriteBatch.Begin();

                    //Draw border
                        float borderWidth = 2f;

                        //Top border
                        spriteBatch.Draw(lineTexture, new Vector2(camera.GetDisplacementX(), camera.GetDisplacementX()), null, Color.White, 0f, Vector2.Zero, new Vector2(camera.GetScreenWidth() - (2*camera.GetDisplacementX()), borderWidth), SpriteEffects.None, 0);

                        //Right border
                        spriteBatch.Draw(lineTexture, new Vector2(camera.GetScreenWidth() - camera.GetDisplacementX(), camera.GetDisplacementY()), null, Color.White, 0f, Vector2.Zero, new Vector2(borderWidth, camera.GetScreenHeight() - (2 * camera.GetDisplacementY())), SpriteEffects.None, 0);
                
                        //Bottom border
                        spriteBatch.Draw(lineTexture, new Vector2(camera.GetDisplacementX(), camera.GetScreenHeight() - (camera.GetDisplacementY())), null, Color.White, 0f, Vector2.Zero, new Vector2(camera.GetScreenWidth() - (2 * camera.GetDisplacementX() - borderWidth), borderWidth), SpriteEffects.None, 0);

                        //Left Border
                        spriteBatch.Draw(lineTexture, new Vector2(camera.GetDisplacementX(), camera.GetDisplacementX()), null, Color.White, 0f, Vector2.Zero, new Vector2(borderWidth, camera.GetScreenHeight() - (2 * camera.GetDisplacementY())), SpriteEffects.None, 0);    
               
                        //Draw Ball
                        if (!m_ball.isDead())
                        {
                            spriteBatch.Draw(ballTexture, ballDestinationRectangle, Color.White);
                        }

                spriteBatch.End();
        }
    }
}
