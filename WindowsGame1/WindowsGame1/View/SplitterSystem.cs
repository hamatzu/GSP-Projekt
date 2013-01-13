﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WindowsGame1.View
{
    class SplitterSystem
    {
        private const int MAX_PARTICLES = 10;
        Texture2D sparkTexture;

        //State 
        SplitterParticle[] allParticles = new SplitterParticle[MAX_PARTICLES];
        private bool shouldUpdateAndDraw;
        private Vector2 systemPosition;

        public SplitterSystem()
        {
            shouldUpdateAndDraw = false;
        }

        internal void initializeParticles(Vector2 a_ModelPosition)
        {
            shouldUpdateAndDraw = true;
            systemPosition = a_ModelPosition;
            //Create array with all particles 
            for (int i = 0; i < MAX_PARTICLES; i++)
            {
                allParticles[i] = new SplitterParticle(a_ModelPosition, i);
            }
        }

        internal void LoadContent(Microsoft.Xna.Framework.Content.ContentManager a_content)
        {
            sparkTexture = a_content.Load<Texture2D>("spark");
        }

        internal void UpdateAndDraw(float a_elapsedTime, SpriteBatch a_spriteBatch, Camera a_camera, Vector2 position)
        {
            if (shouldUpdateAndDraw == true)
            {
                //a_spriteBatch.Begin();
                for (int i = 0; i < MAX_PARTICLES; i++)
                {
                    //Update particle
                    allParticles[i].Update(a_elapsedTime, position, i);

                    //Check if particle is alive
                    if (allParticles[i].IsAlive())
                    {
                        //Get particle position and convert to view coordinates
                        Vector2 particleCenterPosition = a_camera.getViewPosition(allParticles[i].getParticlePostion().X, allParticles[i].getParticlePostion().Y, new Vector2(a_camera.getScreenWidth(), a_camera.getScreenHeight()));

                        //Console.WriteLine(particleCenterPosition.X);
                        //Spark destination rectangle
                        Rectangle destinationRectangle = new Rectangle((int)particleCenterPosition.X, (int)particleCenterPosition.Y, 10, 10);

                        a_spriteBatch.Draw(sparkTexture, destinationRectangle, Color.White);
                    }
                }
                //a_spriteBatch.End();
            }
        }
    }
}