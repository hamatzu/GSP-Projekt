using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WindowsGame1.View
{
    class SmokeSystem
    {
        private const int MAX_PARTICLES = 100;
        Texture2D smokeTexture;

        //State 
        SmokeParticle[] allSmokeParticles = new SmokeParticle[MAX_PARTICLES];
        private float rotation;
        private Vector2 origin;
        private float scale;


        public SmokeSystem(Microsoft.Xna.Framework.Vector2 a_modelPosition)
        {
            //Create array with all particles 
            for (int i = 0; i < MAX_PARTICLES; i++)
            {
                allSmokeParticles[i] = new SmokeParticle(a_modelPosition, i);
            }
        }

        internal void LoadContent(Microsoft.Xna.Framework.Content.ContentManager a_content)
        {
            smokeTexture = a_content.Load<Texture2D>("smoke");
        }

        internal void UpdateAndDraw(float a_elapsedTime, SpriteBatch a_spriteBatch, Camera a_camera, Vector2 position)
        {
            a_spriteBatch.Begin();
                for (int i = 0; i < MAX_PARTICLES; i++)
                {

                    //Update particle
                    allSmokeParticles[i].Update(a_elapsedTime, position, i);

                    //Check if particle is alive
                    if (allSmokeParticles[i].IsAlive())
                    {
                        //Get particle position and convert to view coordinates
                        Vector2 particleCenterPosition = a_camera.convertToView(allSmokeParticles[i].getParticlePostion().X,
                                                                                allSmokeParticles[i].getParticlePostion().Y);

                        //Spark destination rectangle
                        Rectangle destinationRectangle = new Rectangle((int)particleCenterPosition.X, (int)particleCenterPosition.Y, (int)allSmokeParticles[i].getScale(), (int)allSmokeParticles[i].getScale());

                        float a = allSmokeParticles[i].GetVisibility();
                        Color particleColor = new Color(a, a, a, a);

                        scale = allSmokeParticles[i].getScale();
                        rotation = 90f;
                        origin = new Vector2(5,8);

                        //a_spriteBatch.Draw(smokeTexture, position, destinationRectangle, particleColor, rotation, origin, scale, SpriteEffects.None, 0);
                        a_spriteBatch.Draw(smokeTexture, destinationRectangle, particleColor);
                    }
                }
            a_spriteBatch.End();
        }
    }
}
