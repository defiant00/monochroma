using DataTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Chromatic.Code.Renderable
{
	public class Map
	{
		public int Width, Height;
		public Sprite[,] Tiles;
		public bool[,] Blocks;
		public List<Sprite> Decals;
		public List<Light> Lights;
		public Color LightBase;
		Chromatic Game;
		Sprite Block;

		public Map(Chromatic game, MapData data)
		{
			Game = game;
			Block = new Sprite(Game.SpriteMap, "indicator");
			LightBase = data.LightBase;

			Width = data.Tiles.Length - 1;
			Height = data.Tiles[0].Length - 1;
			Tiles = new Sprite[Width + 1, Height + 1];
			for (int y = Height; y >= 0; y--)
			{
				for (int x = Width; x >= 0; x--)
				{
					Tiles[x, y] = new Sprite(Game.SpriteMap, data.Tiles[x][y].Animation)
					{
						Position = new Vector2(x * 32, y * 32),
						Rotation = data.Tiles[x][y].Rotation
					};
				}
			}

			Blocks = new bool[Width, Height];
			for (int y = Height - 1; y >= 0; y--)
			{
				for (int x = Width - 1; x >= 0; x--)
				{
					Blocks[x, y] = data.Blocks[x][y];
				}
			}

			Decals = data.Decals.Select(d => new Sprite(Game.SpriteMap, d.Animation)
			{
				Position = d.Position,
				Rotation = d.Rotation,
			}).ToList();

			Lights = data.Lights.Select(l => new Light
			{
				Rectangle = l.Rectangle,
				Color = l.Color,
			}).ToList();
		}

		public Map(Chromatic game, int width, int height, string fill)
		{
			Game = game;
			Block = new Sprite(Game.SpriteMap, "indicator");
			LightBase = Color.White;

			Width = width;
			Height = height;
			Tiles = new Sprite[Width + 1, Height + 1];
			for (int y = Height; y >= 0; y--)
			{
				for (int x = Width; x >= 0; x--)
				{
					Tiles[x, y] = new Sprite(Game.SpriteMap, fill) { Position = new Vector2(x * 32, y * 32) };
				}
			}

			Blocks = new bool[Width, Height];
			Decals = new List<Sprite>();
			Lights = new List<Light>();
		}

		public MapData ToMapData()
		{
			var data = new MapData { LightBase = LightBase };
			data.Tiles = new MapData.Tile[Width + 1][];
			for (int x = 0; x < Width + 1; x++)
			{
				data.Tiles[x] = new MapData.Tile[Height + 1];
				for (int y = 0; y < Height + 1; y++)
				{
					data.Tiles[x][y] = new MapData.Tile
					{
						Animation = Tiles[x, y].Animation,
						Rotation = Tiles[x, y].Rotation,
					};
				}
			}

			data.Blocks = new bool[Width][];
			for (int x = 0; x < Width; x++)
			{
				data.Blocks[x] = new bool[Height];
				for (int y = 0; y < Height; y++)
				{
					data.Blocks[x][y] = Blocks[x, y];
				}
			}

			data.Decals = Decals.Select(d => new MapData.Decal
			{
				Animation = d.Animation,
				Position = d.Position,
				Rotation = d.Rotation,
			}).ToArray();

			data.Lights = Lights.Select(l => new MapData.Light
			{
				Rectangle = l.Rectangle,
				Color = l.Color,
			}).ToArray();

			return data;
		}

		public void Draw(SpriteBatch spriteBatch, Vector2 offset, bool drawDecals = true, bool drawBlocks = false)
		{
			for (int y = Height; y >= 0; y--)
			{
				for (int x = Width; x >= 0; x--)
				{
					Tiles[x, y].Draw(spriteBatch, Game.SpriteMapTex, offset);
				}
			}
			if (drawDecals) { foreach (var d in Decals) { d.Draw(spriteBatch, Game.SpriteMapTex, offset); } }
			if (drawBlocks)
			{
				Vector2 blockOffset = offset + new Vector2(16, 16);
				for (int y = Height - 1; y >= 0; y--)
				{
					for (int x = Width - 1; x >= 0; x--)
					{
						if (Blocks[x, y])
						{
							Block.Draw(spriteBatch, Game.SpriteMapTex, new Vector2(x * 32, y * 32) + blockOffset, Color.Red);
						}
					}
				}
			}
		}

		public void DrawLight(GraphicsDevice graphicsDevice, Vector2 offset)
		{
			graphicsDevice.Clear(LightBase);
			Game.SpriteBatch.Begin(effect: Game.RadialEffect);
			foreach (var l in Lights)
			{
				Game.RadialEffect.Parameters["Color"].SetValue(l.Color.ToVector4());
				Game.RectangleRenderer.Draw(Game.RadialEffect, l.Rectangle, offset);
			}
			Game.SpriteBatch.End();
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
			Block.Update(ms);
		}
	}
}
