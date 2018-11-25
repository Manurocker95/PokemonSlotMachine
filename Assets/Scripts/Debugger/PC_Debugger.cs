
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.IO;
using System.Diagnostics;
using PokemonCasino.Serialization;
using UnityEditor.SceneManagement;


namespace PokemonCasino.Debugger
{
    public enum DEBUG_TABS
    {
        SETTINGS,
        BUILD,
        ARCHIVES,
        MISCELLANEOUS,
    }

    /// <summary>
    /// JUst for running any scene without needing to initialize the initializing
    /// </summary>
    public class pb_Debugger : EditorWindow
    {
        #region Variables
        [SerializeField] private UnityEngine.Object m_objectToEncrypt;
        [SerializeField] private UnityEngine.Object m_objectToDecrypt;
        [SerializeField] private TextAsset m_jsonToEncrypt;
        [SerializeField] private UnityEngine.Object m_jsonToDecrypt;

        private DEBUG_TABS m_currentTab;
        private int m_toolbarInt;

        private string[] m_toolbarStrings = new string[] { "SETTINGS", "BUILD", "ARCHIVES", "MISCELLANEOUS" };
        [SerializeField] private string m_projectName;
        [SerializeField] private string m_companyName;
        [SerializeField] private string m_bundleVersion;

        #endregion

        #region Main
        /// <summary>
        /// Creates the custom window :D
        /// </summary>
        [MenuItem("Casino/Debugger")]
        public static void ShowWindow()
        {
            GetWindow<pb_Debugger>("Debugger");
        }

        private void Awake()
        {
            m_projectName = PlayerSettings.productName;
            m_companyName = PlayerSettings.companyName;
            m_bundleVersion = PlayerSettings.bundleVersion;

            m_toolbarInt = 0;
            m_currentTab = DEBUG_TABS.SETTINGS;
        }


