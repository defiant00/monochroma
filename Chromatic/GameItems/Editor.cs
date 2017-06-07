using DataTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chromatic.GameItems
{
    class Editor : IGameItem
    {
        Chromatic game;
        bool remove = false;

        SpriteMap spriteMap;
        Texture2D spriteMapTex;

        public Editor(Chromatic game)
        {
            this.game = game;
        }

        public void Draw(GameTime gameTime)
        {
            game.spriteBatch.Begin();
            game.spriteBatch.Draw(spriteMapTex, new Vector2(20, 20), new Rectangle(34, 34, 32, 32), Color.White);
            game.spriteBatch.End();
        }

        public void LoadContent()
        {
            spriteMap = game.Content.Load<SpriteMap>("Data\\sprites");
            spriteMapTex = game.Content.Load<Texture2D>("Images\\sprites");
        }

        public bool Remove()
        {
            return remove;
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
