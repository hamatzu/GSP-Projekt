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

        internal void LoadContent(Microsoft.Xna.Framework.Content.ContentManager a_content, GraphicsDevice a_graphicdevice)
        {
            ballTexture = a_content.Load<Texture2D>("ball");
            whiteTexture = a_content.Load<Texture2D>("white");
            blackTexture = a_content.Load<Texture2D>("black");

        }

        internal void Draw(Model.BallGame ballGame, float elapsedTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Camera camera)
        {

            Vector2 bordertopLeft = new Vector2(8,8);
            Rectangle borderDestinationRectangle = new Rectangle((int)bordertopLeft.X, (int)bordertopLeft.Y, (int)camera.GetScreenWidth() - 18, (int)camera.GetScreenHeight() - 18);

            Vector2 innertopLeft = new Vector2(10, 10);
            Rectangle innerDestinationRectangle = new Rectangle((int)innertopLeft.X, (int)innertopLeft.Y, (int)camera.GetScreenWidth() - 22, (int)camera.GetScreenHeight() - 22);

            //Draw ball
            int viewBallWidth = (int)(ballGame.ball.ball_radius * 2.0f * camera.GetScaleX());
            int viewBallHeight = (int)(ballGame.ball.ball_radius * 2.0f * camera.GetScaleX());

            Vector2 ballviewTopLeft = camera.convertToView(ballGame.ball.ball_centerposition.X - ballGame.ball.getBallWidth() / 2,
                                                                 ballGame.ball.ball_centerposition.Y - ballGame.ball.getBallWidth() / 2);

            Rectangle ballDestinationRectangle = new Rectangle((int)ballviewTopLeft.X, (int)ballviewTopLeft.Y, viewBallWidth, viewBallHeight);


            spriteBatch.Begin();
            spriteBatch.Draw(whiteTexture, borderDestinationRectangle, Color.White);
            spriteBatch.Draw(blackTexture, innerDestinationRectangle, Color.White);
            spriteBatch.Draw(ballTexture, ballDestinationRectangle, Color.White);
            spriteBatch.End();
        }
    }
}
