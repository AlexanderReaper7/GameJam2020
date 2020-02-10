using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tools_XNA.OldCode
{
    // A class that manages pages and buttons inside those pages
    public class Menu
    {
        private Camera2D camera;

        // List for all the pages in your desired menu
        public List<Page> Pages = new List<Page>();
        // A selection variable for pages
        public int CurrentPage;

        public static bool previousMouseDown = false;
        public static bool currentMouseDown = false;

        // Constructor with desired amount of pages
        public Menu(int amountOfPages)
        {

            for (int i = 0; i < amountOfPages; i++)
            {
                Pages.Add(new Page());
            }
        }

        // Easy navigation of buttons and button states, takes in an input of up, down, left and right
        /*
        public void Navigation(bool up, bool down, bool left, bool right)
        {
            if (up)
            {
                // Make the selection go up in current page
                Pages[CurrentPage].SelectUp(false);
            }
            else if (down)
            {
                // Make the selection go down in current page
                Pages[CurrentPage].SelectDown(false);
            }
            else if (left)
            {
                // Make the selection change to left in current page's current button
                Pages[CurrentPage].Buttons[Pages[CurrentPage].ButtonSelection].SelectLeft(false);
            }
            else if (right)
            {
                // Make the selection change to right in current page's current button
                Pages[CurrentPage].Buttons[Pages[CurrentPage].ButtonSelection].SelectRight(false);
            }
        }
        */
        /// <summary>
        /// Updates menu logic
        /// </summary>
        public void Update()
        {
            previousMouseDown = currentMouseDown;
            currentMouseDown = Mouse.GetState().LeftButton == ButtonState.Pressed;

            UpdateMouse(camera.ScreenToWorld(Mouse.GetState().Position().ToVector2()).ToPoint());
        }

        // Update mouse collision with all buttons
        private void UpdateMouse(Point mousePosition)
        {
            Pages[CurrentPage].UpdateMouse(mousePosition);
        }

        // Draw the current selected page
        public void Draw(SpriteBatch spriteBatch)
        {
            Pages[CurrentPage].Draw(spriteBatch);
        }

    }

    public interface IButton
    {
        Action Action { get; set; }
        Rectangle DestinationRectangle { get; set; }
        bool IsHighlighted { get; set; }
        void Draw(SpriteBatch spriteBatch);
    }


    // A class that handles texts, background and buttons
    public class Page
    {
        // Variable for custom background
        public Texture2D Background;
        // Variable for the backgrounds transparency
        public float BackgroundTransparency;
        // Images
        public List<Image> Images = new List<Image>();
        public List<AdvancedImage> AdvancedImages = new List<AdvancedImage>();
        // Variable that handles Texts
        public List<Line> Text = new List<Line>();
        // Variable that handles buttons
        public List<IButton> Buttons = new List<IButton>();
        //
        // A selection variable for buttons

        // Method for adding a background to a page, insert texture and it's transparency value (in %)
        public void AddBackground(Texture2D texture, float transparency)
        {
            Background = texture;
            BackgroundTransparency = transparency;
        }

        public void AddAdvancedImage(params AdvancedImage[] image)
        {
            AdvancedImages.AddRange(image);
        }

        public void AddImage(params Image[] image)
        {
            Images.AddRange(image);
        }

        // Add a text, which font it shall have, what position, if the position is centralized (origin based), what the text is and what color
        public void AddText(SpriteFont font, Vector2 position, bool center, string newText, Color color)
        {
            Text.Add(new Line(font, position, center, newText, color));
        }

        // Add a button with no switching state, takes in font, position and text
        public void AddButton(Button button)
        {
            Buttons.Add(button);
        }

        // Add multiple buttons with no switching state, takes in font, position, the spacing between texts, and an array of text (each string in the array represent a state)
        public void AddButtonList(SpriteFont font, Rectangle rectangle, float spacing, string[] texts, Vector2 textPadding, Color color, Color highlightedColor, Action[] actions)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                Buttons.Add(new Button(font, new Rectangle(rectangle.X, (int)(rectangle.Y + i*spacing), rectangle.Width, rectangle.Height), texts[i], textPadding, color, highlightedColor, actions[i]));                
            }
        }

        public void AddImageButton(params ImageButton[] imageButton)
        {
            Buttons.AddRange(imageButton);
        }

        public IButton GetSelectedButton(Point mousePosition)
        {
            foreach (IButton button in Buttons)
            {
                if (button.DestinationRectangle.Contains(mousePosition))
                {
                    return button;
                }
            }

            // return null if no button is selected
            return null;
        }

        // Update mouse collisions with buttons TODO: add depth to collision
        public void UpdateMouse(Point mousePosition)
        {
            // Get selected button
            IButton selectedButton = GetSelectedButton(mousePosition);

            // highlight only the selected button
            foreach (IButton button in Buttons)
            {
                button.IsHighlighted = button == selectedButton;
            }


            // if no button is selected, skip
            if (selectedButton == null) return; 



            // if mouse is down run the button´s action
            if (Menu.currentMouseDown == true && Menu.previousMouseDown == false) selectedButton.Action.Invoke();
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw background if it exists
            if (Background != null)
            {
                spriteBatch.Draw(Background, new Rectangle(0, 0, 1080, 720), new Color(new Vector4(BackgroundTransparency)));
            }

            // Draw every image
            foreach (Image image in Images)
            {
                image.Draw(spriteBatch);
            }

            foreach (AdvancedImage advancedImage in AdvancedImages)
            {
                advancedImage.Draw(spriteBatch);
            }

            // Draw buttons
            foreach (IButton button in Buttons)
            {
                button.Draw(spriteBatch);
            }

            // Draw every text
            foreach (Line line in Text)
            {
                line.Draw(spriteBatch);
            }
        }

    }

    /// <summary>
    /// Image but with rotation and origin
    /// </summary>
    public class AdvancedImage
    {
        private Texture2D texture;
        private Rectangle destinationRectangle;
        public Color tint;
        public float rotation;
        private Vector2 origin;

        public AdvancedImage(Texture2D texture, Rectangle destinationRectangle, Color tint, float rotation, Vector2 origin)
        {
            this.texture = texture;
            this.destinationRectangle = destinationRectangle;
            this.tint = tint;
            this.rotation = rotation;
            this.origin = origin;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, destinationRectangle, null, tint, rotation,origin,SpriteEffects.None, 1f);
        }
    }


    public class Image
    {
        private Texture2D texture;
        private Rectangle destinationRectangle;
        private Color tint;

        /// <summary>
        /// Creates an image object with a tint
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="destinationRectangle">Position and size</param>
        /// <param name="tint"></param>
        public Image(Texture2D texture, Rectangle destinationRectangle, Color tint)
        {
            this.texture = texture;
            this.destinationRectangle = destinationRectangle;
            this.tint = tint;
        }

        /// <summary>
        /// Creates an image object
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="destinationRectangle">Position and size</param>
        public Image(Texture2D texture, Rectangle destinationRectangle)
        {
            this.texture = texture;
            this.destinationRectangle = destinationRectangle;
            tint = Color.White;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, destinationRectangle, tint);
        }
    }

    // A class that simplifies a string with a font, position, if it is centered, what text and color
    public class Line
    {
        private SpriteFont font;
        private Vector2 position;
        private bool center;
        private string text;
        private Color color;

        // Constructor
        public Line(SpriteFont font, Vector2 position, bool center, string text, Color color)
        {
            this.font = font;
            this.position = position;
            this.center = center;
            this.text = text;
            this.color = color;
        }

        // Update a specific text
        public void UpdateLine(string text)
        {
            this.text = text;
        }

        // Draw, if center then make adjustments so that position gets into origin, otherwise position is upper left corner of the string
        public void Draw(SpriteBatch spriteBatch)
        {
            if (center)
            {
                spriteBatch.DrawString(font, text, position - new Vector2(font.MeasureString(text).X/2, font.MeasureString(text).Y/2), color);
            }
            else
            {
                spriteBatch.DrawString(font, text, position, color);
            }
            
        }

    }

    // A class that simplifies a button with a font, position and what text
    public class Button : IButton
    {

        public Action Action { get; set; }
        public Rectangle DestinationRectangle { get; set; }
        public bool IsHighlighted { get; set; }

        // Upper left corner of the rectangle
        private Vector2 VectorPosition => DestinationRectangle.Location.ToVector2();

        private SpriteFont font;
        // Origin for text
        private Vector2 textPadding;
        private string text;
        private Color highLightedColor;
        private Color color;


        // Constructor for button
        public Button(SpriteFont font, Rectangle destinationRectangle, string text, Vector2 textPadding, Color color, Color highLightedColor, Action action)
        {
            this.font = font;
            DestinationRectangle = destinationRectangle;
            this.text = text;
            this.textPadding = textPadding;
            this.color = color;
            this.highLightedColor = highLightedColor;
            Action = action;
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw background
            spriteBatch.DrawFilledRectangle(DestinationRectangle, IsHighlighted ? highLightedColor : color);
            
            // Draw text
            spriteBatch.DrawString(font, text, VectorPosition, Color.LightGray, 0f, textPadding, Vector2.One, SpriteEffects.None, 1);
        }
    }

    public class ImageButton : IButton
    {
        public Action Action { get; set; }
        public Rectangle DestinationRectangle { get; set; }
        public bool IsHighlighted { get; set; }

        private Texture2D texture;
        private Color tint;
        private Color highlightedTint;


        /// <summary>
        /// Creates an image button with a tint
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="destinationRectangle">Position and size</param>
        /// <param name="tint"></param>
        /// <param name="highlightedTint"></param>
        /// <param name="action"></param>
        public ImageButton(Texture2D texture, Rectangle destinationRectangle, Color tint, Color highlightedTint, Action action)
        {
            this.texture = texture;
            this.DestinationRectangle = destinationRectangle;
            this.tint = tint;
            this.highlightedTint = highlightedTint;
            Action = action;
        }

        /// <summary>
        /// Creates an image button without color
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="destinationRectangle">Position and size</param>
        /// <param name="action"></param>
        public ImageButton(Texture2D texture, Rectangle destinationRectangle, Action action)
        {
            this.texture = texture;
            DestinationRectangle = destinationRectangle;
            tint = Color.White;
            highlightedTint = Color.White;
            Action = action;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, DestinationRectangle, IsHighlighted ? highlightedTint : tint);
        }
    }

}
