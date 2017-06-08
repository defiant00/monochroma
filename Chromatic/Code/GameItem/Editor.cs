using Chromatic.Code.Renderable;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Chromatic.Code.GameItem
{
	class Editor : IGameItem
	{
		Chromatic game;
		bool remove = false;
        Map map;

		public bool Remove { get { return remove; } }

		public Editor(Chromatic game)
		{
			this.game = game;
		}

		public void Draw(GameTime gameTime)
		{
			game.spriteBatch.Begin();
            map.Draw(game.spriteBatch, new Vector2(32, 32));
			game.spriteBatch.End();
		}

		public void LoadContent()
		{
            map = new Map(game, 20, 20, "t_gggg");
		}

		public void UnloadContent()
		{
		}

		public void Update(GameTime gameTime)
		{
			double ms = gameTime.ElapsedGameTime.TotalMilliseconds;
            map.Update(ms);

            var mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                Debug.WriteLine((mouse.X / 32) + ", " + (mouse.Y / 32));
            }
		}
	}
}
