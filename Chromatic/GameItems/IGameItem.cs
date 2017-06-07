using Microsoft.Xna.Framework;

namespace Chromatic.GameItems
{
    public interface IGameItem
    {
        void LoadContent();
        void UnloadContent();
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
        bool Remove();
    }
}
