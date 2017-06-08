using Chromatic.Code.Renderable;
using Microsoft.Xna.Framework;

namespace Chromatic.Code.GameItem
{
	class Editor : IGameItem
	{
		Chromatic game;
		bool remove = false;

		public bool Remove { get { return remove; } }

		Sprite test;
		Sprite t2;

		public Editor(Chromatic game)
		{
			this.game = game;
		}

		public void Draw(GameTime gameTime)
		{
			game.spriteBatch.Begin();
			test.Draw(game.spriteBatch, game.spriteMapTex, new Vector2(16, 16));
			for (int i = 0; i < 10000; i++)
				t2.Draw(game.spriteBatch, game.spriteMapTex, new Vector2(game.random.Next(1280), game.random.Next(720)));
			//t2.Draw(game.spriteBatch, game.spriteMapTex, new Vector2(21, 16));
			game.spriteBatch.End();
		}

		public void LoadContent()
		{
			test = new Sprite(game.spriteMap["t_gggg"]);
			t2 = new Sprite(game.spriteMap["d_flower1"]);
		}

		public void UnloadContent()
		{
		}

		public void Update(GameTime gameTime)
		{
			double ms = gameTime.ElapsedGameTime.TotalMilliseconds;
			test.Update(ms);
			t2.Update(ms);
		}
	}
}
