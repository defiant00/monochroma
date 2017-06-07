using System.Collections.Generic;

namespace GenSpriteMap
{
	public class SpriteInput
	{
		public string Name { get; set; }
		public List<SpriteInputFrame> Frames { get; set; }
		public int LoopIndex { get; set; }
	}

    public class SpriteInputFrame
    {
        public string Name { get; set; }
        public double DisplayTime { get; set; }
        public bool FlipX { get; set; }
        public bool FlipY { get; set; }
        public float Rotation { get; set; }
    }
}
