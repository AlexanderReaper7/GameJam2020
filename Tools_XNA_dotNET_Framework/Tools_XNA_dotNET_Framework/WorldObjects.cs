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
        /// <param name="camera"></param>
        public void Draw(GameTime gameTime, Camera camera)
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

    public class Ground : IWorldSpot
    {
        private CustomModel model;

        public void Draw(GameTime gameTime, Camera camera)
        {
            model.Draw(camera.View, camera.Projection, camera.);
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}