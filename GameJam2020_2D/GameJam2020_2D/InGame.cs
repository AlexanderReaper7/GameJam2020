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
        public Player player;
        public static string playerName = "AAA";

        TilesMap level1;
        TilesMap level2;
        TilesMap level3;
        TilesMap level4;
        TilesMap level5;
        TilesMap level6;
        TilesMap level7;
        TilesMap level8;

        private Texture2D compassTexture;
        GraphicsDeviceManager graphics;

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
            Win,
        };

        Highscore scoreboard;
        MenuManager menu;

        public InGame(Highscore scoreboard, MenuManager menu, GraphicsDeviceManager graphics)
        {
            this.scoreboard = scoreboard;
            this.menu = menu;
            this.graphics = graphics;
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

            // Set font for timer. Static so only needs to be set for one level
            TilesMap.LoadContent(content);

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
            ///     X50-X53: Dispenser
            ///     X06: Door open
            ///     X07: Door closed
            ///     X08: Portal
            ///     X09: Trap door closed
            ///     X10: Trap door open
            ///     X11: Keys
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
                {101,000,101,101,101,000,000,000,000,000,000,000,000,000,000},
                {101,000,101,000,101,000,000,000,000,000,000,000,000,000,000},
                {101,101,101,103,101,000,000,000,000,000,000,000,000,000,000},
                {101,000,000,000,101,101,000,000,000,000,000,000,000,000,000},
                {000,000,000,000,000,101,101,101,104,000,000,000,000,000,000},
                {000,000,000,000,000,000,101,000,000,000,000,000,000,000,000},
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
            }, 168, 12, 100, 100, 200, 1,
            content.Load<Texture2D>("Textures/projectile"));

            level2.Generate(new int[,] {
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
                {201,201,201,000,000,000,000,000,000,000,000,000,000,000,000},
                {201,201,201,201,201,000,000,000,000,000,000,000,000,000,000},
                {201,201,201,201,201,206,201,000,000,000,000,000,000,000,000},
                {000,201,201,000,000,201,201,201,201,000,000,000,000,000,000},
                {000,201,201,000,000,201,201,000,201,201,204,000,000,000,000},
                {000,201,201,000,000,201,201,000,201,000,000,000,000,000,000},
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
            }, 168, 12, 110, 120, 270, 2,
            content.Load<Texture2D>("Textures/projectile"));

            level3.Generate(new int[,] {
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
                {101,101,000,000,101,101,101,000,000,104,000,000,000,000,000},
                {000,101,000,000,106,000,101,152,000,101,000,000,000,000,000},
                {152,101,101,101,101,000,101,000,000,101,101,101,000,000,000},
                {000,101,000,000,101,000,101,152,000,000,000,101,101,101,000},
                {000,101,000,000,101,000,101,000,000,101,101,101,000,101,000},
                {101,101,000,101,101,000,101,000,000,101,000,101,000,101,152},
                {101,000,000,101,000,000,101,101,101,101,000,101,101,101,000},
                {101,101,101,101,000,000,101,000,000,101,000,000,000,000,000},
                {000,000,151,000,000,000,101,101,101,101,000,000,000,000,000},
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
            }, 168, 12, 90, 120, 290, 3,
            content.Load<Texture2D>("Textures/projectile"));

            level4.Generate(new int[,] {
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
                {201,000,000,000,203,000,000,000,000,208,000,000,000,000,000},
                {201,000,000,000,203,000,000,000,000,201,000,000,000,000,000},
                {201,201,208,000,000,208,000,201,000,201,000,000,000,000,000},
                {000,202,202,202,000,201,000,201,203,201,000,000,000,000,000},
                {201,201,201,203,201,201,000,201,203,201,206,201,204,000,000},
                {000,000,000,203,201,201,000,201,203,201,000,000,000,000,000},
                {000,000,000,000,201,201,208,201,000,201,000,000,000,000,000},
                {000,000,000,000,251,000,000,000,000,000,000,000,000,000,000},
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
            }, 168, 12, 110, 120, 270, 4,
            content.Load<Texture2D>("Textures/projectile"));

            level5.Generate(new int[,] {
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
                {101,103,108,101,103,150,101,101,101,000,000,103,000,000,000},
                {101,103,103,101,101,101,101,101,101,101,108,103,000,000,000},
                {101,108,103,101,101,000,151,101,101,000,000,103,000,000,000},
                {101,000,103,102,102,102,102,102,102,102,102,102,000,000,000},
                {101,108,103,108,000,101,101,101,101,000,000,000,000,000,000},
                {000,000,103,101,153,101,101,101,101,000,000,000,000,000,000},
                {101,108,103,101,101,101,101,000,101,108,000,000,000,000,000},
                {104,000,103,000,151,000,000,000,000,000,000,000,000,000,000},
                {101,108,103,000,000,000,000,000,000,000,000,000,000,000,000},
                {102,102,103,000,000,000,000,000,000,000,000,000,000,000,000},
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
            }, 168, 12, 110, 120, 270, 5,
            content.Load<Texture2D>("Textures/projectile"));

            level6.Generate(new int[,] {
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
                {201,201,201,201,000,000,000,000,000,250,000,000,000,000,000},
                {000,201,000,201,253,201,201,201,201,201,201,201,201,000,000},
                {000,201,201,201,000,201,000,000,201,000,201,000,201,000,000},
                {000,201,252,000,000,201,000,000,208,201,201,201,201,000,000},
                {000,201,000,000,201,201,201,000,000,000,000,000,000,000,000},
                {000,208,000,000,201,000,201,252,208,000,000,000,000,000,000},
                {000,000,000,000,201,201,201,252,201,000,000,000,000,000,000},
                {000,000,000,000,000,208,000,000,201,000,000,000,000,000,000},
                {000,000,000,000,000,000,000,253,201,000,000,000,000,000,000},
                {000,000,000,000,000,000,000,000,201,000,000,000,000,000,000},
                {000,000,000,000,000,000,000,253,201,000,000,000,000,000,000},
                {000,000,000,000,000,000,000,000,204,000,000,000,000,000,000},
            }, 168, 12, 90, 120, 270, 6,
            content.Load<Texture2D>("Textures/projectile"));

            level7.Generate(new int[,] {
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
                {000,000,000,000,101,101,101,101,000,000,000,000,000,000,000},
                {000,101,108,000,106,102,000,101,152,000,000,000,000,000,000},
                {101,101,101,000,101,101,000,101,152,108,101,000,000,000,000},
                {101,101,101,101,101,101,000,108,000,101,101,000,000,000,000},
                {000,101,000,000,000,000,000,000,000,101,101,000,000,000,000},
                {000,000,000,000,000,000,000,000,101,101,101,101,000,000,000},
                {101,101,108,000,000,000,000,153,101,101,101,101,000,000,000},
                {000,000,000,000,000,000,000,000,000,151,000,101,000,000,000},
                {104,108,000,000,000,000,000,000,000,000,000,106,000,000,000},
                {000,000,000,000,000,000,000,000,000,000,000,101,000,000,000},
                {000,000,000,000,000,000,000,000,000,000,000,108,000,000,000},
                {000,000,000,000,000,000,000,000,000,000,000,000,000,000,000},
            }, 174, 12, 90, 120, 270, 7,
            content.Load<Texture2D>("Textures/projectile"));

            level8.Generate(new int[,] {
                {000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000},
                {000, 000, 000, 000, 000, 000, 000, 000, 201, 214, 206, 214, 206, 214, 201, 201, 000, 000},
                {000, 000, 000, 000, 000, 201, 201, 000, 201, 214, 201, 214, 201, 214, 201, 201, 201, 000},
                {000, 000, 000, 000, 000, 000, 201, 000, 201, 214, 201, 214, 201, 214, 206, 201, 201, 000},
                {000, 000, 000, 000, 201, 201, 201, 206, 201, 214, 206, 214, 201, 214, 201, 201, 201, 000},
                {000, 000, 000, 000, 000, 201, 000, 201, 000, 000, 000, 000, 000, 000, 201, 204, 201, 000},
                {000, 250, 250, 250, 000, 212, 000, 213, 000, 000, 000, 000, 000, 000, 253, 201, 201, 000},
                {253, 201, 201, 201, 201, 201, 201, 201, 201, 201, 201, 000, 000, 000, 253, 201, 201, 000},
                {000, 201, 201, 201, 201, 201, 201, 201, 000, 201, 201, 000, 000, 000, 000, 251, 000, 000},
                {253, 201, 201, 201, 201, 201, 201, 201, 201, 000, 201, 000, 000, 000, 000, 000, 000, 000},
                {000, 000, 251, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000},
                {000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000},
                {000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000},
            }, 159, 12, 80, 100, 250, 8,
            content.Load<Texture2D>("Textures/projectile"));

            // Load player // Olle A 20-02-11
            player = new Player(
                content.Load<Texture2D>("Textures/Player/MageSpriteBackViewLeft"),
                content.Load<Texture2D>("Textures/Player/MageSpriteFaceViewRight"),
                content.Load<Texture2D>("Textures/Player/MageSpriteFaceViewLeft"),
                content.Load<Texture2D>("Textures/Player/MageSpriteBackViewRight"),
                level1, scoreboard, playerName);

            compassTexture = content.Load<Texture2D>("Textures/compass");
        }

        public void Update(GameTime gameTime)
        {

            // Level specific code // Olle A 200212
            switch (Level)
            {
                case Levels.preLevel1:
                    // Reset level // Olle A 200213
                    level1.Reset();
                    // Update player's level // Olle A 200212
                    player.NewLevel(level1);
                    // Move to next gamestate // Olle A 200213
                    Level++;
                    break;
                case Levels.Level1:
                    // Update // Olle A 200212
                    player.Update(gameTime);
                    level1.Update(gameTime);
                    break;

                case Levels.preLevel2:
                    // Reset level // Olle A 200213
                    level2.Reset();
                    // Update player's level // Olle A 200212
                    player.NewLevel(level2);
                    // Move to next gamestate // Olle A 200213
                    Level++;
                    break;
                case Levels.Level2:
                    // Update // Olle A 200212
                    player.Update(gameTime);
                    level2.Update(gameTime);
                    break;

                case Levels.preLevel3:
                    // Reset level // Olle A 200213
                    level3.Reset();
                    // Update player's level // Olle A 200212
                    player.NewLevel(level3);
                    // Move to next gamestate // Olle A 200213
                    Level++;
                    break;
                case Levels.Level3:
                    // Update // Olle A 200212
                    player.Update(gameTime);
                    level3.Update(gameTime);
                    break;

                case Levels.preLevel4:
                    // Reset level // Olle A 200213
                    level4.Reset();
                    // Update player's level // Olle A 200213
                    player.NewLevel(level4);
                    // Move to next gamestate // Olle A 200213
                    Level++;
                    break;
                case Levels.Level4:
                    // Update // Olle A 200213
                    player.Update(gameTime);
                    level4.Update(gameTime);
                    break;

                case Levels.preLevel5:
                    // Reset level // Olle A 200213
                    level5.Reset();
                    // Update player's level // Olle A 200213
                    player.NewLevel(level5);
                    // Move to next gamestate // Olle A 200213
                    Level++;
                    break;
                case Levels.Level5:
                    // Update // Olle A 200252
                    player.Update(gameTime);
                    level5.Update(gameTime);
                    break;

                case Levels.preLevel6:
                    // Reset level // Olle A 200213
                    level6.Reset();
                    // Update player's level // Olle A 200213
                    player.NewLevel(level6);
                    // Move to next gamestate // Olle A 200213
                    Level++;
                    break;
                case Levels.Level6:
                    // Update // Olle A 200213
                    player.Update(gameTime);
                    level6.Update(gameTime);
                    break;

                case Levels.preLevel7:
                    // Reset level // Olle A 200213
                    level7.Reset();
                    // Update player's level // Olle A 200213
                    player.NewLevel(level7);
                    // Move to next gamestate // Olle A 200213
                    Level++;
                    break;
                case Levels.Level7:
                    // Update // Olle A 200213
                    player.Update(gameTime);
                    level7.Update(gameTime);
                    break;

                case Levels.preLevel8:
                    // Reset level // Olle A 200213
                    level8.Reset();
                    // Update player's level // Olle A 200213
                    player.NewLevel(level8);
                    // Move to next gamestate // Olle A 200213
                    Level++;
                    break;
                case Levels.Level8:
                    // Update // Olle A 200213
                    player.Update(gameTime);
                    level8.Update(gameTime);
                    break;
                case Levels.Win:
                    menu.gameStates = GameStates.Menu;
                    menu.menu.PageSelection = (int)MenuManager.MenuState.Victory;
                    player.ResetGame();
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

            // Draw compass
            spriteBatch.Draw(compassTexture, new Rectangle(graphics.PreferredBackBufferWidth - compassTexture.Width, graphics.PreferredBackBufferHeight- compassTexture.Height, compassTexture.Width, compassTexture.Height), Color.White);
        }
    }
}
