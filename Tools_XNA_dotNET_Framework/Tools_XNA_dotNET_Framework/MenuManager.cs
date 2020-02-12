using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Tools_XNA
{
    public class MenuManager
    {
        public Menu menu = new Menu(8);

        SpriteFont menuFont, textFont, scoreBoardFont;
        Texture2D defaultBackground;
        private ControlScheme input;
        private Game game;
        private GraphicsDeviceManager graphics;

        int screenWidth;
        int screenHeight;
        Rectangle screenSize;

        public MenuManager(Game game, GraphicsDeviceManager graphics)
        {
            this.game = game;
            this.graphics = graphics;

            screenSize = graphics.GraphicsDevice.Viewport.Bounds;
            screenWidth = graphics.GraphicsDevice.Viewport.Width;
            screenHeight = graphics.GraphicsDevice.Viewport.Height;
            input = new ControlScheme();
        }

        public void Update()
        {
            // A small bool to make the full variable a bit smaller
            bool isKeyUp = input.IsKeyUp;
            // Update the variables in control scheme (input)
            input.Update();
            // If player is not in play screen/page, make menu navigation possible
            if (isKeyUp && menu.PageSelection != 1)
            {
                Navigation(input.Up, input.Down, input.Left, input.Right);
            }


            // A region of all the pages and what each button do


            // Main Menu
            // Single activation on select key and mouse button
            if (isKeyUp && input.Select || isKeyUp && input.SelectMouse)
            {

                menu.Pages[menu.PageSelection].Buttons[menu.Pages[menu.PageSelection].ButtonSelection].Run();
                isKeyUp = false;







                // Check if player select first button in main menu (play)
                if (menu.State(0, 0))
                {
                    // Change page to Play screen
                    menu.PageSelection = 1;
                    // Start game
                    

                    // Make single activation key reset
                    
                }

                // Highscore
                if (menu.State(0, 1))
                {
                    menu.PageSelection = 2;
                }

                // Options
                if (menu.State(0, 2))
                {
                    menu.PageSelection = 3;
                }

                // How to play
                if (menu.State(0, 3))
                {
                    menu.PageSelection = 4;
                }

                // Credits
                if (menu.State(0, 4))
                {
                    menu.PageSelection = 5;
                }

                // Exit
                if (menu.State(0, 5))
                {
                    // Exit the game
                    game.Exit();
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
            //    //    menu.PageSelection = 7;
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
                if (menu.State(2, 0))
                {
                    menu.PageSelection = 0;
                }
                isKeyUp = false;

            }
            // If Left shift and R is pressed while in Highscore page, clean/reset the score board
            if (menu.PageSelection == 2 && Keyboard.GetState().IsKeyDown(Keys.LeftShift) && Keyboard.GetState().IsKeyDown(Keys.R))
            {
                //saveFile.ResetHighScore();
            }

            // Options (Select with input.select)
            if (isKeyUp && input.Select || isKeyUp && input.SelectMouse)
            {
                // Controls
                if (menu.State(3, 0))
                {
                    menu.PageSelection = 0;
                    // Reset single activation key
                    isKeyUp = false;
                }

                // Speed
                if (menu.State(3, 1))
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
                if (menu.State(3, 2))
                {
                    // Toggle fullscreen
                    graphics.ToggleFullScreen();
                    // Reset single activation key
                    isKeyUp = false;
                }

                // Back
                if (menu.State(3, 3))
                {
                    menu.PageSelection = 0;
                    isKeyUp = false;
                }

            }

            // Options Change State with arrows
            if (isKeyUp)
            {
                // Control Settings
                // Checks what state the button is in (Switching button state with left/right keys is in menu.navigation method)
                if (menu.State(3, 0, 0))
                {
                    // If button state = 0, make options hold false
                    //options.ControlsHold = false;
                }
                if (menu.State(3, 0, 1))
                {
                    // If button state = 1, make options hold true
                    //options.ControlsHold = true;
                }

                // Speed Settings
                // Modified button
                // Checks if button is selected
                if (menu.State(3, 1))
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
                if (menu.State(4, 0))
                {
                    menu.PageSelection = 0;
                    isKeyUp = false;
                }

            }

            // Credits
            if (isKeyUp && input.Select || isKeyUp && input.SelectMouse)
            {
                // Back
                if (menu.State(5, 0))
                {
                    menu.PageSelection = 0;
                    isKeyUp = false;
                }

            }

            // GameOver
            if (isKeyUp && input.Select || isKeyUp && input.SelectMouse)
            {
                // Highscore
                if (menu.State(6, 0))
                {
                    // Move to highscore page
                    menu.PageSelection = 2;
                    // Reset game after game over
                    //game.ResetGame();
                    // Reset single activation key
                    isKeyUp = false;
                }

                // Back
                if (menu.State(6, 1))
                {
                    // Move to main menu
                    menu.PageSelection = 0;
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
                if (menu.State(7, 0))
                {
                    // Move back to play screen
                    menu.PageSelection = 1;
                    // Start game again
                    //initializeGame = true;
                    // Reset single activation key
                    isKeyUp = false;
                }

                // Reset
                if (menu.State(7, 1))
                {
                    // Move back to play screen
                    menu.PageSelection = 1;
                    // Reset game
                    //game.ResetGame();
                    // Start game again
                    //initializeGame = true;
                    // Reset single activation key
                    isKeyUp = false;
                }

                // Back to Menu
                if (menu.State(7, 2))
                {
                    // Move back to main menu
                    menu.PageSelection = 0;
                    // Reset single activation key
                    isKeyUp = false;
                }

                // Exit
                if (menu.State(7, 3))
                {
                    game.Exit();
                }

            }

        }

        public void LoadMenues(ContentManager Content)
        {
            // Fonts
            menuFont = Content.Load<SpriteFont>(@"Fonts/TestFont");
            textFont = Content.Load<SpriteFont>(@"Fonts/TestFont");
            scoreBoardFont = Content.Load<SpriteFont>(@"Fonts/TestFont");

            // Textures
            defaultBackground = Content.Load<Texture2D>(@"Textures/TestTexture");


            // Menu Pages and buttons
            // All pages in the program, see MenuManager.cs for more info
            // Menu
            menu.Pages[0].AddButtonList_Single(menuFont, new Vector2(60), 60f, new[] { "Play", "Level Select", "How To Play", "Highscore", "Credits", "Exit" });

            // Level Select
            menu.Pages[2].AddBackground(defaultBackground, 0.9f);
            menu.Pages[2].AddButton_Single(menuFont, new Vector2(60, 560), "Back");

            // HowToPlay
            menu.Pages[3].AddBackground(defaultBackground, 0.8f);
            menu.Pages[3].AddText(textFont, new Vector2(80), false, "BananPaj" + Environment.NewLine + "Do not touch the edge nor your body!", Color.White);
            menu.Pages[3].AddText(textFont, new Vector2(80, 240), false, "Controls:" + Environment.NewLine + "Play with either A and d or" + Environment.NewLine + "Left and Right Keys on keyboard" + Environment.NewLine + Environment.NewLine + "You can also play with" + Environment.NewLine + "Left and Right Mouse buttons", Color.White);
            menu.Pages[3].AddButton_Single(menuFont, new Vector2(80, 560), "Back");

            // Highscore
            menu.Pages[4].AddBackground(defaultBackground, 0.8f);
            menu.Pages[4].AddText(textFont, new Vector2(80), false, "BananPaj" + Environment.NewLine + "Do not touch the edge nor your body!", Color.White);
            menu.Pages[4].AddText(textFont, new Vector2(80, 240), false, "Controls:" + Environment.NewLine + "Play with either A and d or" + Environment.NewLine + "Left and Right Keys on keyboard" + Environment.NewLine + Environment.NewLine + "You can also play with" + Environment.NewLine + "Left and Right Mouse buttons", Color.White);
            menu.Pages[4].AddButton_Single(menuFont, new Vector2(80, 560), "Back");


            // Credits
            menu.Pages[5].AddBackground(defaultBackground, 0.9f);
            menu.Pages[5].AddText(menuFont, new Vector2(screenWidth / 2, screenHeight / 3), true, "Credits", Color.White);
            menu.Pages[5].AddText(textFont, new Vector2(screenWidth / 2, screenHeight / 2), true, "Game made by Julius", Color.White);
            menu.Pages[5].AddButton_Single(menuFont, new Vector2(60, 560), "Back");

            // GameOver
            menu.Pages[6].AddText(menuFont, new Vector2(screenWidth / 2, screenHeight / 5), true, "GameOver", Color.Red);
            menu.Pages[6].AddText(menuFont, new Vector2(screenWidth / 2, screenHeight / 3), true, "Score: ", Color.White);
            menu.Pages[6].AddButton_Single(menuFont, new Vector2(60, 460), "Highscore");
            menu.Pages[6].AddButton_Single(menuFont, new Vector2(60, 560), "Back");

            //// Pause
            //menu.Pages[7].AddButtonList_Single(menuFont, new Vector2(60), 100f, new[] { "Resume", "Reset", "Back to Menu", "Exit" });

        }

        public void Navigation(bool up, bool down, bool left, bool right)
        {
            menu.Navigation(up, down, left, right);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            menu.Draw(spriteBatch, screenSize);
        }
    }
}
