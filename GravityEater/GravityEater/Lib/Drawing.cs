using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GravityEater.Lib
{
    public static class Drawing
    {
        public static void DrawLine(GraphicsDevice graphics, Vector2 start, Vector2 end, Color color)
        {
            try
            {
                var vertices = new VertexPositionColor[2];
                vertices[0] = new VertexPositionColor(new Vector3(start, 0), color);
                vertices[1] = new VertexPositionColor(new Vector3(end, 0), color);
                graphics.DrawUserPrimitives(PrimitiveType.LineList, vertices, 0, 1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DrawCircle(GraphicsDevice graphics, Vector2 position, float radius, Color color)
        {
            var vertices = new VertexPositionColor[100];
            for (int i = 0; i < 99; i++)
            {
                var angle = (float) (i/100.0*Math.PI*2);
                vertices[i].Position = new Vector3(
                    200 + (float) Math.Cos(angle)*100,
                    200 + (float) Math.Sin(angle)*100,
                    0);
                vertices[i].Color = Color.Black;
            }
            vertices[99] = vertices[0];
            graphics.DrawUserPrimitives(PrimitiveType.LineStrip, vertices, 0, 99);
        }

        public static void DrawText(SpriteBatch batch, SpriteFont font, string text, Vector2 position, Color color,
            bool shadow)
        {
            if (shadow)
                batch.DrawString(font, text, new Vector2(position.X + 1, position.Y + 1), Color.Black);
            batch.DrawString(font, text, position, color);
        }

        public static void DrawText(SpriteBatch batch, SpriteFont font, string text, Vector2 position, Color color,
            bool shadow, Color shadowColor)
        {
            if (shadow)
                batch.DrawString(font, text, new Vector2(position.X + 1, position.Y + 1), shadowColor);
            batch.DrawString(font, text, position, color);
        }
    }
}