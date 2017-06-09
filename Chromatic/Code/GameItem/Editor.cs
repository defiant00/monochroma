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
        Vector2 Offset;

        public bool Remove { get { return _Remove; } }

        public Editor(Chromatic game)
        {
            Game = game;
        }

        public void Draw(GameTime gameTime)
        {
            Game.SpriteBatch.Begin();
            Map.Draw(Game.SpriteBatch, Offset);
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

            int offX = (int)Offset.X;
            int offY = (int)Offset.Y;

            var mouse = Mouse.GetState();
            int mx = mouse.X - offX;
            int my = mouse.Y - offY;
            int tx = mx / 32;
            int ty = my / 32;

            if (mouse.LeftButton == ButtonState.Pressed && mx > -1 && my > -1 && tx < Map.Width && ty < Map.Height)
            {
                var t = Map.Tiles[tx, ty];
                t.Play("t_wwww");
            }

            float change = (float)ms / 5;
            var kbd = Keyboard.GetState();
            if (kbd.IsKeyDown(Keys.W)) { Offset.Y += change; }
            if (kbd.IsKeyDown(Keys.S)) { Offset.Y -= change; }
            if (kbd.IsKeyDown(Keys.A)) { Offset.X += change; }
            if (kbd.IsKeyDown(Keys.D)) { Offset.X -= change; }
        }
    }
}
