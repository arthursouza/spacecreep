using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceCreep.Client.Lib.Graphics;
using SpaceCreep.Client.Lib.Input;

namespace SpaceCreep.Client.Lib.Scene
{
    public class GameOverScene : Scene
    {
        //private MainMenu mainMenu;

        private bool play;
        private Rectangle playButton = new Rectangle(290, 400, 220, 64);

        public GameOverScene(Game game)
        {
            Game = game;
            SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        public override void MouseClick(MouseButton button)
        {
            if (button == MouseButton.Left)
                if (play)
                {
                    GameGraphics.SoundSelect.Play();
                    Game.State = GameState.GameStarted;
                }
        }

        public override void Update(GameTime gameTime)
        {
            play = playButton.Contains(InputManager.MousePositionPoint);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            if (play)
                SpriteBatch.Draw(GameGraphics.GameOverMenu2, Game.GraphicsDevice.Viewport.Bounds, Color.White);
            else
                SpriteBatch.Draw(GameGraphics.GameOverMenu1, Game.GraphicsDevice.Viewport.Bounds, Color.White);

            var points = Game.HighScores.OrderByDescending(x => x).Take(5);

            var pos = new Vector2(GameConfig.Config.WindowWidth / 2, 60);

            var lineHeight = 60f;

            foreach (var score in points)
            {
                var scoreText = score.ToString("000 000 000 000");
                var textSize = Fonts.Arial12.MeasureString(scoreText);

                Drawing.DrawText(SpriteBatch, Fonts.Arial12, scoreText, new Vector2(0, lineHeight) + pos - new Vector2(textSize.X / 2, 0), Color.LightPink, true);
                pos = pos + new Vector2(0, lineHeight);
            }

            //mainMenu.Draw(spriteBatch);

            //spriteBatch.Draw(GameGraphics.SelectedItemTexture, playButton, Color.White);

            SpriteBatch.End();
        }

        public override void Load()
        {
            play = false;
            Game.HighScores.Add(Game.Points);
        }
    }
}