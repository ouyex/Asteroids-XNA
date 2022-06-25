using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Asteroids_XNA
{
    public static class InputHelper
    {
        private static KeyboardState currentState;
        private static KeyboardState previousState;

        public static KeyboardState UpdateState()
        {
            previousState = currentState;
            currentState = Keyboard.GetState();
            return currentState;
        }

        public static bool GetKey(Keys key)
        {
            return currentState.IsKeyDown(key);
        }

        public static bool GetKeyPressed(Keys key)
        {
            return currentState.IsKeyDown(key) && !previousState.IsKeyDown(key);
        }
    }
}
