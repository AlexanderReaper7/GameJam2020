using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools_XNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam2020_3D
{
    public class Player
    {
        private static Model model;

        public CustomModel customModel;

        public Player(Vector3 position, Vector3 rotation, GraphicsDevice graphicsDevice)
        {
            customModel = new CustomModel(model, position, rotation, Vector3.One, graphicsDevice);
        }

        public static void LoadContent(ContentManager content)
        {
            model = content.Load<Model>(@"Models/Player");
        }

        public void Move(Vector2 change)
        {
            Vector3 p = customModel.Position;
            p.X += change.X;
            p.Z += change.Y;
            customModel.Position = p;
        }

        public void Rotate(float change)
        {
            Vector3 r = customModel.Rotation;
            r.Y += change;
            customModel.Rotation = r;
        }

        public void Draw(Camera camera)
        {
            customModel.Draw(camera.View, camera.Projection, camera.Position);
        }
    }
}