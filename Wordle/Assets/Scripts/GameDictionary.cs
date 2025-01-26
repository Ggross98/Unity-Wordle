using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class GameDictionary : SingletonMonoBehaviour<GameDictionary>
{
    private Vocabulary data;

    public string GetRandomWord(int characterNum){

        var wl = data.wordLists[characterNum-1];
        var count = wl.count;
        var list = wl.list;

        return list[Random.Range(0, count)];
    }

    void Awake()
    {
        LoadVocabulary("Vocabulary/CODA20000.json");
    }

    /// <summary>
    /// 从StreamingAssets读取单词表文件
    /// </summary>
    /// <param name="filePath"></param>
    private void LoadVocabulary(string fileName){

        var filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            data = JsonReader.Instance.GetData<Vocabulary>(json);
        }
        else
        {
            Debug.LogError("Vocabulary file not found: " + filePath);
        }
    }

    public bool IsValidWord(string word){
        if(data == null) return false;
        var num = word.Length;
        if(num < 1 || num > data.wordLists.Count ) return false;
        else{
            var list = data.wordLists[num-1].list;
            Debug.Log("Test valid "+list.Count);
            foreach(var str in list){
                if(str.ToUpper().Equals(word)) return true;
            }
            return false;
        }
    }
}

public class Vocabulary{
    public List<WordList> wordLists = new List<WordList>();

}

[System.Serializable]
public class WordList{
    public int num = 0;
    public int count = 0;
    public List<string> list = null;
}
