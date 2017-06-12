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
			Vector2 floorOffset = Offset.Floor();
			Game.GraphicsDevice.SetRenderTarget(Game.SpriteTarget);
			Game.GraphicsDevice.Clear(Game.BackColor);
            Game.SpriteBatch.Begin();
            Map.Draw(Game.SpriteBatch, floorOffset);
            Game.SpriteBatch.End();

			Game.GraphicsDevice.SetRenderTarget(Game.LightTarget);
			Game.GraphicsDevice.Clear(Color.Gray);
			Game.RadialEffect.Draw(new Rectangle(0, 0, 800, 600), new Color(1f, 0, 0), Offset);
			Game.RadialEffect.Draw(new Rectangle(300, 0, 800, 600), new Color(0, 0.4f, 0), Offset);
			Game.RadialEffect.Draw(new Rectangle(150, 150, 800, 600), new Color(0, 0, 1f), Offset);

			Game.SpriteBatch.Begin();
			Game.RectangleSprite.Draw(Game.SpriteBatch, Game.SpriteMapTex, new Rectangle(Offset.ToPoint(), new Point(300, 120)), Color.White);
			Game.SpriteBatch.End();

			Game.GraphicsDevice.SetRenderTarget(Game.InterfaceTarget);
			Game.GraphicsDevice.Clear(Color.Transparent);
        }

        public void LoadContent()
        {
            Map = new Map(Game, 42, 24, "t_gggg");
		}

		public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
            double ms = gameTime.ElapsedGameTime.TotalMilliseconds;
            Map.Update(ms);

			Vector2 floorOffset = Offset.Floor();
			int offX = (int)floorOffset.X;
			int offY = (int)floorOffset.Y;

            

            int mx = Game.Input.Mouse.X - offX;
            int my = Game.Input.Mouse.Y - offY;
            int tx = mx / 32;
            int ty = my / 32;

            if (Game.Input.Mouse.LeftButton == InputState.ButtonState.Pressed && mx > -1 && my > -1 && tx < Map.Width && ty < Map.Height)
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
