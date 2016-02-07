using System;
using System.Collections.Generic;
using System.Linq;
using GravityEater.Lib;
using GravityEater.Lib.Graphics;
using GravityEater.Lib.Input;
using GravityEater.Lib.Map;
using GravityEater.Lib.Objects;
using GravityEater.Lib.Scene;
using GravityEater.Lib.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GravityEater
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        private readonly GraphicsDeviceManager graphics;
        private GameState _state;
        private GameState nextState;
        private SpriteBatch spriteBatch;

        public BasicEffect BasicEffect;
        public BlendState BlendState;
        public int UniqueObjectId = 0;
        public Transition CurrentTransition { get; set; }
        public bool Paused
        {
            get { return GameConfig.Config.GamePaused; }
            set { GameConfig.Config.GamePaused = value; }
        }
        public GameState State
        {
            get { return _state; }
            set { ChangeState(value); }
        }
        public Map CurrentMap { get; set; }
        public Character Player { get; set; }
        public TimeSpan TimePlayed { get; set; }

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

        public Dictionary<GameState, Scene> Scenes { get; set; }
        
        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Exiting += Game_Exiting;
            Window.Title = "The World Eater";
            IsMouseVisible = true;

            GameConfig.Load();
            InputConfiguration.Load();

            Paused = false;
            
            IsFixedTimeStep = false;

            SetResolution();
        }

        private void Game_Exiting(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            BasicEffect = new BasicEffect(graphics.GraphicsDevice)
            {
                VertexColorEnabled = true
            };

            BlendState = new BlendState
            {
                AlphaSourceBlend = Blend.One,
                AlphaDestinationBlend = Blend.One,
                ColorBlendFunction = BlendFunction.Add
            };

            Scenes = new Dictionary<GameState, Scene>();

            InputManager.LastMouseState = InputManager.MouseState;
            InputManager.LastKeyboardState = InputManager.KeyboardState;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            var monsterTexture = Content.Load<Texture2D>("MonsterSprite2");
            GameGraphics.MonsterSpriteIdle = Content.Load<Texture2D>("MonsterSpriteIdle");
            GameGraphics.SpaceTextures = Content.Load<Texture2D>("SpaceTileset");
            GameGraphics.SpaceTextures2 = Content.Load<Texture2D>("SpaceTileset2");
            GameGraphics.SpaceTextures3 = Content.Load<Texture2D>("SpaceTileset3");
            GameGraphics.Ship1 = Content.Load<Texture2D>("Ships/Ship1");
            GameGraphics.BigShip1 = Content.Load<Texture2D>("Ships/BigShip1Flat");
            GameGraphics.Explosion1 = Content.Load<Texture2D>("Explosion1");
            GameGraphics.Star1 = Content.Load<Texture2D>("RandomStuff/Star1");
            GameGraphics.Planet1 = Content.Load<Texture2D>("Planet1");
            GameGraphics.CharacterHpBar = Content.Load<Texture2D>("Interface/HpBar");
            GameGraphics.CharacterHpBarBg = Content.Load<Texture2D>("Interface/HpBarBack");
            GameGraphics.HealthKit = Content.Load<Texture2D>("HealthKit1");
            GameGraphics.MonsterTrack = Content.Load<Texture2D>("MonsterTrack");
            GameGraphics.SelectedItemTexture = Content.Load<Texture2D>("selectedItemTexture");

            GameGraphics.Menu1 = Content.Load<Texture2D>("Menu1");
            GameGraphics.Menu2 = Content.Load<Texture2D>("Menu2");

            GameGraphics.SoundExplosion = Content.Load<SoundEffect>("Sounds/Explosion");
            GameGraphics.SoundExplosionBig = Content.Load<SoundEffect>("Sounds/BigExplosion");
            GameGraphics.SoundHeal = Content.Load<SoundEffect>("Sounds/Heal");

            Player = new Character(new SpriteAnimation(GameGraphics.MonsterSpriteIdle, 200, 11))
            {
                Position = MapHelper.GetPixelsFromTileCenter(new Vector2(3, 3)),
                MaxHp = 100,
                Hp = 100
            };
            

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Fonts.Load(Content);
            GameGraphics.Load(Content);

            Scenes[GameState.MainMenu] = new MainMenuScene(this);
            //Scenes[GameState.LoadGame] = new LoadGameScene(this);
            //Scenes[GameState.Help] = new HelpScene(this);
            //Scenes[GameState.GameStatsHelp] = new GameStatsHelpScene(this);
            Scenes[GameState.GameStarted] = new GameStartedScene(this);
            //Scenes[GameState.NewGame] = new NewGameScene(this);
            State = GameState.MainMenu;
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            TimePlayed = TimePlayed.Add(new TimeSpan(0, 0, 0, 0, (int)gameTime.ElapsedGameTime.TotalMilliseconds));

            UpdateMouse(gameTime);
            UpdateKeyboard();

            if (CurrentTransition != null)
                CurrentTransition.Update(gameTime);

            if (Scenes.ContainsKey(State))
                Scenes[State].Update(gameTime);

            //if (GameConfig.Config.FullScreen != graphics.IsFullScreen)
            //    graphics.ToggleFullScreen();
            
            base.Update(gameTime);
        }

        private void UpdateKeyboard()
        {
            InputManager.KeyboardState = Keyboard.GetState();

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

            if (InputManager.MouseState.LeftButton == ButtonState.Pressed && InputManager.LastMouseState.LeftButton == ButtonState.Released)
            {
                //if ((gameTime.TotalGameTime - InputManager.LastMouseLeftClick).TotalMilliseconds <= InputConfiguration.Config.DoubleClickDelay)
                //    MouseDoubleClickLeft();
                //else
                //    MouseLeftClick();

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
            else if (InputManager.MouseState.LeftButton == ButtonState.Released && InputManager.LastMouseState.LeftButton == ButtonState.Pressed)
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

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (Scenes.ContainsKey(State))
            {
                Scenes[State].Draw(gameTime);
            }

            //DrawTransition(gameTime);

            base.Draw(gameTime);
        }

        private void ChangeState(GameState value)
        {
            if (Scenes.ContainsKey(value))
            {
                _state = value;
                Scenes[value].Load();
            }
        }
    }
}
