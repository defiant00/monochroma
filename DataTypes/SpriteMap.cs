using System.Collections.Generic;

namespace DataTypes
{
	public class SpriteMap
    {
		public List<Area> Areas { get; set; }
		public List<Sprite> Sprites { get; set; }

		public SpriteMap()
		{
			Areas = new List<Area>();
			Sprites = new List<Sprite>();
		}

		public class Area
		{
			public int X { get; set; }
			public int Y { get; set; }
			public int Width { get; set; }
			public int Height { get; set; }
		}

		public class Sprite
		{
			public string Name { get; set; }
			public List<int> Indices { get; set; }
			public int FrameRate { get; set; }
			public bool Looped { get; set; }
			public bool FlipX { get; set; }
			public bool FlipY { get; set; }
		}
	}
}
