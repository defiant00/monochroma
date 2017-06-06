using System.Collections.Generic;

namespace GenSpriteMap
{
	public class SpriteInput
	{
		public string Name { get; set; }
		public List<string> Frames { get; set; }
		public int FrameRate { get; set; }
		public bool Looped { get; set; }
		public bool FlipX { get; set; }
		public bool FlipY { get; set; }
	}
}
