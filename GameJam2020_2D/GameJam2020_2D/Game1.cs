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

        MenuManager menuManager = new MenuManager();
        ControlScheme input = new ControlScheme();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            // TODO: Add your initialization logic here
            menuManager.Screen(new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
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

            menuManager.LoadMenues(Content);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            // A small bool to make the full variable a bit smaller
            bool isKeyUp = input.IsKeyUp;
            // Update the variables in control scheme (input)
            input.Update();
            // If player is not in play screen/page, make menu navigation possible
            if (isKeyUp && menuManager.menu.PageSelection != 1)
            {
                menuManager.Navigation(input.Up, input.Down, input.Left, input.Right);
            }


            // A region of all the pages and what each button do
            #region All Menus
            


            // Main Menu
            // Single activation on select key and mouse button
            if (isKeyUp && input.Select || isKeyUp && input.SelectMouse)
            {
                // Check if player select first button in main menu (play)
                if (menuManager.menu.State(0, 0))
                {
                    // Change page to Play screen
                    menuManager.menu.PageSelection = 1;
                    // Start game
                    //initializeGame = true;
                    // Make single activation key reset
                    isKeyUp = false;
                }

                // Highscore
                if (menuManager.menu.State(0, 1))
                {
                    menuManager.menu.PageSelection = 2;
                    isKeyUp = false;
                }

                // Options
                if (menuManager.menu.State(0, 2))
                {
                    menuManager.menu.PageSelection = 3;
                    isKeyUp = false;
                }

                // How to play
                if (menuManager.menu.State(0, 3))
                {
                    menuManager.menu.PageSelection = 4;
                    isKeyUp = false;
                }

                // Credits
                if (menuManager.menu.State(0, 4))
                {
                    menuManager.menu.PageSelection = 5;
                    isKeyUp = false;
                }

                // Exit
                if (menuManager.menu.State(0, 5))
                {
                    // Exit the game
                    this.Exit();
                }

            }

            //// Play
            //// If pause (Back) button is activated, or if mouse presses on pause button in the left top corner
            //if (isKeyUp && input.Back || isKeyUp && input.SelectMouse && input.MousePointer.Intersects(pauseButton))
            //{
            //    //// Only if game is active (you cannot pause when the player see a countdown)
            //    //if (game.GameActive)
            //    //{
            //    //    // Move to pause menu
            //    //    menuManager.menu.PageSelection = 7;
            //    //    // Pause game
            //    //    game.PauseGame();
            //    //    // Reset single activation key
            //    //    isKeyUp = false;

            //    //}

            //}

            // HighScore
            if (isKeyUp && input.Select || isKeyUp && input.SelectMouse)
            {
                // Back
                if (menuManager.menu.State(2, 0))
                {
                    menuManager.menu.PageSelection = 0;
                    isKeyUp = false;
                }

            }
            // If Left shift and R is pressed while in Highscore page, clean/reset the score board
            if (menuManager.menu.PageSelection == 2 && Keyboard.GetState().IsKeyDown(Keys.LeftShift) && Keyboard.GetState().IsKeyDown(Keys.R))
            {
                //saveFile.ResetHighScore();
            }

            // Options (Select with input.select)
            if (isKeyUp && input.Select || isKeyUp && input.SelectMouse)
            {
                // Controls
                if (menuManager.menu.State(3, 0))
                {
                    // If button gets pressed (with selection key/mouse button), change button state, loop it so that it doesn't get stuck
                    menuManager.menu.Pages[3].Buttons[0].SelectRight(true);
                    // Reset single activation key
                    isKeyUp = false;
                }

                // Speed
                if (menuManager.menu.State(3, 1))
                {
                    // If button gets pressed (with selection key/mouse button), add 0.1f speed
                    //options.Speed += 0.1f;
                    //// If it gets over 2f, change it to 0.2f
                    //if (options.Speed > 2.01f)
                    //{
                    //    options.Speed = 0.2f;
                    //}
                    // Reset single activation key
                    isKeyUp = false;
                }

                // Fullscreen
                if (menuManager.menu.State(3, 2))
                {
                    // Toggle fullscreen
                    graphics.ToggleFullScreen();
                    // Reset single activation key
                    isKeyUp = false;
                }

                // Back
                if (menuManager.menu.State(3, 3))
                {
                menuManager.menu.PageSelection = 0;
                    isKeyUp = false;
                }

            }

            // Options Change State with arrows
            if (isKeyUp)
            {
                // Control Settings
                // Checks what state the button is in (Switching button state with left/right keys is in menu.navigation method)
                if (menuManager.menu.State(3, 0, 0))
                {
                    // If button state = 0, make options hold false
                    //options.ControlsHold = false;
                }
                if (menuManager.menu.State(3, 0, 1))
                {
                    // If button state = 1, make options hold true
                    //options.ControlsHold = true;
                }

                // Speed Settings
                // Modified button
                // Checks if button is selected
                if (menuManager.menu.State(3, 1))
                {
                    // If left is pressed and is more than 0.2f
                    //if (input.Left && options.Speed > 0.2f)
                    //    // Subtract speed with 0.1f
                    //    options.Speed -= 0.1f;

                    // If right is pressed and is less than 2f
                    //if (input.Right && options.Speed < 2f)
                    //    // Add speed with 0.1f
                    //    options.Speed += 0.1f;
                }


            }

            // HowToPlay
            if (isKeyUp && input.Select || isKeyUp && input.SelectMouse)
            {
                // Back
                if (menuManager.menu.State(4, 0))
                {
                    menuManager.menu.PageSelection = 0;
                    isKeyUp = false;
                }

            }

            // Credits
            if (isKeyUp && input.Select || isKeyUp && input.SelectMouse)
            {
                // Back
                if (menuManager.menu.State(5, 0))
                {
                    menuManager.menu.PageSelection = 0;
                    isKeyUp = false;
                }

            }

            // GameOver
            if (isKeyUp && input.Select || isKeyUp && input.SelectMouse)
            {
                // Highscore
                if (menuManager.menu.State(6, 0))
                {
                    // Move to highscore page
                    menuManager.menu.PageSelection = 2;
                    // Reset game after game over
                    //game.ResetGame();
                    // Reset single activation key
                    isKeyUp = false;
                }

                // Back
                if (menuManager.menu.State(6, 1))
                {
                    // Move to main menu
                    menuManager.menu.PageSelection = 0;
                    // Reset game after game over
                    //game.ResetGame();
                    // Reset single activation key
                    isKeyUp = false;
                }

            }

            // Pause
            if (isKeyUp && input.Select || isKeyUp && input.SelectMouse)
            {
                // Resume
                if (menuManager.menu.State(7, 0))
                {
                    // Move back to play screen
                    menuManager.menu.PageSelection = 1;
                    // Start game again
                    //initializeGame = true;
                    // Reset single activation key
                    isKeyUp = false;
                }

                // Reset
                if (menuManager.menu.State(7, 1))
                {
                    // Move back to play screen
                    menuManager.menu.PageSelection = 1;
                    // Reset game
                    //game.ResetGame();
                    // Start game again
                    //initializeGame = true;
                    // Reset single activation key
                    isKeyUp = false;
                }

                // Back to Menu
                if (menuManager.menu.State(7, 2))
                {
                    // Move back to main menu
                    menuManager.menu.PageSelection = 0;
                    // Reset single activation key
                    isKeyUp = false;
                }

                // Exit
                if (menuManager.menu.State(7, 3))
                {
                    this.Exit();
                }

            }

            #endregion



            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            menuManager.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
