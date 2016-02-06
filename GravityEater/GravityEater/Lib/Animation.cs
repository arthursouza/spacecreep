using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GravityEater.Lib.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GravityEater.Lib
{
    public class Animation : GameObject
    {
        public SpriteAnimation Sprite { get; set; }

        public void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
            Sprite.Animate();
        }

        public override void Draw(SpriteBatch batch)
        {
            Sprite.Draw(batch, Position, 1, 0f);
        }
    }
}
