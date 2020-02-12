using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tools_XNA;

namespace GameJam2020_3D
{
    public class InGame
    {
        Game1 game;
        private GraphicsDeviceManager graphics;

#if DEBUG
        private FreeCamera freeCamera;
        private bool freeCameraActive = false;
        private MouseState lastMouseState;
#endif
        private World world;
        private IsometricCamera camera;


        public InGame(Game1 game, GraphicsDeviceManager graphics)
        {
            this.game = game;
            this.graphics = graphics;
        }

        public void Initialize()
        {
            camera = new IsometricCamera(Vector3.Zero, 10000f, 1f, float.MaxValue, graphics.GraphicsDevice);

#if DEBUG
            lastMouseState = Mouse.GetState();
            freeCamera = new FreeCamera(graphics.GraphicsDevice, MathHelper.ToRadians(153), MathHelper.ToRadians(5), new Vector3(1000, 1000, -2000));
#endif
        }

        public void LoadContent(ContentManager content)
        {
            // Load Models
            WorldObjects3D.Ground.LoadContent(content, @"Models/GroundStandard");
            // Load World
            LoadLevel(Level.CreateFilled(graphics.GraphicsDevice));
        }

        public void LoadLevel(Level level)
        {
            // Set world
            world = level.World;
            // Recalculate zoom level for isometric camera
            ConfigureCamera();
        }
        /// <summary>
        /// Size of sides in pixels
        /// </summary>
        private const float padding = 0f;
        public void ConfigureCamera()
        {
            //TODO: move world -y to not block camera
            // Zoom camera to fit world
            // Get largest side
            float largestSide = (float)Math.Sqrt(Math.Pow(world.RealSize.Z, 2) + Math.Pow(world.RealSize.X, 2));
            // Get largest screen side
            int lsg = Math.Min(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);

            camera.Zoom = largestSide / lsg;
        }


        public void Update(GameTime gameTime)
        {
            world.Update(gameTime);
            camera.Update();
#if DEBUG
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad1)) freeCameraActive = true;
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad2)) freeCameraActive = false;
            UpdateFreeCamera(gameTime);
#endif

        }

#if DEBUG
        private void UpdateFreeCamera(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyState = Keyboard.GetState();

            // Calculate how much the camera should rotate
            float deltaX = lastMouseState.X - mouseState.X;
            float deltaY = lastMouseState.Y - mouseState.Y;

            // Rotate camera
            freeCamera.Rotate(deltaX * 0.01f, deltaY * 0.01f);

            Vector3 translation = Vector3.Zero;

            if (keyState.IsKeyDown(Keys.W)) translation += Vector3.Forward * 10f;
            if (keyState.IsKeyDown(Keys.S)) translation += Vector3.Backward * 10f;
            if (keyState.IsKeyDown(Keys.A)) translation += Vector3.Left * 10f;
            if (keyState.IsKeyDown(Keys.D)) translation += Vector3.Right * 10f;
            if (keyState.IsKeyDown(Keys.Space)) translation += Vector3.Up * 10f;

            // Move camera
            freeCamera.Move(translation);

            // Update camera
            freeCamera.Update();

            // Update lastMouseState
            lastMouseState = mouseState;
        }
#endif


        public void Draw(GameTime gameTime)
        {
#if DEBUG
            if (freeCameraActive)
            {
                world.Draw(gameTime, freeCamera);
            }
            else
            {
                world.Draw(gameTime, camera);
            }
#endif
#if !DEBUG
            // World
            world.Draw(gameTime, camera);
            // UI
            
#endif

        }
    }
}
