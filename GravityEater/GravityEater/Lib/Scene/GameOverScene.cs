using System.CodeDom;
using System.Linq;
using GravityEater.Lib.Graphics;
using GravityEater.Lib.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GravityEater.Lib.Scene
{
    public class GameOverScene : Scene
    {
        //private MainMenu mainMenu;

        private bool play = false;
        private Rectangle playButton = new Rectangle(290, 400, 220, 64);
        public GameOverScene(Game game)
        {
            Game = game;
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        public override void MouseClick(MouseButton button)
        {
            if (button == MouseButton.Left)
            {
                //switch (mainMenu.SelectedOption)
                //{
                //    case MainMenuFunction.NewGame:
                //        Game.NewGame();
                //        break;
                //    case MainMenuFunction.LoadGame:
                //        Game.StartTransition(GameState.LoadGame);
                //        break;
                //    case MainMenuFunction.Help:
                //        Game.StartTransition(GameState.Help);
                //        break;
                //    case MainMenuFunction.Exit:
                //        //Exit();
                //        break;
                //    case MainMenuFunction.GameStats:
                //        Game.StartTransition(GameState.GameStatsHelp);
                //        break;
                //    default:
                //        break;
                //}
            }
        }

        public override void Update(GameTime gameTime)
        {
            play = playButton.Contains(InputManager.MousePositionPoint);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            if (play)
            {
                spriteBatch.Draw(GameGraphics.GameOverMenu2, Game.GraphicsDevice.Viewport.Bounds, Color.White);
            }
            else
            {
                spriteBatch.Draw(GameGraphics.GameOverMenu1, Game.GraphicsDevice.Viewport.Bounds, Color.White);
            }

            var points = Game.HighScores.OrderByDescending(x => x).Take(5);

            var pos = new Vector2(GameConfig.Config.WindowWidth/2, 60);

            var lineHeight = 60f;

            foreach (var score in points)
            {
                var textSize = Fonts.ArialBlack14Italic.MeasureString(score.ToString("N"));

                Drawing.DrawText(spriteBatch, Fonts.ArialBlack14Italic, score.ToString("N"), new Vector2(0, lineHeight) + pos - new Vector2(textSize.X/2, 0), Color.LightPink, true);
                pos = pos + new Vector2(0, lineHeight);
            }

            //mainMenu.Draw(spriteBatch);

            //spriteBatch.Draw(GameGraphics.SelectedItemTexture, playButton, Color.White);

            spriteBatch.End();
        }

        public override void MouseDown(MouseButton button)
        {
            if (play)
            {
                GameGraphics.SoundSelect.Play();
                Game.State = GameState.GameStarted;
            }

            base.MouseDown(button);
        }

        public override void Load()
        {
            play = false;
            Game.HighScores.Add(Game.Points);
        }
    }
}