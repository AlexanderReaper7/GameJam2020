using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Tools_XNA;

namespace GameJam2020_2D
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static MenuManager menuManager;
        private InGame inGame;
        public static GameStates gameState;
        Highscore scoreBoard;
        /// <summary>
        /// The nameselect class used to allow a player to create their own name and then store it for use regarding the high score 
        /// </summary>
        NameSelect nameSelect = new NameSelect();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferMultiSampling = true;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            scoreBoard = new Highscore();
            scoreBoard.Initialize();
            menuManager = new MenuManager(this, graphics, scoreBoard);
            menuManager.gameStates = GameStates.Menu;
            inGame = new InGame(scoreBoard, menuManager, graphics);
            inGame.Initialize();

            Window.Title = "Weird tiles in space";

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            SoundManager.LoadContent(Content, GraphicsDevice);
            menuManager.LoadMenues(Content);
            inGame.LoadContent(Content);
            nameSelect.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                menuManager.ChangePage(MenuManager.MenuState.Main);
                menuManager.gameStates = GameStates.Menu;
                inGame.player.lifes = 6;
                inGame.player.playerAlive = true;
                inGame.player.ResetGame();
             }
            // Toggle fullscreen when pressing F
            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                graphics.IsFullScreen = !graphics.IsFullScreen;
                graphics.ApplyChanges();
            }

            switch (menuManager.gameStates)
            {
                case GameStates.Menu:
                    menuManager.Update(gameTime);
                    break;
                case GameStates.Game:
                    inGame.Update(gameTime);
                    if (inGame.player.playerAlive == false)
                    {
                        SoundManager.Hurt.Play();
                        if (inGame.player.lifes <= 1)
                        {
                            menuManager.ChangePage(MenuManager.MenuState.GameOver);
                            menuManager.gameStates = GameStates.Menu;
                            inGame.player.lifes = 6;
                        }
                        inGame.player.playerAlive = true;
                        inGame.player.ResetGame();
                    }
                    break;
                case GameStates.PreLevel1:
                    InGame.Level = InGame.Levels.preLevel1;
                    InGame.Timer = 0;
                    menuManager.gameStates = GameStates.Game;
                    break;
                case GameStates.PreLevel2:
                    InGame.Level = InGame.Levels.preLevel1;
                    // Set timer to 999999 which disables it. // Olle A 20-04-18
                    InGame.Timer = 999999;
                    menuManager.gameStates = GameStates.Game;
                    break;
                case GameStates.PreLevel3:
                    InGame.Level = InGame.Levels.preLevel3;
                    // Set timer to 999999 which disables it. // Olle A 20-04-18
                    InGame.Timer = 999999;
                    menuManager.gameStates = GameStates.Game;
                    break;
                case GameStates.PreLevel4:
                    InGame.Level = InGame.Levels.preLevel4;
                    // Set timer to 999999 which disables it. // Olle A 20-04-18
                    InGame.Timer = 999999;
                    menuManager.gameStates = GameStates.Game;
                    break;
                case GameStates.PreLevel5:
                    InGame.Level = InGame.Levels.preLevel5;
                    // Set timer to 999999 which disables it. // Olle A 20-04-18
                    InGame.Timer = 999999;
                    menuManager.gameStates = GameStates.Game;
                    break;
                case GameStates.PreLevel6:
                    InGame.Level = InGame.Levels.preLevel6;
                    // Set timer to 999999 which disables it. // Olle A 20-04-18
                    InGame.Timer = 999999;
                    menuManager.gameStates = GameStates.Game;
                    break;
                case GameStates.PreLevel7:
                    InGame.Level = InGame.Levels.preLevel7;
                    // Set timer to 999999 which disables it. // Olle A 20-04-18
                    InGame.Timer = 999999;
                    menuManager.gameStates = GameStates.Game;
                    break;
                case GameStates.PreLevel8:
                    InGame.Level = InGame.Levels.preLevel8;
                    // Set timer to 999999 which disables it. // Olle A 20-04-18
                    InGame.Timer = 999999;
                    menuManager.gameStates = GameStates.Game;
                    break;
                //case GameStates.InsertName:
                //    // Updates the various code in the nameselect class 
                //    nameSelect.Update(gameTime);
                //    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            //if (menuManager.menuState == MenuManager.MenuState.GameOver || menuManager.menuState == MenuManager.MenuState.Victory)
            //    inGame.player.ResetGame();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            switch (menuManager.gameStates)
            {
                case GameStates.Menu:
                    menuManager.Draw(spriteBatch);
                    break;
                case GameStates.Game:
                    inGame.Draw(spriteBatch, gameTime);
                    if (MenuManager.exclusiveBool) InGame.Level = InGame.Levels.preLevel1;
                    MenuManager.exclusiveBool = false;
                    break;
                //case GameStates.InsertName:
                //    nameSelect.Draw(spriteBatch);
                //    break;
                default:
                    break;
                    //throw new ArgumentOutOfRangeException();
            }
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
