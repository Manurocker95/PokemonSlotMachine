
using UnityEngine;
using System.Collections;
using System;
using FullSerializer;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Runtime.Serialization;
using System.Reflection;

namespace PokemonCasino.Serialization
{
    /// <summary>
    /// * This is a serializator script that can serialize any Unity Script. 
    /// * Call it for saving data for example. 
    /// * You can save the data in a binary form or even Encrypt it for more security.
    /// 
    /// * Usually, this will be used for saving JSONs to .bin or .data files. 
    /// * For saving a JSON or Object to Binary File, just call:
    /// 
    /// UGT_ComplexFormatter.SaveJSONStringToBinaryFile(JsonString, PathToSave);
    /// UGT_ComplexFormatter.SaveObjectToBinaryFile(JsonString, PathToSave);
    /// 
    /// * You can load data from binaryFile using 
    /// 
    /// UGT_ComplexFormatter.LoadJSONStringFromBinaryFile(FilePath);
    /// 
    /// * And the same goes for DES Encryption:
    /// 
    /// UGT_ComplexFormatter.SaveJSONStringToDESString(jsonString, path, keyToDecrypt);
    /// 
    /// An example of key to decrypt any file is in the class below: EncryptionSetup
    /// 
    /// just set your own key so anyone can decrypt your save data
    /// </summary>
    public static class PC_ComplexFormatter
    {


        /*************************************
         * Save Load  object<->binary file:
         * ***********************************/
        /// <summary>
        /// Saves the object to binary file.
        /// USE:
        /// MyComplexFormatter.SaveObjectToBinaryFile (new testSaveMeClass (path, Vector3.one * 3), "/Resources/test.txt", "");
        /// </summary>
        /// <param name="myObjectToSave">My object to save.</param>
        /// <param name="path">Path.</param>
        /// <param name="extension">Extension.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void SaveObjectToBinaryFile<T>(T myObjectToSave, string path, string extension)
        {
            //string json = SerializeToJSON (typeof(T), myObjectToSave);
            string json = SerializeToJSON(myObjectToSave);

            SaveJSONStringToBinaryFile(json, path);
        }

        /// <summary>
        /// Loads the object from binary file.
        /// USE:
        //testSaveMeClass test =  (testSaveMeClass)MyComplexFormatter.LoadObjectFromBinaryFile(typeof(testSaveMeClass),"/Resources/test.txt");
        /// </summary>
        /// <returns>The object from binary file.</returns>
        /// <param name="T">T.</param>
        /// <param name="path">Path.</param>
        public static object LoadObjectFromBinaryFile(Type T, string path)
        {
            string json = LoadJSONStringFromBinaryFile(path);
            //T o = (T) DeserializeFromJSON (typeof(T), json);
            //return o;
            return DeserializeFromJSON(T, json);

        }


        /// <summary>
        /// Loads the object from binary file.
        /// USE:
        /// testSaveMeClass test =  MyComplexFormatter.LoadObjectFromBinaryFile<testSaveMeClass>("/Resources/test.txt");
        /// </summary>
        /// <returns>The object from binary file.</returns>
        /// <param name="path">Path.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T LoadObjectFromBinaryFile<T>(string path)
        {
            string json = LoadJSONStringFromBinaryFile(path);
            //T o = (T) DeserializeFromJSON (typeof(T), json);
            //return o;
            return (T)DeserializeFromJSON(typeof(T), json);

        }

        /*************************************
         * Full Serializer  object<->json
         * ***********************************/
        private static readonly fsSerializer _serializer = new fsSerializer();

        /// <summary>
        /// Serialize the specified type and value.
        /// returns a json string
        /// USE: 		
        /// string json = MyComplexFormatter.SerializeToJSON(typeof(testSaveMeClass),new testSaveMeClass(path,Vector3.one*3));
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="value">Value.</param>
        private static string SerializeToJSON(Type type, object value)
        {
            // serialize the data
            fsData data;
            _serializer.TrySerialize(type, value, out data).AssertSuccessWithoutWarnings();

            // emit the data via JSON
            return fsJsonPrinter.CompressedJson(data);
        }

        /// <summary>
        /// Serialize the specified type and value.
        /// returns a json string
        /// USE: 		
        /// string json = MyComplexFormatter.SerializeToJSON(new testSaveMeClass(path,Vector3.one*3));
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="value">Value.</param>
        public static string SerializeToJSON<T>(T myObjectToSave)
        {
            // serialize the data
            fsData data;
            _serializer.TrySerialize(typeof(T), myObjectToSave, out data).AssertSuccessWithoutWarnings();

            // emit the data via JSON
            return fsJsonPrinter.CompressedJson(data);
        }

        private static object DeserializeFromJSON(Type type, string serializedState)
        {
            // step 1: parse the JSON data
            fsData data = fsJsonParser.Parse(serializedState);

