using System;
using Microsoft.Win32.SafeHandles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tools_XNA
{
    // Alexander Ö 
    /// <summary>
    /// Camera for isometric view
    /// </summary>
    public class IsometricCamera : Camera
    {
        private float zoom;

        // Distance between target and camera
        public float Distance { get; set; }

        // Boundaries for distance
        public float MinDistance { get; set; }
        public float MaxDistance { get; set; }

        public float Zoom
        {
            get { return zoom;}
            set
            {
                zoom = value;
                this.GenerateOrthographicProjectionMatrix();
            }
        }

        public Vector3 Target { get; set; }

        public IsometricCamera(Vector3 target, float distance, float minDistance, float maxDistance, GraphicsDevice graphicsDevice)
            : base(graphicsDevice, ProjectionMatrixType.Orthographic)
        {
            // Clamp Distance
            Distance = MathHelper.Clamp(distance, minDistance, maxDistance);
            Target = target;
            this.GenerateOrthographicProjectionMatrix();
        }

        public void Move(float distanceChange)
        {
            Distance += distanceChange;
            Distance = MathHelper.Clamp(Distance, MinDistance, MaxDistance);
        }

        public void Rotate()
        {
            throw new NotImplementedException();
        }


        enum Angles // TODO: rotation
        {
            UpLeft,
            DownLeft
        }

        protected void GenerateOrthographicProjectionMatrix()
        {
            Projection = Matrix.CreateOrthographic(GraphicsDevice.Viewport.Width * Zoom, GraphicsDevice.Viewport.Height * Zoom, 0.00001f, 10000000.0f);
        }

        public override void Update()
        {
            // Create isometric rotation matrix
            Matrix rotationMatrix = Matrix.CreateRotationX((float)Math.Asin(Math.Tan(MathHelper.ToRadians(-30f)))) * Matrix.CreateRotationY(MathHelper.ToRadians(45f + 180));
            Vector3 transform = Vector3.Transform(new Vector3(0,0,Distance), rotationMatrix);
            Position = Target + transform;
            View = Matrix.CreateLookAt(Position, Target, Vector3.Up); // Q: transform up vector?
        }

    }
}
