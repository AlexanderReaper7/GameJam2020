using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tools_XNA
{
    public class FreeCamera : Camera
    {
        public float Yaw { get; set; }
        public float Pitch { get; set; }

        public Vector3 Target { get; set; }

        private Vector3 _translation;

        public FreeCamera(GraphicsDevice graphicsDevice, float yaw, float pitch, Vector3 position) : base(graphicsDevice, ProjectionMatrixType.Perspective)
        {
            Yaw = yaw;
            Pitch = pitch;
            Position = position;

            _translation = Vector3.Zero;
        }

        public void Rotate(float yawChange, float pitchChange)
        {
            Yaw += yawChange;
            Pitch += pitchChange;
        }

        public void Move(Vector3 translation)
        {
            _translation = translation;
        }

        public override void Update()
        {
            // Calculate rotation matrix
            Matrix rotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, 0);

            // Move position and reset translation
            _translation = Vector3.Transform(_translation,  rotation);
            Position += _translation;
            _translation = Vector3.Zero;

            // Calculate new target
            Vector3 forward = Vector3.Transform(Vector3.Forward, rotation);
            Target = Position + forward;

            // Calculate up vector
            Vector3 up = Vector3.Transform(Vector3.Up, rotation);

            // Calculate view matrix
            View = Matrix.CreateLookAt(Position, Target, up);
        }
    }
}
