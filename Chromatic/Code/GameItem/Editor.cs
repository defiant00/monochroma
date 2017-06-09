using Chromatic.Code.Renderable;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Chromatic.Code.GameItem
{
	class Editor : IGameItem
	{
		Chromatic Game;
		bool _Remove = false;
		Map Map;

		public bool Remove { get { return _Remove; } }

		public Editor(Chromatic game)
		{
			Game = game;
		}

		public void Draw(GameTime gameTime)
		{
			Game.SpriteBatch.Begin();
			Map.Draw(Game.SpriteBatch, new Vector2(32, 32));
			Game.SpriteBatch.End();
		}

		public void LoadContent()
		{
			Map = new Map(Game, 20, 20, "t_gggg");
		}

		public void UnloadContent()
		{
		}

		public void Update(GameTime gameTime)
		{
			double ms = gameTime.ElapsedGameTime.TotalMilliseconds;
			Map.Update(ms);

			var mouse = Mouse.GetState();
			int x = mouse.X / 32;
			int y = mouse.Y / 32;

			if (mouse.LeftButton == ButtonState.Pressed && mouse.X > -1 && mouse.Y > -1 && x < Map.Width && y < Map.Height)
			{
				var t = Map.Tiles[x, y];
				t.Play("t_wwww");
			}
		}
	}
}
