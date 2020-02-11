using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcBallCamera
{
    class IsometricCamera : Camera
    {

        // Distance between target and camera
        public float Distance { get; set; }

        // Boundaries for distance
        public float MinDistance { get; set; }
        public float MaxDistance { get; set; }

        public Vector3 Position { get; private set; }
        public Vector3 Target { get; set; }

        public IsometricCamera(Vector3 target, float distance, float minDistance, float maxDistance, GraphicsDevice graphicsDevice)
            : base(graphicsDevice, ProjectionMatrixType.Orthographic)
        {
            // Clamp Distance
            Distance = MathHelper.Clamp(distance, minDistance, maxDistance);
            Target = target;

            Update();
            
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

        public void Zoom(float distance)
        {
            Distance = distance;
            Distance = MathHelper.Clamp(Distance, MinDistance, MaxDistance);

        }

        public override void Update()
        {
            Matrix rotationMatrix = Matrix.CreateRotationX((float)Math.Asin(Math.Tan(MathHelper.ToRadians(30f)))) * Matrix.CreateRotationY(MathHelper.ToRadians(45f));

            Vector3 transform = Vector3.Transform(new Vector3(0,0,Distance), rotationMatrix);

            Position = Target + transform;
            View = Matrix.CreateLookAt(Position, Target, Vector3.Up);
        }

    }
}
