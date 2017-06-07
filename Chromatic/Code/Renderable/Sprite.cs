using DataTypes;
using System;

namespace Chromatic.Code.Renderable
{
    public class Sprite
    {
        public SpriteData data { get; set; }

        int currFrame;
        double timeLeft;

        public Sprite(SpriteData data, Random random = null)
        {
            this.data = data;

            if (random == null || data.Frames.Length < 2) { Reset(); }
            else
            {
                currFrame = random.Next(data.Frames.Length);
                timeLeft = random.NextDouble() * data.Frames[currFrame].DisplayTime;
            }
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