            // step 2: deserialize the data
            object deserialized = null;
            _serializer.TryDeserialize(data, type, ref deserialized).AssertSuccessWithoutWarnings();

            return deserialized;
        }


        /// <summary>
        /// Deserializes object from JSONString.
        /// USE:
        /// MyTestObject test = MyComplexFormatter.DeserializeFromJSON<MyTestObject>(jsonString);
        /// </summary>
        /// <returns>The object.</returns>
        /// <param name="jsonString">Json string.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T DeserializeFromJSON<T>(string jsonString)
        {

            // step 1: parse the JSON data
            fsData data = fsJsonParser.Parse(jsonString);

            // step 2: deserialize the data
            object deserialized = null;
            _serializer.TryDeserialize(data, typeof(T), ref deserialized).AssertSuccessWithoutWarnings();

            return (T)deserialized;
        }




        /*************************************
         * Binary Formatter  JSON<->object<->binary file
         * ***********************************/

        /// <summary>
        /// Saves jsonString to file in binary format.
        /// path like: "/Resources/test.json"
        /// </summary>
        /// <param name="jsonString">Json string.</param>
        /// <param name="path">Path.</param>
        public static void SaveJSONStringToBinaryFile(string jsonString, string path)
        {
            //Step 1: create formater
            BinaryFormatter bf = new BinaryFormatter();

            //Step 2: create the file and open it.
            //this is where the data is going to be saved.
            FileStream file = File.Create(path);
            //		Debug.Log("saved To: "+ Application.dataPath + pathExtensionString);


            //Step 3: Create a data container and fill it.
            //what data are we going to save:
            //DO NOT serialize a Monodevelop. leads to wierd things...  

            //using a constructor method to fill data container.
            //PlayerStatsData data = new PlayerStatsData(GetComponent<PlayerStatsController>().statsData);

            /*You can use a constructor like before or use setters like this if youve written them.
            data.health = health;
            data.experience = experience;
            data.setHealth(health);
            */
            //Debug.Log("data to save = "+jsonString);

            //Step 4: serialize the dataContainer into the file and then close the file.
            bf.Serialize(file, new Dumbdata(jsonString));//bf.Serialize(file, new Dumbdata(jsonString.ToString()));
            file.Close();

            //Debug.Log("saved file closed succesfully");
            //one solution for web: serialize into a string and then pass the string to playerprefs. and then send over internet.


        }

        /// <summary>
        /// Saves the an object to binary file, saved in a json format.
        /// </summary>
        /// <param name="myObjectToSave">My object to save.</param>
        /// <param name="path">Path.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void SaveObjectToBinaryFile<T>(T myObjectToSave, string path, bool toBinary = true)
        {
            try
            {
                if (!toBinary)
                    File.WriteAllText(path, SerializeToJSON(myObjectToSave));
                else
                    SaveJSONStringToBinaryFile(SerializeToJSON(myObjectToSave), path);

            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed writing to path: " + path + "\n" + e.ToString());
            }

        }

        /// <summary>
        /// Gets json string from file saved in binary format.
        /// </summary>
        /// <returns>The from file.</returns>
        /// <param name="path">Path.</param>
        public static string LoadJSONStringFromBinaryFile(string path)
        {
            //Step 1: Check if the file exists.
            if (File.Exists(path))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(path, FileMode.Open);

                //its necesary to cast the generic object into the playerdatacontainer specifically.
                Dumbdata data = (Dumbdata)bf.Deserialize(file);

                file.Close();

                //Debug.Log("unload finished succesfully");
                return data.jsonString;

            }
            return "";
        }

        /// <summary>
        /// Saves to JSON file.
        /// opens,writes,closes   Overwrites existing file.
        /// </summary>
        /// <param name="jsonData">Json data.</param>
        /// <param name="path">Path.</param>
        public static void SaveJSONStringToJSONFile(string jsonString, string path)
        {
            File.WriteAllText(path, jsonString); // opens,writes,closes   Overwrites existing file.
        }

        /// <summary>
        /// Saves ANY class to a JSON file thanks to Generic.
        /// USE:JSON_Tools.SaveToJSONFile(player,"/Resources/playerdata.json");
        /// or: JSON_Tools.SaveToJSONFile<PlayerData>(player,"/Resources/playerdata.json");
        /// 
        /// NOTE: it saves under application.dataPath+path.
        /// </summary>
        /// <param name="myobject">a c# object to save.</param>
        /// <param name="path">Path.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void SaveObjectoToJSONFile<T>(T myobject, string path)
        {
            string jsonString = SerializeToJSON(myobject);
            SaveJSONStringToJSONFile(jsonString, path);
        }


        public static string ObjectToCreatedJSON<T>(T myobject, string path)
        {
            string jsonString = SerializeToJSON(myobject);
            SaveJSONStringToJSONFile(jsonString, path);

            return jsonString;
        }

