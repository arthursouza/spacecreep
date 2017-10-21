using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceCreep.Client.Lib
{
    public static class Fonts
    {
        public static SpriteFont Arial12 { get; set; }
        
        public static void Load(ContentManager content)
        {
            Arial12 = content.Load<SpriteFont>("Fonts/Arial12");
        }
    }
}