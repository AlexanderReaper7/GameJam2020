using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Tools_XNA
{
    public class MenuManager
    {
        public Menu menu = new Menu(8);

        SpriteFont menuFont, textFont, scoreBoardFont;
        Texture2D defaultBackground;

        int screenWidth;
        int screenHeight;
        Rectangle screenSize;

        public void Screen(Rectangle ScreenSize)
        {
            screenSize = ScreenSize;
            screenWidth = ScreenSize.Width;
            screenHeight = ScreenSize.Height;
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
            menu.Pages[0].AddButtonList_Single(menuFont, new Vector2(60), 60f, new[] { "Play", "Highscore", "Options", "How To Play", "Credits", "Exit" });

            // HighScore
            menu.Pages[2].AddBackground(defaultBackground, 0.9f);
            menu.Pages[2].AddButton_Single(menuFont, new Vector2(60, 560), "Back");

            // Options
            menu.Pages[3].AddButtonList_Multi(menuFont, new Vector2(60), 100f, new List<string[]>() { new[] { "Controls: Press", "Controls: Hold" }, new[] { "Speed: Sonic" }, new[] { "Fullscreen" } });
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
