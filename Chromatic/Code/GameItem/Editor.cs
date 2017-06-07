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

        public Editor(Chromatic game)
        {
            this.game = game;
        }

        public void Draw(GameTime gameTime)
        {
            game.spriteBatch.Begin();
            test.Draw();
            //game.spriteBatch.Draw(game.spriteMapTex, new Vector2(x, y), rect, Color.White);
            game.spriteBatch.End();
        }

        public void LoadContent()
        {
            test = new Sprite(game.spriteMap["t_gwgw_g"]);
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
            double ms = gameTime.ElapsedGameTime.TotalMilliseconds;
            test.Update(ms);
        }
    }
}
