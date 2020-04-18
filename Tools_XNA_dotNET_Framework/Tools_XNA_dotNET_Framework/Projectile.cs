using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tools_XNA
{
    public class Projectile
    {
        // If projectile exists in 2D or 3D space // Olle A 200210
        private bool is2D;

        // Variables for 2D
        Vector2 Position2D;
        Vector2 velocity2D;
        public int TilePosition;
        private int direction;
        Texture2D texture;
        public TilesMap tilemap;
        float moveTimer = 0;
        float moveTimerMax = 50;

        // Variables for 3D
        Vector3 Position3D;
        Vector3 velocity3D;
        Model model;

        // Olle A 200210
        /// <summary>
        /// Constructor for 2D world
        /// </summary>
        /// <param name="speed">Speed to move at</param>
        /// <param name="tilePosition">Position as int in tilemap</param>
        /// <param name="direction">Direction of travel: left, right, up, down</param>
        /// <param name="texture">Texture of projectile</param>
        public Projectile(float speed, int tilePosition, string direction, TilesMap tilemap, Texture2D texture)
        {
            // Convert direction to momvent on tilemap grid // Olle A 200407
            switch (direction)
            {
                case "down":
                    this.direction = 1;
                    break;
                case "up":
                    this.direction = -1;
                    break;
                case "left":
                    this.direction += tilemap.TileMapWidth;
                    break;
                case "right":
                    this.direction -= tilemap.TileMapWidth;
                    break;
            }

            this.texture = texture;
            TilePosition = tilePosition + this.direction;
            this.tilemap = tilemap;

            // Game is 2D
            is2D = true;
        }

        // Olle A, Gustav H 200210
        /// <summary>
        /// Constructor for 3D world
        /// </summary>
        /// <param name="speed">Speed to move at</param>
        /// <param name="Position3D">Position as vector3</param>
        /// <param name="direction">Direction of travel</param>
        /// <param name="model">3D model of projectile</param>
        public Projectile(float speed, Vector3 Position3D, Vector3 direction, Model model)
        {
            this.model = model;
            this.Position3D = Position3D;

            // Normalize direction
            direction.Normalize();

            // Calculate velocity of the projectile
            velocity3D = direction * speed;

            // Game is 3D
            is2D = false;
        }

        // Olle A 200210
        /// <summary>
        /// Update. Moves projectile and checks for collisions
        /// </summary>
        public void Remove()
        {
            // TODO: Code for culling this projectile instance
            throw new NotImplementedException();
        }

        // Olle A 200210
        /// <summary>
        /// Update. Moves projectile and checks for collisions
        /// </summary>
        public void Update(GameTime gameTime)
        {
            // Move projectile according to time depending on 2D or 3D
            if (is2D)
            {
                // Move after tilemap grid // Olle A. 200407
                moveTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (moveTimerMax <= moveTimer)
                {
                    TilePosition += direction;
                    moveTimer = 0;
                }
            }
            else Position3D += velocity3D;
        }



        // Olle A 200210
        /// <summary>
        /// Checks if projectile collides with something
        /// Could also be done from another class
        /// </summary>
        private void checkCollision()
        {
            //Check if projectile enters new tile and act accordingly
                //Kill player or something

           throw new NotImplementedException();
        }

        // Olle A 200211
        // Gustav H, Olle A 200210
        /// <summary>
        /// Draw function for 2D
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            try
            {
                // Draw texture
                spriteBatch.Draw(texture, tilemap.CollisionTiles[TilePosition].Rectangle.Center.ToVector2() - new Vector2(texture.Width / 2, texture.Height / 2), Color.White);
            }
            catch { }
        }

        // Olle A 200211
        /// <summary>
        /// Draw function for 3D
        /// </summary>
        public void Draw(Camera camera)
        {
            // Draw model using supplied camera
            model.Draw(camera.View, camera.Projection, camera.Projection);
        }
    }
}