        public static string ObjectToJSON<T>(T myobject)
        {
            return SerializeToJSON(myobject);
        }

        public static TextAsset ObjectToCreatedJSONAsset<T>(T myobject, string path)
        {
            string jsonString = SerializeToJSON(myobject);
            SaveJSONStringToJSONFile(jsonString, path);

            TextAsset asset = new TextAsset(jsonString);

            return asset;
        }

        public static TextAsset ObjectToJSONAsset<T>(T myobject)
        {
            string jsonString = SerializeToJSON(myobject);
            TextAsset asset = new TextAsset(jsonString);

            return asset;
        }

        /// <summary>
        /// Loads the Json from a file.
        /// opens, reads and closes file
        /// </summary>
        /// <returns>The JSON from file.</returns>
        /// <param name="path">Path.</param>
        public static T DeserializeObjectFromJSONFile<T>(string path)
        {

            string jsonString = File.ReadAllText(path); // opens, reads and closes file
            return DeserializeFromJSON<T>(jsonString);

        }

        /// Loads the Json from a file.
        /// opens, reads and closes file
        /// </summary>
        /// <returns>The JSON from file.</returns>
        /// <param name="path">Path.</param>
        public static T LoadObjectFromJSONFile<T>(string path)
        {

            string jsonString = File.ReadAllText(path); // opens, reads and closes file
            return DeserializeFromJSON<T>(jsonString);

        }
        #region DES ENCRYPTION

        public static void SaveObjectToEncryptedBinaryFile<T>(T myObjectToSave, string path, string key = EncryptionSetup.PRIVATE_KEY)
        {
            string encryptedString = SaveJSONStringToDESString(SerializeToJSON(myObjectToSave), path, key);
            SaveJSONStringToBinaryFile(encryptedString, path);
        }

        public static void SaveJSONStringToEncryptedBinaryFile(string jsonString, string path, string key = EncryptionSetup.PRIVATE_KEY)
        {
            string encryptedString = SaveJSONStringToDESString(jsonString, path, key);
            SaveJSONStringToBinaryFile(encryptedString, path);
        }

        public static string LoadJSONStringFromEncryptedBinaryFile(string path, string key = EncryptionSetup.PRIVATE_KEY)
        {
            string encryptedString = LoadJSONStringFromBinaryFile(path);
            return LoadJSONStringFromDESString(encryptedString, key);
        }

        public static T LoadObjectFromEncryptedBinaryFile<T>(string path, string key = EncryptionSetup.PRIVATE_KEY)
        {
            string encryptedString = LoadJSONStringFromBinaryFile(path);
            string jsonString = LoadJSONStringFromDESString(encryptedString, key);

            if (string.IsNullOrEmpty(jsonString))
                return default(T);

            return DeserializeFromJSON<T>(jsonString);
        }


        public static T LoadObjectFromDESFile<T>(string path, string key = EncryptionSetup.PRIVATE_KEY)
        {
            string jsonString = LoadJSONStringFromDESFile(path, key);

            if (string.IsNullOrEmpty(jsonString))
                return default(T);

            return DeserializeFromJSON<T>(jsonString);
        }

        public static void SaveObjectToDESFile<T>(T myObjectToSave, string path, string key = EncryptionSetup.PRIVATE_KEY)
        {
            string jsonString = SerializeToJSON(myObjectToSave);
            SaveJSONStringToDESFile(jsonString, path, key);
        }

        /* Made by Manuel Rodríguez Matesanz with DES Encryption. May fail */
        public static void SaveJSONStringToDESFile(string jsonString, string AESPath, string key = EncryptionSetup.PRIVATE_KEY)
        {
            try
            {
                Stream stream = File.Open(AESPath, FileMode.Create);
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Binder = new VersionDeserializationBinder();

                string strEncrypt = EncryptionSetup.PRIVATE_KEY;

                byte[] dv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

                byte[] byKey = Encoding.UTF8.GetBytes(strEncrypt.Substring(0, 8));


                //DES ENCRYPTION
                DESCryptoServiceProvider des = null;

                using (des = new DESCryptoServiceProvider())
                {
                    using (Stream cryptoStream = new CryptoStream(stream, des.CreateEncryptor(byKey, dv), CryptoStreamMode.Write))
                    {
                        bformatter.Serialize(cryptoStream, jsonString);
                    }
                }

                stream.Close();
            }
            catch (UnauthorizedAccessException)
            {
                Debug.LogError("UnauthorizedAccessException");
            }

            //File.WriteAllText(AESPath, jsonString);
        }

