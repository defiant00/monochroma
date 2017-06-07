using Microsoft.Xna.Framework;

namespace Chromatic.Code.GameItem
{
	public interface IGameItem
	{
		bool Remove { get; }

		void LoadContent();
		void UnloadContent();
		void Update(GameTime gameTime);
		void Draw(GameTime gameTime);
	}
}
