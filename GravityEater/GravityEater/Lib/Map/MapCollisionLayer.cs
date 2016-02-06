using System;
using LotusLibrary.UI;
using LotusLibrary.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LotusLibrary
{
    public class MapCollisionLayer
    {
        public MapCollisionLayer()
        {
        }

        public MapCollisionLayer(int width, int height)
        {
            Width = width;
            Height = height;
            Map = new int[Height, Width];
        }

        public int MapId { get; set; }
        public int[,] Map { get; set; }

        /// <summary>
        ///     Width of the map in tiles
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        ///     Height of the map in tiles
        /// </summary>
        public int Height { get; set; }

        public CollisionValues GetCollisionValue(Vector2 cell)
        {
            return GetCollisionValue((int) cell.X, (int) cell.Y);
        }

        public CollisionValues GetCollisionValue(int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
                return (CollisionValues) Map[y, x];

            return CollisionValues.Impassable;
        }

        public string GetCollisionArray()
        {
            int i = 0;
            var array = new string[Width*Height];
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    array[i++] = Map[y, x].ToString();
                }

            return string.Join(",", array);
        }

        public void SetCollisionArray(string array)
        {
            string[] strArray = array.Split(',');
            Map = new int[Height, Width];
            int i = 0;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Int32.TryParse(strArray[i++], out Map[y, x]);
                }
            }
        }

        public void Draw(SpriteBatch batch)
        {
            string Coll;
            SpriteFont font = Fonts.ArialBlack14;
            Vector2 size = font.MeasureString("X");
            Color c;

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Coll = GetCollisionValue(x, y) == CollisionValues.Impassable ? "X" : "O";
                    c = GetCollisionValue(x, y) == CollisionValues.Impassable
                        ? new Color(150, 0, 0, 50)
                        : new Color(0, 150, 0, 50);

                    Drawing.DrawText(
                        batch,
                        font,
                        Coll,
                        new Vector2(
                            x*MapHelper.TileSize + 5, //(MapHelper.TileSize / 2 - size.X / 2),
                            y*MapHelper.TileSize + 5), //(MapHelper.TileSize / 2 - size.Y / 2)),
                        c,
                        true);
                }
            }
        }

        public void SetCollisionValue(Vector2 MouseTile, CollisionValues collisionValues)
        {
            if (MouseTile.X < Width && MouseTile.Y < Height && MouseTile.X >= 0 && MouseTile.Y >= 0)
                Map[(int) MouseTile.Y, (int) MouseTile.X] = (int) collisionValues;
        }
    }
}