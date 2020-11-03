using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Generic script that is used to save
/// and load objects within binary files
/// </summary>
public static class SaveManager
{
    /// <summary>
    /// Method used to save an object into a binary file
    /// 
    /// Note: filePath SHOULD NOT INCLUDE PERSTENT DATA PATH as I apply it here
    /// </summary>
    /// <param name="saveContent">The object to store</param>
    /// <param name="filePath">Filepath to save the file</param>
    /// <param name="overwrite">Whether to overwrite the file or not</param>
    /// <returns></returns>
    public static bool SaveContent(System.Object saveContent, string filePath, bool overwrite = true)
    {
        string fullPath = Application.persistentDataPath + filePath;

        FileMode mode = FileMode.Create;
        if (File.Exists(fullPath) && !overwrite) mode = FileMode.CreateNew;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(fullPath, mode);
        try
        {
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

    /// <summary>
    /// Method to load an object from a binary file
    /// 
    /// Note: filePath SHOULD NOT INCLUDE PERSTENT DATA PATH as I apply it here
    /// </summary>
    /// <param name="filePath">Path of the file to open</param>
    /// <returns>The object retrieved from the file</returns>
    public static System.Object LoadContent(string filePath)
    {
        string fullPath = Application.persistentDataPath + filePath;

        if (File.Exists(fullPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(fullPath, FileMode.Open);
            try
            {
                System.Object loadedObject = formatter.Deserialize(stream);
                stream.Close();

                return loadedObject;
            }
            catch (Exception e)
            {
                Debug.Log("File could not be loaded:\n" + fullPath + "\n" + e);
            }
        }

        return null;
    }
}
