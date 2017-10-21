using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceCreep.Client.Lib
{
    public static class Drawing
    {
        public static void DrawLine(GraphicsDevice graphics, Vector2 start, Vector2 end, Color color)
        {
            var vertices = new VertexPositionColor[2];
            vertices[0] = new VertexPositionColor(new Vector3(start, 0), color);
            vertices[1] = new VertexPositionColor(new Vector3(end, 0), color);
            graphics.DrawUserPrimitives(PrimitiveType.LineList, vertices, 0, 1);
        }

        public static void DrawText(SpriteBatch batch, SpriteFont font, string text, Vector2 position, Color color,
            bool shadow)
        {
            if (shadow)
                batch.DrawString(font, text, new Vector2(position.X + 1, position.Y + 1), Color.Black);
            batch.DrawString(font, text, position, color);
        }
        
    }
}