using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Demo1
{
    class Player : Sprite
    {
        # region Fields


        const int PLAYER_SPEED = 150;
        const String theAssetName = "filler sheet";
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_RIGHT = 1;
        const int MOVE_LEFT = -1;
        const int SPRITE_WIDTH = 128;
        const int SPRITE_HEIGHT = 128;

        enum State
        {
            Walking
        }

        State currentState = State.Walking;
        Vector2 mSpeed = Vector2.Zero;
        Vector2 mDirection = Vector2.Zero;

        KeyboardState mPreviousKeyboardState;


        # endregion

        # region Update


        private void UpdateMovement(KeyboardState currentKeyboardState)
        {
            if (currentState == State.Walking)
            {
                if (currentKeyboardState.IsKeyDown(Keys.Right))
                {
                    if (mDirection.X != MOVE_RIGHT)
                    {
                        source = new Rectangle(0, 2 * SPRITE_HEIGHT, SPRITE_WIDTH, SPRITE_HEIGHT);
                    }
                    mSpeed.X = PLAYER_SPEED;
                    mDirection.X = MOVE_RIGHT;
                }
                else if (currentKeyboardState.IsKeyDown(Keys.Left))
                {
                    if (mDirection.X != MOVE_LEFT)
                    {
                        source = new Rectangle(0, SPRITE_HEIGHT, SPRITE_WIDTH, SPRITE_HEIGHT);
                    }
                    mSpeed.X = PLAYER_SPEED;
                    mDirection.X = MOVE_LEFT;
                }
                else
                {
                    if (mDirection != Vector2.Zero)
                    {
                        source = new Rectangle(0, 0, SPRITE_WIDTH, SPRITE_HEIGHT);
                    }
                    mSpeed = Vector2.Zero;
                    mDirection = Vector2.Zero;
                }
            }
        }
        public void Update(GameTime gameTime)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();
            UpdateMovement(currentKeyboardState);
            mPreviousKeyboardState = currentKeyboardState;
            base.Update(gameTime, mSpeed, mDirection);
        }

        # endregion
    }
}
