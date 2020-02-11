using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Tools_XNA
{
    public class SkyBox
    {
        private CustomModel model;
        private Effect effect;
        private GraphicsDevice graphics;

        public SkyBox(ContentManager Content, GraphicsDevice GraphicsDevice, TextureCube Texture)
        {
            model = new CustomModel(Content.Load<Model>("Models/skysphere_mesh"), Vector3.Zero, Vector3.Zero, Vector3.One,
                GraphicsDevice);

            effect = Content.Load<Effect>("Effects/skysphere_effect");
            effect.Parameters["CubeMap"].SetValue(Texture);

            model.SetModelEffect(effect, false);

            this.graphics = GraphicsDevice;
        }

        public void Draw(Matrix View, Matrix Projection, Vector3 CameraPosition)
        {
            // Disable the depth buffer
            graphics.DepthStencilState = DepthStencilState.None;

            // Move the model with sphere
            model.Position = CameraPosition;

            model.Draw(View, Projection, CameraPosition);

            graphics.DepthStencilState = DepthStencilState.Default;
        }
    }
}
