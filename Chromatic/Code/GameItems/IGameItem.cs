using Microsoft.Xna.Framework;

namespace Chromatic.Code.GameItems
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
