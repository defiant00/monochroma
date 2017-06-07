using Microsoft.Xna.Framework;

namespace DataTypes
{
    public class SpriteData
    {
        public Frame[] Frames { get; set; }
        public int LoopIndex { get; set; }

        public class Frame
        {
            public Rectangle Rect { get; set; }
            public double DisplayTime { get; set; }
            public bool FlipX { get; set; }
            public bool FlipY { get; set; }
            public float Rotation { get; set; }
        }
    }
}
