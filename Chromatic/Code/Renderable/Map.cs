using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Chromatic.Code.Renderable
{
    class Map
    {
        int width, height;
        Sprite[,] tiles;
        List<Decal> decals;
        Texture2D texture;

        public Map() { }

        public Map(Chromatic game, int width, int height, string fill)
        {
            var sd = game.spriteMap[fill];
            texture = game.spriteMapTex;

            this.width = width;
            this.height = height;
            tiles = new Sprite[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tiles[x, y] = new Sprite(fill, sd, game.random);
                }
            }

            decals = new List<Decal>();
            for (int i = 0; i < 250; i++)
            {
                decals.Add(new Decal
                {
                    Sprite = new Sprite("d_flower1", game.spriteMap["d_flower1"], game.random),
                    Position = new Vector2(game.random.Next((width - 1) * 32), game.random.Next((height - 1) * 32)),
                });
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tiles[x, y].Draw(spriteBatch, texture, new Vector2(x * 32, y * 32) + offset);
                }
            }
            foreach (var d in decals) { d.Sprite.Draw(spriteBatch, texture, d.Position + offset); }
        }

        public void Update(double ms)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tiles[x, y].Update(ms);
                }
            }
            foreach (var d in decals) { d.Sprite.Update(ms); }
        }

        class Decal
        {
            public Sprite Sprite { get; set; }
            public Vector2 Position { get; set; }
        }
    }
}
