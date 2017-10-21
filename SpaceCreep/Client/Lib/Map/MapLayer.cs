using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceCreep.Client.Lib.Map
{
    public class MapLayer
    {

        public MapLayer(int width, int height)
        {
            Width = width;
            Height = height;
            Map = new int[Height, Width];
            ClearMap();
        }

        public int[,] Map { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        
        public void ClearMap()
        {
            for (var j = 0; j < Width; j++)
            for (var i = 0; i < Height; i++)
                Map[i, j] = -1;
        }
        
        public void Draw(SpriteBatch batch, Vector2 min, Vector2 max, Tileset tileset, Color color, int opacity)
        {
            min.X = (int) MathHelper.Max(min.X, 0);
            min.Y = (int) MathHelper.Max(min.Y, 0);
            max.X = (int) MathHelper.Min(max.X, Width);
            max.Y = (int) MathHelper.Min(max.Y, Height);

            for (var x = (int) min.X; x < max.X; x++)
            for (var y = (int) min.Y; y < max.Y; y++)
            {
                var textureId = Map[y, x];

                if (textureId != -1 && tileset != null)
                {
                    //Vector2 offset = Vector2.Zero;
                    var offset = new Vector2(1, 1);

                    batch.Draw(
                        tileset.TextureMap,
                        new Rectangle(
                            x * MapHelper.TileSize,
                            y * MapHelper.TileSize,
                            (int) (MapHelper.TileSize + offset.X),
                            (int) (MapHelper.TileSize + offset.Y)),
                        tileset.GetRectForTexture(textureId),
                        new Color(color.R, color.G, color.B, opacity));
                }
            }
        }
    }
}