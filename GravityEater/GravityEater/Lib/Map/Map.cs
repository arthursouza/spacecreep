using System.Collections.Generic;
using GravityEater.Lib.Objects;
using LotusLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GravityEater.Lib.Map
{
    public class Map
    {
        public enum LayerDraw
        {
            All,
            Upper,
            Lower
        }

        public Map()
        {
            MapEnemies = new List<Enemy>();
            MapNpcs = new List<Npc>();
            MapObjects = new List<MapObject>();
            //MapEvents = new List<Event>();
            Layers = new List<MapLayer>();
            Animations = new List<Animation>();
            //MapMatrix = new MapMatrix();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public string Label
        {
            get { return Id + ". " + Name; }
        }

        public int Width { get; set; }
        public int Height { get; set; }

        public int WidthInPixels
        {
            get { return Width*MapHelper.TileSize; }
        }

        public int HeightInPixels
        {
            get { return Height*MapHelper.TileSize; }
        }

        
        public List<Animation> Animations { get; set; }
        public Tileset Tileset { get; set; }
        public List<MapLayer> Layers { get; set; }
        //public MapCollisionLayer CollisionLayer { get; set; }
        public List<Enemy> MapEnemies { get; set; }
        public List<Npc> MapNpcs { get; set; }
        public List<MapObject> MapObjects { get; set; }
        //public List<Event> MapEvents { get; set; }

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

            for (int i = 0; i < layers.Count; i++)
            {
                color = Color.White;
                layers[i].Draw(spriteBatch, min, max, testTileSet, color, color.A);
            }
        }
    }
}