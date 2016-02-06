//using GravityEater.Lib.Graphics;
//using GravityEater.Lib.Input;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace GravityEater.Lib.Scene
//{
//    public class MainMenuScene : Scene
//    {
//        //private MainMenu mainMenu;

//        //public MainMenuScene(LotusGame game)
//        //{
//        //    Game = game;
//        //    spriteBatch = new SpriteBatch(Game.GraphicsDevice);
//        //}

//        public override void MouseClick(MouseButton button)
//        {
//            if (button == MouseButton.Left)
//            {
//                //switch (mainMenu.SelectedOption)
//                //{
//                //    case MainMenuFunction.NewGame:
//                //        Game.NewGame();
//                //        break;
//                //    case MainMenuFunction.LoadGame:
//                //        Game.StartTransition(GameState.LoadGame);
//                //        break;
//                //    case MainMenuFunction.Help:
//                //        Game.StartTransition(GameState.Help);
//                //        break;
//                //    case MainMenuFunction.Exit:
//                //        //Exit();
//                //        break;
//                //    case MainMenuFunction.GameStats:
//                //        Game.StartTransition(GameState.GameStatsHelp);
//                //        break;
//                //    default:
//                //        break;
//                //}
//            }
//        }

//        public override void Update(GameTime gameTime)
//        {
//            mainMenu.Update(gameTime);
//        }

//        public override void Draw(GameTime gameTime)
//        {
//            spriteBatch.Begin();
//            spriteBatch.Draw(GameGraphics.MainMenu, Game.GraphicsDevice.Viewport.Bounds, Color.White);
//            mainMenu.Draw(spriteBatch);
//            spriteBatch.End();
//        }

//        public override void Load()
//        {
//            Game.SavedGames = GameSave.LoadAll();
            
//            mainMenu = new MainMenu(Game);
//        }
//    }
//}