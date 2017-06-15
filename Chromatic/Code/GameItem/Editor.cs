using Chromatic.Code.Renderable;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chromatic.Code.GameItem
{
    class Editor : IGameItem
    {
        enum State
        {
            Main,
            SelectTile
        }
        State CurState;

        Chromatic Game;
        bool _Remove = false;
        Map Map;
        Vector2 Offset;

        List<Sprite> TileSprites;
        Sprite CurrentTile;

        public bool Remove { get { return _Remove; } }

        public Editor(Chromatic game)
        {
            Game = game;
        }

        public void Draw(GameTime gameTime)
        {
            Vector2 floorOffset = Offset.Floor();
            if (CurState == State.Main)
            {
                Game.GraphicsDevice.SetRenderTarget(Game.SpriteTarget);
                Game.GraphicsDevice.Clear(Game.BackColor);
                Game.SpriteBatch.Begin();
                Map.Draw(Game.SpriteBatch, floorOffset, true);
                Game.SpriteBatch.End();

                Game.GraphicsDevice.SetRenderTarget(Game.LightTarget);
                Game.GraphicsDevice.Clear(Color.White);
                //Game.RadialEffect.Draw(new Rectangle(0, 0, 800, 600), new Color(1f, 0, 0), Offset);

                Game.GraphicsDevice.SetRenderTarget(Game.InterfaceTarget);
                Game.GraphicsDevice.Clear(Color.Transparent);
                Game.SpriteBatch.Begin();
                Game.RectangleSprite.Draw(Game.SpriteBatch, Game.SpriteMapTex, new Rectangle(0, 0, 1280, 40), Color.Black);
                CurrentTile.Draw(Game.SpriteBatch, Game.SpriteMapTex, Vector2.Zero);
                Game.SpriteBatch.End();
            }
            else if (CurState == State.SelectTile)
            {
                Game.GraphicsDevice.SetRenderTarget(Game.SpriteTarget);
                Game.GraphicsDevice.Clear(Game.BackColor);

                Game.SpriteBatch.Begin();
                foreach (var t in TileSprites) { t.Draw(Game.SpriteBatch, Game.SpriteMapTex, Vector2.Zero); }
                Game.SpriteBatch.End();

                Game.GraphicsDevice.SetRenderTarget(Game.LightTarget);
                Game.GraphicsDevice.Clear(Color.White);

                Game.GraphicsDevice.SetRenderTarget(Game.InterfaceTarget);
                Game.GraphicsDevice.Clear(Color.Transparent);
            }
        }

        public void LoadContent()
        {
            Map = new Map(Game, 42, 24, "t_gggg");
            CurrentTile = new Sprite(Game.SpriteMap, "t_wwww") { Position = new Vector2(17, 17) };

            TileSprites = FitSprites("t_");
        }

        private List<Sprite> FitSprites(string prefix)
        {
            var sprites = new List<Sprite>();
            var areas = new List<Rectangle>();
            areas.Add(new Rectangle(0, 0, 1280, 720));
            foreach (var tKey in Game.SpriteMap.Keys.Where(k => k.StartsWith(prefix)))
            {
                var spr = Game.SpriteMap[tKey];
                var outerSize = new Point(spr.Frames[0].Rectangle.Width + 1, spr.Frames[0].Rectangle.Height + 1);
                bool placed = false;
                for (int i = 0; i < areas.Count && !placed; i++)
                {
                    var area = areas[i];
                    if (outerSize.X <= area.Width && outerSize.Y <= area.Height)
                    {
                        sprites.Add(new Sprite(Game.SpriteMap, tKey) { Position = area.Location.ToVector2() + (spr.Frames[0].Rectangle.Size.ToVector2() / 2).Floor() });

                        areas.RemoveAt(i);
                        if (outerSize.Y < area.Height)
                        {
                            areas.Insert(i, new Rectangle(area.X, area.Y + outerSize.Y, area.Width, area.Height - outerSize.Y));
                        }
                        if (outerSize.X < area.Width)
                        {
                            areas.Insert(i, new Rectangle(area.X + outerSize.X, area.Y, area.Width - outerSize.X, outerSize.Y));
                        }
                        placed = true;
                    }
                }
                if (!placed)
                {
                    throw new Exception(tKey + " won't fit!");
                }
            }
            return sprites;
        }

        public void UnloadContent() { }

        public void Update(GameTime gameTime)
        {
            double ms = gameTime.ElapsedGameTime.TotalMilliseconds;
            Map.Update(ms);
            foreach (var t in TileSprites) { t.Update(ms); }

            if (CurState == State.Main)
            {
                Vector2 floorOffset = Offset.Floor();
                int offX = (int)floorOffset.X;
                int offY = (int)floorOffset.Y;

                int mX = Game.Input.Mouse.X - offX;
                int mY = Game.Input.Mouse.Y - offY;
                int blockX = mX / 32;
                int blockY = mY / 32;
                int mTileX = mX + 16;
                int mTileY = mY + 16;
                int tileX = mTileX / 32;
                int tileY = mTileY / 32;

                if (Game.Input.Mouse.Y < 40)
                {
                    if (Game.Input.Mouse.X < 34)
                    {
                        if (Game.Input.Mouse.LeftButton == InputState.ButtonState.JustPressed)
                        {
                            CurState = State.SelectTile;
                        }
                        else if (Game.Input.Mouse.RightButton == InputState.ButtonState.JustPressed)
                        {
                            CurrentTile.Rotation = (CurrentTile.Rotation + MathHelper.PiOver2) % MathHelper.TwoPi;
                        }
                    }
                }
                else if (Game.Input.Mouse.LeftButton == InputState.ButtonState.Pressed && mTileX > -1 && mTileY > -1 && tileX <= Map.Width && tileY <= Map.Height)
                {
                    var t = Map.Tiles[tileX, tileY];
                    t.Play(CurrentTile.Animation);
                    t.Rotation = CurrentTile.Rotation;
                }
                else if (Game.Input.Mouse.RightButton == InputState.ButtonState.JustPressed && mX > -1 && mY > -1 && blockX < Map.Width && blockY < Map.Height)
                {
                    Map.Blocks[blockX, blockY] = !Map.Blocks[blockX, blockY];
                }

                float change = (float)ms / 5;
                var kbd = Keyboard.GetState();
                if (kbd.IsKeyDown(Keys.W)) { Offset.Y += change; }
                if (kbd.IsKeyDown(Keys.S)) { Offset.Y -= change; }
                if (kbd.IsKeyDown(Keys.A)) { Offset.X += change; }
                if (kbd.IsKeyDown(Keys.D)) { Offset.X -= change; }
            }
            else if (CurState == State.SelectTile)
            {
                if (Game.Input.Mouse.LeftButton == InputState.ButtonState.JustPressed)
                {
                    var t = TileSprites.FirstOrDefault(s => s.Contains(Game.Input.Mouse.X, Game.Input.Mouse.Y));
                    if (t != null)
                    {
                        CurrentTile.Play(t.Animation);
                        CurState = State.Main;
                    }
                }
            }
        }
    }
}
