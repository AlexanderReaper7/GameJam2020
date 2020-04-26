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
            Texture2D logo = Content.Load<Texture2D>(@"Shared/Menu/logo");


            // StartMenu
            menuState = MenuState.Main;
            menu.PageSelection = (int)menuState;

            // All pages in the program, see Menu.cs for more info

            menu.Pages[(int)MenuState.InsertName].AddBackground(defaultBackground);

            menu.Pages[(int)MenuState.Main].AddBackground(mainMenu);
            menu.Pages[(int)MenuState.Main].AddBackground(logo);
            menu.Pages[(int)MenuState.Main].AddButtonList_Single(menuFont, new Vector2(screenWidth / 10, screenHeight / 5), 80f, new[] { "Play", "Level Select", "Instructions", "Highscore", "Credits", "Exit" },
                new Action[] { () => gameStates = GameStates.PreLevel1, () => ChangePage(MenuState.LevelSelect), () => ChangePage(MenuState.Instructions), () => ChangePage(MenuState.HighscoreBoard), () => { ChangePage(MenuState.Credits); credits.Reset(); }, () => game.Exit() });

            menu.Pages[(int)MenuState.LevelSelect].AddBackground(defaultBackground);
            menu.Pages[(int)MenuState.LevelSelect].AddText(menuFont, new Vector2(screenWidth / 2, screenHeight / 7), true, "Select a level", Color.White);
            menu.Pages[(int)MenuState.LevelSelect].AddText(menuFont, new Vector2(screenWidth / 2, screenHeight / 7 + 45 ), true, "(Time will only be saved if you start from level 1)", Color.White);
            menu.Pages[(int)MenuState.LevelSelect].AddButtonList_Single(menuFont, new Vector2(570, screenHeight / 5 + 30), 50f, new[] { "Level 1", "Level 2", "Level 3", "Level 4", "Level 5", "Level 6", "Level 7", "Level 8" },
                new Action[] { () => gameStates = GameStates.PreLevel1, () => gameStates = GameStates.PreLevel2, () => gameStates = GameStates.PreLevel3, () => gameStates = GameStates.PreLevel4, () => gameStates = GameStates.PreLevel5, () => gameStates = GameStates.PreLevel6, () => gameStates = GameStates.PreLevel7, () => gameStates = GameStates.PreLevel8, });
            menu.Pages[(int)MenuState.HighscoreBoard].AddButton_Single(menuFont, new Vector2(80, 660), "Back", () => ChangePage(MenuState.Main));

            menu.Pages[(int)MenuState.LevelSelect].AddBackground(defaultBackground);
            menu.Pages[(int)MenuState.LevelSelect].AddButton_Single(menuFont, new Vector2(60, 560), "Back", () => ChangePage(MenuState.Main));
            
            menu.Pages[(int)MenuState.HighscoreBoard].AddBackground(defaultBackground);
            menu.Pages[(int)MenuState.HighscoreBoard].AddText(menuFont, new Vector2(screenWidth / 2, screenHeight / 7), true, "Highscore", Color.White);
            menu.Pages[(int)MenuState.HighscoreBoard].AddButton_Single(menuFont, new Vector2(80, 660), "Back", () => ChangePage(MenuState.Main));
            
            menu.Pages[(int)MenuState.GameOver].AddBackground(defaultBackground);
            menu.Pages[(int)MenuState.GameOver].AddText(menuFont, new Vector2(screenWidth / 2, screenHeight / 5), true, "Game Over", Color.Red);
            menu.Pages[(int)MenuState.GameOver].AddText(menuFont, new Vector2(screenWidth / 2, screenHeight / 3), true, "Better luck next time", Color.White);
            menu.Pages[(int)MenuState.GameOver].AddButton_Single(menuFont, new Vector2(80, 560), "Back", () => ChangePage(MenuState.Main));

            
            menu.Pages[(int)MenuState.Victory].AddBackground(defaultBackground);
            menu.Pages[(int)MenuState.Victory].AddText(menuFont, new Vector2(screenWidth / 2, screenHeight / 5), true, "Victory", Color.Yellow);
            menu.Pages[(int)MenuState.Victory].AddText(menuFont, new Vector2(screenWidth / 2, screenHeight / 3), true, "Your time: " + score.ToString(), Color.White);
            menu.Pages[(int)MenuState.Victory].AddText(menuFont, new Vector2(screenWidth / 2, screenHeight / 3 + 60), true, "", Color.White);
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

            if (score < 999999) menu.Pages[(int)MenuState.Victory].Text[1].Text = "Your time: " + score.ToString() + " seconds";
            else menu.Pages[(int)MenuState.Victory].Text[1].Text = "(Start from first level to get a time)";
            if (scoreBoard.LoadData(scoreBoard.Filename).Time[0] < 999999) menu.Pages[(int)MenuState.Victory].Text[2].Text = "Highscore: " + scoreBoard.LoadData(scoreBoard.Filename).Time[0] + " seconds";


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
                try { scoreBoard.Draw(spriteBatch, menuFont); }
                catch { }
                }
        }
    }
}
