using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chromatic.Code.Renderable.Effects
{
	public class SolidColorEffect
	{
		Effect Effect;
		Chromatic Game;
		VertexPosition[] vertices = new VertexPosition[4];
		readonly short[] indices = { 0, 1, 2, 0, 2, 3 };

		public SolidColorEffect(Chromatic game)
		{
			Game = game;
		}

		public void LoadContent()
		{
			Effect = Game.Content.Load<Effect>(@"Effects\SolidColor");
		}

		public void Draw(Rectangle rectangle, Color color, Vector2 offset)
		{
			var offsetv3 = new Vector3(offset, 0);
			vertices[0].Position = new Vector3(rectangle.X, rectangle.Y, 0) + offsetv3;
			vertices[1].Position = new Vector3(rectangle.X + rectangle.Width, rectangle.Y, 0) + offsetv3;
			vertices[2].Position = new Vector3(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height, 0) + offsetv3;
			vertices[3].Position = new Vector3(rectangle.X, rectangle.Y + rectangle.Height, 0) + offsetv3;
			Effect.Parameters["Color"].SetValue(color.ToVector4());
			foreach (var pass in Effect.CurrentTechnique.Passes)
			{
				pass.Apply();
				Game.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, 4, indices, 0, 2);
			}
		}
	}
}
