using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using WindowsGame1.View;
using WindowsGame1.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Controller
{
    class GameController
    {
        private MouseState mouseOld;
        private SoundEffect fire;
        private SoundEffectInstance fireInstance;

        internal void Update(float a_gameTime, Camera a_camera, GameModel a_game, SpriteBatch a_spriteBatch, Microsoft.Xna.Framework.Content.ContentManager a_content)
        {
            MouseState mouse = Mouse.GetState();

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                if (mouseOld.LeftButton == ButtonState.Released)
                {
                    Vector2 modelPosition = a_camera.convertToModel(mouse.X, mouse.Y);
                    ExplosionSystem explosion = new ExplosionSystem(modelPosition);
                    explosion.LoadContent(a_content);
                    a_game.addExplosion(explosion);

                    fireInstance = fire.CreateInstance();
                    fireInstance.Play();
                }
            }

            mouseOld = mouse;
        }

        internal void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            fire = Content.Load<SoundEffect>("fire");
        }
    }
}
