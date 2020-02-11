using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tools_XNA
{
    public abstract class Camera
    {
        private Matrix view;
        private Matrix projection;

        public Matrix Projection
        {
            get { return projection; }

            protected set
            {
                projection = value;
                generateFrustrum();
            }
        }

        public Matrix View
        {
            get { return view; }

            set
            {
                view = value;
                generateFrustrum();
            }
        }

        public Vector3 Position { get; protected set; }

        public BoundingFrustum Frustum { get; private set; }

        protected GraphicsDevice GraphicsDevice { get; set; }

        public Camera(GraphicsDevice graphicsDevice, ProjectionMatrixType pmt)
        {
            GraphicsDevice = graphicsDevice;
            switch (pmt)
            {
                case ProjectionMatrixType.Perspective:
                    GeneratePerspectiveProjectionMatrix();
                    break;
                case ProjectionMatrixType.Orthographic:
                    GenerateOrthographicProjectionMatrix();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(pmt), pmt, null);
            }
        }

        public enum ProjectionMatrixType
        {
            Perspective,
            Orthographic
        }

        protected void GeneratePerspectiveProjectionMatrix()
        {
            PresentationParameters pp = GraphicsDevice.PresentationParameters;

            float aspectRatio = (float) pp.BackBufferWidth / (float) pp.BackBufferHeight;

            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 0.1f, 1000000.0f);
        }

        protected void GenerateOrthographicProjectionMatrix()
        {
            Projection = Matrix.CreateOrthographic(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0.1f, 1000000.0f);
        }

        public virtual void Update()
        {

        }

        private void generateFrustrum()
        {
            Matrix viewProjection = View * Projection;
            Frustum = new BoundingFrustum(viewProjection);
        }

        public bool BoundingVolumeIsInView(BoundingSphere sphere)
        {
            return (Frustum.Contains(sphere) != ContainmentType.Disjoint);
        }

        public bool BoundingVolumeIsInView(BoundingBox box)
        {
            return (Frustum.Contains(box) != ContainmentType.Disjoint);
        }
    }
}