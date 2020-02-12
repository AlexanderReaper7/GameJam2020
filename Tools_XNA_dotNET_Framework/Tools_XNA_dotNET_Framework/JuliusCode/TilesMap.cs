// Created this class | Julius 18-11-21

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using static Tools_XNA.Tiles;

namespace Tools_XNA
{
    public class TilesMap
    {
        // "Get" kod för collision och rektangelns bredd/längd | Julius 18-11-21
        private List<CollisionTiles> collisionTiles = new List<CollisionTiles>();
        public List<CollisionTiles> CollisionTiles
        {
            get { return collisionTiles; }
        }

        private int width, height;
        public int Width
        {
            get { return width; }
        }
        public int Height
        {
            get { return height; }
        }

        // Olle A 20-02-12
        public int TileMapWidth;
        public int Size;
        public int StartingPosition;

        // En oanvänd construktor | Julius 18-11-26
        public TilesMap() { }

        // Update
        public void Update()
        {
            // Remove tile (make it air) if player has walked on it but is no longer standing on it // Olle A 200212
            for (int i = 0; i < collisionTiles.Count; i++)
            {
                if (collisionTiles[i].HasBeenWalkedOn && !collisionTiles[i].IsOnTile)
                {
                    collisionTiles[i].Type = 0;
                }
            }
        }

        // En kartgenererare där man bestämmer vart och vilken typ av tile som finns i en array (map) och pixelstorleken på tilens sida (size) | Julius 18-11-21
        public void Generate(int[,] map, int StartingPosition, int tileMapWidth, int size, int xOffset, int yOffset)
        {
            TileMapWidth = tileMapWidth;
            this.StartingPosition = StartingPosition;
            Size = size;

            // Ändrade så att y räknas baklänges. Fixar så att tiles:en ritas ut i rätt ordning | Olle A 20-02-11
            for (int x = 0; x < map.GetLength(1); x++)
                for (int y = map.GetLength(0)-1; y > 0; y--)
                {
                    int number = map[y, x];

                    // Om "i" in ("Tile" + i) är större än noll så har tilen en kollision, tror jag | Julius 18-11-21
                    collisionTiles.Add(new CollisionTiles(number, new Rectangle(x * size/2 + y*size/2 + xOffset, y * size/5 - x*size/5 + yOffset, size, size)));
                    width = (x + 1) * size;
                    height = (y + 1) * size;
                }

            // Vänder håll på listan så att tiles:en ritas ut korrekt | Olle A 20-02-11
            collisionTiles.Reverse();
        }

        // Ritar ut alla rektanglar | Julius 18-11-21

        public void Draw(SpriteBatch spriteBatch)
        {

            foreach (CollisionTiles tile in collisionTiles)
                tile.Draw(spriteBatch);
        }

    }
}
