using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace manlyMiniGames
{
    class Rocket : Sprite
    {
        const int MAX_DISTANCE = 7;
        float frameTimeElapsed = 0.0f;
        const float FRAME_TIME_TO_UPDATE = 0.0833f; // 12 frames per second

        enum rocketState
        {
            Traveling, Exploding
        }

        rocketState mCurrentState = rocketState.Traveling;

        public bool visible = false;

        Vector2 startPosition, speed, direction;

        //animation variables
        int currentXFrame = 0;
        int currentYFrame = 0;
        const int MAX_FRAME_WIDTH = 384;
        public static int FRAME_WIDTH = 128;
        public static int FRAME_HEIGHT = 128;

        const String assetName = "RocketSheet";

        public void LoadContent(ContentManager theContentManager)
        {
            base.LoadContent(theContentManager, assetName); 
        }

        public void Update(GameTime gameTime)
        {
            if (visible && mCurrentState == rocketState.Traveling
                && Vector2.Distance(startPosition, mPosition) > MAX_DISTANCE)
            {
                mCurrentState = rocketState.Exploding;
                currentYFrame = 0;
                currentXFrame = 0;
            }

            if (visible && mCurrentState == rocketState.Traveling)
            {
                base.Update(gameTime, speed, direction);
            }
            else if (mCurrentState == rocketState.Exploding)
            {
                explodeRocket(gameTime);
            }
        }

        private void explodeRocket(GameTime gameTime)
        {
            frameTimeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (frameTimeElapsed > FRAME_TIME_TO_UPDATE)
            {
                frameTimeElapsed -= FRAME_TIME_TO_UPDATE;
                //Jump to the next frame
                if (currentXFrame < MAX_FRAME_WIDTH)
                {
                    currentXFrame += FRAME_WIDTH;
                }
                else
                {
                    mCurrentState = rocketState.Traveling;
                    visible = false;
                }
                source = new Rectangle(currentXFrame, currentYFrame, FRAME_WIDTH, FRAME_HEIGHT);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Map map)
        {
            if (visible)
            {
                base.Draw(spriteBatch, map);
            }
        }

        public void Fire(Vector2 theStartPosition, Vector2 theSpeed, Vector2 theDirection)
        {
            currentYFrame = FRAME_HEIGHT;
            if (theDirection.X == MOVE_LEFT )
                currentXFrame = 0;
            else
                currentXFrame = FRAME_WIDTH;
            source = new Rectangle(currentXFrame, currentYFrame, FRAME_WIDTH, FRAME_HEIGHT);
            mPosition = theStartPosition;
            startPosition = theStartPosition;
            speed = theSpeed;
            direction = theDirection;
            visible = true;
            mCurrentState = rocketState.Traveling;
        }
    }
}
