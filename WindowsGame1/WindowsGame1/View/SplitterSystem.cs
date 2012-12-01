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
        private const int MAX_PARTICLES = 100;
        Texture2D sparkTexture;

        //State 
        SplitterParticle[] allParticles = new SplitterParticle[MAX_PARTICLES];

        public SplitterSystem(Microsoft.Xna.Framework.Vector2 a_modelPosition)
        {
            
            for (int i = 0; i < MAX_PARTICLES; i++)
            {
                allParticles[i] = new SplitterParticle(a_modelPosition, i);
            }
        }

        internal void LoadContent(Microsoft.Xna.Framework.Content.ContentManager a_content)
        {
            sparkTexture = a_content.Load<Texture2D>("spark");
        }

        internal void UpdateAndDraw(float a_elapsedTime, SpriteBatch a_spriteBatch, Camera a_camera, Vector2 position)
        {
            a_spriteBatch.Begin();
                for (int i = 0; i < MAX_PARTICLES; i++)
                {

                    //Updatera partikeln
                    allParticles[i].Update(a_elapsedTime, position, i);

                    //Rita bara ut levande partiklar
                    if (allParticles[i].IsAlive())
                    {

                        //Viewpositioner
                        Vector2 viewCenterPosition = a_camera.convertToView(allParticles[i].getParticlePostion().X,
                                                                                allParticles[i].getParticlePostion().Y);

                        //Räkna ut storleken
                        Rectangle destinationRectangle = new Rectangle((int)viewCenterPosition.X, (int)viewCenterPosition.Y, 10, 10);

                        //Console.WriteLine(viewCenterPosition.X + ", " + viewCenterPosition.Y);

                        //rita ut partikeln
                        a_spriteBatch.Draw(sparkTexture, destinationRectangle, Color.White);
                    }
                }
            a_spriteBatch.End();
        }
    }
}
