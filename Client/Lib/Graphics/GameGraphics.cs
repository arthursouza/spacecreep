using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceCreep.Client.Lib.Graphics
{
    public static class GameGraphics
    {
        public static Texture2D CollisionRadius { get; set; }
        public static Texture2D SpaceTextures { get; set; }
        public static Texture2D SpaceTextures3 { get; set; }
        public static Texture2D Ship1 { get; set; }
        public static Texture2D Explosion1 { get; set; }
        public static Texture2D BigShip1 { get; set; }
        public static Texture2D Star1 { get; internal set; }
        public static Texture2D MonsterSpriteIdle { get; set; }
        public static Texture2D HealthKit { get; set; }
        public static Texture2D MonsterTrack { get; set; }
        public static Texture2D Planet1 { get; set; }

        public static Texture2D Menu1 { get; set; }
        public static Texture2D Menu2 { get; set; }
        public static SoundEffect SoundExplosion { get; internal set; }
        public static SoundEffect SoundExplosionBig { get; set; }
        public static SoundEffect SoundHeal { get; set; }
        public static Texture2D MonsterSpriteAttack { get; internal set; }
        public static Texture2D GameOverMenu1 { get; set; }
        public static Texture2D GameOverMenu2 { get; set; }
        public static Texture2D Ship2 { get; set; }
        public static SoundEffect SoundSelect { get; set; }
        public static Texture2D MovementCrosshair { get; set; }

        public static void Load(ContentManager content)
        {
            CollisionRadius = content.Load<Texture2D>("Circle 400px");
        }

        #region Buttons 

        public static Texture2D CharacterHpBar { get; set; }
        public static Texture2D CharacterHpBarBg { get; set; }

        #endregion
    }
}