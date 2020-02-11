using System;
using Microsoft.Xna.Framework;

namespace Tools_XNA
{
    public class Camera2D
    {
        private Game _game;
        private Rectangle WindowSize => _game.Window.ClientBounds;
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;

                // If there's a limit set and the camera is not transformed clamp position to limits TODO: make position limiting work with zoom and rotation
                if (Limits != null)
                {
                    _position.X = MathHelper.Clamp(_position.X, Limits.Value.X, Limits.Value.X + Limits.Value.Width - WindowSize.Width);
                    _position.Y = MathHelper.Clamp(_position.Y, Limits.Value.Y, Limits.Value.Y + Limits.Value.Height - WindowSize.Height);
                }
            }
        }
        public Vector2 Origin { get; set; }
        public float Rotation { get; set; }
        public float Zoom { get; set; }
        public Rectangle? Limits
        {
            get { return _limits; }
            set
            {
                if (value != null)
                {
                    // Assign limit but make sure it's always bigger than the windowSize TODO:  else zoom to fit
                    _limits = new Rectangle
                    {
                        X = value.Value.X,
                        Y = value.Value.Y,
                        Width = Math.Max(WindowSize.Width, value.Value.Width),
                        Height = Math.Max(WindowSize.Height, value.Value.Height)
                    };

                    // Validate camera position with new limit
                    Position = Position;
                }
                else
                {
                    _limits = null;
                }
            }
        }

        private Rectangle? _limits;
        private Vector2 _position;

        /// <summary>
        /// The rectangle of what the camera shows
        /// </summary>
        public Rectangle CameraWorldRect => new Rectangle(
            (int)((Position.X - Origin.X) / Zoom),
            (int)((Position.Y - Origin.Y) / Zoom),
            (int)(WindowSize.Width / Zoom),
            (int)(WindowSize.Height / Zoom));

        /// <summary>
        /// the origin coordinate manipulated by the zoom factor
        /// </summary>
        public Vector2 ZoomedOrigin => Origin * Zoom;


        public Camera2D(Game game)
        {
            _game = game;
            Origin = Vector2.Zero;
            Zoom = 1f;
            Position = Vector2.Zero;
            Rotation = 0f;
        }

        #region Parallax

        /// <summary>
        /// Returns the view matrix with translation scaled with parallax, to add no parallax set parallax to Vector.One
        /// </summary>
        /// <param name="parallax"></param>
        /// <returns></returns>
        public Matrix GetViewMatrix(Vector2 parallax)
        {
            // To add parallax, simply multiply it by the position
            return Matrix.CreateTranslation(new Vector3(-Position * parallax, 0.0f)) *
                   Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                   Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateScale(Zoom, Zoom, 1) *
                   Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
        }

        /// <summary>
        /// Transforms a Vector2 with the view matrix and parallax scaling
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <param name="parallax"></param>
        /// <returns></returns>
        public Vector2 WorldToScreen(Vector2 worldPosition, Vector2 parallax)
        {
            return Vector2.Transform(worldPosition, GetViewMatrix(parallax));
        }

        /// <summary>
        /// Transforms a Vector2 with the inverted view matrix and parallax scaling
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <param name="parallax"></param>
        /// <returns></returns>
        public Vector2 ScreenToWorld(Vector2 screenPosition, Vector2 parallax)
        {
            return Vector2.Transform(screenPosition, Matrix.Invert(GetViewMatrix(parallax)));
        }


        #endregion

        /// <summary>
        /// Returns the view matrix
        /// </summary>
        /// <returns></returns>
        public Matrix GetViewMatrix() // TODO: cache result?
        {
                return Matrix.CreateTranslation(new Vector3(-Position, 0.0f)) *
                       Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                       Matrix.CreateRotationZ(Rotation) *
                       Matrix.CreateScale(Zoom, Zoom, 1) *
                       Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
        }

        /// <summary>
        /// Transforms a Vector2 with the view matrix
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, GetViewMatrix());
        }


        /// <summary>
        /// Transforms a Vector2 with the inverted view matrix
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <returns></returns>
        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, Matrix.Invert(GetViewMatrix()));
        }


        public void LookAt(Vector2 position)
        {
            Position = position - ZoomedOrigin;
        }

        public void LookAt(Point position)
        {
            LookAt(new Vector2(position.X,position.Y));
        }

        public void ZoomToMatchHeight(Rectangle rectangle)
        {
            // Scale zoom to fit location
            Zoom = (float) _game.Window.ClientBounds.Height / (float) rectangle.Height;
        }

        public void ZoomToMatchWidth(Rectangle rectangle)
        {
            Zoom = (float)_game.Window.ClientBounds.Width / (float)rectangle.Width;
        }
    }
}
