        using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Tools_XNA
{
    public class Menu
    {
        
            // List for all the pages in your desired menu
            public List<Page> Pages;
            // A selection variable for pages
            public int PageSelection;

            // Constructor with desired amount of pages
            public Menu(int amountOfPages)
            {
                Pages = new List<Page>(amountOfPages);
                for (int i = 0; i < amountOfPages; i++)
                {
                    Pages.Add(new Page());
                }
            }

            // State bool, when used, it checks if your current selection is on the same page and button, it is to make it easier to write what each button shall do in another class
            public bool State(int page, int button)
            {
                if (PageSelection == page)
                {
                    if (Pages[page].ButtonSelection == button)
                    {
                        return true;
                    }

                }

                return false;

            }

            // State bool, when used, it checks if your current selection is on the same page and button and it's state, it is to make it easier to write what each button state shall do in another class
            public bool State(int page, int button, int state)
            {
                if (PageSelection == page)
                {
                    if (Pages[page].ButtonSelection == button)
                    {
                        if (Pages[page].Buttons[button].StateSelection == state)
                        {
                            return true;
                        }
                    }

                }

                return false;

            }

        // Easy navigation of buttons and button states, takes in an input of up, down, left and right
        public bool Navigation(bool up, bool down, bool left, bool right)
        {
            if (up)
            {
                // Make the selection go up in current page
                Pages[PageSelection].SelectUp(false);
                return false;
            }
            else if (down)
            {
                // Make the selection go down in current page
                Pages[PageSelection].SelectDown(false);
                return false;
            }
            else if (left)
            {
                // Make the selection change to left in current page's current button
                Pages[PageSelection].Buttons[Pages[PageSelection].ButtonSelection].SelectLeft(false);
                return false;
            }
            else if (right)
            {
                // Make the selection change to right in current page's current button
                Pages[PageSelection].Buttons[Pages[PageSelection].ButtonSelection].SelectRight(false);
                return false;
            }
            else return true;
        }

            // Update mouse collision with all buttons
            public void UpdateMouse(Rectangle mousePointer)
            {
                Pages[PageSelection].UpdateMouse(mousePointer);
            }

            // Draw the current selected page
            public void Draw(SpriteBatch spriteBatch, Rectangle screenSize)
            {
                Pages[PageSelection].Draw(spriteBatch, screenSize);
            }

        }

        // A class that handles texts, background and buttons
        public class Page
        {
            // Variable for custom background
            public Texture2D Background;
            // Variable for the backgrounds transparency
            public float BackgroundTransparency;
            // Variable that handles Texts
            public List<Line> Text = new List<Line>();
            // Variable that handles buttons
            public List<Button> Buttons = new List<Button>();
            // A selection variable for buttons
            public int ButtonSelection;

            // Constructor
            public Page()
            {
            }
            // Constructor with a list of buttons
            public Page(List<Button> buttons)
            {
                Buttons = buttons;
            }

            // Method for adding a background to a page, insert texture and it's transparency value (in %)
            public void AddBackground(Texture2D texture, float transparency)
            {
                Background = texture;
                BackgroundTransparency = transparency;
            }
        public void AddBackground(Texture2D texture)
        {
            Background = texture;
            BackgroundTransparency = 1f;
        }

        // Add a text, which font it shall have, what position, if the position is centralized (origin based), what the text is and what color
        public void AddText(SpriteFont font, Vector2 position, bool center, string newText, Color color)
            {
                Text.Add(new Line(font, position, center, newText, color));
            }

            // Add a button with no switching state, takes in font, position and text
            public void AddButton_Single(SpriteFont font, Vector2 position, string text, Action action)
            {
                Buttons.Add(new Button(font, position, text, action));
            }

            // Add a button with a switching state, takes in font, position and an array of text (each string in the array represent a state)
            public void AddButton_Multi(SpriteFont font, Vector2 position, string[] text, Action action)
            {
                Buttons.Add(new Button(font, position, text, action));
            }

            // Add multiple buttons with no switching state, takes in font, position, the spacing between texts, and an array of text (each string in the array represent a state)
            public void AddButtonList_Single(SpriteFont font, Vector2 startPosition, float spacing, string[] texts, Action[] actions)
            {
                for (int i = 0; i < texts.Length; i++)
                {
                    Buttons.Add(new Button(font, new Vector2(startPosition.X, startPosition.Y + i * spacing), texts[i], actions[i]));
                }
            }

            // Add multiple buttons with a switching state, takes in font, position, the spacing between texts, and a list of an array of text (each string in the array represent a state, each array represent a button)
            public void AddButtonList_Multi(SpriteFont font, Vector2 startPosition, float spacing, List<string[]> texts, Action action)
            {
                for (int i = 0; i < texts.Count; i++)
                {
                    Buttons.Add(new Button(font, new Vector2(startPosition.X, startPosition.Y + i * spacing), texts[i], action));
                }
            }

            // Navigation method, go down (loop and get back to top when reaching the end)
            public void SelectDown(bool loop)
            {
                if (loop)
                {
                    ButtonSelection++;
                    if (ButtonSelection > Buttons.Count - 1)
                    {
                        ButtonSelection = 0;
                    }
                }
                else
                {
                    if (ButtonSelection < Buttons.Count - 1)
                    {
                        ButtonSelection++;
                    }
                }
            }

            // Navigation method, go up (loop and get back to bottom when reaching the beginning)
            public void SelectUp(bool loop)
            {
                if (loop)
                {
                    ButtonSelection--;
                    if (ButtonSelection < 0)
                    {
                        ButtonSelection = Buttons.Count - 1;
                    }
                }
                else
                {
                    if (ButtonSelection > 0)
                    {
                        ButtonSelection--;
                    }
                }
            }

            // Update mouse collisions with buttons
            public void UpdateMouse(Rectangle mousePointer)
            {
                // For every button
                for (int i = 0; i < Buttons.Count; i++)
                {
                    // Check collisions
                    if (mousePointer.Intersects(Buttons[i].HitBox))
                    {
                        // The one that collides, make that one the selection
                        ButtonSelection = i;
                    }
                }
            }

            // Draw
            public void Draw(SpriteBatch spriteBatch, Rectangle screenSize)
            {
                // If background have a texture, draw it across the screen.
                if (Background != null)
                {
                    spriteBatch.Draw(Background, screenSize, new Color(new Vector4(BackgroundTransparency)));
                }
                // For every button, if i is same as selection, make that button have a highlight (white instead of gray)
                for (int i = 0; i < Buttons.Count; i++)
                {
                    Buttons[i].HighLight = i == ButtonSelection;
                    // Draw buttons
                    Buttons[i].Draw(spriteBatch);
                }

                // Draw every text in page
                for (int i = 0; i < Text.Count; i++)
                {
                    Text[i].Draw(spriteBatch);
                }
            }

        }

        // A class that simplifies a string with a font, position, if it is centered, what text and color
        public class Line
        {
            public SpriteFont Font;
            public Vector2 Position;
            public bool Center;
            public string Text;
            public Color Color;

            // Constructor
            public Line(SpriteFont font, Vector2 position, bool center, string text, Color color)
            {
                Font = font;
                Position = position;
                Center = center;
                Text = text;
                Color = color;
            }

            // Update a specific text
            public void UpdateLine(string text)
            {
                Text = text;
            }

            // Draw, if center then make adjustments so that position gets into origin, otherwise position is upper left corner of the string
            public void Draw(SpriteBatch spriteBatch)
            {
                if (Center)
                {
                    spriteBatch.DrawString(Font, Text, Position - new Vector2(Font.MeasureString(Text).X / 2, Font.MeasureString(Text).Y / 2), Color);
                }
                else
                {
                    spriteBatch.DrawString(Font, Text, Position, Color);
                }

            }

        }

        // A class that simplifies a button with a font, position and what text
        public class Button
        {
            private Action action;
            public SpriteFont Font;
            public Vector2 Position;
            // A rectangle for checking if it intersects with mouse
            public Rectangle HitBox;
            public string[] Text = new string[1];
            // A selection variable for button states
            public int StateSelection;
            // A bool that declares if the button shall be highlighted (different color than basic)
            public bool HighLight;

            // Constructor for multi state buttons
            public Button(SpriteFont font, Vector2 position, string[] text, Action action)
            {
                Font = font;
                Position = position;
                Text = text;
                StateSelection = 0;
                this.action = action;
            }

            // Constructor for single state buttons
            public Button(SpriteFont font, Vector2 position, string text, Action action)
            {
                Font = font;
                Position = position;
                Text[0] = text;
                StateSelection = 0;
                this.action = action;
            }

        // Navigation method, go right in button states (loop and get back to beginning when reaching the end)
        public void SelectRight(bool loop)
            {
                if (loop)
                {
                    StateSelection++;
                    if (StateSelection > Text.Length - 1)
                    {
                        StateSelection = 0;
                    }
                }
                else
                {
                    if (StateSelection < Text.Length - 1)
                    {
                        StateSelection++;
                    }
                }

            }

            // Navigation method, go left in button states (loop and get back to end when reaching the beginning)
            public void SelectLeft(bool loop)
            {
                if (loop)
                {
                    StateSelection--;
                    if (StateSelection < 0)
                    {
                        StateSelection = Text.Length - 1;
                    }
                }
                else
                {
                    if (StateSelection > 0)
                    {
                        StateSelection--;
                    }
                }
            }

            public void Run()
            {
                action.Invoke();
            }

            // Draw
            public void Draw(SpriteBatch spriteBatch)
            {
                // HitBox rectangle is text position & font measurement of text to length and height
                HitBox = new Rectangle((int)Position.X, (int)Position.Y, (int)Font.MeasureString(Text[StateSelection]).X, (int)Font.MeasureString(Text[StateSelection]).Y);
                // Draw text (based on selection) with font and everything, if highlight is true, then úse white, else use gray
                spriteBatch.DrawString(Font, Text[StateSelection], Position, HighLight ? Color.White : new Color(new Vector4(new Vector3(1), 0.3f)));
            }



        }


    }

