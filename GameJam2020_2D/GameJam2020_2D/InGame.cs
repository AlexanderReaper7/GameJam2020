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

        TilesMap level1;
        TilesMap level2;
        TilesMap level3;
        TilesMap level4;
        TilesMap level5;
        TilesMap level6;
        TilesMap level7;
        TilesMap level8;


        // Which level to start in // Olle A 200212
        public static Levels Level = Levels.Level1;

        // The different game states // Olle A 200212
        public enum Levels
        {
            Level1,
            Level2,
            Level3,
            Level4,
            Level5,
            Level6,
            Level7,
            Level8,
        };


        public InGame()
        {
            
        }

        public void Initialize()
        {
            level1 = new TilesMap();
            level2 = new TilesMap();
            level3 = new TilesMap();
            level4 = new TilesMap();
            level5 = new TilesMap();
            level6 = new TilesMap();
            level7 = new TilesMap();
            level8 = new TilesMap();
        }

        public void LoadContent(ContentManager content)
        {
            // Load the textures for the Tiles // Olle A 20-02-11
            Tiles.Content = content;

            // Generate levels // Olle A 20-02-11
           level1.Generate(new int[,] {
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

            level2.Generate(new int[,] {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {2,2,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {2,2,1,1,1,1,1,1,1,1,1,1,1,1,1},
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
                level1);
        }

        public void Update(GameTime gameTime)
        {
            // Level specific code // Olle A 200212
            switch (Level)
            {
                case Levels.Level1:
                    // Change level // Olle A 200212
                    player.tileMap = level1;
                    // Update // Olle A 200212
                    player.Update(gameTime);
                    level1.Update();
                    break;

                case Levels.Level2:
                    // Change level // Olle A 200212
                    player.tileMap = level2;
                    // Update // Olle A 200212
                    player.Update(gameTime);
                    level2.Update();
                    break;

                case Levels.Level3:
                    // Change level // Olle A 200212
                    player.tileMap = level3;
                    // Update // Olle A 200212
                    player.Update(gameTime);
                    level3.Update();
                    break;

                case Levels.Level4:
                    // Change level // Olle A 200212
                    player.tileMap = level4;
                    // Update // Olle A 200212
                    player.Update(gameTime);
                    level4.Update();
                    break;

                case Levels.Level5:
                    // Change level // Olle A 200212
                    player.tileMap = level5;
                    // Update // Olle A 200212
                    player.Update(gameTime);
                    level5.Update();
                    break;

                case Levels.Level6:
                    // Change level // Olle A 200212
                    player.tileMap = level6;
                    // Update // Olle A 200212
                    player.Update(gameTime);
                    level6.Update();
                    break;

                case Levels.Level7:
                    // Change level // Olle A 200212
                    player.tileMap = level7;
                    // Update // Olle A 200212
                    player.Update(gameTime);
                    level7.Update();
                    break;

                case Levels.Level8:
                    // Change level // Olle A 200212
                    player.tileMap = level8;
                    // Update // Olle A 200212
                    player.Update(gameTime);
                    level8.Update();
                    break;

                    
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Draw(SpriteBatch spriteBatch ,GameTime gameTime)
        {
            // Level specific code // Olle A 200212
            switch (Level)
            {
                case Levels.Level1:
                    // Draw // Olle A 200212
                    level1.Draw(spriteBatch);
                    player.Draw(spriteBatch);
                    break;

                case Levels.Level2:
                    // Draw // Olle A 200212
                    level2.Draw(spriteBatch);
                    player.Draw(spriteBatch);
                    break;

                case Levels.Level3:
                    // Draw // Olle A 200212
                    level3.Draw(spriteBatch);
                    player.Draw(spriteBatch);
                    break;

                case Levels.Level4:
                    // Draw // Olle A 200212
                    level4.Draw(spriteBatch);
                    player.Draw(spriteBatch);
                    break;

                case Levels.Level5:
                    // Draw // Olle A 200212
                    level5.Draw(spriteBatch);
                    player.Draw(spriteBatch);
                    break;

                case Levels.Level6:
                    // Draw // Olle A 200212
                    level6.Draw(spriteBatch);
                    player.Draw(spriteBatch);
                    break;

                case Levels.Level7:
                    // Draw // Olle A 200212
                    level7.Draw(spriteBatch);
                    player.Draw(spriteBatch);
                    break;

                case Levels.Level8:
                    // Draw // Olle A 200212
                    level8.Draw(spriteBatch);
                    player.Draw(spriteBatch);
                    break;


                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
