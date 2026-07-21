using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace QuestMaker.Runtime.SaveSystem
{
    internal static class SaveFileHandler
    {
        
        private static string FILE_PATH => Path.Combine(Application.persistentDataPath, RuntimeSettings.FILE_NAME);

        public static bool Exists => File.Exists(FILE_PATH);

        public static bool TryWrite(List<SaveDataBase> saveData)
        {
            if (saveData == null || saveData.Count == 0) return false;


            SaveFile saveFile = CreateSaveFile(saveData);

            try
            {
                string json = JsonUtility.ToJson(saveFile, true);
                // we write to a temp path in case of error we dont corrupt the previous one
                string tempPath = FILE_PATH + ".tmp";
                File.WriteAllText(tempPath, json);

                //We replace the previous with the temp or move the temp and mark it as the main.
                if (File.Exists(FILE_PATH))
                    File.Replace(tempPath, FILE_PATH, null);
                else
                    File.Move(tempPath, FILE_PATH);

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveFileHandler] Failed to write save file: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// Casts the save data back to the concrete classes and creates a save file.
        /// </summary>
        /// <param name="saveData"></param>
        /// <returns></returns>
        private static SaveFile CreateSaveFile(List<SaveDataBase> saveData)
        {
            SaveFile saveFile = new();
            foreach (SaveDataBase data in saveData)
            {
                switch (data)
                {
                    case PlayerSaveDataBase player: saveFile.player = player; break;
                    case InventorySaveDataBase inventory: saveFile.inventory = inventory; break;
                    case QuestsSaveDataBase quests: saveFile.quests = quests; break;
                    default:
                        Debug.LogWarning($"[SaveFileHandler] SaveFile unknown save data {data?.GetType().Name}");
                        break;
                }
            }
            return saveFile;
        }

        public static bool TryRead(out List<SaveDataBase> saveData)
        {
            saveData = null;
            if (!Exists) return false;

            try
            {
                SaveFile file = JsonUtility.FromJson<SaveFile>(File.ReadAllText(FILE_PATH));
                if (file == null) return false;

                saveData = new List<SaveDataBase>();
                ValidateSaveData(saveData, file.player);
                ValidateSaveData(saveData, file.inventory);
                ValidateSaveData(saveData, file.quests);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveFileHandler] Failed to read save file: {e.Message}");
                return false;
            }
        }

        public static void Delete()
        {
            if (Exists) File.Delete(FILE_PATH);
        }

        private static void ValidateSaveData(List<SaveDataBase> entries, SaveDataBase data)
        {
            if (data != null && !string.IsNullOrEmpty(data.saveID))
                entries.Add(data);
        }
    }
}