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
    public class PlayerManager
    {
        public Player player;

        const float tileDis = WorldObjects3D.TileScalar + WorldObjects3D.TileOffset;
        public Vector3 playerPos
        {
            get { return player.customModel.Position; }
            set { player.customModel.Position = value; }
        }

        private bool walk = false;
        private int walkTime = 0;

        public PlayerManager(GraphicsDevice graphicsDevice)
        {
            player = new Player(new Vector3(0,200,0), Vector3.Zero, graphicsDevice);
        }

        public void Update(GameTime gameTime)
        {
            //playerPos.Y = 800;
            KeyboardState keyState = Keyboard.GetState();
            if((keyState.IsKeyDown(Keys.A))&& walk == false)
            {
                player.Move(new Vector2(tileDis, 0)); 
                walk = true;
                walkTime = 200;
            }
            if ((keyState.IsKeyDown(Keys.D))&& walk == false)
            {
                player.Move(new Vector2(-tileDis, 0));
                walk = true;
                walkTime = 200;
            }
            if ((keyState.IsKeyDown(Keys.W))&& walk == false)
            {
                player.Move(new Vector2(0, tileDis));
                walk = true;
                walkTime = 200;
            }
            if ((keyState.IsKeyDown(Keys.S))&& walk == false)
            {
                player.Move(new Vector2(0, -tileDis));
                walk = true;
                walkTime = 200;
            }
            if (walkTime <= 0) walk = false;
            walkTime -= gameTime.ElapsedGameTime.Milliseconds;
        }
    }
}
