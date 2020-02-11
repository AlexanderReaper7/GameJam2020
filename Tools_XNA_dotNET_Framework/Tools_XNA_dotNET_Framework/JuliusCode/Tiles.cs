// Created this class | Julius 18-11-21

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Tools_XNA
{
   public class Tiles
    {
        protected Texture2D texture;

        private Rectangle rectangle;
        public Rectangle Rectangle
        {
            get { return rectangle; }
            protected set { rectangle = value; }
        }

        private static ContentManager content;
        public static ContentManager Content
        {
            protected get { return content; }
            set { content = value; }
        }

        // Rita ut rektangelns textur | Julius 18-11-21
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }

        // En barnclass som används i "TilesMap.cs" | Julius 18-11-21
        public class CollisionTiles : Tiles
        {
            // Läser in en textur och sammankopplar den tillsamans med en rektangel | Julius 18-11-21
            public CollisionTiles(int i, Rectangle newRectangle)
            {
                // Laddar texturnamn efter "i" värdet, (kan ladda in Tile1.png och Tile2.png med en linje av kod) | Julius 18-11-21
                texture = Content.Load<Texture2D>("Textures/Tiles/" + i);
                this.Rectangle = newRectangle;
            }
        }


    }
}

