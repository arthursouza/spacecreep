using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceCreep.Client.Lib.Input
{
    public static class InputManager
    {
        public static Vector2 MousePosition => new Vector2(MouseState.X, MouseState.Y);

        public static Point MousePositionPoint => new Point(MouseState.X, MouseState.Y);

        public static MouseState MouseState { get; set; }
        public static KeyboardState KeyboardState { get; set; }
        public static KeyboardState LastKeyboardState { get; set; }
        public static TimeSpan LastMouseLeftClick { get; set; }
        
        public static Vector2 MouseToMapVector => new Vector2((int) (MousePosition.X + Camera.Position.X), (int) (MousePosition.Y + Camera.Position.Y));

        public static Vector2 MovementVector { get; set; }
        
        public static MouseState LastMouseState { get; set; }

        public static bool KeyPress(Keys key)
        {
            return KeyboardState.IsKeyDown(key) && LastKeyboardState.IsKeyUp(key);
        }

        private enum InputType
        {
            Joystick,
            Keyboard
        }
    }
}