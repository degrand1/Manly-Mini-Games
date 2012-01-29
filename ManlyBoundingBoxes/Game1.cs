using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace ManlyBoundingBoxes
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GameObject topWall;
        GameObject bottomWall;

        GameObject playerOne;
        GameObject playerTwo;
        KeyboardState keyboardState;

        GameObject ball;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D wallTexture = Content.Load<Texture2D>("wall");
            topWall = new GameObject(wallTexture, Vector2.Zero);
            bottomWall = new GameObject(wallTexture, 
                new Vector2(0, Window.ClientBounds.Height - wallTexture.Height));

            Texture2D paddleTexture = Content.Load<Texture2D>("paddle");
            Vector2 position;

            position = new Vector2(0, (Window.ClientBounds.Height - paddleTexture.Height) / 2);
            playerOne = new GameObject(paddleTexture, position);

            position = new Vector2((Window.ClientBounds.Width - paddleTexture.Width),
                (Window.ClientBounds.Height - paddleTexture.Height) / 2);
            playerTwo = new GameObject(paddleTexture, position);

            Texture2D ballTexture = Content.Load<Texture2D>("ball");
            position = new Vector2(playerOne.BoundingBox.Right + 1,
                (Window.ClientBounds.Height - ballTexture.Height) / 2);

            ball = new GameObject(ballTexture, position, new Vector2(8f, -8f));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            keyboardState = Keyboard.GetState();

            ball.Position += ball.Velocity;

            if (keyboardState.IsKeyDown(Keys.W))
                playerOne.Position.Y -= 10f;

            if (keyboardState.IsKeyDown(Keys.S))
                playerOne.Position.Y += 10f;

            if (keyboardState.IsKeyDown(Keys.Up))
                playerTwo.Position.Y -= 10f;

            if (keyboardState.IsKeyDown(Keys.Down))
                playerTwo.Position.Y += 10f;

            CheckPaddleWallCollision();
            CheckBallCollision();

            base.Update(gameTime);
        }

        private void CheckBallCollision()
        {
            if (ball.BoundingBox.Intersects(topWall.BoundingBox)
                || ball.BoundingBox.Intersects(bottomWall.BoundingBox))
            {
                ball.Velocity.Y *= -1;
                ball.Position += ball.Velocity;
            }

            if (ball.BoundingBox.Intersects(playerOne.BoundingBox)
                || ball.BoundingBox.Intersects(playerTwo.BoundingBox))
            {
                ball.Velocity.X *= -1;
                ball.Position += ball.Velocity;
            }

            if ((ball.Position.X < -ball.BoundingBox.Width)
                || (ball.Position.X > Window.ClientBounds.Width))
                setInStartPosition();
        }

        private void setInStartPosition()
        {
            playerOne.Position.Y = (Window.ClientBounds.Height - playerOne.BoundingBox.Height) / 2;

            playerTwo.Position.Y = (Window.ClientBounds.Height - playerTwo.BoundingBox.Height) / 2;

            ball.Position.X = playerOne.BoundingBox.Right + 1;

            ball.Position.Y = (Window.ClientBounds.Height - ball.BoundingBox.Height) / 2;

            ball.Velocity = new Vector2(8f, -8f);
        }

        private void CheckPaddleWallCollision()
        {
            if (playerOne.BoundingBox.Intersects(topWall.BoundingBox))
                playerOne.Position.Y = topWall.BoundingBox.Bottom;

            if (playerOne.BoundingBox.Intersects(bottomWall.BoundingBox))
                playerOne.Position.Y = bottomWall.BoundingBox.Y - playerOne.BoundingBox.Height;

            if (playerTwo.BoundingBox.Intersects(topWall.BoundingBox))
                playerTwo.Position.Y = topWall.BoundingBox.Bottom;

            if (playerTwo.BoundingBox.Intersects(bottomWall.BoundingBox))
                playerTwo.Position.Y = bottomWall.BoundingBox.Y - playerTwo.BoundingBox.Height;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            topWall.Draw(spriteBatch);
            bottomWall.Draw(spriteBatch);

            playerOne.Draw(spriteBatch);
            playerTwo.Draw(spriteBatch);

            ball.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