        /// <summary>
        /// On GUI: Paint gui stuff
        /// </summary>
        private void OnGUI()
        {

            GUILayout.Label("Template Debugger Window", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("We have here everything needed for helping us to go faster.", MessageType.Info);
            EditorGUILayout.Space();

            m_toolbarInt = GUILayout.Toolbar(m_toolbarInt, m_toolbarStrings);
            m_currentTab = (DEBUG_TABS)m_toolbarInt;


            switch (m_currentTab)
            {
                case DEBUG_TABS.SETTINGS:
                    EditorGUILayout.HelpBox("This section has everything related to project settings.", MessageType.Info);
                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    GUILayout.Label("--- PROJECT PROPERTIES --- ", EditorStyles.centeredGreyMiniLabel);
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginVertical();
                    m_projectName = EditorGUILayout.TextField("Project Name:", m_projectName);
                    m_companyName = EditorGUILayout.TextField("Company Name:", m_companyName);
                    m_bundleVersion = EditorGUILayout.TextField("Bundle Version:", m_bundleVersion);
                    EditorGUILayout.Space();
                    if (GUILayout.Button("Apply properties"))
                    {
                        UnityEngine.Debug.Log("Changing Name and company");
                        PlayerSettings.productName = m_projectName;
                        PlayerSettings.companyName = m_companyName;
                        PlayerSettings.bundleVersion = m_bundleVersion;
                    }
                    GUILayout.EndVertical();
                    this.Repaint();


                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    GUILayout.Label("--- OPEN SETTINGS TABS --- ", EditorStyles.centeredGreyMiniLabel);
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    EditorGUILayout.BeginHorizontal();

                    if (GUILayout.Button("Open Player Settings"))
                    {
                        EditorApplication.ExecuteMenuItem("Edit/Project Settings/Player");
                    }

                    if (GUILayout.Button("Open Quality Settings"))
                    {
                        EditorApplication.ExecuteMenuItem("Edit/Project Settings/Quality");
                    }

                    if (GUILayout.Button("Open Graphics Settings"))
                    {
                        EditorApplication.ExecuteMenuItem("Edit/Project Settings/Graphics");
                    }
                    GUILayout.EndHorizontal();

                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();

                    if (GUILayout.Button("Open Physics Settings"))
                    {
                        EditorApplication.ExecuteMenuItem("Edit/Project Settings/Physics");
                    }

                    if (GUILayout.Button("Open Input Settings"))
                    {
                        EditorApplication.ExecuteMenuItem("Edit/Project Settings/Input");
                    }

                    if (GUILayout.Button("Open Tags and layers"))
                    {
                        EditorApplication.ExecuteMenuItem("Edit/Project Settings/Tags and Layers");
                    }
                    GUILayout.EndHorizontal();
                    break;

                case DEBUG_TABS.MISCELLANEOUS:

                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    GUILayout.Label("--- PLAYER PREFS --- ", EditorStyles.centeredGreyMiniLabel);
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    EditorGUILayout.Space();
                    if (GUILayout.Button("Delete all player prefs"))
                    {
                        PlayerPrefs.DeleteAll();
                        PlayerPrefs.Save();
                    }
                    EditorGUILayout.Space();

                    break;

                case DEBUG_TABS.BUILD:
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    GUILayout.Label("--- BUILD --- ", EditorStyles.centeredGreyMiniLabel);
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    EditorGUILayout.Space();
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Build .exe"))
                    {
                        UnityEngine.Debug.Log("Building .exe for Unity Game Template");
                        BuildGame();
                    }

                    if (GUILayout.Button("Build And run "))
                    {
                        UnityEngine.Debug.Log("Building .exe for Unity Game Template and running");
                        BuildGame(true);
                    }
                    GUILayout.EndHorizontal();

                    // installer
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    GUILayout.Label("--- INSTALLER --- ", EditorStyles.centeredGreyMiniLabel);
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    EditorGUILayout.Space();
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Create installer"))
                    {
                        UnityEngine.Debug.Log("Creating installer for  Unity Game Template");
                        CreateInstaller();
                    }

                    if (GUILayout.Button("Build and Installer "))
                    {
                        UnityEngine.Debug.Log("Building .exe for  Unity Game Template and creating installer");
                        BuildAndCreateInstaller();
                    }
                    GUILayout.EndHorizontal();
                    break;


                case DEBUG_TABS.ARCHIVES:
                    EditorGUILayout.HelpBox("This section has everything related to default archives: Themes, users... and it's encryption.", MessageType.Info);
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    GUILayout.Label("--- ENCRYPTION --- ", EditorStyles.centeredGreyMiniLabel);
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    EditorGUILayout.HelpBox("Encrypt any JSON easy peasy.", MessageType.Info);

                    m_jsonToEncrypt = (TextAsset)EditorGUILayout.ObjectField("JSON To Encrypt:", m_jsonToEncrypt, typeof(TextAsset), true);
                    GUILayout.BeginHorizontal();

                    if (GUILayout.Button("JSON To Binary"))
                    {
                        JSONToBinary();
                    }

                    if (GUILayout.Button("JSON To DES Encryption"))
                    {
                        JSONToDES();
                    }
                    GUILayout.EndHorizontal();

                    EditorGUILayout.Space();
                    m_jsonToDecrypt = (UnityEngine.Object)EditorGUILayout.ObjectField("File To Decrypt:", m_jsonToDecrypt, typeof(UnityEngine.Object), true);
                    GUILayout.BeginHorizontal();

                    if (GUILayout.Button("Binary To JSON"))
                    {
                        BinaryToJSON();
                    }

                    if (GUILayout.Button("DES Encryption to JSON"))
                    {
                        DESToJSON();
                    }
                    GUILayout.EndHorizontal();
                    EditorGUILayout.Space();

                    EditorGUILayout.HelpBox("Encrypt any Object easy peasy.", MessageType.Info);
                    m_objectToEncrypt = EditorGUILayout.ObjectField("Object To Encrypt:", m_objectToEncrypt, typeof(UnityEngine.Object), true);
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Object To JSON"))
                    {
                        ObjectToJSON();
                    }

                    if (GUILayout.Button("Object To Binary"))
                    {
                        ObjectToBinary();
                    }

                    if (GUILayout.Button("Object To DES Encryption"))
                    {
                        ObjectToDES();
                    }
                    GUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    m_objectToDecrypt = (UnityEngine.Object)EditorGUILayout.ObjectField("Object To Decrypt:", m_objectToDecrypt, typeof(UnityEngine.Object), true);
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("JSON to Object"))
                    {
                        JSONToObject();
                    }

                    if (GUILayout.Button("Binary to Object"))
                    {
                        BinaryToObject();
                    }

                    if (GUILayout.Button("DES Encryption to Object"))
                    {
                        DESToObject();
                    }
                    GUILayout.EndHorizontal();
                    EditorGUILayout.Space();

                    break;

            }

        }
        #endregion

