using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GravityEater.Lib.Graphics
{
    public static class GameGraphics
    {
        public static Texture2D SelectedItemTexture { get; set; }

        #region Scene backgrounds
        
        public static Texture2D MainMenu { get; set; }
        
        #endregion
        
        #region Buttons 

        public static Texture2D CharacterHpBar { get; set; }
        public static Texture2D CharacterHpBarBg { get; set; }

        #endregion
        public static Texture2D MouseClick { get; set; }
        public static Texture2D TargetCircle { get; set; }
        public static Texture2D BigCircle { get; set; }
        public static Texture2D CloseButton { get; set; }
        public static Texture2D Button { get; set; }
        public static Texture2D ButtonHover { get; set; }
        public static Texture2D GameOver { get; set; }
        public static Texture2D BlankBarBackground { get; set; }
        public static Texture2D BlankBar { get; set; }
        public static Texture2D ButtonPressed { get; set; }
        public static Texture2D GamePaused { get; set; }
        public static Texture2D HelpMenu { get; set; }
        public static Texture2D GameStatsHelpMenu { get; set; }
        public static Texture2D EmptyTransition { get; set; }

        public static Color DialogTextColor { get; set; }
        public static Color SelectedOptionColor { get; set; }

        public static Texture2D CollisionRadius { get; set; }
        public static Texture2D SpaceTextures { get; set; }
        public static Texture2D SpaceTextures2 { get; set; }
        public static Texture2D Ship1 { get; set; }
        public static Texture2D Explosion1 { get; set; }

        public static void Load(ContentManager content)
        {
            //EmptyTransition = content.Load<Texture2D>("Images/black10px");
            //Button = content.Load<Texture2D>("Images/Button");
            //ButtonHover = content.Load<Texture2D>("Images/Button_hover");
            //ButtonPressed = content.Load<Texture2D>("Images/Button_pressed");
            //SelectedItemTexture = content.Load<Texture2D>("Textures/selectedItemTexture");
            //MainMenu = content.Load<Texture2D>("Textures/Windows/Main Menu");

            CollisionRadius = content.Load<Texture2D>("Circle 400px");
        }
    }
}