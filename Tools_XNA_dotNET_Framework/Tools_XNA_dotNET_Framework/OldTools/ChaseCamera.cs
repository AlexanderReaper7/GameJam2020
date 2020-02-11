using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tools_XNA
{
    public class ChaseCamera : Camera
    {


        // Variables for camera and targeted position
        public Vector3 Position { get; private set; }
        public Vector3 Target { get; set; }

        // Variables for camera position relative from targets position and where on target the camera shall focus on
        public Vector3 PositionOffset { get; set; }
        public Vector3 TargetOffset { get; set; }

        public Vector3 FollowTargetPosition { get; private set; }
        public Vector3 FollowTargetRotation { get; private set; }

        // Variable for the cameras rotation
        public Vector3 RelativeCameraRotation { get; set; }

        // Variable for how "react-full" the camera shall be when the target moves
        private float springiness = 0.15f;

        public float Springiness
        {
            get { return springiness; }
            set { springiness = MathHelper.Clamp(value, 0, 1); }
        }

        // Constructor
        public ChaseCamera(Vector3 PositionOffset, Vector3 TargetOffset, Vector3 RelativeCameraRotation, GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            this.PositionOffset = PositionOffset;
            this.TargetOffset = TargetOffset;
            this.RelativeCameraRotation = RelativeCameraRotation;
        }

        // Method that changes cameras desired position
        public void Move(Vector3 NewFollowTargetPosition, Vector3 NewFollowTargetRotation)
        {
            this.FollowTargetPosition = NewFollowTargetPosition;
            this.FollowTargetRotation = NewFollowTargetRotation;
        }

        // Method that changes camera rotation, without restrict (kinda like an ArcBall)
        public void Rotate(Vector3 RotationChange)
        {
            this.RelativeCameraRotation += RotationChange;
        }


        public override void Update()
        {
            // Smash together the desired rotation of the model and camera
            Vector3 combinedRotation = FollowTargetRotation + RelativeCameraRotation;

            // Calculate rotation matrix for the camera
            Matrix rotation = Matrix.CreateFromYawPitchRoll(combinedRotation.Y, combinedRotation.X, combinedRotation.Z);

            // Calculate where the camera shall be
            Vector3 desiredPosition = FollowTargetPosition + Vector3.Transform(PositionOffset, rotation);

            // Change between the current position and the desired position (In a smooth way)
            Position = Vector3.Lerp(Position, desiredPosition, Springiness);

            // Calculate the new target position
            Target = FollowTargetPosition + Vector3.Transform(TargetOffset, rotation);

            // Calculate up vector from rotation matrix
            Vector3 up = Vector3.Transform(Vector3.Up, rotation);

            // Calculate the view matrix
            View = Matrix.CreateLookAt(Position, Target, up);
        }
    }
}
