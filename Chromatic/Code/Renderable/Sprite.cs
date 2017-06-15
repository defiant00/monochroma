using DataTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Chromatic.Code.Renderable
{
    public class Sprite
    {
        Dictionary<string, SpriteData> AllData;
        SpriteData CurrentData;

        string _Animation;
        public string Animation { get { return _Animation; } }
        public Vector2 Position;
        public float Z;
        public float Rotation;

        int CurrFrame;
        double TimeLeft;
        Vector2 Origin;

        public Sprite(Dictionary<string, SpriteData> data, string animation, Random random = null)
        {
            AllData = data;
            Play(animation, random);
        }

        public bool Contains(int x, int y)
        {
            Vector2 tl = Position - Origin;
            Rectangle r = CurrentData.Frames[0].Rectangle;
            return !(x < tl.X || y < tl.Y || x > (tl.X + r.Width) || y > (tl.Y + r.Height));
        }

        public void Play(string animation, Random random = null)
        {
            _Animation = animation;
            CurrentData = AllData[_Animation];
            Origin = (CurrentData.Frames[0].Rectangle.Size.ToVector2() / 2).Floor();
            if (random == null || CurrentData.Frames.Length < 2) { Reset(); }
            else
            {
                CurrFrame = random.Next(CurrentData.Frames.Length);
                TimeLeft = random.NextDouble() * CurrentData.Frames[CurrFrame].DisplayTime;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture, Vector2 offset, Color color)
        {
            var f = CurrentData.Frames[CurrFrame];
            spriteBatch.Draw(texture, Position + offset, f.Rectangle, color, Rotation + f.Rotation, Origin, 1, f.Effects, Z);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture, Vector2 offset)
        {
            Draw(spriteBatch, texture, offset, Color.White);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture, Rectangle destRect, Color color)
        {
            var f = CurrentData.Frames[CurrFrame];
            spriteBatch.Draw(texture, destRect, f.Rectangle, color);
        }

        public void Update(double ms)
        {
            if (CurrentData.Frames.Length > 1)
            {
                TimeLeft -= ms;
                while (TimeLeft < 0)
                {
                    CurrFrame++;
                    if (CurrFrame >= CurrentData.Frames.Length)
                    {
                        CurrFrame = CurrentData.LoopIndex;
                    }
                    TimeLeft += CurrentData.Frames[CurrFrame].DisplayTime;
                }
            }
        }

        public void Reset()
        {
            CurrFrame = 0;
            TimeLeft = CurrentData.Frames[0].DisplayTime;
        }
    }
}
