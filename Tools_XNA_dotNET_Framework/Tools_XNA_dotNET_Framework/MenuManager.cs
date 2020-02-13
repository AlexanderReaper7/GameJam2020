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
        private Highscore scoreBoard = new Highscore();

        public GameStates gameStates;

        public enum MenuState : byte
        {
            InsertName,
            Main,
            LevelSelect,
            HighscoreBoard,
            GameOver,
            Victory,
            Credits
        }
        public MenuState menuState = MenuState.Main;
        

        int screenWidth;
        int screenHeight;
        Rectangle screenSize;

        public MenuManager(Game game, GraphicsDeviceManager graphics)
        {
            this.game = game;
            this.graphics = graphics;
            gameStates = GameStates.Menu;
            screenSize = graphics.GraphicsDevice.Viewport.Bounds;
            screenWidth = graphics.GraphicsDevice.Viewport.Width;
            screenHeight = graphics.GraphicsDevice.Viewport.Height;
            input = new ControlScheme();
        }

        public void Update()
        {
            // Update the variables in control scheme (input)
            input.Update();

            if (menuState == MenuState.LevelSelect)
            {

            }
            else if (menuState == MenuState.Credits)
            {

            }
            else
            {
                ButtonNavigation();
                ButtonSelect();
            }
            
            
        }

        public void LoadMenues(ContentManager Content)
        {
            // Fonts
            menuFont = Content.Load<SpriteFont>(@"Shared/Fonts/Main");
            textFont = Content.Load<SpriteFont>(@"Shared/Fonts/TestFont");
            scoreBoardFont = Content.Load<SpriteFont>(@"SHared/Fonts/TestFont");

            // Textures
            defaultBackground = Content.Load<Texture2D>(@"Shared/Menu/Background");
            

            // StartMenu
            menuState = MenuState.Main;
            menu.PageSelection = (int)menuState;

            // All pages in the program, see Menu.cs for more info

            menu.Pages[(int)MenuState.InsertName].AddBackground(defaultBackground);

            menu.Pages[(int)MenuState.Main].AddBackground(defaultBackground);
            menu.Pages[(int)MenuState.Main].AddButtonList_Single(menuFont, new Vector2(screenWidth/10, screenHeight/5), 80f, new[] { "Play", "Level Select", "Highscore", "Credits", "Exit" },
                new Action[] { () => gameStates = GameStates.Game, () => ChangePage(MenuState.LevelSelect), () => ChangePage(MenuState.HighscoreBoard), () => ChangePage(MenuState.Credits), () => game.Exit() });
            
            menu.Pages[(int)MenuState.LevelSelect].AddBackground(defaultBackground);
            menu.Pages[(int)MenuState.LevelSelect].AddButton_Single(menuFont, new Vector2(60, 560), "Back", () => ChangePage(MenuState.Main));
            
            menu.Pages[(int)MenuState.HighscoreBoard].AddBackground(defaultBackground);
            menu.Pages[(int)MenuState.HighscoreBoard].AddButton_Single(menuFont, new Vector2(80, 560), "Back", () => ChangePage(MenuState.Main));
            
            menu.Pages[(int)MenuState.GameOver].AddBackground(defaultBackground);
            menu.Pages[(int)MenuState.GameOver].AddText(menuFont, new Vector2(screenWidth / 2, screenHeight / 5), true, "Game Over", Color.Red);
            menu.Pages[(int)MenuState.GameOver].AddButton_Single(menuFont, new Vector2(80, 560), "Back", () => ChangePage(MenuState.Main));

            
            menu.Pages[(int)MenuState.Victory].AddBackground(defaultBackground);
            menu.Pages[(int)MenuState.Victory].AddText(menuFont, new Vector2(screenWidth / 2, screenHeight / 3), true, "Victory", Color.Yellow);
            menu.Pages[(int)MenuState.Victory].AddButton_Single(menuFont, new Vector2(60, 560), "Back", () => ChangePage(MenuState.Main));
            
            menu.Pages[(int)MenuState.Credits].AddText(menuFont, new Vector2(screenWidth / 2, screenHeight / 5), true, "Credits", Color.White);
            menu.Pages[(int)MenuState.Credits].AddText(menuFont, new Vector2(screenWidth / 2, screenHeight / 3), true, "(To be implemented)", Color.White);
            menu.Pages[(int)MenuState.Credits].AddButton_Single(menuFont, new Vector2(60, 460), "Highscore", () => ChangePage(MenuState.HighscoreBoard));
            menu.Pages[(int)MenuState.Credits].AddButton_Single(menuFont, new Vector2(60, 560), "Back", () => ChangePage(MenuState.Main));
            
        }

        public void ButtonNavigation()
        {
            if (input.PrevIsKeyUp && input.Up || input.PrevIsKeyUp && input.Down || input.PrevIsKeyUp && input.Left || input.PrevIsKeyUp && input.Right)
            menu.Navigation(input.Up, input.Down, input.Left, input.Right);
        }

        public void ButtonSelect()
        {
            if (input.PrevIsKeyUp && input.Select || input.PrevIsKeyUp && input.SelectMouse)
                // Activate button
                menu.Pages[menu.PageSelection].Buttons[menu.Pages[menu.PageSelection].ButtonSelection].Run();
        }

        public void ChangePage(MenuState page)
        {
            menu.PageSelection = (int)page;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(menuState == MenuState.InsertName)
            {

            }
            else menu.Draw(spriteBatch, screenSize);
            if(menu.PageSelection == (int)MenuState.HighscoreBoard)
            {
                scoreBoard.Draw(spriteBatch, menuFont, new Vector2(60), new Vector2(0, 60), Color.White);
            }
        }
    }
}
