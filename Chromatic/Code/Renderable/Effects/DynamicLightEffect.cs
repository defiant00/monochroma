using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chromatic.Code.Renderable.Effects
{
	public class DynamicLightEffect
	{
		Effect Effect;
		Chromatic Game;

		public DynamicLightEffect(Chromatic game)
		{
			Game = game;
		}

		public void LoadContent()
		{
			Effect = Game.Content.Load<Effect>(@"Effects\DynamicLight");
		}

		public void Draw(Texture2D texture, Texture2D light)
		{
			Effect.Parameters["Light"].SetValue(light);

			Game.SpriteBatch.Begin(effect: Effect);
			Game.SpriteBatch.Draw(texture, Game.OutputRectangle, Color.White);
			Game.SpriteBatch.End();
		}
	}
}