        public static string SaveJSONStringToDESString(string jsonString, string AESPath, string key = EncryptionSetup.PRIVATE_KEY)
        {
            try
            {
                Stream stream = File.Open(AESPath, FileMode.Create);
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Binder = new VersionDeserializationBinder();

                string strEncrypt = EncryptionSetup.PRIVATE_KEY;

                byte[] dv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

                byte[] byKey = Encoding.UTF8.GetBytes(strEncrypt.Substring(0, 8));


                //DES ENCRYPTION
                DESCryptoServiceProvider des = null;

                using (des = new DESCryptoServiceProvider())
                {
                    using (Stream cryptoStream = new CryptoStream(stream, des.CreateEncryptor(byKey, dv), CryptoStreamMode.Write))
                    {
                        bformatter.Serialize(cryptoStream, jsonString);
                    }
                }

                stream.Close();

                return File.ReadAllText(AESPath);

            }
            catch (UnauthorizedAccessException)
            {
                Debug.LogError("UnauthorizedAccessException");
            }

            return "";
        }


        public static string LoadJSONStringFromDESString(string contents, string key = EncryptionSetup.PRIVATE_KEY)
        {
            try
            {
                // convert string to stream
                byte[] byteArray = Encoding.ASCII.GetBytes(contents);
                //byte[] byteArray = Encoding.UTF8.GetBytes(contents);
                Stream stream = new MemoryStream(byteArray);

                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Binder = new VersionDeserializationBinder();

                string strEncrypt = EncryptionSetup.PRIVATE_KEY;

                byte[] dv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

                byte[] byKey = Encoding.UTF8.GetBytes(strEncrypt.Substring(0, 8));

                string jsonString;

                DESCryptoServiceProvider des = null;

                using (des = new DESCryptoServiceProvider())
                {
                    using (Stream cryptoStream = new CryptoStream(stream, des.CreateDecryptor(byKey, dv), CryptoStreamMode.Read))
                    {
                        jsonString = bformatter.Deserialize(cryptoStream) as string;
                    }
                }

                stream.Close();

                if (string.IsNullOrEmpty(jsonString))
                    Debug.LogError("COULDNT LOAD DATA");

                return jsonString;
            }

            catch (UnauthorizedAccessException)
            {
                Debug.LogError("UnauthorizedAccessException");
            }
            catch (FileNotFoundException)
            {
                Debug.Log("No save data found");
            }

            return "";
        }


        public static void SaveBinaryFromDESFile(string AESPath, string BinaryPath, string key = EncryptionSetup.PRIVATE_KEY)
        {
            string jsonString = LoadJSONStringFromDESFile(AESPath, key);

            if (string.IsNullOrEmpty(jsonString))
                return;

            SaveJSONStringToBinaryFile(jsonString, BinaryPath);
        }

        public static string LoadJSONStringFromDESFile(string path, string key = EncryptionSetup.PRIVATE_KEY)
        {
            try
            {
                Stream stream = File.Open(path, FileMode.Open);
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Binder = new VersionDeserializationBinder();

                string strEncrypt = EncryptionSetup.PRIVATE_KEY;

                byte[] dv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

                byte[] byKey = Encoding.UTF8.GetBytes(strEncrypt.Substring(0, 8));

                string jsonString;

                DESCryptoServiceProvider des = null;

                using (des = new DESCryptoServiceProvider())
                {
                    using (Stream cryptoStream = new CryptoStream(stream, des.CreateDecryptor(byKey, dv), CryptoStreamMode.Read))
                    {
                        jsonString = bformatter.Deserialize(cryptoStream) as string;
                    }
                }

                stream.Close();

                if (string.IsNullOrEmpty(jsonString))
                    Debug.LogError("COULDNT LOAD DATA");

                return jsonString;
            }

            catch (UnauthorizedAccessException)
            {
                Debug.LogError("UnauthorizedAccessException");
            }
            catch (FileNotFoundException)
            {
                Debug.Log("No save data found");
            }

            return "";
        }

        #endregion


    }

    #region Dummy data for JSON
    [System.Serializable]
    public class Dumbdata
    {
        public string jsonString;
        public Dumbdata(string json)
        {
            jsonString = json;
        }
    }
    #endregion

    #region Encryption Setup

    /// <summary>
    /// Binder de serialización
    /// </summary>
    public class VersionDeserializationBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            if (!string.IsNullOrEmpty(assemblyName) && !string.IsNullOrEmpty(typeName))
            {
                Type typeToDeserialize = null;

                assemblyName = Assembly.GetExecutingAssembly().FullName;

                // The following line of code returns the type. 
                typeToDeserialize = Type.GetType(String.Format("{0}, {1}", typeName, assemblyName));

                return typeToDeserialize;
            }

            return null;
        }
    }


    public static class EncryptionSetup
    {
        public const string PRIVATE_KEY = "*#4$%^.++q~!cfr0(_!#$@$!&#&#*&@(7cy9rn8r265&$@&*E^184t44tq2cr9o2w34f6";
    }
    #endregion
}
