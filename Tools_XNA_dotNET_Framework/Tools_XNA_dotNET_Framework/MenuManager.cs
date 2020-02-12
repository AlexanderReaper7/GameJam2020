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
            Navigation(input.Up, input.Down, input.Left, input.Right);

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
