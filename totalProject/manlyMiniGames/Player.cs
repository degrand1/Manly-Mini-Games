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
        bool directionChanged = false;

        public void LoadContent(ContentManager theContentManager)
        {
            source = new Rectangle(0, 0, X_FRAME_WIDTH, Y_FRAME_HEIGHT);
            base.LoadContent(theContentManager, theAssetName);
        }

        //update the player's movement
        private void UpdateMovement(KeyboardState currentKeyboardState){
            if (mCurrentState == State.Walking)
            {
                //Update the direction of movement
                if (currentKeyboardState.IsKeyDown(Keys.Right) && !currentKeyboardState.IsKeyDown(Keys.Left))
                {
                    //Wasn't look right before, but am now
                    if (mDirection.X != MOVE_RIGHT)
                    {
                        currentYFrame = 2 * Y_FRAME_HEIGHT;
                        directionChanged = true;
                    }
                    mSpeed.X = PLAYER_SPEED;
                    mDirection.X = MOVE_RIGHT;
                }
                else if (currentKeyboardState.IsKeyDown(Keys.Left) && !currentKeyboardState.IsKeyDown(Keys.Right))
                {
                    //Wasn't looking left before, but am now
                    if (mDirection.X != MOVE_LEFT)
                    {
                        currentYFrame = 1 * Y_FRAME_HEIGHT;
                        directionChanged = true;
                    }
                    mSpeed.X = PLAYER_SPEED;
                    mDirection.X = MOVE_LEFT;
                }
                else //Zero out the speed, the player isn't moving
                {
                    if (mDirection.X != 0)
                    {
                        currentYFrame = 0;
                        directionChanged = true;
                    }
                    mDirection = Vector2.Zero;
                    mSpeed = Vector2.Zero;
                }
            }
        }
        public void Update(GameTime theGameTime)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();
            UpdateMovement(currentKeyboardState);
            mPreviousKeyboardState = currentKeyboardState;
            if (directionChanged)
            {
                source = new Rectangle(currentXFrame, currentYFrame, X_FRAME_WIDTH, Y_FRAME_HEIGHT);
                directionChanged = false;
            }
            base.Update(theGameTime, mSpeed, mDirection);
        }
    }
}
