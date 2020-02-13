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
            preLevel1,
            Level1,
            preLevel2,
            Level2,
            preLevel3,
            Level3,
            preLevel4,
            Level4,
            preLevel5,
            Level5,
            preLevel6,
            Level6,
            preLevel7,
            Level7,
            preLevel8,
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


            // Olle A 20-02-13
            /// HOW TILE IDS WORK:
            /// IDs consist of 3 numbers "XXX"
            /// 
            /// First number designates theme.
            ///     1XX: Sci-fi
            ///     2XX: Fantasy
            /// 
            /// The other two numbers represent the type of tile:
            ///     X01: Ground
            ///     X02: Wall
            ///     X03: Wall Mirrored
            ///     X04: End portal
            ///     X05: Dispenser
            ///     X06: Door open
            ///     X07: Door closed
            ///     X08: Portal
            ///     X09: Trap door closed
            ///     X10: Trap door open
            ///     
            /// Air (no tile) is "000" regardless of theme.
            /// 
            /// Examples:
            /// 101 = Ground tile in sci-fi theme
            /// 201 = Ground tile in fantasy theme
            /// 104 = End portal in sci-fi theme
            /// 204 = End portal in fantasy theme

            // Generate levels // Olle A 20-02-11
            level1.Generate(new int[,] {
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
                {101,101,101,101,101,101,101,101,101,101,101,101,101,101,101},
                {101,101,101,101,101,101,101,101,101,101,101,101,101,101,101},
                {101,101,101,101,101,101,101,101,101,101,101,101,101,101,101},
                {101,101,101,101,101,101,101,101,101,101,101,101,101,101,101},
                {101,101,101,101,101,101,101,101,101,101,101,101,101,101,101},
                {101,101,101,101,101,101,101,101,101,101,101,101,101,101,101},
                {101,101,101,101,104,101,000,000,101,101,101,101,101,101,101},
                {101,101,101,101,101,101,000,000,101,101,101,101,101,101,101},
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
                {000,101,000,102,000,000,000,000,000,101,000,000,000,000,000},
                {000,101,000,102,102,000,000,000,101,102,102,102,000,000,000},
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
            }, 1, 12, 50, 100, 200);

            level2.Generate(new int[,] {
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
                {201,201,201,201,201,201,201,201,201,201,201,201,201,201,201},
                {201,201,201,201,201,201,201,201,201,201,201,201,201,201,201},
                {201,201,201,201,201,201,201,201,201,201,201,201,201,201,201},
                {201,201,201,201,201,201,201,201,201,201,201,201,201,201,201},
                {201,201,201,201,201,201,201,201,201,201,201,201,201,201,201},
                {201,201,201,201,201,201,201,201,201,201,201,201,201,201,201},
                {201,201,201,201,201,201,000,000,201,201,201,201,201,201,201},
                {201,201,201,201,201,201,000,000,201,201,201,201,201,201,201},
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
                {000,201,000,202,000,000,000,000,000,201,000,000,000,000,000},
                {000,201,000,202,202,000,000,000,201,202,202,202,000,000,000},
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
            }, 200, 12, 50, 100, 200);

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
                case Levels.preLevel1:
                    // Reset level // Olle A 200213
                    level1.Reset();
                    // Change level // Olle A 200212
                    player.tileMap = level1;
                    // Move to next gamestate // Olle A 200213
                    Level++;
                    break;
                case Levels.Level1:
                    // Update // Olle A 200212
                    player.Update(gameTime);
                    level1.Update();
                    break;

                case Levels.preLevel2:
                    // Reset level // Olle A 200213
                    level2.Reset();
                    // Change level // Olle A 200212
                    player.tileMap = level2;
                    // Move to next gamestate // Olle A 200213
                    Level++;
                    break;
                case Levels.Level2:
                    // Update // Olle A 200212
                    player.Update(gameTime);
                    level2.Update();
                    break;

                case Levels.preLevel3:
                    // Reset level // Olle A 200213
                    level3.Reset();
                    // Change level // Olle A 200212
                    player.tileMap = level3;
                    // Move to next gamestate // Olle A 200213
                    Level++;
                    break;
                case Levels.Level3:
                    // Change level // Olle A 200212
                    player.tileMap = level3;
                    // Update // Olle A 200212
                    player.Update(gameTime);
                    level3.Update();
                    break;

                case Levels.preLevel4:
                    // Reset level // Olle A 200213
                    level4.Reset();
                    // Change level // Olle A 200212
                    player.tileMap = level4;
                    // Move to next gamestate // Olle A 200213
                    Level++;
                    break;
                case Levels.Level4:
                    // Change level // Olle A 200212
                    player.tileMap = level4;
                    // Update // Olle A 200212
                    player.Update(gameTime);
                    level4.Update();
                    break;

                case Levels.preLevel5:
                    // Reset level // Olle A 200213
                    level5.Reset();
                    // Change level // Olle A 200212
                    player.tileMap = level5;
                    // Move to next gamestate // Olle A 200213
                    Level++;
                    break;
                case Levels.Level5:
                    // Change level // Olle A 200212
                    player.tileMap = level5;
                    // Update // Olle A 200212
                    player.Update(gameTime);
                    level5.Update();
                    break;

                case Levels.preLevel6:
                    // Reset level // Olle A 200213
                    level6.Reset();
                    // Change level // Olle A 200212
                    player.tileMap = level6;
                    // Move to next gamestate // Olle A 200213
                    Level++;
                    break;
                case Levels.Level6:
                    // Change level // Olle A 200212
                    player.tileMap = level6;
                    // Update // Olle A 200212
                    player.Update(gameTime);
                    level6.Update();
                    break;

                case Levels.preLevel7:
                    // Reset level // Olle A 200213
                    level7.Reset();
                    // Change level // Olle A 200212
                    player.tileMap = level7;
                    // Move to next gamestate // Olle A 200213
                    Level++;
                    break;
                case Levels.Level7:
                    // Change level // Olle A 200212
                    player.tileMap = level7;
                    // Update // Olle A 200212
                    player.Update(gameTime);
                    level7.Update();
                    break;

                case Levels.preLevel8:
                    // Reset level // Olle A 200213
                    level8.Reset();
                    // Change level // Olle A 200212
                    player.tileMap = level8;
                    // Move to next gamestate // Olle A 200213
                    Level++;
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
                    //throw new ArgumentOutOfRangeException();
                    break;
            }
        }
    }
}
