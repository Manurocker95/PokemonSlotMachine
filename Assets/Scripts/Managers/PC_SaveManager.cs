/*===============================================================*
 *                                                               *
 *       Script made by Manuel Rodríguez Matesanz                *
 *          Free to use if credits are given                     *
 *                                                               *
 *===============================================================*/

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using PokemonCasino.Setup;
using PokemonCasino.Serialization;

namespace PokemonCasino.Save
{
    public class PC_SaveManager : PC_SingletonMonobehaviour<PC_SaveManager>
    {
        /// <summary>
        /// Serialized game data
        /// </summary>
        public PC_SaveData m_gameData;
        /// <summary>
        /// Folder created in Documents for the save file
        /// </summary>
        [SerializeField] private string m_gameName = "Pokémon Casino";
        /// <summary>
        /// GameName
        /// </summary>
        private string m_path;

        // Use this for initialization
        void Start()
        {
            m_gameData = new PC_SaveData();
            m_path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\" + m_gameName;

            if (!Directory.Exists(m_path))
            {
                Directory.CreateDirectory(m_path);
                Debug.Log("Creating Path in " + m_path);
            }

            m_path += "\\" + PC_SaveSetup.SAVE_FILE;

            PC_EventManager.StartListening(PC_EventSetup.SAVE_GAME, SaveGame);
            PC_EventManager.StartListening(PC_EventSetup.LOAD_GAME, LoadGame);
        }
        /// <summary>
        /// Save the game to a file - Just a test - set your data as needed
        /// </summary>
        public void SaveGame()
        {
            PC_SaveData currentData = PC_Casino.Instance.GetCommonData();
            m_gameData.coins = currentData.coins;
            m_gameData.hasCoinCase = currentData.hasCoinCase;

            if (PC_GameSetup.Game_Settings.ENCRYPT_SAVE_FILES)
                PC_ComplexFormatter.SaveObjectToDESFile(m_gameData, m_path);
            else if (PC_GameSetup.Game_Settings.USE_BINARY_SAVE_FILES)
                PC_ComplexFormatter.SaveObjectToBinaryFile(m_gameData, m_path);
            else
                PC_ComplexFormatter.SaveObjectoToJSONFile(m_gameData, m_path);

            Debug.Log("Saved the game data in " + m_path);
        }
        /// <summary>
        /// Load the data from the file
        /// </summary>
        public void LoadGame()
        {
            if (File.Exists(m_path))
            {
                if (PC_GameSetup.Game_Settings.ENCRYPT_SAVE_FILES)
                    m_gameData = PC_ComplexFormatter.LoadObjectFromDESFile<PC_SaveData>(m_path);
                else if (PC_GameSetup.Game_Settings.USE_BINARY_SAVE_FILES)
                    m_gameData = PC_ComplexFormatter.LoadObjectFromBinaryFile<PC_SaveData>(m_path);
                else
                    m_gameData = m_gameData = PC_ComplexFormatter.LoadObjectFromJSONFile<PC_SaveData>(m_path);

                PC_Casino.Instance.SetSaveData(m_gameData);

                Debug.Log("Loaded the game data from " + m_path);
            }
            else
            {
                // We create initial Data
                PC_Casino.Instance.InitData();
                SaveGame();
                PC_EventManager.TriggerEvent(PC_EventSetup.GAME_LOADED);
            }
        }

        private void OnApplicationQuit()
        {
            if (PC_GameSetup.Game_Settings.SAVE_ON_EXIT)
                SaveGame();
        }
    }

}
