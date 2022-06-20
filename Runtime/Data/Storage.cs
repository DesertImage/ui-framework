using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace DesertImage.Data
{
    public enum SaveFormat
    {
        JSON,
        Binary
    }

    public static class Storage
    {
        private static string DefaultSavePath;

        private static readonly BinaryFormatter _binaryFormatter = new BinaryFormatter();

        private static bool _isInited;

        static Storage()
        {
            DefaultSavePath = Application.persistentDataPath + $"/data.gd";
        }

        public static void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        #region SAVE

        public static void Save<T>(T data, string fileName = "", SaveFormat format = SaveFormat.JSON) where T : class
        {
            var filePath = string.IsNullOrEmpty(fileName)
                ? DefaultSavePath
                : Application.persistentDataPath + $"/{fileName}.gd";

            switch (format)
            {
                case SaveFormat.JSON:
                    var jsonData = JsonUtility.ToJson(data, true);
                    File.WriteAllText(filePath, jsonData);
                    break;

                case SaveFormat.Binary:
                    var fileStream = File.Open(filePath, FileMode.OpenOrCreate);

                    _binaryFormatter.Serialize(fileStream, data);

                    fileStream.Close();

                    break;
            }
        }

        public static void SaveInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        public static void SaveString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public static void SaveFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        public static void SaveBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }

        #endregion

        #region LOAD

        public static T Load<T>(SaveFormat saveFormat = SaveFormat.JSON) where T : class, new()
        {
            return Load<T>(string.Empty, saveFormat);
        }

        public static T Load<T>(string fileName, SaveFormat saveFormat = SaveFormat.JSON) where T : class, new()
        {
            var filePath = string.IsNullOrEmpty(fileName)
                ? DefaultSavePath
                : Application.persistentDataPath + $"/{fileName}.gd";

            if (!File.Exists(filePath))
            {
                return new T();
            }

            var data = default(T);

            switch (saveFormat)
            {
                case SaveFormat.JSON:
                    var json = File.ReadAllText(filePath);
                    data = JsonUtility.FromJson<T>(json);
                    break;

                case SaveFormat.Binary:
                    var fileStream = File.Open(filePath, FileMode.Open);

                    data = (T) _binaryFormatter.Deserialize(fileStream);

                    fileStream.Close();
                    break;
            }

            return data != null && data.Equals(default(T)) ? new T() : data;
        }

        public static int LoadInt(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public static string LoadString(string key, string defaultValue = "")
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        public static float LoadFloat(string key, float defaultValue = 0f)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        public static bool LoadBool(string key, bool defaultValue = false)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
        }

        #endregion

        /// <summary>
        /// Delete ALL data
        /// </summary>
        public static void Clear()
        {
            if (!File.Exists(DefaultSavePath))
            {
                Debug.Log($"[Storage] there is no save file in {DefaultSavePath}");
                return;
            }

            File.Delete(DefaultSavePath);

#if DEBUG
            UnityEngine.Debug.Log("[Storage] file deleted " + DefaultSavePath);
#endif
        }

#if UNITY_EDITOR

        [MenuItem("Storage/Clear")]
        private static void ClearEditor()
        {
            Clear();
        }
#endif
    }
}