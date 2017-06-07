using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace DataTypes
{
	public class SpriteMap
    {
		public List<Rectangle> Areas { get; set; }
		public Dictionary<string, Sprite> Sprites { get; set; }

		public SpriteMap()
		{
			Areas = new List<Rectangle>();
			Sprites = new Dictionary<string, Sprite>();
		}

		public class Sprite
		{
			public List<int> Indices { get; set; }
			public int FrameRate { get; set; }
			public bool Looped { get; set; }
			public bool FlipX { get; set; }
			public bool FlipY { get; set; }
		}
	}
}
