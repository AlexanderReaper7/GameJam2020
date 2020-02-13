using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    // Theo
    // Alexander
    public class PlayerManager
    {
        public Player player;
        public InGame game;

        public Vector3 RealPosition
        {
            get { return player.customModel.Position; }
            set { player.customModel.Position = value; }
        }

        public Vector3 WorldPosition
        {
            get { return worldPosition; }
            set {
                worldPosition = value;
                RealPosition = game.world.CalculateRealPosition(value);
            }
        }

        private bool walk = false;
        private int walkTime = 0;
        private Vector3 worldPosition;

        public PlayerManager(InGame game)
        {
            this.game = game;
            player = new Player(new Vector3(0, 200, 0), Vector3.Zero, game.game.GraphicsDevice);
            WorldPosition = new Vector3(0,0,0); // NOTE: starting position
        }


        /// <summary>
        /// Checks if the tile in this coord is ground
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <param name="world"></param>
        /// <returns></returns>
        private bool CheckGround(int x, int y, int z)
        {
            //  get type in position
            try
            {
                Type t = game.world.worldSpots[x, y, z].GetType();
                return t == typeof(WorldObjects3D.Ground);
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }

        public void Move(Vector3 change)
        {
            // Calculate next position
            Vector3 p = worldPosition;
            p.X += change.X;
            p.Y += change.Y;
            p.Z += change.Z;
            // Check if next position is valid ground
            if (CheckGround((int) p.X, (int) p.Y, (int) p.Z))
            {
                // move
                WorldPosition = p;
            }
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState(); // TODO: change movement directions to correct ones
            if ((keyState.IsKeyDown(Keys.A)) && walk == false)
            {
                Move(Vector3.Right);
                walk = true;
                walkTime = 200;
            }

            if ((keyState.IsKeyDown(Keys.D)) && walk == false)
            {
                Move(Vector3.Left);
                walk = true;
                walkTime = 200;
            }

            if ((keyState.IsKeyDown(Keys.W)) && walk == false)
            {
                Move(Vector3.Backward);
                walk = true;
                walkTime = 200;
            }

            if ((keyState.IsKeyDown(Keys.S)) && walk == false)
            {
                Move(Vector3.Forward);
                walk = true;
                walkTime = 200;
            }

            if (walkTime <= 0) walk = false;
            walkTime -= gameTime.ElapsedGameTime.Milliseconds;
        }
    }
}