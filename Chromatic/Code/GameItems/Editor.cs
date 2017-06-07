using Microsoft.Xna.Framework;

namespace Chromatic.Code.GameItems
{
	class Editor : IGameItem
	{
		Chromatic game;
		bool remove = false;

		public bool Remove { get { return remove; } }

		Rectangle rect;

		public Editor(Chromatic game)
		{
			this.game = game;
		}

		public void Draw(GameTime gameTime)
		{
			game.spriteBatch.Begin();
			for (int x = 0; x < 640; x += 32)
			{
				for (int y = 0; y < 640; y += 32)
				{
					game.spriteBatch.Draw(game.spriteMapTex, new Vector2(x, y), rect, Color.White);
				}
			}
			game.spriteBatch.End();
		}

		public void LoadContent()
		{
			rect = game.spriteMap.Areas[game.spriteMap.Sprites["t_gwgw_g"].Indices[0]];
		}

		public void UnloadContent()
		{
		}

		public void Update(GameTime gameTime)
		{
		}
	}
}
