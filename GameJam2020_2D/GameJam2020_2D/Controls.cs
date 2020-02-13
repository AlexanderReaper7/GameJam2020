using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameJam2020_2D
{
    public class Controls
    {
        // For this class, remember to set previousXState = currentXState in other classes so that you can use the methods

        public KeyboardState currentKeyboardState;

        public KeyboardState previousKeyboardState;

        public MouseState currentMouseState;

        public MouseState previousMouseState;

        public GamePadState currentGamePadState;

        public GamePadState previousGamePadState;

        private PlayerIndex playerIndex;

        public Controls(KeyboardState CurrentKeyboardState, KeyboardState PreviousKeyboardState)
        {
            this.currentKeyboardState = CurrentKeyboardState;

            this.previousKeyboardState = PreviousKeyboardState;
        }

        public Controls(KeyboardState CurrentKeyboardState, KeyboardState PreviousKeyboardState, MouseState CurrentMouseState, MouseState PreviousMouseState)
        {
            this.currentKeyboardState = CurrentKeyboardState;

            this.previousKeyboardState = PreviousKeyboardState;

            this.currentMouseState = CurrentMouseState;

            this.previousMouseState = PreviousMouseState;
        }

        public Controls(GamePadState CurrentGamePadState, GamePadState PreviousGamePadState, PlayerIndex PlayerIndex)
        {
            this.currentGamePadState = CurrentGamePadState;

            this.previousGamePadState = PreviousGamePadState;

            this.playerIndex = PlayerIndex;
        }

        public void Update(GameTime Gametime)
        {
            UpdateGamePadControls(Gametime);

            UpdateKeyboardControls(Gametime);
        }

        public void UpdateKeyboardControls(GameTime GameTime)
        {
            currentKeyboardState = Keyboard.GetState();

            currentMouseState = Mouse.GetState();
        }

        public void UpdateGamePadControls(GameTime GameTime)
        {
            currentGamePadState = GamePad.GetState(playerIndex);
        }

        public bool SingleActivationKey(Keys Key)
        {
            return currentKeyboardState.IsKeyDown(Key) && previousKeyboardState.IsKeyUp(Key);
        }

        public bool HoldingDownKey(Keys Key)
        {
            return currentKeyboardState.IsKeyDown(Key);
        }

        public bool SingleActivationButton(Buttons Button)
        {
            return currentGamePadState.IsButtonDown(Button) && previousGamePadState.IsButtonUp(Button);
        }

        public bool HoldingDownButton(Buttons Button)
        {
            return currentGamePadState.IsButtonDown(Button);
        }
    }
}
