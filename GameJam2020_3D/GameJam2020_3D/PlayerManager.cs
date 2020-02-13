﻿using System;
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
            private set {
                worldPosition = value;
                RealPosition = game.world.CalculateRealPosition(value);
            }
        }

        private bool walk = false;
        private int walkTime = 0;
        private Vector3 worldPosition;

        public PlayerManager(InGame game, Vector3 startingPosition)
        {
            this.game = game;
            player = new Player(Vector3.Zero, Vector3.Zero, game.game.GraphicsDevice);
            WorldPosition = startingPosition;
        }


        /// <summary>
        /// Checks if the tile in this coord is ground
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <param name="world"></param>
        /// <returns></returns>
        private bool CheckType(int x, int y, int z, Type type)
        {
            //  get type in position
            try
            {
                Type t = game.world.worldSpots[x, y, z].GetType();
                return t == type;
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
            // Check if next position is valid
            if (CheckType((int)p.X, (int)p.Y +1, (int)p.Z, typeof(WorldObjects3D.Air)))
            {
                if (CheckType((int)p.X, (int)p.Y, (int)p.Z, typeof(WorldObjects3D.Ground)))
                {
                    // move
                    WorldPosition = p;
                    return;
                }
                else if (CheckType((int)p.X, (int)p.Y, (int)p.Z, typeof(WorldObjects3D.FallingGround)))
                {
                    ((WorldObjects3D.FallingGround)game.world.worldSpots[(int) worldPosition.X, (int) worldPosition.Y, (int) worldPosition.Z]).falling = true;
                    // move
                    WorldPosition = p;
                    return;
                }
            }

            // TODO: Die
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