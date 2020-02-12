using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.PeerToPeer.Collaboration;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Tools_XNA;
using static Tools_XNA.Tiles;

namespace GameJam2020_2D
{
    public class InGame
    {
        Player player;
        TilesMap testLevel;

        public InGame()
        {
            
        }

        public void Initialize()
        {
            testLevel = new TilesMap();
        }

        public void LoadContent(ContentManager content)
        {
            // Load the textures for the Tiles // Olle A 20-02-11
            Tiles.Content = content;

            // Generates a map of tiles (prototype) // Olle A 20-02-11
            testLevel.Generate(new int[,] {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,0,0,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,0,0,1,1,1,1,1,1,1},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,1,0,2,0,0,0,0,0,1,0,0,0,0,0},
                {0,1,0,2,2,0,0,0,1,2,2,2,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},

            }, 12, 50, 100, 200);

            // Load player // Olle A 20-02-11
            player = new Player(
                content.Load<Texture2D>("Textures/Player/MageSpriteBackViewLeft"),
                content.Load<Texture2D>("Textures/Player/MageSpriteFaceViewRight"),
                content.Load<Texture2D>("Textures/Player/MageSpriteFaceViewLeft"),
                content.Load<Texture2D>("Textures/Player/MageSpriteBackViewRight"),
                testLevel);
        }

        public void Update(GameTime gameTime)
        {
            // Update player // Olle A 200212
            player.Update(gameTime);
            // Check collisions against tiles // Olle A 200211
            testLevel.Update();
        }

        public void Draw(SpriteBatch spriteBatch ,GameTime gameTime)
        {
            // Draw level // Olle A 200211
            testLevel.Draw(spriteBatch);
            // Draw player // Olle A 200211
            player.Draw(spriteBatch);
        }
    }
}
