using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Chromatic.Code.Renderable
{
    public class Map
    {
        public int Width, Height;
        public Sprite[,] Tiles;
        public bool[,] Blocks;
        public List<Sprite> Decals;
        Texture2D MapTexture;
        Sprite Block;

        public Map(Chromatic game, int width, int height, string fill)
        {
            MapTexture = game.SpriteMapTex;
            Block = new Sprite(game.SpriteMap, "indicator");

            Width = width;
            Height = height;
            Tiles = new Sprite[Width + 1, Height + 1];
            for (int y = Height; y >= 0; y--)
            {
                for (int x = Width; x >= 0; x--)
                {
                    Tiles[x, y] = new Sprite(game.SpriteMap, fill) { Position = new Vector2(x * 32, y * 32) };
                }
            }

            Blocks = new bool[Width, Height];
            Decals = new List<Sprite>();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset, bool drawDecals = true, bool drawBlocks = false)
        {
            for (int y = Height; y >= 0; y--)
            {
                for (int x = Width; x >= 0; x--)
                {
                    Tiles[x, y].Draw(spriteBatch, MapTexture, offset);
                }
            }
            if (drawDecals) { foreach (var d in Decals) { d.Draw(spriteBatch, MapTexture, offset); } }
            if (drawBlocks)
            {
                Vector2 blockOffset = offset + new Vector2(16, 16);
                for (int y = Height - 1; y >= 0; y--)
                {
                    for (int x = Width - 1; x >= 0; x--)
                    {
                        if (Blocks[x, y])
                        {
                            Block.Draw(spriteBatch, MapTexture, new Vector2(x * 32, y * 32) + blockOffset, Color.Red);
                        }
                    }
                }
            }
        }

        public void Update(double ms)
        {
            for (int y = Height; y >= 0; y--)
            {
                for (int x = Width; x >= 0; x--)
                {
                    Tiles[x, y].Update(ms);
                }
            }
            foreach (var d in Decals) { d.Update(ms); }
        }
    }
}
