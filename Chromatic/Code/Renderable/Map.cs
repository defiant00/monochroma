using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Chromatic.Code.Renderable
{
	class Map
	{
		public int Width, Height;
		public Sprite[,] Tiles;
		List<Sprite> Decals;
		Texture2D MapTexture;

		public Map() { }

		public Map(Chromatic game, int width, int height, string fill)
		{
			MapTexture = game.SpriteMapTex;

			Width = width;
			Height = height;
			Tiles = new Sprite[Width, Height];
			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					Tiles[x, y] = new Sprite(game.SpriteMap, fill) { Position = new Vector2(x * 32, y * 32) };
				}
			}

			Decals = new List<Sprite>();
			for (int i = 0; i < 1000; i++)
			{
				Decals.Add(new Sprite(game.SpriteMap, "d_flower1", game.Random)
				{
					Position = new Vector2(game.Random.Next((Width - 1) * 32) + 16, game.Random.Next((Height - 1) * 32) + 16)
				});
			}
		}

		public void Draw(SpriteBatch spriteBatch, Vector2 offset)
		{
            Vector2 tileOffset = offset + new Vector2(16, 16);
			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					Tiles[x, y].Draw(spriteBatch, MapTexture, tileOffset);
				}
			}
			foreach (var d in Decals) { d.Draw(spriteBatch, MapTexture, offset); }
		}

		public void Update(double ms)
		{
			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					Tiles[x, y].Update(ms);
				}
			}
			foreach (var d in Decals) { d.Update(ms); }
		}
	}
}
