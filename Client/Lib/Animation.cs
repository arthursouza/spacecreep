using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceCreep.Client.Lib.Objects;
using SpaceCreep.Client.Lib.Sprite;

namespace SpaceCreep.Client.Lib
{
    public class Animation : GameObject
    {
        public string Name { get; set; }

        public SpriteAnimation Sprite { get; set; }

        public void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
            Sprite.Animate();
        }

        public override void Draw(SpriteBatch batch)
        {
            Sprite.Draw(batch, Position);
        }
    }
}