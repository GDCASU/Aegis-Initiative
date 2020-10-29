using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Generic script that is used to save
/// and load content within binary files
/// </summary>
public static class SaveManager
{
    public static bool SaveContent(System.Object saveContent, string filePath, bool overwrite = true)
    {
        string fullPath = Application.persistentDataPath + filePath;

        FileMode mode = FileMode.Create;
        if (File.Exists(fullPath) && !overwrite) mode = FileMode.CreateNew;

        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(fullPath, mode);
            formatter.Serialize(stream, saveContent);
            stream.Close();
        }
        catch (Exception e)
        {
            Debug.Log("Could not save content:\n" + e);
            return false;
        }

        return true;
    }

    public static System.Object LoadContent(string filePath)
    {
        string fullPath = Application.persistentDataPath + filePath;

        try
        {
            if (File.Exists(fullPath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(fullPath, FileMode.Open);

                System.Object loadedObject = formatter.Deserialize(stream);
                stream.Close();

                return loadedObject;
            }
        }
        catch(Exception e)
        {
            Debug.Log("File could not be loaded:\n" + fullPath + "\n" + e);
        }
        
        return null;
    }
}
