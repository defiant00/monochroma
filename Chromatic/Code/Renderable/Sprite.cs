using DataTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Chromatic.Code.Renderable
{
	public class Sprite
	{
		public SpriteData data { get; set; }

        string name;
		int currFrame;
		double timeLeft;
		Vector2 origin;

		public Sprite(string name, SpriteData data, Random random = null)
		{
            this.name = name;
			this.data = data;

			origin = data.Frames[0].Rectangle.Size.ToVector2() / 2;

			if (random == null || data.Frames.Length < 2) { Reset(); }
			else
			{
				currFrame = random.Next(data.Frames.Length);
				timeLeft = random.NextDouble() * data.Frames[currFrame].DisplayTime;
			}
		}

		public void Draw(SpriteBatch spriteBatch, Texture2D texture, Vector2 offset, float depth = 0, float rotation = 0)
		{
			var f = data.Frames[currFrame];
			spriteBatch.Draw(texture, offset, f.Rectangle, Color.White, f.Rotation + rotation, origin, 1, f.Effects, depth);
		}

		public void Update(double ms)
		{
			if (data.Frames.Length > 1)
			{
				timeLeft -= ms;
				while (timeLeft < 0)
				{
					currFrame++;
					if (currFrame >= data.Frames.Length)
					{
						currFrame = data.LoopIndex;
					}
					timeLeft += data.Frames[currFrame].DisplayTime;
				}
			}
		}

		public void Reset()
		{
			currFrame = 0;
			timeLeft = data.Frames[0].DisplayTime;
		}
	}
}
