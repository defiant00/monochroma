using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chromatic.Code.Renderable.Effects
{
	public class RadialEffect
	{
		Effect Effect;
		Chromatic Game;
		VertexPositionTexture[] vertices = new VertexPositionTexture[4];
		readonly short[] indices = { 0, 1, 2, 0, 2, 3 };

		public RadialEffect(Chromatic game)
		{
			Game = game;
			vertices[0].TextureCoordinate = new Vector2(0, 0);
			vertices[1].TextureCoordinate = new Vector2(1, 0);
			vertices[2].TextureCoordinate = new Vector2(1, 1);
			vertices[3].TextureCoordinate = new Vector2(0, 1);
		}

		public void LoadContent()
		{
			Effect = Game.Content.Load<Effect>(@"Effects\Radial");
			Effect.Parameters["wpvMatrix"].SetValue(Game.WPVMatrix);
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
