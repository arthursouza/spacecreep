using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SpaceCreep.Client.Lib;
using SpaceCreep.Client.Lib.Graphics;
using SpaceCreep.Client.Lib.Input;
using SpaceCreep.Client.Lib.Map;
using SpaceCreep.Client.Lib.Objects;
using SpaceCreep.Client.Lib.Scene;
using SpaceCreep.Client.Lib.Sprite;

namespace SpaceCreep.Client
{
    /// <summary>
    ///     This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        private readonly GraphicsDeviceManager graphics;
        private GameState state;
        private Song music;

        public BasicEffect BasicEffect;
        
        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Exiting += Game_Exiting;
            Window.Title = "The World Eater";
            IsMouseVisible = true;

            GameConfig.Load();

            GameConfig.Config.GamePaused = false;

            IsFixedTimeStep = false;
            HighScores = new List<int>();
            SetResolution();
        }
        
        public GameState State
        {
            get { return state; }
            set { ChangeState(value); }
        }

        public Map CurrentMap { get; set; }
        public Character Player { get; set; }
        public TimeSpan TimePlayed { get; set; }

        public List<int> HighScores { get; set; }

        public int Points { get; private set; }

        private Dictionary<GameState, Scene> Scenes { get; set; }

        public void ChangePoints(int value)
        {
            Points += value;

            if (Points < 0)
                Points = 0;
        }

        private void SetResolution()
        {
            if (GameConfig.Config.WindowWidth > GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width ||
                GameConfig.Config.WindowHeight > GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
            {
                GameConfig.Config.WindowWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                GameConfig.Config.WindowHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }

            graphics.PreferredBackBufferWidth = GameConfig.Config.WindowWidth;
            graphics.PreferredBackBufferHeight = GameConfig.Config.WindowHeight;
        }

        private void Game_Exiting(object sender, EventArgs e)
        {
        }

        protected override void Initialize()
        {
            BasicEffect = new BasicEffect(graphics.GraphicsDevice)
            {
                VertexColorEnabled = true
            };

    Scenes = new Dictionary<GameState, Scene>();

            InputManager.LastMouseState = InputManager.MouseState;
            InputManager.LastKeyboardState = InputManager.KeyboardState;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            #region Load Stuff

            GameGraphics.MonsterSpriteIdle = Content.Load<Texture2D>("MonsterSpriteIdle");
            GameGraphics.SpaceTextures = Content.Load<Texture2D>("SpaceTileset");
            //GameGraphics.SpaceTextures2 = Content.Load<Texture2D>("SpaceTileset2");
            GameGraphics.SpaceTextures3 = Content.Load<Texture2D>("SpaceTileset3");
            GameGraphics.Ship1 = Content.Load<Texture2D>("Ships/Ship1");
            GameGraphics.Ship2 = Content.Load<Texture2D>("Ships/Ship2");

            GameGraphics.BigShip1 = Content.Load<Texture2D>("Ships/BigShip1Flat");

            GameGraphics.Explosion1 = Content.Load<Texture2D>("Explosion1");
            GameGraphics.Star1 = Content.Load<Texture2D>("RandomStuff/Star1");
            GameGraphics.Planet1 = Content.Load<Texture2D>("Planet1");

            GameGraphics.CharacterHpBar = Content.Load<Texture2D>("Interface/HpBar2");
            GameGraphics.CharacterHpBarBg = Content.Load<Texture2D>("Interface/HpBarBack2");

            GameGraphics.HealthKit = Content.Load<Texture2D>("HealthKit1");
            GameGraphics.MonsterTrack = Content.Load<Texture2D>("MonsterTrack");
            
            GameGraphics.Menu1 = Content.Load<Texture2D>("Menu1");
            GameGraphics.Menu2 = Content.Load<Texture2D>("Menu2");

            GameGraphics.GameOverMenu1 = Content.Load<Texture2D>("GameOverMenu1");
            GameGraphics.GameOverMenu2 = Content.Load<Texture2D>("GameOverMenu2");

            GameGraphics.SoundExplosion = Content.Load<SoundEffect>("Sounds/Explosion");
            GameGraphics.SoundExplosionBig = Content.Load<SoundEffect>("Sounds/BigExplosion");
            GameGraphics.SoundHeal = Content.Load<SoundEffect>("Sounds/Heal");
            GameGraphics.SoundSelect = Content.Load<SoundEffect>("Sounds/Select1");
            music = Content.Load<Song>("Sounds/Searching");
            GameGraphics.MonsterSpriteAttack = Content.Load<Texture2D>("MonsterSprite2");

            #endregion

            GameGraphics.MovementCrosshair = Content.Load<Texture2D>("MovementHelper");

            MediaPlayer.Play(music);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.3f;

            NewGame();
            
            Fonts.Load(Content);
            GameGraphics.Load(Content);

            Scenes[GameState.MainMenu] = new MainMenuScene(this);
            Scenes[GameState.GameStarted] = new GameStartedScene(this);
            Scenes[GameState.GameOver] = new GameOverScene(this);

            State = GameState.MainMenu;
        }

        public void NewGame()
        {
            Player = new Character(new SpriteAnimation(GameGraphics.MonsterSpriteIdle, 200, 11))
            {
                Position = MapHelper.GetPixelsFromTileCenter(new Vector2(3, 3)),
                MaxHp = 100,
                Hp = 100
            };

            Points = 0;

            Player.AttackSprite = new SpriteAnimation(GameGraphics.MonsterSpriteAttack, 200, 4);

            #region GenerateMap

            var firstMap = new Map
            {
                Height = 50,
                Width = 50,
                Tileset = new Tileset
                {
                    Columns = 4,
                    TextureMap = GameGraphics.SpaceTextures3,
                    TileSize = 64
                },
                Layers = new List<MapLayer>
                {
                    new MapLayer(50, 50)
                }
            };

            var textureMap = new int[50, 50];

            var rand = new Random(DateTime.Now.Millisecond);
            for (var y = 0; y < 50; y++)
            for (var x = 0; x < 50; x++)
                textureMap[x, y] = rand.Next(0, 16);

            CurrentMap = firstMap;

            firstMap.Layers[0].Map = textureMap;

            #endregion
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            TimePlayed = TimePlayed.Add(new TimeSpan(0, 0, 0, 0, (int) gameTime.ElapsedGameTime.TotalMilliseconds));

            UpdateMouse(gameTime);
            UpdateKeyboard();

            if (Scenes.ContainsKey(State))
                Scenes[State].Update(gameTime);
            
            base.Update(gameTime);
        }

        private void UpdateKeyboard()
        {
            InputManager.KeyboardState = Keyboard.GetState();

            if (InputManager.KeyPress(Keys.OemPlus))
                MediaPlayer.Volume += 0.1f;
            if (InputManager.KeyPress(Keys.OemMinus))
                MediaPlayer.Volume -= 0.1f;

            if (Scenes.ContainsKey(State))
                Scenes[State].UpdateKeyboardInput();

            InputManager.LastKeyboardState = InputManager.KeyboardState;
        }

        private void UpdateMouse(GameTime gameTime)
        {
            InputManager.MouseState = Mouse.GetState();

            if (InputManager.MousePosition.X > GraphicsDevice.Viewport.Width || InputManager.MousePosition.X < 0 ||
                InputManager.MousePosition.Y > GraphicsDevice.Viewport.Height || InputManager.MousePosition.Y < 0)
                return;

            //#region Mouse Right Button

            //if (InputManager.MouseState.RightButton == ButtonState.Pressed &&
            //    InputManager.LastMouseState.RightButton == ButtonState.Released)
            //{
            //    if ((gameTime.TotalGameTime - InputManager.LastMouseRightClick).TotalMilliseconds <=
            //        InputConfiguration.Config.DoubleClickDelay)
            //    {
            //        InputManager.LastMouseRightClick = gameTime.TotalGameTime;
            //        MouseDoubleClickRight();
            //    }
            //    else
            //    {
            //        InputManager.LastMouseRightClick = gameTime.TotalGameTime;
            //        MouseRightClick();
            //    }
            //}

            //#endregion

            #region Mouse Left Button

            if (InputManager.MouseState.LeftButton == ButtonState.Pressed &&
                InputManager.LastMouseState.LeftButton == ButtonState.Released)
            {
                //if ((gameTime.TotalGameTime - InputManager.LastMouseLeftClick).TotalMilliseconds <= InputConfiguration.Config.DoubleClickDelay)
                //    MouseDoubleClickLeft();
                //else
                MouseClick(MouseButton.Left);

                InputManager.LastMouseLeftClick = gameTime.TotalGameTime;
            }
            else if (InputManager.MouseState.LeftButton == ButtonState.Pressed)
            {
                MouseDown(MouseButton.Left);

                //if (InputManager.LastMouseState.X != InputManager.MouseState.X || InputManager.LastMouseState.Y != InputManager.MouseState.Y)
                //{
                //    MouseDrag();
                //}
            }
            else if (InputManager.MouseState.LeftButton == ButtonState.Released &&
                     InputManager.LastMouseState.LeftButton == ButtonState.Pressed)
            {
                MouseUp(MouseButton.Left);
            }

            #endregion

            //if (InputManager.MouseState.ScrollWheelValue != InputManager.LastMouseState.ScrollWheelValue)
            //{
            //    MouseScroll();
            //}


            if (Scenes.ContainsKey(State))
                Scenes[State].UpdateMouseInput();

            InputManager.LastMouseState = InputManager.MouseState;
        }

        private void MouseClick(MouseButton left)
        {
            if (Scenes.ContainsKey(State))
                Scenes[State].MouseClick(left);
        }

        private void MouseUp(MouseButton left)
        {
            if (Scenes.ContainsKey(State))
                Scenes[State].MouseUp(left);
        }

        private void MouseDown(MouseButton mouseButton)
        {
            if (Scenes.ContainsKey(State))
                Scenes[State].MouseDown(mouseButton);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (Scenes.ContainsKey(State))
                Scenes[State].Draw(gameTime);

            //DrawTransition(gameTime);

            base.Draw(gameTime);
        }

        private void ChangeState(GameState value)
        {
            if (Scenes.ContainsKey(value))
            {
                state = value;
                Scenes[value].Load();
            }
        }
    }
}