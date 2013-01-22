﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Threading;

namespace WindowsGame1.View
{
    class ExplosionSystem
    {
        private const int INIT_PARTICLES = 200;
        private Texture2D smokeTexture;
        private Texture2D splitterTexture;
        private Vector2 textureSplitterOrigin;
        private Vector2 textureSmokeOrigin;

        //State 
        List<SmokeParticle> allSmokeParticles = new List<SmokeParticle>();
        public List<SplitterParticle> allSplitterParticles = new List<SplitterParticle>();
        private float rotation;
        private float scale;
        private Vector2 systemPosition;


        private float sm_particlesPerSecond = 20f;
        private float sm_releaseRate = 0f;
        private float sm_releaseTimer = 0f;
        private float sm_particleSystemTL = 0f;
        private int sm_totalParticles = 0;
        private int sm_maxParticles = 4;

        //Explosion
        private float timeElapsed;
        private Vector2 numFrames = new Vector2(4f, 6f);
        private float numberOfFrames = 24;
        private float maxTime = 1f;
        private float frameX;
        private float frameY;
        private int textureTileSize = 128;
        private Texture2D explosionTexture;
        private Vector2 textureExplosionOrigin;


        public ExplosionSystem(Microsoft.Xna.Framework.Vector2 a_modelPosition)
        {
            systemPosition = a_modelPosition;
            sm_releaseRate = 1f / (float)sm_particlesPerSecond;
            //Create array with all particles 
            for (int i = 0; i < INIT_PARTICLES; i++)
            {
                allSplitterParticles.Add(new SplitterParticle(a_modelPosition, i));
            }
        }

        internal void LoadContent(Microsoft.Xna.Framework.Content.ContentManager a_content)
        {
            smokeTexture = a_content.Load<Texture2D>("smoke");
            splitterTexture = a_content.Load<Texture2D>("spark");
            explosionTexture = a_content.Load<Texture2D>("explosion");

            textureSmokeOrigin = new Vector2(smokeTexture.Width / 2f, smokeTexture.Height / 2f);
            textureSplitterOrigin = new Vector2(splitterTexture.Width / 2f, splitterTexture.Height / 2f);
            textureExplosionOrigin = new Vector2(textureTileSize / 2f, textureTileSize / 2f);
        }

        internal void UpdateExplosion(float a_elapsedTime, SpriteBatch a_spriteBatch, Camera a_camera)
        {
            a_spriteBatch.Begin();
            UpdateAndDrawSplitter(a_elapsedTime, a_spriteBatch, a_camera);
            UpdateAndDrawSmoke(a_elapsedTime, a_spriteBatch, a_camera);
            UpdateAndDrawExplosion(a_elapsedTime, a_spriteBatch, a_camera);
            a_spriteBatch.End();
        }

        internal void UpdateAndDrawSmoke(float a_elapsedTime, SpriteBatch a_spriteBatch, Camera a_camera)
        {
            sm_particleSystemTL += a_elapsedTime;

            sm_releaseTimer += a_elapsedTime;

            while (sm_releaseTimer >= sm_releaseRate)
            {
                if (sm_totalParticles < sm_maxParticles)
                {
                    allSmokeParticles.Add(new SmokeParticle(systemPosition, sm_totalParticles));
                    sm_totalParticles += 1;
                }
                sm_releaseTimer -= sm_releaseRate;
            }

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

                    float a = allSmokeParticles.ElementAt(index).GetVisibility();
                    Color particleColor = new Color(a, a, a, a);

                    scale = allSmokeParticles.ElementAt(index).getScale();

                    rotation = allSmokeParticles.ElementAt(index).getRotation();

                    a_spriteBatch.Draw(smokeTexture, particleCenterPosition, null, particleColor, rotation, textureSmokeOrigin, scale, SpriteEffects.None, 0);

                }

            }


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

        internal void UpdateAndDrawSplitter(float a_elapsedTime, SpriteBatch a_spriteBatch, Camera a_camera)
        {
            for (int index = 0; index < allSplitterParticles.Count; index++)
            {
                //Update particle
                allSplitterParticles.ElementAt(index).Update(a_elapsedTime, systemPosition, index);

                //Check if particle is alive
                if (allSplitterParticles.ElementAt(index).IsAlive())
                {
                    //Get particle position and convert to view coordinates
                    Vector2 particleCenterPosition = a_camera.convertToView(allSplitterParticles.ElementAt(index).getParticlePostion().X,
                                                                            allSplitterParticles.ElementAt(index).getParticlePostion().Y);

                    //Spark destination rectangle
                    Rectangle destinationRectangle = new Rectangle((int)particleCenterPosition.X, (int)particleCenterPosition.Y, splitterTexture.Width, splitterTexture.Height);

                    float a = allSplitterParticles.ElementAt(index).GetVisibility();
                    Color particleColor = new Color(a, a, a, a);

                    scale = allSplitterParticles.ElementAt(index).getScale();

                    rotation = allSplitterParticles.ElementAt(index).getRotation();

                    a_spriteBatch.Draw(splitterTexture, particleCenterPosition, null, particleColor, rotation, textureSplitterOrigin, scale, SpriteEffects.None, 0);
                }
            }


            for (int z = 0; z < allSplitterParticles.Count; z++)
            {
                allSplitterParticles.ElementAt(z).Update(a_elapsedTime, systemPosition, z);
                if (!allSplitterParticles.ElementAt(z).IsAlive())
                {
                    allSplitterParticles.RemoveAt(z);
                    Console.WriteLine("removed");
                    z--;
                }
            }
        }

        internal void UpdateAndDrawExplosion(float a_elapsedTime, SpriteBatch a_spriteBatch, Camera a_camera)
        {
            timeElapsed += a_elapsedTime;
            float percentAnimated = timeElapsed / maxTime;
            int frame = (int)(percentAnimated * numberOfFrames);
            frameX = frame % numFrames.X;
            frameY = frame / numFrames.X;

            Rectangle sourcerectangle = new Rectangle(textureTileSize * (int)frameX, textureTileSize * (int)frameY, textureTileSize, textureTileSize);

            //get particle position and convert to view coordinates
            Vector2 explosioncenterposition = a_camera.convertToView(systemPosition.X - 1, systemPosition.Y - 1);
            Rectangle destrect = new Rectangle((int)explosioncenterposition.X, (int)explosioncenterposition.Y, 128, 128);

            a_spriteBatch.Draw(explosionTexture, destrect, sourcerectangle, Color.White);
        }
    }
}