        #region BUILD AND INSTALLER

        private string BuildGame(bool runIt = false, string _path = "")
        {
            UnityEngine.Debug.ClearDeveloperConsole();

            // Get filename.
            string path = string.IsNullOrEmpty(_path) ? EditorUtility.SaveFolderPanel("Elige la carpeta para buildear", "", "Build") : _path;

            int numLevels = EditorSceneManager.sceneCountInBuildSettings;
            string[] levels = new string[numLevels];

            for (int i = 0; i < numLevels; i++)
            {
                levels[i] = EditorBuildSettings.scenes[i].path;
            }

            string name = PlayerSettings.productName;

            // Build player.
            BuildPipeline.BuildPlayer(levels, path + "/" + name + ".exe", BuildTarget.StandaloneWindows, BuildOptions.None);

            if (runIt)
            {
                // Run the game (Process class from System.Diagnostics).
                Process proc = new Process();
                proc.StartInfo.FileName = Path.GetFullPath(path + name + ".exe");
                proc.Start();
            }

            return path;
        }
        // Remember to change the script install.bat in "Executable" to build from here with INNO Setup
        private void CreateInstaller(string buildPath = "")
        {
            if (string.IsNullOrEmpty(buildPath))
                buildPath = EditorUtility.SaveFolderPanel("Choose build folder", "", "Build");

            string name = PlayerSettings.productName;

            if (!Directory.Exists(Path.GetFullPath(buildPath + "/" + name + "_Data")))
            {
                UnityEngine.Debug.LogWarning("The game build doesn't exist. The game will be built before creating the installer.");
                BuildGame();
            }

            if (File.Exists(Path.GetFullPath(buildPath + "/" + name + ".exe")))
            {
                UnityEngine.Debug.Log("Moving " + name + ".exe");
                FileUtil.ReplaceFile(buildPath + "/" + name + ".exe", System.IO.Directory.GetParent(buildPath) + "/" + name + ".exe");
            }

            string batFile = "Executable/install.bat";

            if (File.Exists(batFile))
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.FileName = Path.GetFullPath(batFile);
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
            }
            else
            {
                UnityEngine.Debug.LogError("Bat file couldn't be loaded");
            }

            string installerDir = "Installer/Output";
            if (Directory.Exists(installerDir))
                System.Diagnostics.Process.Start(Path.GetFullPath(installerDir));
        }

        private void BuildAndCreateInstaller()
        {
            // Get filename.
            string path = BuildGame();
            CreateInstaller(path);
        }

        #endregion

        #region MISCELLANEOUS

        #endregion

        #region ARCHIVES

        private void ObjectToJSON()
        {

        }
        private void ObjectToBinary()
        {

        }
        private void ObjectToDES()
        {

        }

        private void JSONToObject()
        {

        }
        private void BinaryToObject()
        {

        }
        private void DESToObject()
        {

        }

        private void JSONToBinary()
        {
            string path = EditorUtility.OpenFolderPanel("Select a folder where the binary file will be created", Application.dataPath, m_jsonToEncrypt.name + ".bin");
            PC_ComplexFormatter.SaveJSONStringToDESFile(m_jsonToEncrypt.text, path, EncryptionSetup.PRIVATE_KEY);
        }
        private void BinaryToJSON()
        {

        }

        private void DESToJSON()
        {
            string EncryptedFilePath = EditorUtility.OpenFilePanel("Select the file that will be decrypted", Application.dataPath, m_jsonToDecrypt.name + ".JSON");
            string jsonString = PC_ComplexFormatter.LoadJSONStringFromDESFile(EncryptedFilePath, EncryptionSetup.PRIVATE_KEY);
            string WhereToSave = EditorUtility.OpenFolderPanel("Select the folder where the JSON file will be created", Application.dataPath, m_jsonToDecrypt.name + ".JSON");
            PC_ComplexFormatter.SaveJSONStringToJSONFile(jsonString, WhereToSave);
        }

        private void JSONToDES()
        {
            string path = EditorUtility.OpenFolderPanel("Select the folder where the encrypted file will be created", Application.dataPath, m_jsonToEncrypt.name + ".data");
            PC_ComplexFormatter.SaveJSONStringToDESFile(m_jsonToEncrypt.text, path, EncryptionSetup.PRIVATE_KEY);
        }
        #endregion
    }

}
#endif