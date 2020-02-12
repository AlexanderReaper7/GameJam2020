using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Tools_XNA;

namespace GameJam2020_3D
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Level level;
        private IsometricCamera camera;

#if DEBUG
        private FreeCamera freeCamera;
        private bool freeCameraActive = false;
        private MouseState lastMouseState;
#endif
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsFixedTimeStep = false;
            graphics.PreferMultiSampling = true;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            camera = new IsometricCamera(Vector3.Zero, 10000f, 1f, float.MaxValue, GraphicsDevice);

#if DEBUG
            lastMouseState = Mouse.GetState();
            freeCamera = new FreeCamera(GraphicsDevice, MathHelper.ToRadians(153), MathHelper.ToRadians(5), new Vector3(1000, 1000, -2000));
#endif
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // Load Models
            WorldObjects3D.Ground.LoadContent(Content, @"Models/GroundStandard");
            // Load World
            LoadLevel(Level.CreateFilled(GraphicsDevice));
        }

        public void LoadLevel(Level level)
        {
            // Set world
            this.level = level;
            // Recalculate zoom level for isometric camera
            ConfigureCamera();
        }
        public void ConfigureCamera()
        {
            //TODO: move world -y to not block camera
            // Zoom camera to fit world
            // Get largest side
            float largestSide = (float) Math.Sqrt(Math.Pow(level.World.RealSize.Z, 2) + Math.Pow(level.World.RealSize.X, 2));
            // Get largest screen side
            int lsg = Math.Min(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            camera.Zoom = largestSide / lsg;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            level.World.Update(gameTime);
            camera.Update();
#if DEBUG
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad1)) freeCameraActive = true;
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad2)) freeCameraActive = false;
            UpdateFreeCamera(gameTime);
#endif

            base.Update(gameTime);
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


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
#if DEBUG
            if (freeCameraActive)
            {
                level.World.Draw(gameTime, freeCamera);
            }
            else
            {
                level.World.Draw(gameTime,camera);
            }
#endif
#if !DEBUG
            // World
            world.Draw(gameTime, camera);
            // UI
            
#endif
            base.Draw(gameTime);
        }
    }
}
