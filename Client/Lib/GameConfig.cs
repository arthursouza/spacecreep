namespace SpaceCreep.Client.Lib
{
    public class GameConfig
    {
        private static GameConfig config;
        private static readonly string filePath = "Config/gameConfig.xml";

        public GameConfig()
        {
            DebugMode = false;
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
        
        public bool DebugMode { get; set; }
        public bool GamePaused { get; set; }

        public int WindowHeight { get; set; } = 600;

        public int WindowWidth { get; set; } = 800;

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