using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Tools_XNA
{
    public class WorldObjects3D
    {
        public abstract class WorldSpot
        {
            public Vector3 position;

            public virtual void Draw(GameTime gameTime, Camera camera)
            {
            }

            public virtual void Update(GameTime gameTime)
            {
            }
        }


    /// <summary>
    /// Standard ground
    /// </summary>
    public class Ground : WorldSpot
        {
            private static Model model;
            public CustomModel customModel;

            public Ground(GraphicsDevice graphicsDevice)
            {
                customModel = new CustomModel(model, Vector3.Zero, Vector3.Zero, Vector3.One, graphicsDevice);
            }

            public override void Draw(GameTime gameTime, Camera camera)
            {
                customModel.Draw(camera.View, camera.Projection, camera.Position);
            }

            public override void Update(GameTime gameTime)
            {
                customModel.Position = position;
            }

            public static void LoadContent(ContentManager content, string path)
            {
                if (model == null) model = content.Load<Model>(path);
            }
        }

        /// <summary>
        /// An empty space in the world
        /// </summary>
        public class Air : WorldSpot
        {
            /// <summary>
            /// Does not draw anything
            /// </summary>
            /// <param name="gameTime"></param>
            /// <param name="camera"></param>
            public override void Draw(GameTime gameTime, Camera camera)
            {
            }

            /// <summary>
            /// Does not do anything
            /// </summary>
            /// <param name="gameTime"></param>
            public override void Update(GameTime gameTime)
            {
            }
        }
    }
}