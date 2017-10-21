using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceCreep.Client.Lib.Graphics;
using SpaceCreep.Client.Lib.Input;

namespace SpaceCreep.Client.Lib.Scene
{
    public class MainMenuScene : Scene
    {
        //private MainMenu mainMenu;

        private bool play;
        private Rectangle playButton = new Rectangle(290, 400, 220, 64);

        public MainMenuScene(Game game)
        {
            Game = game;
            SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
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
            SpriteBatch.Begin();
            if (play)
                SpriteBatch.Draw(GameGraphics.Menu2, Game.GraphicsDevice.Viewport.Bounds, Color.White);
            else
                SpriteBatch.Draw(GameGraphics.Menu1, Game.GraphicsDevice.Viewport.Bounds, Color.White);

            //mainMenu.Draw(spriteBatch);

            //spriteBatch.Draw(GameGraphics.SelectedItemTexture, playButton, Color.White);

            SpriteBatch.End();
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
        }
    }
}