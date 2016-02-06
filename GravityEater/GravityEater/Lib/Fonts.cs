using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GravityEater.Lib
{
    public class Fonts
    {
        public static Color SelectedMenuItemColor;
        public static SpriteFont Arial12 { get; set; }
        public static SpriteFont ArialBlack12 { get; set; }
        public static SpriteFont ArialBlack14 { get; set; }
        public static SpriteFont ArialBlack16 { get; set; }
        public static SpriteFont ArialBlack20 { get; set; }
        public static SpriteFont ArialBlack14Italic { get; set; }
        public static SpriteFont Verdana8 { get; set; }
        public static SpriteFont Verdana16 { get; set; }
        public static SpriteFont Verdana18 { get; set; }
        public static SpriteFont Verdana25 { get; set; }
        public static SpriteFont Verdana30 { get; set; }
        public static SpriteFont CourierNew12 { get; set; }
        public static SpriteFont TrebuchetMS12 { get; set; }
        public static SpriteFont TrebuchetMS14 { get; set; }
        public static SpriteFont BlackChancery36 { get; set; }

        public Dictionary<int, SpriteFont> Arial()
        {
            return new Dictionary<int, SpriteFont>()
            {
                { 12, Fonts.Arial12 },
            };
        }

        public Dictionary<int, SpriteFont> ArialBlack()
        {
            return new Dictionary<int, SpriteFont>()
            {
                { 14, Fonts.ArialBlack14 },
                { 16, Fonts.ArialBlack16 },
                { 20, Fonts.ArialBlack20 },
            };
        }

        public Dictionary<int, SpriteFont> Verdana()
        {
            return new Dictionary<int, SpriteFont>()
            {
                { 8, Fonts.Verdana8 },
                { 16, Fonts.Verdana16 },
                { 18, Fonts.Verdana18 },
                { 25, Fonts.Verdana25 },
                { 30, Fonts.Verdana30 },
            };
        }

        //{12, Fonts.CourierNew12       },
        //{12, Fonts.TrebuchetMS12      },
        //{14, Fonts.TrebuchetMS14      },
        //{36, Fonts.BlackChancery36     },



        /// <summary>
        ///     Faz a quebra de linha de um texto para que caiba numa área especifica.
        /// </summary>
        /// <param name="text"></param>
        /// <returns>Retorna a string formatada</returns>
        public static string WrapText(string text, int width, SpriteFont spriteFont)
        {
            if (text.Contains(" "))
            {
                var sb = new StringBuilder();
                string[] words = text.Split(' ');
                float lineWidth = 0f;

                float spaceWidth = spriteFont.MeasureString(" ").X;

                foreach (string word in words)
                {
                    if (!word.StartsWith("\n"))
                    {
                        Vector2 size = spriteFont.MeasureString(word);

                        if (lineWidth + size.X < width)
                        {
                            sb.Append(word + " ");
                            lineWidth += size.X + spaceWidth;
                        }
                        else
                        {
                            sb.Append("\n" + word + " ");
                            lineWidth = size.X + spaceWidth;
                        }
                    }
                    else
                    {
                        Vector2 size = spriteFont.MeasureString(word);
                        sb.Append(word + " ");
                        lineWidth = size.X + spaceWidth;
                    }
                }
                return sb.ToString().Trim();
            }
            return text;
        }

        public static void Load(ContentManager content)
        {
            SelectedMenuItemColor = new Color(255, 102, 0);
            Arial12 = content.Load<SpriteFont>("Fonts/Arial12");
            ArialBlack12 = content.Load<SpriteFont>("Fonts/ArialBlack12");
            ArialBlack14 = content.Load<SpriteFont>("Fonts/ArialBlack14");
            ArialBlack14Italic = content.Load<SpriteFont>("Fonts/ArialBlack14Italic");
            ArialBlack20 = content.Load<SpriteFont>("Fonts/ArialBlack20");
            ArialBlack16 = content.Load<SpriteFont>("Fonts/ArialBlack16");
            Verdana8 = content.Load<SpriteFont>("Fonts/Verdana8");
            Verdana16 = content.Load<SpriteFont>("Fonts/Verdana16");
            Verdana18 = content.Load<SpriteFont>("Fonts/Verdana18");
            Verdana25 = content.Load<SpriteFont>("Fonts/Verdana25");
            Verdana30 = content.Load<SpriteFont>("Fonts/Verdana30");
            TrebuchetMS14 = content.Load<SpriteFont>("Fonts/TrebuchetMS 14");
            TrebuchetMS12 = content.Load<SpriteFont>("Fonts/TrebuchetMS 12");
            //BlackChancery36 = content.Load<SpriteFont>("Fonts/BlackChancery36");
        }
    }
}