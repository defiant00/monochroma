using Microsoft.Xna.Framework;
using System;

namespace Chromatic.Code
{
	public static class Extensions
	{
		public static Vector2 Floor(this Vector2 v)
		{
			return new Vector2((float)Math.Floor(v.X), (float)Math.Floor(v.Y));
		}
	}
}
