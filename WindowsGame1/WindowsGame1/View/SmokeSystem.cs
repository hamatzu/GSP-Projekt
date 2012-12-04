﻿using System;
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
        private int particlesPerSecond = 6;
        private float releaseRate = 0f;
        private Vector2 textureOrigin;
        private float initialDelayRemaining = 0f;
        private float releaseTimer = 0f;




        public SmokeSystem(Microsoft.Xna.Framework.Vector2 a_modelPosition)
        {
            systemPosition = a_modelPosition;
            releaseRate = 1.0f / (float)particlesPerSecond;
            //Create array with all particles 
            
        }

        internal void LoadContent(Microsoft.Xna.Framework.Content.ContentManager a_content)
        {
            smokeTexture = a_content.Load<Texture2D>("smoke");
            textureOrigin = new Vector2(smokeTexture.Width / 2f, smokeTexture.Height / 2f);
        }

        internal void UpdateAndDraw(float a_elapsedTime, SpriteBatch a_spriteBatch, Camera a_camera)
        {
            
            // update the initial delay
            if (initialDelayRemaining > 0.0f)
            {
                initialDelayRemaining -= a_elapsedTime;
                return;
            }
            
            for (int z = 0; z < allSmokeParticles.Count; z++)
            {
                allSmokeParticles.ElementAt(z).Update(a_elapsedTime, systemPosition, z);
                if (allSmokeParticles.ElementAt(z).getParticleTL() <= 0)
                {
                    allSmokeParticles.RemoveAt(z);
                    z--;
                }
            }

            releaseTimer += a_elapsedTime;
            while (releaseTimer >= releaseRate)
            {
                for (int i = 0; i < INIT_PARTICLES; i++)
                {
                    allSmokeParticles.Add(new SmokeParticle(systemPosition, i));
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
                    Rectangle destinationRectangle = new Rectangle((int)particleCenterPosition.X, (int)particleCenterPosition.Y, (int)allSmokeParticles.ElementAt(index).getScale(), (int)allSmokeParticles.ElementAt(index).getScale());

                    float a = allSmokeParticles.ElementAt(index).GetVisibility();
                    Color particleColor = new Color(a, a, a, a);

                    scale = allSmokeParticles.ElementAt(index).getScale();
                    rotation = 90f;
                    //origin = new Vector2(5, 8);


                    //a_spriteBatch.Draw(smokeTexture, position, destinationRectangle, particleColor, rotation, origin, scale, SpriteEffects.None, 0);
                    a_spriteBatch.Draw(smokeTexture, destinationRectangle, Color.White);

                }
                else
                {
                    //Create array with all particles 
                    for (int i = 0; i < INIT_PARTICLES; i++)
                    {
                        allSmokeParticles.Add(new SmokeParticle(systemPosition, i));
                    }
                }
            }
            a_spriteBatch.End();
        }
    }
}
