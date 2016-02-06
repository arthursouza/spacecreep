using System;
using GravityEater.Lib;
using GravityEater.Lib.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LotusLibrary
{
    public class MapLayer
    {
        public MapLayer()
        {
            Map = new int[Height, Width];
        }

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
        public int Opacity { get; set; }
        public int Id { get; set; }
        public int MapId { get; set; }
        public bool UpperLayer { get; set; }

        public int WidthInPixels
        {
            get { return MapHelper.TileSize*Width; }
        }

        public int HeightInPixels
        {
            get { return MapHelper.TileSize*Height; }
        }

        public string GetMapArray()
        {
            var map = new string[Width*Height];
            int i = 0;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    map[i++] = Map[y, x].ToString();
                }
            }
            return string.Join(",", map);
        }

        public void SetMap(string array)
        {
            string[] strArray = array.Split(',');
            Map = new int[Height, Width];
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                    Int32.TryParse(strArray[j*Width + i], out Map[j, i]);
        }

        public void ClearMap()
        {
            for (int j = 0; j < Width; j++)
                for (int i = 0; i < Height; i++)
                {
                    Map[i, j] = -1;
                }
        }


        public void Draw(SpriteBatch batch, Vector2 min, Vector2 max, Tileset tileset, Color color, int opacity)
        {
            Opacity = opacity;
            
            min.X = (int) MathHelper.Max(min.X, 0);
            min.Y = (int) MathHelper.Max(min.Y, 0);
            max.X = (int) MathHelper.Min(max.X, Width);
            max.Y = (int) MathHelper.Min(max.Y, Height);

            for (var x = (int) min.X; x < max.X; x++)
            {
                for (var y = (int) min.Y; y < max.Y; y++)
                {
                    int textureId = Map[y, x];

                    if (textureId != -1 && tileset != null)
                    {
                        //Vector2 offset = Vector2.Zero;
                        Vector2 offset = new Vector2(1, 1);

                        batch.Draw(
                            tileset.TextureMap,
                            new Rectangle(
                                x*MapHelper.TileSize,
                                y*MapHelper.TileSize,
                                (int) (MapHelper.TileSize + offset.X),
                                (int) (MapHelper.TileSize + offset.Y)),
                            tileset.GetRectForTexture(textureId),
                            new Color(color.R, color.G, color.B, opacity));
                    }
                }
            }
        }

        public void SetTexture(Vector2 tile, int texture)
        {
            if (tile.Y < Height && tile.X < Width &&
                tile.Y >= 0 && tile.X >= 0)
                Map[(int) tile.Y, (int) tile.X] = texture;
        }

        public bool IsValidTile(Vector2 tile)
        {
            return (tile.Y < Height && tile.X < Width &&
                    tile.Y >= 0 && tile.X >= 0);
        }

        public void ClearTile(Vector2 tile)
        {
            if (tile.Y < Height && tile.X < Width &&
                tile.Y >= 0 && tile.X >= 0)
                Map[(int) tile.Y, (int) tile.X] = -1;
        }

        public int GetTexture(Vector2 tile)
        {
            if (tile.Y < Height && tile.X < Width &&
                tile.Y >= 0 && tile.X >= 0)
                return Map[(int) tile.Y, (int) tile.X];

            return -1;
        }
    }
}