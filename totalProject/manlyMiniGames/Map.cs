using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace manlyMiniGames
{
    class Map
    {
        private const int TILE_SIZE = 32;       // Width and height in pixels of each tile.
        private const UInt16 mapWidth = 64;     // Width of the map in tiles.
        private const UInt16 mapHeight = 64;    // Height of the map in tiles.

        // This array stores the textures that map to the ids stored in the grid.
        private Texture2D[] tiles = new Texture2D[2];

        // This member stores which tile should be drawn in the center of the screen.
        public Vector2 center = new Vector2(8.0f, 56.0f);

        private float scale = 1.0f;

        private string assetName = "brick";

        // This is the 2D array that represents our map.  Only two values are used currently:
        //  0 - An empty tile
        //  1 - A brick tile
        private UInt16[,] grid;

        public Map()
        {
            // Create the 2D grid which will store the map data.
            grid = new UInt16[mapWidth, mapHeight];

            //Create borders around the map
            for (int y = 0; y < mapHeight; y++)
            {
                grid[0, y] = 1;
                grid[mapWidth - 1, y] = 1;
            }

            for (int x = 0; x < mapWidth; x++)
            {
                grid[x, 0] = 1;
                grid[x, mapHeight - 1] = 1;
            }

        }

        /// <summary>
        /// Loading the textures
        /// </summary>
        /// <param name="content">The manager handling the textures</param>
        public void LoadContent(ContentManager content)
        {
            tiles[1] = content.Load<Texture2D>(assetName);
        }

        /// <summary>
        /// Renders a texture onto the map using the given spritebatch and map position
        /// </summary>
        /// <param name="batch"> Spritebatch to render the texture </param>
        /// <param name="tex"> Texture we want to render </param>
        /// <param name="pos"> Map position where the texture is rendered </param
        public void drawOnMap(SpriteBatch batch, Texture2D tex, Vector2 pos)
        {
            //If the tile is empty
            if (tex == null)
                return;

            //Find the screen position for the texture
            Vector2 scrpos = worldToScreen(pos);

            batch.Draw(tex, scrpos, null, Color.White, 0f, Vector2.Zero, 
                scale, SpriteEffects.None, 0f);
        }

        /// <summary>
        /// Render the map to the screeen
        /// 
        public void Draw(SpriteBatch batch)
        {
            Vector2 topLeft = new Vector2(0.0f, 0.0f);
            Vector2 bottomRight = new Vector2(Globals.screenWidth, Globals.screenHeight);
            Vector2 topLeftTile, bottomRightTile;
            int x, y;

            // Obtain the top left and bottom right tiles according to our world coordinates
            topLeftTile = screenToWorld(topLeft);
            bottomRightTile = screenToWorld(bottomRight);

            //Loop through the tiles that we know to be on the screen.
            for (y = (int)topLeftTile.Y; y <= (int)bottomRightTile.Y; y++)
            {
                for (x = (int)topLeftTile.X; x <= (int)bottomRightTile.X; x++)
                {
                    //get the id of the tile at the current position
                    UInt16 id = Grid(x, y);

                    //Render the correct texture at the current position
                    drawOnMap(batch, tiles[id], new Vector2((float)x, (float)y));
                }
            }
        }

        /// <summary>
        /// Center the screen on the given world coordinate
        /// </summary>
        /// <param name="pos">World Coordinate</param>
        public void setViewPos(Vector2 pos)
        {
            //Move, don't teleport, toward the center position
            center.X += (pos.X - center.X) * 0.1f;
            center.Y += (pos.Y - center.Y) * 0.1f;
        }

        /// <summary>
        /// Returns the grid at the relative position with bounds checking
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>The tile tipe</returns>
        private UInt16 Grid(int x, int y)
        {
            if (x >= 0 && x < mapWidth &&
                y >= 0 && y < mapHeight)
                return grid[x, y];

            //All tiles stored outside of the map are empty
            return 0;
        }

        /// <summary>
        /// Returns the screen position matching the given world position
        /// </summary>
        /// <param name="pos">world position</param>
        /// <returns>screen position</returns>
        public Vector2 worldToScreen(Vector2 pos)
        {
            pos -= center;
            pos.X *= scale * TILE_SIZE;
            pos.Y *= scale * TILE_SIZE;
            pos.X += Globals.screenWidth / 2;
            pos.Y += Globals.screenHeight / 2;
            return pos;
        }

        /// <summary>
        ///  Returns the world position relative to the screen position given
        /// </summary>
        /// <param name="pos">The screen position</param>
        /// <returns>The world position</returns>
        public Vector2 screenToWorld(Vector2 pos)
        {
            pos.X -= Globals.screenWidth / 2;
            pos.Y -= Globals.screenHeight / 2;
            pos.X /= scale * TILE_SIZE;
            pos.Y /= scale * TILE_SIZE;
            pos += center;
            return pos;
        }
    }
}
