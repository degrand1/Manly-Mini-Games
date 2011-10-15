using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace manlyMiniGames
{
    class Sprite
    {
        public const int MOVE_UP = -1;
        public const int MOVE_DOWN = 1;
        public const int MOVE_LEFT = -1;
        public const int MOVE_RIGHT = 1;

        public Vector2 mPosition = new Vector2(8.0f, 56.0f);
        public Rectangle source;
        private Texture2D mSpriteTexture;

        //Load the texture for the sprite using the Content Pipeline
        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            mSpriteTexture = theContentManager.Load<Texture2D>(theAssetName);
        }

        //Update the Sprite's position
        public void Update(GameTime theGameTime, Vector2 theSpeed, Vector2 theDirection)
        {
            mPosition += theSpeed * theDirection * (float)theGameTime.ElapsedGameTime.TotalSeconds;
        }
        //Draws the sprite to the screen
        public virtual void Draw(SpriteBatch theSpriteBatch, Map map)
        {
            //Find the screen position for the texture
            Vector2 scrpos = map.worldToScreen(mPosition);

            //map.drawOnMap(theSpriteBatch, mSpriteTexture, mPosition, source);
            theSpriteBatch.Draw(mSpriteTexture, scrpos, source, Color.White);
        }

        public virtual void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(mSpriteTexture, mPosition, source, Color.White);
        }
    }
}
