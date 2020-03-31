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
        Texture2D texture;
        // Rectangle for collision // Olle A 20-03-31
        public Rectangle Rectangle
        {
            get { return new Rectangle((int)Position2D.X, (int)Position2D.Y, texture.Width, texture.Height); }
        }

        // Variables for 3D
        Vector3 Position3D;
        Vector3 velocity3D;
        Model model;

        // Olle A 200210
        /// <summary>
        /// Constructor for 2D world
        /// </summary>
        /// <param name="speed">Speed to move at</param>
        /// <param name="Position2D">Position as vector2</param>
        /// <param name="direction">Direction of travel</param>
        /// <param name="texture">Texture of projectile</param>
        public Projectile(float speed, Vector2 Position2D, Vector2 direction, Texture2D texture)
        {
            this.texture = texture;
            this.Position2D = Position2D;

            // Normalize direction
            direction.Normalize();

            // Calculate velocity of the projectile
            velocity2D = direction * speed;

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
        public void Update()
        {
            // Move projectile according to time depending on 2D or 3D
            if (is2D) Position2D += velocity2D;
            else Position3D += velocity3D;


            // Check for collisions
            //CheckCollision();
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
            // Draw texture
            spriteBatch.Draw(texture, new Rectangle((int)Position2D.X, (int)Position2D.Y, texture.Width, texture.Height), Color.White);
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
