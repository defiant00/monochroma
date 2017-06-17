using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chromatic.Code.Renderable
{
	public class RectangleRenderer
	{
		Chromatic Game;
		VertexPositionTexture[] vertices = new VertexPositionTexture[4];
		readonly short[] indices = { 0, 1, 2, 0, 2, 3 };

		public RectangleRenderer(Chromatic game)
		{
			Game = game;
			vertices[0].TextureCoordinate = new Vector2(0, 0);
			vertices[1].TextureCoordinate = new Vector2(1, 0);
			vertices[2].TextureCoordinate = new Vector2(1, 1);
			vertices[3].TextureCoordinate = new Vector2(0, 1);
		}

		public void Draw(Effect effect, Rectangle rectangle, Vector2 offset)
		{
			var offsetv3 = new Vector3(offset, 0);
			vertices[0].Position = new Vector3(rectangle.X, rectangle.Y, 0) + offsetv3;
			vertices[1].Position = new Vector3(rectangle.X + rectangle.Width, rectangle.Y, 0) + offsetv3;
			vertices[2].Position = new Vector3(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height, 0) + offsetv3;
			vertices[3].Position = new Vector3(rectangle.X, rectangle.Y + rectangle.Height, 0) + offsetv3;
			foreach (var pass in effect.CurrentTechnique.Passes)
			{
				pass.Apply();
				Game.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, 4, indices, 0, 2);
			}
		}
	}
}
