using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Tools_XNA
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    namespace Tools_XNA
    {
        public class IsometricCamera
        {
            Matrix view;
            Matrix projection;

            public Matrix Projection
            {
                get { return projection; }
                protected set
                {
                    projection = value;
                    generateFrustum();
                }
            }

            public Matrix View
            {
                get { return view; }
                protected set
                {
                    view = value;
                    generateFrustum();
                }
            }

            // Create frustum
            public BoundingFrustum Frustum { get; private set; }

            protected GraphicsDevice GraphicsDevice { get; set; }

            // Constructor
            public IsometricCamera(GraphicsDevice graphicsDevice)
            {
                this.GraphicsDevice = graphicsDevice;

                generateProjectionMatrix(MathHelper.PiOver4);
            }

            // Converting the camera view to an 2D image
            private void generateProjectionMatrix(float FieldOfView)
            {
                // Load window (2D viewport) information
                PresentationParameters pp = GraphicsDevice.PresentationParameters;

                // Aspect ratio is width divided by height
                float aspectRatio = (float)pp.BackBufferWidth / (float)pp.BackBufferHeight;


                this.projection = Matrix.CreateOrthographic(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0.01f, 100000.0f);
            }

            // "Open" update method for any class to override
            public virtual void Update()
            {

            }

            // Generate frustum that acts as a "render frustum", everything within frustum will be rendered, which is based on view and projection
            private void generateFrustum()
            {
                Matrix viewProjection = View * Projection;
                Frustum = new BoundingFrustum(viewProjection);
            }

            // Look if spheres is within frustum
            public bool BoundingVolumeIsInView(BoundingSphere sphere)
            {
                return (Frustum.Contains(sphere) != ContainmentType.Disjoint);
            }

            // Look if boxes is within frustum
            public bool BoundingVolumeIsInView(BoundingBox box)
            {
                return (Frustum.Contains(box) != ContainmentType.Disjoint);
            }
        }
    }
}
