using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceCreep.Client.Lib.Map
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

                var x = textureIndex % Columns;
                var y = textureIndex / Columns;

                return new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize);
            }
            return new Rectangle();
        }
    }
}