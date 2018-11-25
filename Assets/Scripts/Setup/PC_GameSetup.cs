/*===============================================================*
 *                                                               *
 *       Script made by Manuel Rodríguez Matesanz                *
 *          Free to use if credits are given                     *
 *                                                               *
 *===============================================================*/


namespace PokemonCasino.Setup
{
    public static class PC_GameSetup
    {
        public static class General
        {
            public const string WEB_URL = "http://manuelrodriguezmatesanz.com";
        }

        public static class Animations
        {
            public const string ATTACK = "Attack ";
        }

        public static class PlayerPrefs
        {
            public const string LAST_LANGUAGE = "CPG_LastLanguagePP";
        }

        public static class Tags
        {
  
        }

        public static class Game_Settings
        {
            public const bool SAVE_ON_EXIT = true;
            /// <summary>
            /// We want a binary JSON? If not encrypted, can be easily modified using Visual Code
            /// </summary>
            public const bool USE_BINARY_SAVE_FILES = false;
            /// <summary>
            /// Encrypt the save file using DES Encryption? The best option on final build is 
            /// encrypt and use binary - It will be a "double trouble" file! 
            /// </summary>
            public const bool ENCRYPT_SAVE_FILES = true;

        }

        public static class Casino
        {
            /// <summary>
            /// How many coins we can have at max
            /// </summary>
            public const int COIN_LIMIT = 99999;
        }

        public static class SlotMachine
        {

            public const string PLAY = "play";
            public const string STOP = "stop";
            public const string INSERT = "insert";
            public const string PRESS = "press";
            public const string LOSE = "lose";
            public const string WIN = "win";
            public const string BONUS = "bonus";
        }
    }

}
