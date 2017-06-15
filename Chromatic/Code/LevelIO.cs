using Chromatic.Code.Renderable;
using Microsoft.Xna.Framework;

namespace Chromatic.Code
{
    public class LevelIO
    {
        public Tile[][] Tiles;
        public bool[][] Blocks;
        public Decal[] Decals;

        public LevelIO() { }

        public LevelIO(Map map)
        {
            Tiles = new Tile[map.Width + 1][];
            for (int x = 0; x < map.Width + 1; x++)
            {
                Tiles[x] = new Tile[map.Height + 1];
                for (int y = 0; y < map.Height + 1; y++)
                {
                    Tiles[x][y] = new Tile
                    {
                        Animation = map.Tiles[x, y].Animation,
                        Rotation = map.Tiles[x, y].Rotation,
                    };
                }
            }

            Blocks = new bool[map.Width][];
            for (int x = 0; x < map.Width; x++)
            {
                Blocks[x] = new bool[map.Height];
                for (int y = 0; y < map.Height; y++)
                {
                    Blocks[x][y] = map.Blocks[x, y];
                }
            }

            Decals = new Decal[map.Decals.Count];
            for (int i = 0; i < map.Decals.Count; i++)
            {
                Decals[i] = new Decal
                {
                    Animation = map.Decals[i].Animation,
                    Position = map.Decals[i].Position,
                    Rotation = map.Decals[i].Rotation,
                };
            }
        }

        public Map ToMap(Chromatic game)
        {
            var map = new Map(game, Tiles.Length - 1, Tiles[0].Length - 1, "t_gggg");

            for (int x = 0; x < Tiles.Length; x++)
            {
                for (int y = 0; y < Tiles[0].Length; y++)
                {
                    map.Tiles[x, y].Play(Tiles[x][y].Animation);
                    map.Tiles[x, y].Rotation = Tiles[x][y].Rotation;
                }
            }

            for (int x = 0; x < Blocks.Length; x++)
            {
                for (int y = 0; y < Blocks[0].Length; y++)
                {
                    map.Blocks[x, y] = Blocks[x][y];
                }
            }

            for (int i = 0; i < Decals.Length; i++)
            {
                map.Decals.Add(new Sprite(game.SpriteMap, Decals[i].Animation)
                {
                    Position = Decals[i].Position,
                    Rotation = Decals[i].Rotation,
                });
            }

            return map;
        }

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
    }
}
