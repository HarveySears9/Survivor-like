using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveFile
{
    private static string fileName = "SaveFile.txt";

    public static void SaveData<T>(T data)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            Debug.LogError("Filename not set for saving data");
            return;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, fileName);

        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(stream, data);
        }
    }

    public static T LoadData<T>()
    {
        if (string.IsNullOrEmpty(fileName))
        {
            Debug.LogError("FileName not set for loading data");
            return default(T);
        }

        string path = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    T data = (T)formatter.Deserialize(stream);
                    return data;
                }
            }
            else
            {
                Debug.LogWarning("Save File not Found");
                return default(T);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error loading data: " + e.Message);
            return default(T);
        }
    }


    [System.Serializable]
    public class Data
    {
        public int skinEquipped;
        public int coins;
    }
}
