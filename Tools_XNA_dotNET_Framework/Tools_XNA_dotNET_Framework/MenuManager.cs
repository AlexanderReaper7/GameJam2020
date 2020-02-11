using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Tools_XNA
{
    class MenuManager
    {


        public enum Menues
        {
            InsertName,
            Main,
            LevelSelcet,
            Level,
            HighScore,
            GameOver,
            Victory,
            Credits
        }

        private Menues state = Menues.Main; // TODO: Change to InsertName Menu

        public Menues State
        {
            get { return state; }
            set
            {
                menu.CurrentPage = (int)value;
                state = value;
            }
        }

        public int MenuesAmount => Enum.GetNames(typeof(Menues)).Length;

        // Create menu pages
        Menu menu = new Menu(Enum.GetNames(typeof(Menues)).Length);
        // Uneccerary?
        Menu level = new Menu(10);

        SpriteFont menuFont;
        

        public void LoadMenues(ContentManager Content)
        {

            // Color theme
            Color primary = new Color(3, 169, 244), primaryLight = new Color(103, 218, 255), primaryDark = new Color(0, 122, 193);
            Color backColor = primaryLight, highLightColor = primary;
            Color logoBlue = new Color(24, 20, 111), logoYellow = new Color(88, 80, 161);
            Rectangle mainRec = new Rectangle(4, 1080 - 82 * (MenuesAmount - 1), 712, 80);
            Rectangle logoRect = new Rectangle(720 / 2 - (480 / 2), 20, 480, 480);
            Vector2 padding = new Vector2(-20);
            Texture2D logo = content.Load<Texture2D>(@"Images/logo");
            Texture2D circle = content.Load<Texture2D>(@"Images/newcircle");






            // Textures
            //mouseTexture = Content.Load<Texture2D>(@"MousePointer");
            //pauseButtonTexture = Content.Load<Texture2D>(@"PauseButton");
            //defaultBackground = Content.Load<Texture2D>(@"Background");

            // Fonts
            menuFont = Content.Load<SpriteFont>(@"Fonts/TestFont");
            //textFont = Content.Load<SpriteFont>(@"Fonts/Text");
            //scoreBoardFont = Content.Load<SpriteFont>(@"Fonts/Score");


            // Menu Pages and buttons
            // All pages in the program, see MenuManager.cs for more info
            // Menu
            menu.Pages[0].AddButtonList(menuFont, new Vector2(60), 100f, new[] { "Play", "Highscore", "Options", "How To Play", "Credits", "Exit" });

            // HighScore
            //menu.Pages[2].AddBackground(defaultBackground, 0.9f);
            menu.Pages[2].AddButton(new Button(menuFont, new Rectangle(60, 560, 200, 60), "Back", Vector2.Zero, Color.White, Color.Cyan, ));

            // Options
            menu.Pages[3].AddButtonList_Multi(menuFont, new Vector2(60), 100f, new List<string[]>() { new[] { "Controls: Press", "Controls: Hold" }, new[] { "Speed: " + options.Speed.ToString() }, new[] { "Fullscreen" } });
            menu.Pages[3].AddButton(menuFont, new Vector2(60, 560), "Back");

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

        public void Draw(SpriteBatch spriteBatch)
        {
            menu.Draw(spriteBatch);
        }
    }
}
