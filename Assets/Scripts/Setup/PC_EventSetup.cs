/*===============================================================*
 *                                                               *
 *       Script made by Manuel Rodríguez Matesanz                *
 *          Free to use if credits are given                     *
 *                                                               *
 *===============================================================*/

namespace PokemonCasino.Setup
{
    public static class PC_EventSetup
    {
        public const string SAVE_GAME = "SaveGameEvent";
        public const string LOAD_GAME = "LoadGameEvent";
        public const string GAME_LOADED = "GameLoadedEvent";

        public static class Localization
        {
            public const string TRANSLATE_TEXTS = "TranslateTextsEvent";
        }

        public static class Example
        {
            public const string EXAMPLE_EVENT = "ExampleEvent";
            public const string EXAMPLE_EVENT_2 = "Example2Event";
            public const string EXAMPLE_EVENT_3 = "Example3Event";
        }

        public static class Scene
        {
            public const string LOAD_SCENE = "LoadSceneEvent";
            public const string LOAD_SCENE_WITH_EVENT = "LoadSceneWithEventEvent";
        }

        public static class SlotMachine
        {
            public const string LOSE = "LoseSlotMahcineEvent";
            public const string CAN_INSERT = "CanInsertSlotMahcineEvent";
            public const string PLAY_ANIMATION = "PlayAnimationSlotMahcineEvent";

        }

        public static class Menu
        {
            public const string OPEN_WEB = "OpenWebEvent";
            public const string EXIT = "ExitGameEvent";
        }
    }
}
