namespace pristi
{
    public class Settings{
        private int m_InternalScreenHeight = 180;
        public int GetInternalScreenHeight() => m_InternalScreenHeight;
        private int m_InternalScreenWidth = 320;
        public int GetInteralScreenWidth() => m_InternalScreenWidth;
        private int m_ScreenHeight = 800;
        public int GetScreenHeight() => m_ScreenHeight;
        private int m_ScreenWidth = 1280;
        public int GetScreenWidth() => m_ScreenWidth;
        private bool m_FullScreen = false;
        public bool IsFullscreen() => m_FullScreen;


        public Settings(){

        }
    }

    public static class StaticSettings
    {
        private static int tileWidth = 16;
        public static int GetTileWidth() => tileWidth;
        private static int tileHeight = 16;
        public static int GetTileHeight() => tileHeight;
    }
}
