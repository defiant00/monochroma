using Chromatic.Code.GameItem;
using DataTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Chromatic
{
    public class Chromatic : Game
    {
        public GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;
        public Color BackColor = new Color(0f, 0.1f, 0.3f, 1f);
        List<IGameItem> GameItems = new List<IGameItem>();
        public Random Random = new Random();
        public Matrix WPVMatrix;

        public Dictionary<string, SpriteData> SpriteMap;
        public Texture2D SpriteMapTex;

        public Chromatic()
        {
            Graphics = new GraphicsDeviceManager(this);
            Graphics.PreferredBackBufferWidth = 1280;
            Graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";

            WPVMatrix = Matrix.CreateOrthographicOffCenter(0, Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight, 0, -1, 1);
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;

            GameItems.Add(new Editor(this));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            SpriteMap = Content.Load<Dictionary<string, SpriteData>>("Data\\sprites");
            SpriteMapTex = Content.Load<Texture2D>("Images\\sprites");

            foreach (var item in GameItems) { item.LoadContent(); }
        }

        protected override void UnloadContent()
        {
            foreach (var item in GameItems) { item.UnloadContent(); }
        }

        protected override void Update(GameTime gameTime)
        {
            for (int i = GameItems.Count - 1; i >= 0; i--)
            {
                if (GameItems[i].Remove)
                {
                    GameItems[i].UnloadContent();
                    GameItems.RemoveAt(i);
                }
                else { GameItems[i].Update(gameTime); }
            }

            Window.Title = (gameTime.IsRunningSlowly ? "SLOW!" : "Fine");

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(BackColor);

            foreach (var item in GameItems) { item.Draw(gameTime); }

            base.Draw(gameTime);
        }

        public void AddItem(IGameItem item)
        {
            item.LoadContent();
            GameItems.Add(item);
        }
    }
}
