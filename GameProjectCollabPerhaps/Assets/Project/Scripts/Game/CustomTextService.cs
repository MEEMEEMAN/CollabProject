using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

/// <summary>
/// A custom made text read/writer class. for an easier life. used mainly for Json serialization.
/// </summary>
public static class CustomTextService
{

    /// <summary>
    /// Serialize a class and output it's json value. make sure your class is marked with [System.Serializable]
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Object"></param>
    /// <returns></returns>
    public static string SerializeJson<T>(T Object)
    {
        string jsonCode = JsonUtility.ToJson(Object);
        return jsonCode;
    }


    /// <summary>
    /// DeSerialize jsonCode and cast it to your selected class.
    /// you should cast your output into the same class with whom you serialized it in the first place.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="jsonCode"></param>
    /// <returns></returns>
    public static T DeSerializeJson<T>(string jsonCode)
    {
        return JsonUtility.FromJson<T>(jsonCode);
    }

    /// <summary>
    /// Should be used before build.
    /// </summary>
    /// <param name="path"></param>
    public static void WriteJsonToDirectory(string data,string path, string filename)
    {
        using (FileStream fs = new FileStream(path + "/"+filename+".json", FileMode.Truncate))
        {
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.Write(data);
            }
            fs.Close();
        }
    }

    /// <summary>
    /// Returns a json text file from the Resources folder, can be used after build.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string ReadJsonFromResources(string path)
    {
        string json = "failed";

        try
        {
            json = (Resources.Load(path) as TextAsset).text;
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }

        return json;
    }
}
