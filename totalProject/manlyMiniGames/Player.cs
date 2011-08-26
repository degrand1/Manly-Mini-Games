using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace manlyMiniGames
{
    class Player : Sprite
    {
        const String theAssetName = "filler sheet";
        const int PLAYER_SPEED = 150;
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;

        //Movement Variables
        enum State
        {
            Walking
        }
        State mCurrentState = State.Walking;
        Vector2 mSpeed = Vector2.Zero;
        Vector2 mDirection = Vector2.Zero;

        KeyboardState mPreviousKeyboardState;
        
        //Animation Variables
        const int X_FRAME_WIDTH = 128;
        const int Y_FRAME_WIDTH = 128;
        const int X_FRAME_HEIGHT = 128;
        const int Y_FRAME_HEIGHT = 128;
        int currentYFrame = 0;
        int currentXFrame = 0;

        public void LoadContent(ContentManager theContentManager)
        {
            source = new Rectangle(0, 0, X_FRAME_WIDTH, Y_FRAME_HEIGHT);
            base.LoadContent(theContentManager, theAssetName);
        }

        //update the player's movement
        private void UpdateMovement(KeyboardState currentKeyboardState){
            if (mCurrentState == State.Walking)
            {
                //Zero out the speed incase the player isn't moving
                mDirection = Vector2.Zero;
                mSpeed = Vector2.Zero;
                currentYFrame = 0;

                //Update the direction of movement
                if (currentKeyboardState.IsKeyDown(Keys.Right))
                {
                    //Wasn't look right before, but am now
                    if (mDirection.X != MOVE_RIGHT)
                    {
                        currentYFrame = 2 * Y_FRAME_HEIGHT;
                    }
                    mSpeed.X = PLAYER_SPEED;
                    mDirection.X = MOVE_RIGHT;
                }
                else if (currentKeyboardState.IsKeyDown(Keys.Left))
                {
                    //Wasn't looking left before, but am now
                    if (mDirection.X != MOVE_LEFT)
                    {
                        currentYFrame = 1 * Y_FRAME_HEIGHT;
                    }
                    mSpeed.X = PLAYER_SPEED;
                    mDirection.X = MOVE_LEFT;
                }
            }
        }
        public void Update(GameTime theGameTime)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();
            UpdateMovement(currentKeyboardState);
            mPreviousKeyboardState = currentKeyboardState;
            source = new Rectangle(currentXFrame, currentYFrame, X_FRAME_WIDTH, Y_FRAME_HEIGHT);
            base.Update(theGameTime, mSpeed, mDirection);
        }
    }
}
