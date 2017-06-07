using Chromatic.GameItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Chromatic
{
    public class Chromatic : Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public static Color backColor = new Color(0f, 0.1f, 0.3f, 1f);
        List<IGameItem> gameItems = new List<IGameItem>();
        public Random random = new Random();
        public Matrix WPVMatrix;

        public Chromatic()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";

            WPVMatrix = Matrix.CreateOrthographicOffCenter(0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, 0, -1, 1);
        }

        protected override void Initialize()
        {
            AddItem(new Editor(this));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            foreach (var item in gameItems) { item.LoadContent(); }
        }

        protected override void UnloadContent()
        {
            foreach (var item in gameItems) { item.UnloadContent(); }
        }

        protected override void Update(GameTime gameTime)
        {
            for (int i = gameItems.Count - 1; i >= 0; i--)
            {
                if (gameItems[i].Remove())
                {
                    gameItems[i].UnloadContent();
                    gameItems.RemoveAt(i);
                }
                else { gameItems[i].Update(gameTime); }
            }

            this.Window.Title = (gameTime.IsRunningSlowly ? "SLOW!" : "Fine");

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backColor);

            foreach (var item in gameItems) { item.Draw(gameTime); }

            base.Draw(gameTime);
        }

        public void AddItem(IGameItem item)
        {
            item.LoadContent();
            gameItems.Add(item);
        }
    }
}
