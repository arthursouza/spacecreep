using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceCreep.Client.Lib.Objects;
using SpaceCreep.Client.Lib.Sprite;

namespace SpaceCreep.Client.Lib.Map
{
    public class MapObject : GameObject
    {
        public MapObject()
        {
            Hp = 50;
        }

        public int Hp { get; set; }

        public SpriteAnimation Animation { get; set; }
        
        public override void Draw(SpriteBatch batch)
        {
            if (Animation != null)
            {
                Animation.Position = Position;
                Animation.Draw(batch);
            }
        }
    }
}