﻿using Microsoft.Xna.Framework;
using System;

namespace Chromatic.Code
{
	public class InputState
	{
		public MouseState Mouse;

		public InputState(Rectangle area)
		{
			Mouse = new MouseState(area);
		}

		public void Update()
		{
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

		public class MouseState
		{
			public ButtonState LeftButton { get { return GetState(LeftPressed, LeftPriorPressed); } }
			public ButtonState RightButton { get { return GetState(RightPressed, RightPriorPressed); } }
			public int RawX, RawY, X, Y;
			bool LeftPressed, RightPressed, LeftPriorPressed, RightPriorPressed;
			int OutputOffsetX, OutputOffsetY;
			float OutputScale;

			public MouseState(Rectangle area) { SetupArea(area); }

			public void Update()
			{
				LeftPriorPressed = LeftPressed;
				RightPriorPressed = RightPressed;

				var m = Microsoft.Xna.Framework.Input.Mouse.GetState();
				LeftPressed = m.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
				RightPressed = m.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
				RawX = m.X;
				RawY = m.Y;
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