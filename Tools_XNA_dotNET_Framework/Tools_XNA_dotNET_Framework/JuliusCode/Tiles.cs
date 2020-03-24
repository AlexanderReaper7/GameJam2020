// Created this class | Julius 18-11-21

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Tools_XNA
{
   public class Tiles
    {
        // Bool that is used to know if the player has already walked on this tile // Olle A 20-02-11
        public bool HasBeenWalkedOn = false;
        // Bool that is used to know if the player is currently on this tile // Olle A, Emil C.A. 20-02-12
        public bool IsOnTile = false;
        // Type of tile, 0 is air // Olle A 20-02-11
        public int Type;

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
            if (Type != 0)  spriteBatch.Draw(texture, rectangle, Color.White);
        }

        // En barnclass som används i "TilesMap.cs" | Julius 18-11-21
        public class CollisionTiles : Tiles
        {
            // Läser in en textur och sammankopplar den tillsamans med en rektangel | Julius 18-11-21
            public CollisionTiles(int i, Rectangle newRectangle)
            {
                // Workaround that loads dispenser texture // Olle A 20-03-17
                if (i == 150 || i == 151 || i == 152 || i == 153)
                    texture = Content.Load<Texture2D>("Textures/Tiles/105");

                else if (i == 250 || i == 251 || i == 252 || i == 253)
                    texture = Content.Load<Texture2D>("Textures/Tiles/205");
                else
                {
                    // Laddar texturnamn efter "i" värdet, (kan ladda in Tile1.png och Tile2.png med en linje av kod) | Julius 18-11-21
                    texture = Content.Load<Texture2D>("Textures/Tiles/" + i);
                }
                this.Rectangle = newRectangle;

                // Update type of tile // Olle A 20-02-11
                Type = i;
            }


            public void ChangeType(int newType)
            {

                Type = newType;
                texture = Content.Load<Texture2D>("Textures/Tiles/" + newType);
            }


        }


    }
}

