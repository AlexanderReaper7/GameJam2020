// Created this class | Julius 18-11-21

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using static Tools_XNA.Tiles;
using Microsoft.Xna.Framework.Content;

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
        // List without any tiles removed. Used to reset the level // Olle A 200213
        private List<CollisionTiles> collisionTilesUntouched = new List<CollisionTiles>();

        private int width, height;
        public int Width
        {
            get { return width; }
        }
        public int Height
        {
            get { return height; }
        }

        static SpriteFont font;
        public int LevelNumber;

        // Olle A 20-02-12
        public int TileMapWidth;
        public int Size;
        public int StartingPosition;

        // Timer used to time shots // Olle A 20-03-17
        private float shotTimer = 3;
        private float shotTimerMax = 5f;
        public List<Projectile> Projectiles = new List<Projectile>();
        private Texture2D shotTexture;

        // En oanvänd construktor | Julius 18-11-26
        public TilesMap() { }

        //Load Content
        public static void LoadContent(ContentManager Content)
        {
            font = Content.Load<SpriteFont>(@"Shared/Fonts/testfont");
        }

        // Update
        public void Update(GameTime gameTime)
        {
            // Remove tile (make it air) if player has walked on it but is no longer standing on it // Olle A 200212
            for (int i = 0; i < collisionTiles.Count; i++)
            {
                if (collisionTiles[i].HasBeenWalkedOn && !collisionTiles[i].IsOnTile)
                {
                    collisionTiles[i].Type = 0;
                }

                // Do not proceed unless it is time for dispensers to shoot // Olle A 20-03-17
                if (shotTimer > 5f)
                {
                    // Create the projectile // Olle A 20-03-24
                    Projectile projectile;

                    // Shoot projectiles with direction depending on dispenser type // Olle A 20-03-17
                    switch (collisionTiles[i].Type)
                    {
                        // Up
                        case 150:
                        case 250:
                            projectile = new Projectile(0.01f, i, "up", this, shotTexture);
                            Projectiles.Add(projectile);
                            break;
                        // Down
                        case 151:
                        case 251:
                            projectile = new Projectile(0.01f, i, "down", this, shotTexture);
                            Projectiles.Add(projectile);
                            break;
                        // Left
                        case 152:
                        case 252:
                            projectile = new Projectile(0.01f, i, "left", this, shotTexture);
                            Projectiles.Add(projectile);
                            break;
                        // Right
                        case 153:
                        case 253:
                            projectile = new Projectile(0.01f, i, "right", this, shotTexture);
                            Projectiles.Add(projectile);
                            break;
                    }
               }

                // Update all bullets // Olle A 20-03-24
                foreach (Projectile p in Projectiles)
                {
                    // Remove bullet if it's colliding with something that isn't clear tile // Olle A 20-04-17
                    if (collisionTiles[p.TilePosition].Type != 000 &&
                        collisionTiles[p.TilePosition].Type != 101 &&
                        collisionTiles[p.TilePosition].Type != 201 &&
                        collisionTiles[p.TilePosition].Type != 115 &&
                        collisionTiles[p.TilePosition].Type != 215 &&
                        collisionTiles[p.TilePosition].Type != 107 &&
                        collisionTiles[p.TilePosition].Type != 207)
                    {
                        Projectiles.Remove(p);
                        break;
                    }
                    p.Update(gameTime);
                }
            }

            // Reset timer // Olle A 20-03-24
            if (shotTimer > shotTimerMax)
            {
                shotTimer = 0;
            }

                // Count up
            shotTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        // En kartgenererare där man bestämmer vart och vilken typ av tile som finns i en array (map) och pixelstorleken på tilens sida (size) | Julius 18-11-21
        public void Generate(int[,] map, int StartingPosition, int tileMapWidth, int size, int xOffset, int yOffset, int LevelNumber, Texture2D shotTexture)
        {
            TileMapWidth = tileMapWidth;
            this.StartingPosition = StartingPosition;
            Size = size;
            this.shotTexture = shotTexture;

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

            // Save a backup of the list so that it can be restored later // Olle A 200213
            collisionTilesUntouched = collisionTiles.ConvertAll(x => new CollisionTiles(x.Type, x.Rectangle));
        }

        // Ritar ut alla rektanglar | Julius 18-11-21
        public void Draw(SpriteBatch spriteBatch)
        {

            foreach (CollisionTiles tile in collisionTiles)
                tile.Draw(spriteBatch);

            // Draw all bulletst // Olle A 20-03-24
            foreach (Projectile projectile in Projectiles)
            {
                projectile.Draw(spriteBatch);
            }

        }

        /// <summary>
        /// Resets the tile map to an unedited state // Olle A 200213
        /// Also clear projectiles // Olle A 200324
        /// </summary>
        public void Reset()
        {
            collisionTiles = collisionTilesUntouched.ConvertAll(x => new CollisionTiles(x.Type, x.Rectangle));
            Projectiles.Clear();
        }

    }
}
