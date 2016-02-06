using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GravityEater.Lib.Objects
{
    public class Npc : Character
    {
        public Npc()
        {
            Wander = true;
        }

        public int FirstDialogId { get; set; }
        public bool Wander { get; set; }
        public bool IsAlly { get; set; }

        /// <summary>
        /// The default dialog is triggered as soon as the character enters the npc speaking radius
        /// </summary>
        public bool AutomaticTrigger { get; set; }
        
        public new void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}