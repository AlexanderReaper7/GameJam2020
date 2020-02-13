using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tools_XNA
{
    public class Level
    {
        public readonly string Name;
        /// <summary>
        /// Terrain
        /// </summary>
        public World World { get; set; }

        public readonly Vector3 StartingPosition;


        public Level(string name, World world, Vector3 startingPosition)
        {
            Name = name;
            World = world;
        }

        #region Create


        public static Level CreateDefault(GraphicsDevice graphicsDevice)
        {
            int x = 12, y = 2, z = 12;
            World world = new World(x, y, z);

            // fill world with air and ground
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                    for (int k = 0; k < z; k++)
                         world.worldSpots[i,j,k] = j > 0 ? (WorldObjects3D.WorldSpot) new WorldObjects3D.Air() : new WorldObjects3D.Ground(graphicsDevice);

            return new Level("Default", world, Vector3.Zero);
        }

        public static Level CreateFilled(GraphicsDevice graphicsDevice)
        {
            int x = 12, y = 2, z = 12;
            World world = new World(x, y, z);

            // fill world with air and ground
            for (int i = 0; i < x; i++)
            for (int j = 0; j < y; j++)
            for (int k = 0; k < z; k++)
                world.worldSpots[i, j, k] = new WorldObjects3D.Ground(graphicsDevice);

            return new Level("DefaultFilled", world, Vector3.Zero);
        }

        public static Level CreateMaze(GraphicsDevice graphicsDevice)
        {
            int x = 12, y = 2, z = 12;
            World world = new World(x, y, z);

            // fill world with air and ground
            for (int i = 0; i < x; i++)
            for (int j = 0; j < y; j++)
            for (int k = 0; k < z; k++)
            {
                if (j == 0) // Walkable ground
                {
                    world.worldSpots[i, j, k] = new WorldObjects3D.Ground(graphicsDevice);
                    continue;
                }

                if (k % 2 == 0 && i % 2 == 0) // walls
                {
                    world.worldSpots[i, j, k] = new WorldObjects3D.Ground(graphicsDevice);
                    continue;
                }

                // air
                world.worldSpots[i, j, k] = new WorldObjects3D.Air();
            }
            return new Level("DefaultMaze", world, Vector3.Zero);

        }

        public static Level CreateDefaultFalling(GraphicsDevice graphicsDevice)
        {
            int x = 12, y = 2, z = 12;
            World world = new World(x, y, z);

            // fill world with air and ground
            for (int i = 0; i < x; i++)
            for (int j = 0; j < y; j++)
            for (int k = 0; k < z; k++)
                world.worldSpots[i, j, k] = j > 0 ? (WorldObjects3D.WorldSpot)new WorldObjects3D.Air() : new WorldObjects3D.ScifiGround(graphicsDevice);

            return new Level("DefaultFalling", world, Vector3.Zero);
        }

        #endregion
    }
}
