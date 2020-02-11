using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Tools_XNA
{
    /// <summary>
    /// An empty space in the world
    /// </summary>
    public class Air : IWorldSpot
    {
        /// <summary>
        /// Does not draw anything
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(GameTime gameTime)
        {
        }

        /// <summary>
        /// Does not do anything
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
        }
    }
}
