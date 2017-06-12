using Chromatic.Code;
using Chromatic.Code.GameItem;
using Chromatic.Code.Renderable;
using Chromatic.Code.Renderable.Effects;
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
		public InputState Input;

		public Dictionary<string, SpriteData> SpriteMap;
		public Texture2D SpriteMapTex;

		private Rectangle _OutputRectangle;
		public Rectangle OutputRectangle
		{
			get
			{
				return _OutputRectangle;
			}
			set
			{
				_OutputRectangle = value;
				Input.Mouse.SetupArea(_OutputRectangle);
			}
		}
		public RenderTarget2D SpriteTarget, LightTarget, InterfaceTarget;
		public DynamicLightEffect DynamicLightEffect;
		public RadialEffect RadialEffect;
		public Sprite RectangleSprite;

		public Chromatic()
		{
			Graphics = new GraphicsDeviceManager(this);
			Graphics.PreferredBackBufferWidth = 1280;
			Graphics.PreferredBackBufferHeight = 720;

			DynamicLightEffect = new DynamicLightEffect(this);
			RadialEffect = new RadialEffect(this);

			Input = new InputState(new Rectangle(0, 0, 1280, 720));
			CalcOutputRectangle();

			Content.RootDirectory = "Content";

			WPVMatrix = Matrix.CreateOrthographicOffCenter(0, 1280, 720, 0, -1, 1);
		}

		private void CalcOutputRectangle()
		{
			Vector2 buf = new Vector2(1280, 720);
			Vector2 bb = new Vector2(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);
			Vector2 scaleVec = bb / buf;
			float scale = Math.Min(scaleVec.X, scaleVec.Y);

			Vector2 size = buf * scale;
			Vector2 start = (bb - size) / 2;

			OutputRectangle = new Rectangle(start.ToPoint(), size.ToPoint());
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

			SpriteTarget = new RenderTarget2D(GraphicsDevice, 1280, 720);
			LightTarget = new RenderTarget2D(GraphicsDevice, 1280, 720);
			InterfaceTarget = new RenderTarget2D(GraphicsDevice, 1280, 720);

			DynamicLightEffect.LoadContent();
			RadialEffect.LoadContent();

			RectangleSprite = new Sprite(SpriteMap, "pixel");

			foreach (var item in GameItems) { item.LoadContent(); }
		}

		protected override void UnloadContent()
		{
			foreach (var item in GameItems) { item.UnloadContent(); }
		}

		protected override void Update(GameTime gameTime)
		{
			Input.Update();

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
			foreach (var item in GameItems) { item.Draw(gameTime); }

			GraphicsDevice.SetRenderTarget(null);
			GraphicsDevice.Clear(BackColor);
			DynamicLightEffect.Draw(SpriteTarget, LightTarget);

			SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
			SpriteBatch.Draw(InterfaceTarget, OutputRectangle, Color.White);
			SpriteBatch.End();

			base.Draw(gameTime);
		}

		public void AddItem(IGameItem item)
		{
			item.LoadContent();
			GameItems.Add(item);
		}
	}
}
