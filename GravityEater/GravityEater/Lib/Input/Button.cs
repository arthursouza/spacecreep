//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;

//namespace GravityEater.Lib.Input
//{
//    public class Button : UIComponent
//    {
//        public delegate void ButtonClickHandler(object sender);

//        private bool Pressed;
        
//        public SpriteFont Font;
//        public int Height;
//        public Vector2 Position;

//        public string Text;
//        public int Width;

//        public Vector2 BorderPadding;
//        public Texture2D Texture;
//        public Texture2D TextureHover;
//        public Texture2D TexturePressed;

//        public Button()
//        {
//        }

//        public Button(string text, Texture2D texture = null, Texture2D textureHover = null, Texture2D texturePressed = null)
//        {
//            if (textureHover == null)
//                TextureHover = GameGraphics.ButtonHover;

//            if (texturePressed == null)
//                TexturePressed = GameGraphics.ButtonPressed;

//            if (texture == null)
//                Texture = GameGraphics.Button;
        
//            Font = Fonts.TrebuchetMS14;
//            Vector2 size = Font.MeasureString(text);
//            Position = new Vector2(
//                (GameConfig.Config.WindowWidth - size.X - 30),
//                (GameConfig.Config.WindowHeight - size.Y - 30));

//            BorderPadding = new Vector2(15, 15);
//            Width = (int) (BorderPadding.X*2 + size.X);
//            Height = (int) (BorderPadding.Y*2 + size.Y);
//            Text = text;
//        }

//        private bool MouseOver
//        {
//            get { return Bounds.Contains(InputManager.MousePositionPoint); }
//        }

//        public Rectangle Bounds
//        {
//            get { return new Rectangle((int) Position.X, (int) Position.Y, Width, Height); }
//        }

//        public event ButtonClickHandler Clicked;

//        public override void Draw(SpriteBatch batch)
//        {
//            var texture = Pressed ? TexturePressed : MouseOver ? TextureHover : Texture;

//            Vector2 textSize = Font.MeasureString(Text);
//            var textPosition = new Vector2(Position.X + Width/2 - textSize.X/2, Position.Y + Height/2 - textSize.Y/2);
//            DrawBackground(batch, texture, Bounds, BorderPadding);
//            Drawing.DrawText(batch, Font, Text, textPosition, Color.White, true);
//        }

//        public override void Update()
//        {
//            if (Pressed && InputManager.MouseState.LeftButton == ButtonState.Released)
//            {
//                Clicked.Invoke(this);
//            }

//            Pressed = MouseOver && InputManager.MouseState.LeftButton == ButtonState.Pressed;
//        }


//        protected void DrawBackground(SpriteBatch batch, Texture2D texture, Rectangle area, Vector2 borderPadding)
//        {
//            //Desenhando Bordas
//            // ESQUERDA CIMA
//            batch.Draw(
//                texture,
//                new Rectangle(area.X, area.Y, (int) borderPadding.X, (int) borderPadding.Y),
//                new Rectangle(0, 0, (int) borderPadding.X, (int) borderPadding.Y),
//                Color.White);

//            // ESQUERDA BAIXO
//            batch.Draw(
//                texture,
//                new Rectangle(area.X, (int) (area.Y + area.Height - borderPadding.Y), (int) borderPadding.X,
//                    (int) borderPadding.Y),
//                new Rectangle(0, (int) (texture.Height - borderPadding.Y), (int) borderPadding.X, (int) borderPadding.Y),
//                Color.White);

//            // DIREITA CIMA
//            batch.Draw(
//                texture,
//                new Rectangle(area.X + area.Width - (int) borderPadding.X, area.Y, (int) borderPadding.X,
//                    (int) borderPadding.Y),
//                new Rectangle((int) (texture.Width - borderPadding.X), 0, (int) borderPadding.X, (int) borderPadding.Y),
//                Color.White);

//            // DIREITA BAIXO
//            batch.Draw(
//                texture,
//                new Rectangle(area.X + area.Width - (int) borderPadding.X, area.Y + area.Height - (int) borderPadding.Y,
//                    (int) borderPadding.X, (int) borderPadding.Y),
//                new Rectangle(texture.Width - (int) borderPadding.X, texture.Height - (int) borderPadding.X,
//                    (int) borderPadding.X, (int) borderPadding.Y),
//                Color.White);

//            //Desenhando Cantos'
//            // CIMA
//            batch.Draw(
//                texture,
//                new Rectangle(area.X + (int) borderPadding.X, area.Y, area.Width - (int) borderPadding.X*2,
//                    (int) borderPadding.Y),
//                new Rectangle((int) borderPadding.X, 0, texture.Width - (int) (borderPadding.X*2), (int) borderPadding.Y),
//                Color.White);
//            // BAIXO
//            batch.Draw(
//                texture,
//                new Rectangle(area.X + (int) borderPadding.X, area.Y + area.Height - (int) borderPadding.Y,
//                    area.Width - (int) borderPadding.X*2, (int) borderPadding.X),
//                new Rectangle((int) borderPadding.X, texture.Height - (int) borderPadding.Y,
//                    texture.Width - (int) (borderPadding.X*2), (int) borderPadding.Y),
//                Color.White);
//            // ESQUERDA
//            batch.Draw(
//                texture,
//                new Rectangle(area.X, area.Y + (int) borderPadding.Y, (int) borderPadding.X,
//                    area.Height - (int) borderPadding.Y*2),
//                new Rectangle(0, (int) borderPadding.Y, (int) borderPadding.X,
//                    texture.Height - (int) (borderPadding.Y*2)),
//                Color.White);
//            // DIREITA
//            batch.Draw(
//                texture,
//                new Rectangle(area.X + area.Width - (int) borderPadding.X, area.Y + (int) borderPadding.Y,
//                    (int) borderPadding.X, area.Height - (int) borderPadding.Y*2),
//                new Rectangle(texture.Width - (int) borderPadding.X, (int) borderPadding.Y, (int) borderPadding.X,
//                    texture.Height - (int) (borderPadding.Y*2)),
//                Color.White);

//            //Desenhando Meio
//            batch.Draw(
//                texture,
//                new Rectangle(area.X + (int) borderPadding.X, area.Y + (int) borderPadding.Y,
//                    area.Width - (int) borderPadding.X*2, area.Height - (int) borderPadding.Y*2),
//                new Rectangle((int) borderPadding.X, (int) borderPadding.Y, texture.Width - (int) (borderPadding.X*2),
//                    texture.Height - (int) (borderPadding.Y*2)),
//                Color.White);
//        }
//    }
//}