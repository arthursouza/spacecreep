using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceCreep.Client.Lib.Objects;

namespace SpaceCreep.Client.Lib.Map
{
    public class Map
    {
        public Map()
        {
            MapEnemies = new List<Enemy>();
            MapObjects = new List<MapObject>();
            Layers = new List<MapLayer>();
            Animations = new List<Animation>();
        }
        
        public int Width { get; set; }
        public int Height { get; set; }

        public int WidthInPixels => Width * MapHelper.TileSize;

        public int HeightInPixels => Height * MapHelper.TileSize;
        
        public List<Animation> Animations { get; set; }
        public Tileset Tileset { get; set; }

        public List<MapLayer> Layers { get; set; }
        
        public List<Enemy> MapEnemies { get; set; }
        
        public List<MapObject> MapObjects { get; set; }

        public void Draw(SpriteBatch spriteBatch, Tileset testTileSet, bool onlyDrawVisible)
        {
            Vector2 min;
            Vector2 max;

            if (onlyDrawVisible)
            {
                min = MapHelper.GetTileFromPixels(Camera.Position);
                max = MapHelper.GetTileFromPixels(
                    new Vector2(
                        Camera.Position.X + spriteBatch.GraphicsDevice.Viewport.Width + MapHelper.TileSize,
                        Camera.Position.Y + spriteBatch.GraphicsDevice.Viewport.Height + MapHelper.TileSize));
            }
            else
            {
                min = Vector2.Zero;
                max = new Vector2(
                    Width,
                    Height);
            }

            var layers = Layers;

            Color color;

            for (var i = 0; i < layers.Count; i++)
            {
                color = Color.White;
                layers[i].Draw(spriteBatch, min, max, testTileSet, color, color.A);
            }
        }
    }
}