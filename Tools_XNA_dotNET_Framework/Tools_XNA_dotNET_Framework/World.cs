using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

// Alexander Ö 200210
namespace Tools_XNA
{
    public interface IWorldSpot
    {
        void Draw(GameTime gameTime);
        void Update(GameTime gameTime);
    }


    public class World
    {
        public static IWorldSpot[,,] worldSpots;

        public World(int sizeX, int sizeY, int sizeZ)
        {
            worldSpots = new IWorldSpot[sizeX, sizeY, sizeZ];
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
                    for (int k = 0; i < worldSpots.GetLength(2); k++)
                        worldSpots[i,j,k].Update(gameTime);
        }

        /// <summary>
        /// Draws all world spot
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(GameTime gameTime)
        {
            // For every dimension 
            for (int i = 0; i < worldSpots.GetLength(0); i++)
                for (int j = 0; j < worldSpots.GetLength(1); j++)
                    for (int k = 0; i < worldSpots.GetLength(2); k++)
                        worldSpots[i, j, k].Draw(gameTime);
        }
    }
}
