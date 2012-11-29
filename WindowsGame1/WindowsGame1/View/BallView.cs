using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WindowsGame1.View
{
    class BallView
    {
        //Ball texture
        Texture2D ballTexture;
        Texture2D whiteTexture;
        Texture2D blackTexture;
        Camera camera;

        internal void LoadContent(Microsoft.Xna.Framework.Content.ContentManager a_content, GraphicsDevice a_graphicdevice)
        {
            ballTexture = a_content.Load<Texture2D>("ball");
            whiteTexture = a_content.Load<Texture2D>("white");
            blackTexture = a_content.Load<Texture2D>("black");

        }

        internal void Draw(Model.BallSimulation ballSimulation, float elapsedTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Viewport viewport)
        {
            //Call camera
                camera = new Camera(viewport);

            //Get ball properties
                float ballRadius = ballSimulation.getBall().getBallRadius();
                float ballWidth = ballSimulation.getBall().getBallWidth();
                Vector2 ballCenterPosition = ballSimulation.getBall().getBallCenterPosition();

            //Calculate white rectangle border  
                Vector2 bordertopLeft = new Vector2(ballRadius * (camera.GetScaleX() / 2), ballRadius * (camera.GetScaleY() / 2));
                float offsetX = (ballRadius * (camera.GetScaleX() / 2) * 2);
                float offsetY = (ballRadius * (camera.GetScaleY() / 2) * 2);

                Rectangle borderDestinationRectangle = new Rectangle((int)bordertopLeft.X, (int)bordertopLeft.Y, (int)camera.GetScreenWidth() - (int)offsetX, (int)camera.GetScreenHeight() - (int)offsetY);

            //Calculate black inner rectangle
                Vector2 innertopLeft = new Vector2((bordertopLeft.X + 2f), (bordertopLeft.Y + 2f));
                offsetX = (bordertopLeft.X + 2f) * 2;
                offsetY = (bordertopLeft.X + 2f) * 2;

                Rectangle innerDestinationRectangle = new Rectangle((int)innertopLeft.X, (int)innertopLeft.Y, (int)camera.GetScreenWidth() - (int)offsetX, (int)camera.GetScreenHeight() - (int)offsetY);

            //Calculate ball to draw
                float viewBallWidth = (ballWidth * camera.GetScaleX());
                float viewBallHeight = (ballWidth  * camera.GetScaleX());

                Vector2 ballviewTopLeft = camera.convertToView(ballCenterPosition.X - ballWidth / 2,
                                                               ballCenterPosition.Y - ballWidth / 2);

                Rectangle ballDestinationRectangle = new Rectangle((int)ballviewTopLeft.X, (int)ballviewTopLeft.Y, (int)viewBallWidth, (int)viewBallHeight);


            //Draw border and ball
                spriteBatch.Begin();
                spriteBatch.Draw(whiteTexture, borderDestinationRectangle, Color.White);
                spriteBatch.Draw(blackTexture, innerDestinationRectangle, Color.White);
                spriteBatch.Draw(ballTexture, ballDestinationRectangle, Color.White);
                spriteBatch.End();
        }
    }
}
