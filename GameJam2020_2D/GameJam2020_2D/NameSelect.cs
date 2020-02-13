using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Tools_XNA;

namespace GameJam2020_2D
{
    /// <summary>
    /// The class used to allow the player to choose and save the players name to the high score 
    /// </summary>
    public class NameSelect
    {
        /// <summary>
        ///  An instance of the controls class used to call methods in the class with varying uses 
        /// </summary>
        Controls controls;

        /// <summary>
        /// Contains all of the characters that the player will be able to scroll between. Used for defining the first character of the players name
        /// </summary>
        private char[] characters = new char[] 
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'I',
            'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
            'U', 'V', 'X', 'Y', 'Z'             
        };
        

        /// <summary>
        /// Used to scroll between the various characters in the first array
        /// </summary>
        int[] characterScroller = new int[3] { 0, 0, 0 };

        /// <summary>
        /// Used to loop around the array if the maximum character is exceeded to the minimum character 
        /// </summary>
        int maxCharacterScroller = 34;

        /// <summary>
        /// Used to loop around the array if the minimum character is exceeded to the maximum character
        /// </summary>
        int minCharacterScroller = 0; 

        /// <summary>
        /// The first character for the players name 
        /// </summary>
        private char[] currentCharacter = new char[3];

        public string playerName;

        /// <summary>
        /// The selected button used to scroll between buttons 
        /// </summary>
        private int selectedButton = 0;

        /// <summary>
        /// Used to set the selected button to the minimum amount of buttons if the selected button exceeds the maximum amount of buttons 
        /// </summary>
        private int minAmountButton = 0;

        /// <summary>
        /// Used to set the selected button to the max amount of buttons if the selected buttons goes under the minimum amount of buttons 
        /// </summary>
        private int maxAmountButton = 3;

        SpriteFont selectionText;

        SpriteFont headingText;

        public void LoadContent(ContentManager content)
        {
            // Creates a new instance of the controls with current and previous keyboard 
            controls = new Controls(new KeyboardState(), new KeyboardState());

            // Loads the heading font 
            headingText = content.Load<SpriteFont>(@"Shared/Fonts/HeadingText");

            // Loads the selection font 
            selectionText = content.Load<SpriteFont>(@"Shared/Fonts/SelectionText");
        }

        public void Update(GameTime gameTime)
        {
            // Updates the keyboard controls for the controls class 
            controls.UpdateKeyboardControls(gameTime);

            // If the left or right arrow keys are press, change the currently selected button to those directions

            if (controls.SingleActivationKey(Keys.Left))
            {
                selectedButton--;
            }

            if (controls.SingleActivationKey(Keys.Right))
            {
                selectedButton++;
            }

            // If the selected button is pressed down or up, change the current character inside upwards or downwards. If the lowest or highest character for the selected button is reached, loop around to the lowest or highest character 

            for (int i = 0; i < characterScroller.Count(); i++)
            {

                if (controls.SingleActivationKey(Keys.Up) && selectedButton == i)
                    characterScroller[i]++;
                

                if (controls.SingleActivationKey(Keys.Down) && selectedButton == i)
                    characterScroller[i]--;
                
            }
            // If the selected button exceeds or goes under the maximum/minimum amount of buttons, then loop to the maximum/minimum amount of buttons             

            if (selectedButton < minAmountButton)
            {
                selectedButton = maxAmountButton;
            }

            if (selectedButton > maxAmountButton)
            {
                selectedButton = minAmountButton;
            }

            // If the selected button exceeds or goes under the maximum/minimum amount of characters, then loop to the maximum/minimum amount of characters 

            // The first character looper
            for (int i = 0; i < characterScroller.Count(); i++)
            {
                if (characterScroller[i] < minCharacterScroller)
                {
                    characterScroller[i] = maxCharacterScroller;
                }

                if (characterScroller[i] > maxCharacterScroller)
                {
                    characterScroller[i] = minCharacterScroller;
                }
            }
            // Sets the characters inside the buttons to the current character selected in the array 
            for (int i = 0; i < characterScroller.Count(); i++)
            {
                currentCharacter[i] = characters[characterScroller[i]];
            }
            // If the confirm button is highlighted 
            if (selectedButton == 3 && controls.SingleActivationKey(Keys.Enter))
            {
                // Gets the player name 
                GetPlayerName();

                // Add so that the playerName gets stored somewhere so it can be used in conjunction with the high score 
                InGame.playerName = playerName;

                // Start
                //InGame.Level = InGame.Levels.preLevel1;
                //Game1.gameState = GameStates.Game;
                //Game1.menuManager.gameStates = GameStates.Game;
                //MenuManager.menuState = MenuManager.MenuState.Main;

            }

            // This needs to be at the bottom of the update code otherwise the singleactivation code wont work 
            controls.previousKeyboardState = controls.currentKeyboardState;
        }

        /// <summary>
        /// Used to get the players name when the player has pressed 
        /// </summary>
        public void GetPlayerName()
        {
                playerName = currentCharacter[0].ToString() + currentCharacter[1].ToString() + currentCharacter[2].ToString();
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draws the text at the top of the window which says "Enter your name" 
            spriteBatch.DrawString(headingText, "Enter your name", new Vector2(200, 20), Color.White);

            try {

                for (int i = 0; i < 3; i++)
                {

                    // If the selected button is for example zero, draw the selected version of the button. If the selected button is not zero, draw the unselected version. Repeat for each button here 

                    if (selectedButton == i)
                    {
                        spriteBatch.DrawString(selectionText, currentCharacter[i].ToString(), new Vector2(80 * (i + 1), 300), Color.Gray);
                    }
                    else
                    {
                        spriteBatch.DrawString(selectionText, currentCharacter[i].ToString(), new Vector2(80 * (i + 1), 300), Color.White);
                    }

                }
            }
            catch { }

            if (selectedButton == 3)
                {
                    spriteBatch.DrawString(selectionText, "Confirm", new Vector2(320, 300), Color.Gray);
                }
                else
                {
                    spriteBatch.DrawString(selectionText, "Confirm", new Vector2(320, 300), Color.White);
                }
            }
        }
    }

