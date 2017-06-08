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
        public bool FlipH { get; set; }
        public bool FlipV { get; set; }
        public float Rotation { get; set; }
    }
}
