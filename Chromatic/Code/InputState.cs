using Microsoft.Xna.Framework;
using System;

namespace Chromatic.Code
{
    public class InputState
    {
        public KeyboardState Keyboard;
        public MouseState Mouse;

        public InputState(Rectangle area)
        {
            Keyboard = new KeyboardState();
            Mouse = new MouseState(area);
        }

        public void Update()
        {
            Keyboard.Update();
            Mouse.Update();
        }

        static ButtonState GetState(bool current, bool prior)
        {
            if (current && prior) { return ButtonState.Pressed; }
            else if (current) { return ButtonState.JustPressed; }
            else if (prior) { return ButtonState.JustReleased; }
            return ButtonState.Released;
        }

        public enum ButtonState
        {
            Released,
            JustPressed,
            Pressed,
            JustReleased,
        }

        public class KeyboardState
        {
            private Microsoft.Xna.Framework.Input.KeyboardState CurrentState, PriorState;

            public void Update()
            {
                PriorState = CurrentState;
                CurrentState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            }

            public ButtonState this[Microsoft.Xna.Framework.Input.Keys key]
            {
                get { return GetState(CurrentState.IsKeyDown(key), PriorState.IsKeyDown(key)); }
            }

            public bool IsDown(Microsoft.Xna.Framework.Input.Keys key)
            {
                return CurrentState.IsKeyDown(key);
            }
        }

        public class MouseState
        {
            public ButtonState LeftButton { get { return GetState(LeftPressed, PriorLeftPressed); } }
            public ButtonState RightButton { get { return GetState(RightPressed, PriorRightPressed); } }
			public int ScrollWheel { get { return Scroll - PriorScroll; } }
            public int RawX, RawY, X, Y;

            bool LeftPressed, RightPressed, PriorLeftPressed, PriorRightPressed;
            int OutputOffsetX, OutputOffsetY, Scroll, PriorScroll;
            float OutputScale;

            public MouseState(Rectangle area) { SetupArea(area); }

            public void Update()
            {
                PriorLeftPressed = LeftPressed;
                PriorRightPressed = RightPressed;
				PriorScroll = Scroll;

                var m = Microsoft.Xna.Framework.Input.Mouse.GetState();
                LeftPressed = m.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
                RightPressed = m.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
                RawX = m.X;
                RawY = m.Y;
				Scroll = m.ScrollWheelValue;
                X = (int)Math.Floor((RawX - OutputOffsetX) * OutputScale);
                Y = (int)Math.Floor((RawY - OutputOffsetY) * OutputScale);
            }

            public void SetupArea(Rectangle area)
            {
                OutputOffsetX = area.X;
                OutputOffsetY = area.Y;
                OutputScale = 1280f / area.Width;
            }
        }
    }
}
