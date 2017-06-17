using Microsoft.Xna.Framework;

namespace DataTypes
{
	public class MapData
	{
		public Tile[][] Tiles;
		public bool[][] Blocks;
		public Decal[] Decals;
		public Light[] Lights;
		public Color LightBase;

		public MapData() { }

		public struct Tile
		{
			public string Animation;
			public float Rotation;
		}

		public struct Decal
		{
			public string Animation;
			public Vector2 Position;
			public float Rotation;
		}

		public struct Light
		{
			public Rectangle Rectangle;
			public Color Color;
		}
	}
}
