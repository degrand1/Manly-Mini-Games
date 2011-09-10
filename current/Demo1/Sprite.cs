using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Demo1
{
    class Sprite
    {
        public Vector2 position = new Vector2(0, 0);
        public Rectangle source = new Rectangle(0, 0, 128, 128);
        private Texture2D texture;
        
        //Load the texture for the sprite using the Content Pipeline
        public void LoadContent(ContentManager theContentManager, string assetName)
        {
            texture = theContentManager.Load<Texture2D>(assetName);
        }
        //Draw a sprite to the screen
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, source, Color.White);
        }
        //Update the sprite
        public void Update(GameTime gameTime, Vector2 speed, Vector2 direction)
        {
            position += speed * direction * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
