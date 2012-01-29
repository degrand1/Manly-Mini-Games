using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ManlyBoundingBoxes
{
    class GameObject
    {
        Texture2D texture;
        public Vector2 Position;
        public Vector2 Velocity;

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
            }
        }

        public GameObject(Texture2D texture, Vector2 Position)
        {
            this.texture = texture;
            this.Position = Position;
        }

        public GameObject(Texture2D texture, Vector2 Position, Vector2 Velocity)
        {
            this.texture = texture;
            this.Position = Position;
            this.Velocity = Velocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, Color.White);
        }
    }
}
