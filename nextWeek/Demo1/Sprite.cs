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
        public Vector2 mPosition = new Vector2(0, 0);
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
        public virtual void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(mSpriteTexture, mPosition, source, Color.White);
        }
    }
}
