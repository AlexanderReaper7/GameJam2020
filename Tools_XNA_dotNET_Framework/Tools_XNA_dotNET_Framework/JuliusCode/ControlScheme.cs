using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tools_XNA.OldCode
{
    // A class that update all the different bools from inputs, easily make a control scheme, also handles the mouse position/intersection on screen
    public class ControlScheme
    {
        public bool LeftGame { get; set; }
        public bool RightGame { get; set; }

        public bool Up { get; set; }
        public bool Down { get; set; }
        public bool Left { get; set; }
        public bool Right { get; set; }

        public bool Select { get; set; }
        public bool SelectMouse { get; set; }
        public bool Back { get; set; }

        public bool IsKeyUp { get; set; }

        public Rectangle MousePointer { get; set; }
        public Rectangle PreviousMousePosition { get; set; }
        

        public void Update()
        {
            PreviousMousePosition = MousePointer;
            MousePointer = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1);

            // Left In Game
            if (Keyboard.GetState().IsKeyDown(Keys.A) ||
                Keyboard.GetState().IsKeyDown(Keys.Left) ||
                Mouse.GetState().LeftButton == ButtonState.Pressed)
                LeftGame = true;
            else LeftGame = false;

            // Right In Game 
            if (Keyboard.GetState().IsKeyDown(Keys.D) ||
                Keyboard.GetState().IsKeyDown(Keys.Right) ||
                Mouse.GetState().RightButton == ButtonState.Pressed)
                RightGame = true;
            else RightGame = false;

            // Up
            if (Keyboard.GetState().IsKeyDown(Keys.W) ||
                Keyboard.GetState().IsKeyDown(Keys.Up))
                Up = true;
            else Up = false;

            // Down
            if (Keyboard.GetState().IsKeyDown(Keys.S) ||
                Keyboard.GetState().IsKeyDown(Keys.Down))
                Down = true;
            else Down = false;

            // Left
            if (Keyboard.GetState().IsKeyDown(Keys.A) ||
                Keyboard.GetState().IsKeyDown(Keys.Left))
                Left = true;
            else Left = false;

            // Right
            if (Keyboard.GetState().IsKeyDown(Keys.D) ||
                Keyboard.GetState().IsKeyDown(Keys.Right))
                Right = true;
            else Right = false;

            // Selection
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) ||
                Keyboard.GetState().IsKeyDown(Keys.Space))
                Select = true;
            else Select = false;
            
            // MouseSelection
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                SelectMouse = true;
            else SelectMouse = false;

            // Back
            if (Keyboard.GetState().IsKeyDown(Keys.Back) ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Back = true;
            else Back = false;

            // Looks if any of the navigation keys are up, this can be used as a single activation key
            if (!Up && !Down && !Left && !Right && !Select && !SelectMouse)
            {
                IsKeyUp = true;
            }
            else
            {
                IsKeyUp = false;
            }

        }

        // alpha is used to make the texture fade away, it's a cool but unnecessary effect
        public void DrawMouse(SpriteBatch spriteBatch, Texture2D mouseTexture, float alpha)
        {
            spriteBatch.Draw(mouseTexture, new Vector2(MousePointer.X, MousePointer.Y), new Color(new Vector4(alpha)));
        }


    }
}
