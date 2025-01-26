using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonReader : SingletonMonoBehaviour<JsonReader>
{
    public T GetData<T>(string jsonData){
        if(jsonData.Length > 0){
            T data = JsonUtility.FromJson<T>(jsonData);
            return data;
        }
        Debug.Log("JsonReader: File length equals 0.");
        return default(T);
    }

    // public T GetDataFromJson<T>(string _path)
    // {
    //     _path = System.Environment.CurrentDirectory+"/"+_path;
    //     Debug.Log(_path);

    //     if (File.Exists(_path))
    //     {
    //         using (StreamReader reader = /*File.OpenText(_path)*/ new StreamReader(_path))
    //         {
    //             string readdata = reader.ReadToEnd();

    //             if (readdata.Length > 0)
    //             {
    //                 // Debug.Log(readdata);
    //                 T data = JsonUtility.FromJson<T>(readdata);

    //                 return data;
    //             }
    //         }
    //     }
    //     Debug.Log("Not read");
    //     return default(T);
    // }
}

