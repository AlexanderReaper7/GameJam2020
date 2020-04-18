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
        public Menu menu = new Menu(9);

        SpriteFont menuFont, textFont, scoreBoardFont;
        Texture2D defaultBackground, mainMenu;
        private ControlScheme input;
        private Game game;
        private GraphicsDeviceManager graphics;
        private Highscore scoreBoard;
        public static float score;
        public static bool exclusiveBool = false;

        private Credits credits = new Credits();

        public GameStates gameStates;

        public enum MenuState : byte
        {
            InsertName,
            Main,
            LevelSelect,
            HighscoreBoard,
            GameOver,
            Victory,
            Credits, 
            Instructions
        }
        public static MenuState menuState = MenuState.Main;
        

        int screenWidth;
        int screenHeight;
        Rectangle screenSize;

        public MenuManager(Game game, GraphicsDeviceManager graphics, Highscore scoreBoard)
        {
            this.game = game;
            this.graphics = graphics;
            this.scoreBoard = scoreBoard;
            gameStates = GameStates.Menu;
            screenSize = graphics.GraphicsDevice.Viewport.Bounds;
            screenWidth = graphics.GraphicsDevice.Viewport.Width;
            screenHeight = graphics.GraphicsDevice.Viewport.Height;
            input = new ControlScheme();
        }

        public void Update(GameTime gameTime)
        {
            // Update the variables in control scheme (input)
            input.Update();

            if (menu.PageSelection == (int)MenuState.InsertName)
            {

            }
            else if (menu.PageSelection == (int)MenuState.Credits)
            {
                credits.Update(gameTime);
                ButtonSelect();
            }
            else
            {
                ButtonNavigation();
                ButtonSelect();
            }
            
            
        }

        public void LoadMenues(ContentManager Content)
        {
            // Classes
            credits.LoadContent(Content);

            // Fonts
            menuFont = Content.Load<SpriteFont>(@"Shared/Fonts/Main");
            textFont = Content.Load<SpriteFont>(@"Shared/Fonts/TestFont");
            scoreBoardFont = Content.Load<SpriteFont>(@"Shared/Fonts/TestFont");
            SpriteFont titleFont = Content.Load<SpriteFont>(@"Shared/Fonts/Title");

            // Textures
            defaultBackground = Content.Load<Texture2D>(@"Shared/Menu/Background");
            mainMenu = Content.Load<Texture2D>(@"Shared/Menu/MainMenu");
            Texture2D instructionsMenu = Content.Load<Texture2D>(@"Shared/Menu/Instructions");


            // StartMenu
            menuState = MenuState.Main;
            menu.PageSelection = (int)menuState;

            // All pages in the program, see Menu.cs for more info

            menu.Pages[(int)MenuState.InsertName].AddBackground(defaultBackground);

            menu.Pages[(int)MenuState.Main].AddBackground(mainMenu);
            menu.Pages[(int)MenuState.Main].AddText(titleFont, new Vector2(60, 20), false, "WEIRD TILES IN SPACE", Color.White);
            menu.Pages[(int)MenuState.Main].AddButtonList_Single(menuFont, new Vector2(screenWidth / 10, screenHeight / 5), 80f, new[] { "Play", "Level Select", "Instructions", "Highscore", "Credits", "Exit" },
                new Action[] { () => gameStates = GameStates.PreGame, () => ChangePage(MenuState.LevelSelect), () => ChangePage(MenuState.Instructions), () => ChangePage(MenuState.HighscoreBoard), () => ChangePage(MenuState.Credits), () => game.Exit() });
            
            menu.Pages[(int)MenuState.LevelSelect].AddBackground(defaultBackground);
            menu.Pages[(int)MenuState.LevelSelect].AddButton_Single(menuFont, new Vector2(60, 560), "Back", () => ChangePage(MenuState.Main));
            
            menu.Pages[(int)MenuState.HighscoreBoard].AddBackground(defaultBackground);
            menu.Pages[(int)MenuState.HighscoreBoard].AddButton_Single(menuFont, new Vector2(80, 660), "Back", () => ChangePage(MenuState.Main));
            
            menu.Pages[(int)MenuState.GameOver].AddBackground(defaultBackground);
            menu.Pages[(int)MenuState.GameOver].AddText(menuFont, new Vector2(screenWidth / 2, screenHeight / 5), true, "Game Over", Color.Red);
            menu.Pages[(int)MenuState.GameOver].AddText(menuFont, new Vector2(screenWidth / 2, screenHeight / 3), true, "Better luck next time", Color.White);
            menu.Pages[(int)MenuState.GameOver].AddButton_Single(menuFont, new Vector2(80, 560), "Back", () => ChangePage(MenuState.Main));

            
            menu.Pages[(int)MenuState.Victory].AddBackground(defaultBackground);
            menu.Pages[(int)MenuState.Victory].AddText(menuFont, new Vector2(screenWidth / 2, screenHeight / 5), true, "Victory", Color.Yellow);
            menu.Pages[(int)MenuState.Victory].AddText(menuFont, new Vector2(screenWidth / 2, screenHeight / 3), true, "Time: " + score.ToString(), Color.White);
            menu.Pages[(int)MenuState.Victory].AddButton_Single(menuFont, new Vector2(60, 560), "Back", () => ChangePage(MenuState.Main));
            
            menu.Pages[(int)MenuState.Credits].AddButton_Single(menuFont, new Vector2(60, 560), "Back", () => ChangePage(MenuState.Main));

            menu.Pages[(int)MenuState.Instructions].AddBackground(instructionsMenu);
            menu.Pages[(int)MenuState.Instructions].AddButton_Single(menuFont, new Vector2(60, 560), "Back", () => ChangePage(MenuState.Main));

        }

        public void ButtonNavigation()
        {
            if (input.PrevIsKeyUp && input.Up || input.PrevIsKeyUp && input.Down || input.PrevIsKeyUp && input.Left || input.PrevIsKeyUp && input.Right)
            {
                menu.Navigation(input.Up, input.Down, input.Left, input.Right);
                SoundManager.Select.Play();
            } 
        }

        public void ButtonSelect()
        {
            if (input.PrevIsKeyUp && input.Select)
            {
                // Activate button
                menu.Pages[menu.PageSelection].Buttons[menu.Pages[menu.PageSelection].ButtonSelection].Run();
                SoundManager.Select.Play();
            }
        }

        public void ChangePage(MenuState page)
        {
            menu.PageSelection = (int)page;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            menu.Pages[(int)MenuState.Victory].Text[1].Text = "Time: " + score.ToString();

            if (menu.PageSelection == (int)MenuState.InsertName)
            {

            }
            else if(menu.PageSelection == (int)MenuState.Credits)
            {
                credits.Draw(spriteBatch);
                menu.Draw(spriteBatch, screenSize);
            }
            else menu.Draw(spriteBatch, screenSize);
            if(menu.PageSelection == (int)MenuState.HighscoreBoard)
            {
                try { scoreBoard.Draw(spriteBatch, scoreBoardFont); }
                catch { }
                }
        }
    }
}
