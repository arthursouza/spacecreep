using Microsoft.Xna.Framework.Graphics;

namespace LotusLibrary
{
    public enum TileType
    {
        Common,
        AutoTile
    }

    public class TextureWrapper
    {
        public Texture2D Texture { get; set; }
        public TileType TileType { get; set; }
        public int TextureId { get; set; }
        public string Name { get; set; }
    }
}