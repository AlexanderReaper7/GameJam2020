using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Tools_XNA
{
    public class Credits
    {
        // Size of credits
        public Rectangle size;
        // Scale of credits
        public float scale = 1.0f;    

        // Credits position
        public Vector2 position = new Vector2(300, 150);

        // Texture object used for drawing the credits
        private Texture2D credits;

        // Load credits texture
        public void LoadContent(ContentManager Content)
        {
            // Loads credit sprite
            credits = Content.Load<Texture2D>(@"Shared/Menu/Credits");
            size = new Rectangle(0, 0, credits.Width, credits.Height);
        }

        public void Update(GameTime gameTime)
        {
            // Direction that the credits will move in
            Vector2 aDirection = new Vector2(0, -1);
            // The speed the credits will move with
            Vector2 aSpeed = new Vector2(0, 30);

            position += aDirection * aSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        // Draws credits sprite in the right position
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(credits, position, Color.White);
        }

    }
}
