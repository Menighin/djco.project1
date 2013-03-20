using System.Collections.Generic;

namespace Game_Test_2 {
    class GlobalEnvironment {

        private static float screenWidth;
        private static float screenHeight;
        public const int ENLARGE = 3;
        private static List<Platform> platform = new List<Platform>();

        public static float ScreenWidth {
            get { return screenWidth; }
            set { screenWidth = value; }
        }

        public static float ScreenHeight {
            get { return screenHeight; }
            set { screenHeight = value; }
        }

        public static void addPlatform (Platform p) {
            platform.Add(p);
        }

        public static List<Platform> getPlatformList() {
            return platform;
        }

    }
}
