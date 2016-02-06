using LotusLibrary.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LotusLibrary
{
    public class MapMatrix
    {
        public MapMatrix()
        {
            Size = new Vector2(MapHelper.TileSize, MapHelper.TileSize);
        }

        public MapMatrix(int width, int height)
        {
            Width = width;
            Height = height;
            Size = new Vector2(MapHelper.TileSize, MapHelper.TileSize);
        }

        /// <summary>
        ///     Number of horizontal cells on the matrix
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        ///     Number of vertical cells on the matrix
        /// </summary>
        public int Height { get; set; }

        public Vector2 Size { get; set; }

        public void Draw(GraphicsDevice g)
        {
            for (int j = 0; j <= Width; j++)
            {
                Drawing.DrawLine(g,
                    new Vector2(j * Size.X, 0),
                    new Vector2(j * Size.X, Size.Y * Height),
                    Color.GreenYellow);
            }

            for (int j = 0; j <= Height; j++)
            {
                Drawing.DrawLine(g,
                    new Vector2(0, j * Size.Y),
                    new Vector2(Size.X * Width, j * Size.Y),
                    Color.GreenYellow);
            }
        }

        public void Draw(GraphicsDevice g, Color color)
        {
            for (int j = 0; j <= Width; j++)
            {
                Drawing.DrawLine(g,
                    new Vector2(j * Size.X, 0),
                    new Vector2(j * Size.X, Size.Y * Height),
                    color);
            }

            for (int j = 0; j <= Height; j++)
            {
                Drawing.DrawLine(g,
                    new Vector2(0, j * Size.Y),
                    new Vector2(Size.X * Width, j * Size.Y),
                    color);
            }
        }
    }
}