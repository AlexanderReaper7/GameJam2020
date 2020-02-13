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
        private Highscore scoreBoard;

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
        MenuState menuState = MenuState.Main;
        

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

        public void Update()
        {
            // A small bool to make the full variable a bit smaller
            bool isKeyUp = input.IsKeyUp;
            // Update the variables in control scheme (input)
            input.Update();
            
            // Navigationcode
            if (isKeyUp && input.Up || isKeyUp && input.Down || isKeyUp && input.Left || isKeyUp && input.Right)
            {
                Navigation(input.Up, input.Down, input.Left, input.Right);
                isKeyUp = false;
            }
                

            // Single activation on select key and mouse button
            if (isKeyUp && input.Select || isKeyUp && input.SelectMouse)
            {
                // Activate button
                menu.Pages[menu.PageSelection].Buttons[menu.Pages[menu.PageSelection].ButtonSelection].Run();
                isKeyUp = false;
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

            // InsertName      0
            // MainMenu        1
            // LevelSelect     2
            // HighscoreBoard  3
            // GameOverScreen  4   
            // VictoryScreen   5
            // Credits         6

            menuState = MenuState.Main;
            // StartMenu
            menu.PageSelection = (int)menuState;

            // All pages in the program, see Menu.cs for more info
            // MainMenu
            menu.Pages[1].AddBackground(defaultBackground);
            menu.Pages[1].AddButtonList_Single(menuFont, new Vector2(60), 60f, new[] { "Play", "Level Select", "Highscore", "Credits", "Exit" },
                new Action[] { () => gameStates = GameStates.Game, () => menu.PageSelection = 2, () => menu.PageSelection = 3, () => menu.PageSelection = 6, () => game.Exit() });
            
            menu.Pages[2].AddBackground(defaultBackground);
            menu.Pages[2].AddButton_Single(menuFont, new Vector2(60, 560), "Back", () => menu.PageSelection = 1);
            
            menu.Pages[3].AddBackground(defaultBackground);
            menu.Pages[3].AddButton_Single(menuFont, new Vector2(80, 560), "Back", () => menu.PageSelection = 1);
            
            menu.Pages[4].AddBackground(defaultBackground);
            menu.Pages[4].AddText(textFont, new Vector2(80), false, "BananPaj" + Environment.NewLine + "Do not touch the edge nor your body!", Color.White);
            menu.Pages[4].AddText(textFont, new Vector2(80, 240), false, "Controls:" + Environment.NewLine + "Play with either A and d or" + Environment.NewLine + "Left and Right Keys on keyboard" + Environment.NewLine + Environment.NewLine + "You can also play with" + Environment.NewLine + "Left and Right Mouse buttons", Color.White);
            menu.Pages[4].AddButton_Single(menuFont, new Vector2(80, 560), "Back", () => menu.PageSelection = 1);

            
            menu.Pages[5].AddBackground(defaultBackground, 1f);
            menu.Pages[5].AddText(menuFont, new Vector2(screenWidth / 2, screenHeight / 3), true, "Credits", Color.White);
            menu.Pages[5].AddText(textFont, new Vector2(screenWidth / 2, screenHeight / 2), true, "Game made by Julius", Color.White);
            menu.Pages[5].AddButton_Single(menuFont, new Vector2(60, 560), "Back", () => menu.PageSelection = 1);
            
            menu.Pages[6].AddText(menuFont, new Vector2(screenWidth / 2, screenHeight / 5), true, "GameOver", Color.Red);
            menu.Pages[6].AddText(menuFont, new Vector2(screenWidth / 2, screenHeight / 3), true, "Score: ", Color.White);
            menu.Pages[6].AddButton_Single(menuFont, new Vector2(60, 460), "Highscore", () => menu.PageSelection = 3);
            menu.Pages[6].AddButton_Single(menuFont, new Vector2(60, 560), "Back", () => menu.PageSelection = 1);

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
            if(menu.PageSelection == (int)MenuState.HighscoreBoard)
            {
                scoreBoard.Draw(spriteBatch, menuFont);
            }
        }
    }
}
