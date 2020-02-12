using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        //public Player player { get; set; }


        public Level(string name, World world)
        {
            Name = name;
            World = world;
        }

        #region Create


        public static Level CreateDefault(GraphicsDevice graphicsDevice)
        {
            int x = 12, y = 4, z = 12;
            World world = new World(x, y, z);

            // fill world with air and ground
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                    for (int k = 0; k < z; k++)
                         world.worldSpots[i,j,k] = j > 0 ? (WorldObjects3D.WorldSpot) new WorldObjects3D.Air() : new WorldObjects3D.Ground(graphicsDevice);

            return new Level("Default", world);
        }

        public static Level CreateFilled(GraphicsDevice graphicsDevice)
        {
            int x = 12, y = 4, z = 12;
            World world = new World(x, y, z);

            // fill world with air and ground
            for (int i = 0; i < x; i++)
            for (int j = 0; j < y; j++)
            for (int k = 0; k < z; k++)
                world.worldSpots[i, j, k] = new WorldObjects3D.Ground(graphicsDevice);

            return new Level("DefaultFilled", world);
        }
        #endregion
    }
}
