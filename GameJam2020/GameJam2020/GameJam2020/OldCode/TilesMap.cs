// Created this class | Julius 18-11-21
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrJekyll_MrHyde_TeamCarp;
using static DrJekyll_MrHyde_TeamCarp.Tiles;

namespace DrJekyll_MrHyde_TeamCarp
{
    class TilesMap
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

        // En oanvänd construktor | Julius 18-11-26
        public TilesMap() { }

        // En kartgenererare där man bestämmer vart och vilken typ av tile som finns i en array (map) och pixelstorleken på tilens sida (size) | Julius 18-11-21
        public void Generate(int[,] map, int size)
        {
            for(int x = 0; x < map.GetLength(1); x++)
                for (int y = 0; y < map.GetLength(0); y++)
                {
                    int number = map[y, x];

                    // Om "i" in ("Tile" + i) är större än noll så har tilen en kollision, tror jag | Julius 18-11-21
                    if (number > 0)
                        collisionTiles.Add(new CollisionTiles(number, new Rectangle(x * size, y * size, size, size)));

                    width = (x + 1) * size;
                    height = (y + 1) * size;
                }
        }

        // Ritar ut alla rektanglar | Julius 18-11-21
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (CollisionTiles tile in collisionTiles)
                tile.Draw(spriteBatch);
        }

    }
}
