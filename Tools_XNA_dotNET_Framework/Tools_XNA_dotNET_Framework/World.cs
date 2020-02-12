using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

// Alexander Ö 200210
namespace Tools_XNA
{
    public class World
    {
        public WorldObjects3D.WorldSpot[,,] worldSpots;

        public Tuple<int, int, int> WorldSize
        {
            get
            {
                return new Tuple<int, int, int>(worldSpots.GetLength(0), worldSpots.GetLength(1), worldSpots.GetLength(2));
            }
        }

        public Vector3 RealSize
        {
            get
            {
                var size = WorldSize;
                return (WorldObjects3D.TileScalar + WorldObjects3D.TileOffset) * new Vector3(size.Item1, size.Item2, size.Item3);
            }
        }

        public World(int sizeX, int sizeY, int sizeZ)
        {
            worldSpots = new WorldObjects3D.WorldSpot[sizeX, sizeY, sizeZ];
        }

        /// <summary>
        /// Updates all world spots
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            // For every dimension 
            for (int i = 0; i < worldSpots.GetLength(0); i++)
            for (int j = 0; j < worldSpots.GetLength(1); j++)
            for (int k = 0; k < worldSpots.GetLength(2); k++)
            {
                worldSpots[i, j, k].Update(gameTime);
                worldSpots[i, j, k].WorldPositionUpdate(i,j,k, RealSize);
            }
        }

        /// <summary>
        /// Draws all world spot
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="camera"></param>
        public void Draw(GameTime gameTime, Camera camera)
        {
            // For every dimension 
            for (int i = 0; i < worldSpots.GetLength(0); i++)
            for (int j = 0; j < worldSpots.GetLength(1); j++)
            for (int k = 0; k < worldSpots.GetLength(2); k++)
            {
                worldSpots[i, j, k].Draw(gameTime, camera);
            }
        }

    }
}
