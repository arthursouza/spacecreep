using System.Collections.Generic;
using GravityEater.Lib.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GravityEater.Lib.Map
{
    public class MapObject : GameObject
    {
        public MapObject()
        {
            Hp = 50;
        }

        public int Hp { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public SpriteAnimation Animation { get; set; }
        //public SpriteInformation SpriteInformation { get; set; }
        public Vector2 Origin { get; set; }

        public override void Draw(SpriteBatch batch)
        {
            if (Animation != null)
            {
                Animation.Position = Position;
                Animation.Draw(batch);
            }
        }

        //public void DrawBounds(SpriteBatch batch, GraphicsDevice graphics)
        //{
        //    var size = new Vector2(Animation.Width, Animation.Height);
        //    Color c = Color.Black;

        //    // Top line
        //    Drawing.DrawLine(graphics, Position, new Vector2(Position.X + size.X, Position.Y), c);
        //    // Bottom line
        //    Drawing.DrawLine(graphics, new Vector2(Position.X, Position.Y + size.Y),
        //        new Vector2(Position.X + size.X, Position.Y + size.Y), c);
        //    // Left line
        //    Drawing.DrawLine(graphics, Position, new Vector2(Position.X, Position.Y + size.Y), c);
        //    // Right line
        //    Drawing.DrawLine(graphics, new Vector2(Position.X + size.X, Position.Y),
        //        new Vector2(Position.X + size.X, Position.Y + size.Y), c);
        //    //left > right
        //    Drawing.DrawLine(graphics, Position, new Vector2(Position.X + size.X, Position.Y + size.Y), c);
        //    //right > left
        //    Drawing.DrawLine(graphics, new Vector2(Position.X + size.X, Position.Y),
        //        new Vector2(Position.X, Position.Y + size.Y), c);
        //    //vertical mid line 
        //    Drawing.DrawLine(graphics, new Vector2(Position.X + (size.X/2), Position.Y),
        //        new Vector2(Position.X + (size.X/2), Position.Y + size.Y), c);
        //    //horizontal mid line 
        //    Drawing.DrawLine(graphics, new Vector2(Position.X, Position.Y + (size.Y/2)),
        //        new Vector2(Position.X + size.X, Position.Y + (size.Y/2)), c);
        //}


    }
}