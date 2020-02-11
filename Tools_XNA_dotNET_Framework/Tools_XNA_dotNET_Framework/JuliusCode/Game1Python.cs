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

namespace python
{
    /// <summary>
    /// Controls:
    ///     - WASD          Navigation in menu
    ///     - Enter/Space   Select in menu
    ///     - AD            Left & Right in game
    ///     - L/R Keys or mouse buttons, Left & Right in game
    ///     - ESC/Back      Pause game
    ///     - F11           Switch to fullscreen
    ///     - END           Exit application
    ///     - Shift + R     Reset highscore while in highscore menu
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Public variables that define the screen resolution
        public static int screenWidth = 1280;
        public static int screenHeight = 720;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            // Make the game go smoother (something alexander forced me to do)
            IsFixedTimeStep = false;
            // Apply public variables to define screen resolution
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.ApplyChanges();
        }

        // Variables
        private SaveFile saveFile = new SaveFile();
        ControlScheme input = new ControlScheme();
        InGame game;
        Settings options = new Settings();
        /*
         * Main = menu.pages[0]
         * Play      = 1
         * HighScore = 2
         * Options   = 3
         * HowToPlay = 4
         * Credits   = 5
         * GameOver  = 6
         * Pause     = 7
         */
        MenuManager menu = new MenuManager(8);

        // three different fonts, one for menu text, one for descriptive text, and one for the high score page
        public SpriteFont menuFont, textFont, scoreBoardFont;
        // three textures, one is mouse texture, pause button and background
        public Texture2D mouseTexture, pauseButtonTexture, defaultBackground;
        // A rectangle for the pause button
        public Rectangle pauseButton;
        // A float that decides how much transparency the mouse shall have and a timer for when the mouse shall dim/disappear
        private float mouseVisibility, mouseTimer;
        // A bool that initializes a "startGame" in InGame.cs
        private bool initializeGame;

        protected override void Initialize()
        {
            // Create a savefile if one has not already been created
            saveFile.Initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load options, speed shall be 1, hold controls shall be false
            options.Speed = 1f;
            options.ControlsHold = false;
            // Create a new game
            game = new InGame(input, GraphicsDevice, options);
            // Load the game's assets
            game.Load(Content);

            // Textures
            mouseTexture = Content.Load<Texture2D>(@"MousePointer");
            pauseButtonTexture = Content.Load<Texture2D>(@"PauseButton");
            defaultBackground = Content.Load<Texture2D>(@"Background");

            // Fonts
            menuFont = Content.Load<SpriteFont>(@"Fonts/Main");
            textFont = Content.Load<SpriteFont>(@"Fonts/Text");
            scoreBoardFont = Content.Load<SpriteFont>(@"Fonts/Score");
            

            // Menu Pages and buttons
            // All pages in the program, see MenuManager.cs for more info
            // Menu
            menu.Pages[0].AddButtonList_Single(menuFont, new Vector2(60), 100f, new[] { "Play", "Highscore", "Options", "How To Play", "Credits", "Exit" });

            // HighScore
            menu.Pages[2].AddBackground(defaultBackground, 0.9f);
            menu.Pages[2].AddButton_Single(menuFont, new Vector2(60, 560), "Back");
            
            // Options
            menu.Pages[3].AddButtonList_Multi(menuFont, new Vector2(60), 100f, new List<string[]>() { new []{"Controls: Press", "Controls: Hold"}, new []{"Speed: " + options.Speed.ToString()}, new []{"Fullscreen"}});
            menu.Pages[3].AddButton_Single(menuFont, new Vector2(60, 560), "Back");
            
            // HowToPlay
            menu.Pages[4].AddBackground(defaultBackground, 0.8f);
            menu.Pages[4].AddText(textFont, new Vector2(80), false, "Get as many blobs as possible!" + Environment.NewLine + "Do not touch the edge nor your body!", Color.White);
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
            
            // Pause
            menu.Pages[7].AddButtonList_Single(menuFont, new Vector2(60), 100f, new[] { "Resume", "Reset", "Back to Menu", "Exit" });

        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit when pressing End
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.End))
                this.Exit();

            // Fullscreen
            if (Keyboard.GetState().IsKeyDown(Keys.F11))
                graphics.ToggleFullScreen();


            // If game over
            if (game.GameOver)
            {
                // The saving score will be the current score
                saveFile.PlayerScore = game.Score;
                // If game over and in play page (This will run only once after a game over)
                if (game.GameOver && menu.PageSelection == 1)
                {
                    // Save score into highscore list
                    saveFile.SaveHighScore();
                    // Change page to game over screen
                    menu.PageSelection = 6;
                    // Update the score text in game over page
                    menu.Pages[6].Text[1].UpdateLine("Score: "+ saveFile.PlayerScore);
                }
            }
            

            // A small bool to make the full variable a bit smaller
            bool isKeyUp = input.IsKeyUp;
            // Update the variables in control scheme (input)
            input.Update();
            // If player is not in play screen/page, make menu navigation possible
            if (isKeyUp && menu.PageSelection != 1)
            {
                menu.Navigation(input.Up, input.Down, input.Left, input.Right);
            }
            // If mouse pointer does not move, then do not check if mouse is intersecting with menu buttons
            if (input.MousePointer != input.PreviousMousePosition)
            {
                menu.UpdateMouse(input.MousePointer);
            }







            // A region of all the pages and what each button do
            #region All Menus

            // A safety function that checks if player is outside the play screen and then pauses the game
            if (menu.PageSelection != 1)
            {
                game.GameActive = false;
            }


            // Main Menu
            // Single activation on select key and mouse button
            if (isKeyUp && input.Select || isKeyUp && input.SelectMouse)
            {
                // Check if player select first button in main menu (play)
                if (menu.State(0, 0))
                {
                    // Change page to Play screen
                    menu.PageSelection = 1;
                    // Start game
                    initializeGame = true;
                    // Make single activation key reset
                    isKeyUp = false;
                }

                // Highscore
                if (menu.State(0, 1))
                {
                    menu.PageSelection = 2;
                    isKeyUp = false;
                }

                // Options
                if (menu.State(0, 2))
                {
                    menu.PageSelection = 3;
                    isKeyUp = false;
                }

                // How to play
                if (menu.State(0, 3))
                {
                    menu.PageSelection = 4;
                    isKeyUp = false;
                }

                // Credits
                if (menu.State(0, 4))
                {
                    menu.PageSelection = 5;
                    isKeyUp = false;
                }

                // Exit
                if (menu.State(0, 5))
                {
                    // Exit the game
                    this.Exit();
                }

            }

            // Play
            // If pause (Back) button is activated, or if mouse presses on pause button in the left top corner
            if (isKeyUp && input.Back || isKeyUp && input.SelectMouse && input.MousePointer.Intersects(pauseButton))
            {
                // Only if game is active (you cannot pause when the player see a countdown)
                if (game.GameActive)
                {
                    // Move to pause menu
                    menu.PageSelection = 7;
                    // Pause game
                    game.PauseGame();
                    // Reset single activation key
                    isKeyUp = false;
                    
                }
                
            }

            // HighScore
            if (isKeyUp && input.Select || isKeyUp && input.SelectMouse)
            {
                // Back
                if (menu.State(2, 0))
                {
                    menu.PageSelection = 0;
                    isKeyUp = false;
                }

            }
            // If Left shift and R is pressed while in Highscore page, clean/reset the score board
            if (menu.PageSelection == 2 && Keyboard.GetState().IsKeyDown(Keys.LeftShift) && Keyboard.GetState().IsKeyDown(Keys.R))
            {
                saveFile.ResetHighScore();
            }

            // Options (Select with input.select)
            if (isKeyUp && input.Select || isKeyUp && input.SelectMouse)
            {
                // Controls
                if (menu.State(3, 0))
                {
                    // If button gets pressed (with selection key/mouse button), change button state, loop it so that it doesn't get stuck
                    menu.Pages[3].Buttons[0].SelectRight(true);
                    // Reset single activation key
                    isKeyUp = false;
                }

                // Speed
                if (menu.State(3, 1))
                {
                    // If button gets pressed (with selection key/mouse button), add 0.1f speed
                    options.Speed += 0.1f;
                    // If it gets over 2f, change it to 0.2f
                    if (options.Speed > 2.01f)
                    {
                        options.Speed = 0.2f;
                    }
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
                    options.ControlsHold = false;
                }
                if (menu.State(3, 0, 1))
                {
                    // If button state = 1, make options hold true
                    options.ControlsHold = true;
                }

                // Speed Settings
                // Modified button
                // Checks if button is selected
                if (menu.State(3, 1))
                {
                    // If left is pressed and is more than 0.2f
                    if (input.Left && options.Speed > 0.2f)
                        // Subtract speed with 0.1f
                        options.Speed -= 0.1f;

                    // If right is pressed and is less than 2f
                    if (input.Right && options.Speed < 2f)
                        // Add speed with 0.1f
                        options.Speed += 0.1f;
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
                    game.ResetGame();
                    // Reset single activation key
                    isKeyUp = false;
                }

                // Back
                if (menu.State(6, 1))
                {
                    // Move to main menu
                    menu.PageSelection = 0;
                    // Reset game after game over
                    game.ResetGame();
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
                    initializeGame = true;
                    // Reset single activation key
                    isKeyUp = false;
                }

                // Reset
                if (menu.State(7, 1))
                {
                    // Move back to play screen
                    menu.PageSelection = 1;
                    // Reset game
                    game.ResetGame();
                    // Start game again
                    initializeGame = true;
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
                    this.Exit();
                }

            }

            #endregion








            // Update game
            game.Update(gameTime);
            // If initializeGame, then start game, when it have started, make initializeGame false (so that it wont continue to start)
            if (initializeGame)
            {
                game.StartGame(gameTime);
                if (game.GameActive)
                {
                    initializeGame = false;
                }
            }
            
            // Update camera (It needs to be last thing of everything in InGame.cs)
            game.UpdateCamera();
            
            base.Update(gameTime);
        }
        

        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            

            spriteBatch.Begin();
            // Draw game and menu
            game.Draw(spriteBatch);
            menu.Draw(spriteBatch);

            // Draw speed text specifically by converting the float to an int with a 10% safety marginal and then converting it back to float and divide it by 10, creating a clean, one decimal, float (visually).
            menu.Pages[3].Buttons[1].Text[0] = "Speed: " + (float)(int)(options.Speed*10.1f)/10;

            // If play screen
            if (menu.PageSelection == 1)
            {
                // Look if mouse intersects pause button, if it does, change color from gray to white
                if (input.MousePointer.Intersects(pauseButton))
                {
                    spriteBatch.Draw(pauseButtonTexture, pauseButton = new Rectangle(0, 0, pauseButtonTexture.Width, pauseButtonTexture.Height), Color.White);
                }
                else
                {
                    spriteBatch.Draw(pauseButtonTexture, pauseButton = new Rectangle(0, 0, pauseButtonTexture.Width, pauseButtonTexture.Height), Color.Gray);
                }
            }

            // If highscore page, draw the highscore list
            if (menu.PageSelection == 2)
            {
                saveFile.Draw(spriteBatch, scoreBoardFont);
            }
            

            // If mouse moves, make it 100% visible and reset timer to 2 seconds
            if (input.MousePointer != input.PreviousMousePosition)
            {
                mouseVisibility = 1f;
                mouseTimer = 2000;
            }
            // If mouse does not move, make the 2 second timer count downwards, until it reaches zero, that's when mouse visibility gets reduced by 16% every game tick
            else
            {
                mouseTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (mouseTimer < 0)
                {
                    mouseVisibility /= 1.16f;
                }
            }
            // Draw the mouse with it's visibility (on top of everything else)
            input.DrawMouse(spriteBatch, mouseTexture, mouseVisibility);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
