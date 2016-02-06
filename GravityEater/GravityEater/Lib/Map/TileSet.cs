using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GravityEater.Lib.Map
{
    public class Tileset
    {
        public int TileSize { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public Texture2D TextureMap { get; set; }
        public int Columns { get; set; }

        public Rectangle GetRectForTexture(int textureIndex)
        {
            if (TextureMap != null)
            {
                //int cols = TextureMap.Width / TileSize;

                int x = textureIndex%Columns;
                int y = textureIndex/Columns;

                return new Rectangle(x*TileSize, y*TileSize, TileSize, TileSize);
            }
            return new Rectangle();
        }
    }
}