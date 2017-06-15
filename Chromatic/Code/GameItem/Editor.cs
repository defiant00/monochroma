using Chromatic.Code.Renderable;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Chromatic.Code.GameItem
{
    class Editor : IGameItem
    {
        enum State
        {
            Main,
            SelectTile,
            SelectDecal,
        }
        State CurState = State.Main;

        enum PlacementMode
        {
            Tile,
            Decal,
        }
        PlacementMode CurPlacementMode = PlacementMode.Tile;

        Chromatic Game;
        bool _Remove = false;
        Map Map;
        Vector2 Offset = new Vector2(18, 60);
        List<Sprite> TileSprites;
        List<Sprite> DecalSprites;
        Sprite CurrentTile;
        Sprite CurrentDecal;
        SpriteFont Font;
        bool DrawBlocks = true;
        bool DrawDecals = true;

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
                Map.Draw(Game.SpriteBatch, floorOffset, DrawDecals, DrawBlocks);
                Game.SpriteBatch.End();

                Game.GraphicsDevice.SetRenderTarget(Game.LightTarget);
                Game.GraphicsDevice.Clear(Color.White);
                //Game.RadialEffect.Draw(new Rectangle(0, 0, 800, 600), new Color(1f, 0, 0), Offset);

                Game.GraphicsDevice.SetRenderTarget(Game.InterfaceTarget);
                Game.GraphicsDevice.Clear(Color.Transparent);
                Game.SpriteBatch.Begin();
                Game.RectangleSprite.Draw(Game.SpriteBatch, Game.SpriteMapTex, new Rectangle(0, 0, 1280, 40), Color.Black);
                CurrentTile.Draw(Game.SpriteBatch, Game.SpriteMapTex, Vector2.Zero);
                CurrentDecal.Draw(Game.SpriteBatch, Game.SpriteMapTex, Vector2.Zero);
                Game.SpriteBatch.DrawString(Font, "(B)locks: " + DrawBlocks + "   De(c)als: " + DrawDecals + "   (P)lacement: " + CurPlacementMode, new Vector2(80, 12), Color.White);
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
            else if (CurState == State.SelectDecal)
            {
                Game.GraphicsDevice.SetRenderTarget(Game.SpriteTarget);
                Game.GraphicsDevice.Clear(Game.BackColor);

                Game.SpriteBatch.Begin();
                foreach (var d in DecalSprites) { d.Draw(Game.SpriteBatch, Game.SpriteMapTex, Vector2.Zero); }
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
            CurrentDecal = new Sprite(Game.SpriteMap, "d_flower1") { Position = new Vector2(50, 17) };
            Font = Game.Content.Load<SpriteFont>("Fonts\\Arial");

            TileSprites = FitSprites("t_");
            DecalSprites = FitSprites("d_");
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
            CurrentTile.Update(ms);
            CurrentDecal.Update(ms);
            foreach (var t in TileSprites) { t.Update(ms); }
            foreach (var d in DecalSprites) { d.Update(ms); }

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
                    else if (Game.Input.Mouse.X < 68)
                    {
                        if (Game.Input.Mouse.LeftButton == InputState.ButtonState.JustPressed)
                        {
                            CurState = State.SelectDecal;
                        }
                        else if (Game.Input.Mouse.RightButton == InputState.ButtonState.JustPressed)
                        {
                            CurrentDecal.Rotation = (CurrentDecal.Rotation + MathHelper.PiOver2) % MathHelper.TwoPi;
                        }
                    }
                }
                else
                {
                    if (CurPlacementMode == PlacementMode.Tile)
                    {
                        if (Game.Input.Mouse.LeftButton == InputState.ButtonState.Pressed && mTileX > -1 && mTileY > -1 && tileX <= Map.Width && tileY <= Map.Height)
                        {
                            var t = Map.Tiles[tileX, tileY];
                            t.Play(CurrentTile.Animation);
                            t.Rotation = CurrentTile.Rotation;
                        }
                        else if (Game.Input.Mouse.RightButton == InputState.ButtonState.JustPressed && mX > -1 && mY > -1 && blockX < Map.Width && blockY < Map.Height)
                        {
                            Map.Blocks[blockX, blockY] = !Map.Blocks[blockX, blockY];
                        }
                    }
                    else if (CurPlacementMode == PlacementMode.Decal)
                    {
                        if (Game.Input.Mouse.LeftButton == InputState.ButtonState.JustPressed)
                        {
                            Map.Decals.Add(new Sprite(Game.SpriteMap, CurrentDecal.Animation, Game.Random)
                            {
                                Position = new Vector2(mX, mY),
                                Rotation = CurrentDecal.Rotation,
                            });
                            Map.Decals.Sort((a, b) => (int)(a.Position.Y - b.Position.Y));
                        }
                        else if (Game.Input.Mouse.RightButton == InputState.ButtonState.JustPressed)
                        {
                            var decal = Map.Decals.LastOrDefault(d => d.Animation == CurrentDecal.Animation && d.Contains(mX, mY));
                            if (decal != null)
                            {
                                Map.Decals.Remove(decal);
                            }
                        }
                    }
                }

                float change = (float)ms / 5;

                if (Game.Input.Keyboard.IsDown(Keys.W)) { Offset.Y += change; }
                if (Game.Input.Keyboard.IsDown(Keys.S)) { Offset.Y -= change; }
                if (Game.Input.Keyboard.IsDown(Keys.A)) { Offset.X += change; }
                if (Game.Input.Keyboard.IsDown(Keys.D)) { Offset.X -= change; }

                if (Game.Input.Keyboard[Keys.B] == InputState.ButtonState.JustPressed)
                {
                    DrawBlocks = !DrawBlocks;
                }
                if (Game.Input.Keyboard[Keys.C] == InputState.ButtonState.JustPressed)
                {
                    DrawDecals = !DrawDecals;
                }
                if (Game.Input.Keyboard[Keys.P] == InputState.ButtonState.JustPressed)
                {
                    CurPlacementMode = CurPlacementMode == PlacementMode.Tile ? PlacementMode.Decal : PlacementMode.Tile;
                }

                if (Game.Input.Keyboard.IsDown(Keys.LeftControl) && Game.Input.Keyboard[Keys.S] == InputState.ButtonState.JustPressed)
                {
                    var save = new System.Windows.Forms.SaveFileDialog() { Filter = "XML File|*.xml" };
                    if (save.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        using (var writer = XmlWriter.Create(save.FileName))
                        {
                            IntermediateSerializer.Serialize(writer, new LevelIO(Map), null);
                        }
                    }
                }
                if (Game.Input.Keyboard.IsDown(Keys.LeftControl) && Game.Input.Keyboard[Keys.L] == InputState.ButtonState.JustPressed)
                {
                    var open = new System.Windows.Forms.OpenFileDialog() { Filter = "XML File|*.xml" };
                    if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        using (var reader = XmlReader.Create(open.FileName))
                        {
                            var lvl = IntermediateSerializer.Deserialize<LevelIO>(reader, null);
                            Map = lvl.ToMap(Game);
                        }
                    }
                }
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
            else if (CurState == State.SelectDecal)
            {
                if (Game.Input.Mouse.LeftButton == InputState.ButtonState.JustPressed)
                {
                    var d = DecalSprites.FirstOrDefault(s => s.Contains(Game.Input.Mouse.X, Game.Input.Mouse.Y));
                    if (d != null)
                    {
                        CurrentDecal.Play(d.Animation);
                        CurState = State.Main;
                    }
                }
            }
        }
    }
}
