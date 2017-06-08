using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
            public SpriteEffects Effects { get; set; }
            public float Rotation { get; set; }
        }
    }
}
