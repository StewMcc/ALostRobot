namespace AKID
{
    public class EVENTS
    {
        public const uint BROKEN_ROOM = 46622052U;
        public const uint CRAFT_HYDRODOOR_CLOSE = 2225676259U;
        public const uint CRAFT_HYDRODOOR_OPEN = 1944514261U;
        public const uint CRAFT_POWERBATTERY = 2886315098U;
        public const uint ITEM_PICKUP = 566396871U;
        public const uint ITEM_POPDOWN = 2129524276U;
        public const uint ITEM_POPUP = 796655887U;
        public const uint ITEM_PORT_NEGATIVE = 3038503276U;
        public const uint ITEM_PORT_POSITIVE = 2855366340U;
        public const uint ITEM_PUTDOWN = 662770354U;
        public const uint LANDMARK_RIVER = 1923990202U;
        public const uint LANDMARK_WATERFALL = 2476173174U;
        public const uint LANDSCAPE_ICELAND = 2838966307U;
        public const uint LANDSCAPE_OCEAN = 906407167U;
        public const uint LANDSCAPE_SHUTTLE = 4004066390U;
        public const uint LANDSCAPE_WOODLAND = 1084345483U;
        public const uint MENU_BUTTON_SELECT = 4007661982U;
        public const uint MENU_MUSIC = 4055567060U;
        public const uint MENU_PAUSE = 2170009975U;
        public const uint MENU_RESUME = 2263363174U;
        public const uint MUSIC_MUTE = 1696404602U;
        public const uint MUSIC_RESET = 3282946978U;
        public const uint PLAYER_MOVE_END = 3120155140U;
        public const uint PLAYER_MOVE_START = 1016620215U;
        public const uint SONAR_BEAM = 2077633936U;
        public const uint TELEPORT = 530129416U;
    } // public class EVENTS

    public class STATES
    {
        public class MUSICAUDIOBUS
        {
            public const uint GROUP = 2293803806U;

            public class STATE
            {
                public const uint DEFAULT = 782826392U;
                public const uint MUTED = 3791155954U;
            } // public class STATE
        } // public class MUSICAUDIOBUS

        public class SFXAMBIENCEBUS
        {
            public const uint GROUP = 250260560U;

            public class STATE
            {
                public const uint DEFAULT = 782826392U;
                public const uint MUTED = 3791155954U;
            } // public class STATE
        } // public class SFXAMBIENCEBUS

    } // public class STATES

    public class BANKS
    {
        public const uint INIT = 1355168291U;
        public const uint AUDIO_SOUNDBANK = 3365706963U;
    } // public class BANKS

    public class BUSSES
    {
        public const uint AMBIENCE_BUS = 174546974U;
        public const uint MASTER_AUDIO_BUS = 3803692087U;
        public const uint MASTER_SECONDARY_BUS = 805203703U;
        public const uint MUSIC_BUS = 2680856269U;
        public const uint SOUNDFX_BUS = 2206267635U;
    } // public class BUSSES

    public class AUDIO_DEVICES
    {
        public const uint NO_OUTPUT = 2317455096U;
        public const uint SYSTEM = 3859886410U;
    } // public class AUDIO_DEVICES

}// namespace AKID

