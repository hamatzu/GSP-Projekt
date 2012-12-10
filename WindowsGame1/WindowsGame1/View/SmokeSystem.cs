using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Threading;

namespace WindowsGame1.View
{
    class SmokeSystem
    {
        private const int INIT_PARTICLES = 1;
        Texture2D smokeTexture;

        //State 
        List <SmokeParticle> allSmokeParticles = new List<SmokeParticle>();
        private float rotation;
        private Vector2 origin;
        private float scale;
        private Vector2 systemPosition;
        private float particlesPerSecond = 2f;
        private float releaseRate = 0f;
        private Vector2 textureOrigin;
        private float releaseTimer = 0f;
        private float particleSystemTL = 0f;
        private int totalParticles = 0;
        private int maxParticles = 20;




        public SmokeSystem(Microsoft.Xna.Framework.Vector2 a_modelPosition)
        {
            systemPosition = a_modelPosition;
            releaseRate = 1f / (float)particlesPerSecond;
            //Create array with all particles 
            
        }

        internal void LoadContent(Microsoft.Xna.Framework.Content.ContentManager a_content)
        {
            smokeTexture = a_content.Load<Texture2D>("smoke");
            textureOrigin = new Vector2(smokeTexture.Width / 2f, smokeTexture.Height / 2f);
        }

        internal void UpdateAndDraw(float a_elapsedTime, SpriteBatch a_spriteBatch, Camera a_camera)
        {
            particleSystemTL += a_elapsedTime;
            
            releaseTimer += a_elapsedTime;
            
            while (releaseTimer >= releaseRate)
            {
                if (totalParticles < maxParticles)
                {
                    allSmokeParticles.Add(new SmokeParticle(systemPosition, totalParticles));
                    totalParticles += 1;
                }
                releaseTimer -= releaseRate;
            }

            a_spriteBatch.Begin();
            for (int index = 0; index < allSmokeParticles.Count; index++)
            {

                //Update particle
                allSmokeParticles.ElementAt(index).Update(a_elapsedTime, systemPosition, index);

                //Check if particle is alive
                if (allSmokeParticles.ElementAt(index).IsAlive())
                {
                    //Get particle position and convert to view coordinates
                    Vector2 particleCenterPosition = a_camera.convertToView(allSmokeParticles.ElementAt(index).getParticlePostion().X,
                                                                            allSmokeParticles.ElementAt(index).getParticlePostion().Y);

                    //Spark destination rectangle
                    Rectangle destinationRectangle = new Rectangle((int)particleCenterPosition.X, (int)particleCenterPosition.Y, smokeTexture.Width, smokeTexture.Height);

                    float a = allSmokeParticles.ElementAt(index).GetVisibility();
                    Color particleColor = new Color(a, a, a, a);

                    scale = allSmokeParticles.ElementAt(index).getScale();

                    rotation = allSmokeParticles.ElementAt(index).getRotation();

                    a_spriteBatch.Draw(smokeTexture, particleCenterPosition, null, particleColor, rotation, textureOrigin, scale, SpriteEffects.None, 0);
                    //a_spriteBatch.Draw(smokeTexture, destinationRectangle, particleColor);

                }

            }
            a_spriteBatch.End();


            for (int z = 0; z < allSmokeParticles.Count; z++)
            {
                allSmokeParticles.ElementAt(z).Update(a_elapsedTime, systemPosition, z);
                if (!allSmokeParticles.ElementAt(z).IsAlive())
                {
                    allSmokeParticles.RemoveAt(z);
                    Console.WriteLine("removed");
                    z--;
                }
            }

        }
    }
}
