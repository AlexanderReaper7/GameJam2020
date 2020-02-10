using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Tools_XNA
{
    class World
    {
        public static WorldSpot[,,] worldSpots;

        public World(int sizeX, int sizeY, int sizeZ)
        {
            worldSpots = new WorldSpot[sizeX, sizeY, sizeZ];
        }
    }

    class WorldSpot : IDrawable
    {
        public void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public bool Visible { get; }
        public int DrawOrder { get; }
        public event EventHandler<EventArgs> VisibleChanged;
        public event EventHandler<EventArgs> DrawOrderChanged;
    }
}
