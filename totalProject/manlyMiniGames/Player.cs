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

        List<Rocket> mRockets = new List<Rocket>();
        const int ROCKET_SPEED = 200;
        ContentManager mContentManager;

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
        const int Y_FRAME_HEIGHT = 128;
        int currentYFrame = 0;
        int currentXFrame = 0;
        bool directionChanged = false;

        public void LoadContent(ContentManager theContentManager)
        {
            mContentManager = theContentManager;

            foreach (Rocket aRocket in mRockets)
            {
                aRocket.LoadContent(theContentManager);
            }

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
            if (directionChanged)
            {
                source = new Rectangle(currentXFrame, currentYFrame, X_FRAME_WIDTH, Y_FRAME_HEIGHT);
                directionChanged = false;
            }
        }

        private void UpdateRocket(GameTime theGameTime, KeyboardState theCurrentKeyboardState)
        {
            foreach (Rocket aRocket in mRockets)
            {
                aRocket.Update(theGameTime);
            }

            if(theCurrentKeyboardState.IsKeyDown(Keys.Z) && !mPreviousKeyboardState.IsKeyDown(Keys.Z)){
                shootRocket();
            }
        }

        private void shootRocket()
        {
            if (mCurrentState == State.Walking)
            {
                //Check if there are any invisible rockets in the list
                bool createNewRocket = true;
                Vector2 rocketDirection;
                if (mDirection.X == MOVE_LEFT)
                {
                    rocketDirection = new Vector2(MOVE_LEFT, 0);
                }
                else
                {
                    rocketDirection = new Vector2(MOVE_RIGHT, 0);
                }
                foreach (Rocket aRocket in mRockets)
                {
                    if(!aRocket.visible){
                        createNewRocket = false;
                        aRocket.Fire(mPosition + new Vector2(rocketDirection.X*(X_FRAME_WIDTH / 2),0),
                            new Vector2(ROCKET_SPEED, 0), rocketDirection);
                        break;
                    }
                }

                if (createNewRocket)
                {
                    Rocket aRocket = new Rocket();
                    aRocket.LoadContent(mContentManager);
                    aRocket.Fire(mPosition + new Vector2(rocketDirection.X*(X_FRAME_WIDTH / 2), 0),
                            new Vector2(ROCKET_SPEED, 0), rocketDirection);
                    mRockets.Add(aRocket);
                }
            }
        }

        public void Update(GameTime theGameTime)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();
            UpdateMovement(currentKeyboardState);
            UpdateRocket(theGameTime, currentKeyboardState);
            mPreviousKeyboardState = currentKeyboardState;
            
            base.Update(theGameTime, mSpeed, mDirection);
        }
        public void Draw(SpriteBatch theSpriteBatch)
        {
            foreach (Rocket aRocket in mRockets)
            {
                aRocket.Draw(theSpriteBatch);
            }
            base.Draw(theSpriteBatch);
        }
    }
}
