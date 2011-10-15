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
        const int PLAYER_SPEED = 7;

        List<Rocket> mRockets = new List<Rocket>();
        const int ROCKET_SPEED = 12;
        const int X_ROCKET_OFFSET = 375;
        const int Y_ROCKET_OFFSET = 225;

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

        private void UpdateRocket(GameTime theGameTime, KeyboardState theCurrentKeyboardState, Map map)
        {
            foreach (Rocket aRocket in mRockets)
            {
                aRocket.Update(theGameTime);
            }

            if(theCurrentKeyboardState.IsKeyDown(Keys.Z) && !mPreviousKeyboardState.IsKeyDown(Keys.Z)){
                shootRocket(map);
            }
        }

        private void shootRocket(Map map)
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
                Vector2 rocketStartPosition =
                    map.screenToWorld(new Vector2(mPosition.X + rocketDirection.X * X_FRAME_WIDTH + X_ROCKET_OFFSET, 
                        mPosition.Y + Y_ROCKET_OFFSET));

                Vector2 rocketSpeed = new Vector2(ROCKET_SPEED, 0);
                foreach (Rocket aRocket in mRockets)
                {
                    if(!aRocket.visible){
                        createNewRocket = false;
                        aRocket.Fire(rocketStartPosition, rocketSpeed, rocketDirection);
                        break;
                    }
                }

                if (createNewRocket)
                {
                    Rocket aRocket = new Rocket();
                    aRocket.LoadContent(mContentManager);
                    aRocket.Fire(rocketStartPosition, rocketSpeed, rocketDirection);
                    mRockets.Add(aRocket);
                }
            }
        }

        public void Update(GameTime theGameTime, Map map)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();
            UpdateMovement(currentKeyboardState);
            UpdateRocket(theGameTime, currentKeyboardState, map);
            mPreviousKeyboardState = currentKeyboardState;

            map.setViewPos(mPosition);

            base.Update(theGameTime, mSpeed, mDirection);
        }

        public override void Draw(SpriteBatch theSpriteBatch, Map map)
        {
            foreach (Rocket aRocket in mRockets)
            {
                aRocket.Draw(theSpriteBatch, map);
            }
            base.Draw(theSpriteBatch, map);
        }
    }
}
