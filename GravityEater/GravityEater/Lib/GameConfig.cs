namespace GravityEater.Lib
{
    public class GameConfig
    {
        private static GameConfig config;
        private static string filePath = "Config/gameConfig.xml";

        private int windowHeight = 600;
        private int windowWidth = 800;

        public GameConfig()
        {
            ShowPlayerInfo = true;
            ShowHud = true;
            OnlyDrawVisibleMap = true;
        }

        public static GameConfig Config
        {
            get
            {
                if (config == null) config = new GameConfig();
                return config;
            }
            set { config = value; }
        }

        public bool SendGameStats { get; set; }
        public bool DebugMode { get; set; }
        public bool ShowPlayerInfo { get; set; }
        public bool ShowHud { get; set; }
        public bool GamePaused { get; set; }
        public bool GameStarted { get; set; }
        public bool DrawMapMatrix { get; set; }
        public bool FullScreen { get; set; }
        public bool OnlyDrawVisibleMap { get; set; }
        public bool ShowPathFinding { get; set; }

        public int WindowHeight
        {
            get { return windowHeight; }
            set { windowHeight = value; }
        }

        public int WindowWidth
        {
            get { return windowWidth; }
            set { windowWidth = value; }
        }

        public static void Load()
        {
            Config = Serializer<GameConfig>.DeserializeObject(filePath);
        }

        public static void Save()
        {
            Serializer<GameConfig>.SerializeObject(Config, filePath);
        }
    }
}