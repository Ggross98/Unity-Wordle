using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeGroup : MonoBehaviour
{
    [SerializeField] private Transform cubeParent;

    [SerializeField] private GameObject cubePrefab;

    private List<Cube> cubeList;

    private int index;

    void Awake()
    {
        if(cubeParent == null || cubePrefab == null){
            Debug.LogError("Cube group object lost!");
        }
        else{
            cubeList = new List<Cube>();
        } 

        // LogArray(CompareWord("start", "apple"));
        
    }

    void Start()
    {
        // CreateEmpty(5);
    }

    private void Clear(){
        if(cubeList.Count > 0){
            for(int i = 0; i< cubeList.Count; i++){
                var cube = cubeList[0];
                cubeList.RemoveAt(0);
                Destroy(cube.gameObject);
            }
        }
    }

    public void CreateEmpty(int count = 5){
        Clear();

        var width = 850;
        var rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(width, 100);

        var layout = GetComponent<HorizontalLayoutGroup>();
        layout.childControlWidth = count>7;
        // layout.spacing = count>8 ? 10 : 5;
        // layout.childForceExpandWidth = false;

        for(int i = 0; i<count; i++){
            var obj = Instantiate<GameObject>(cubePrefab, cubeParent);
            var cube = obj.GetComponent<Cube>();
            cube.SetCharacter('0');
            cube.Set(Cube.EMPTY);
            cubeList.Add(cube);
        }

        index = -1;
    }

    public void AddCharacter(char ch){
        index++;
        if(index < cubeList.Count){
            cubeList[index].SetCharacter(ch);
        }else{
            index --;
        }
    }

    public void DeleteCharacter(){
        // Debug.Log("Delete");
        if(index < 0) return;
        cubeList[index].SetCharacter('0');
        if(index >= 0) index--;

    }

    public void CreateWord(string word){
        int count = word.Length;
        CreateEmpty(count);

        for(int i = 0; i<count; i++){
            cubeList[i].SetCharacter(word.ToUpper().ToCharArray()[i]);
        }
    }

    /// <summary>
    /// 当单词完全匹配的时候返回true，否则返回false
    /// </summary>
    /// <param name="word"></param>
    /// <returns></returns>
    public bool CheckWord(string word){
        var str = GetWord();
        var check = CompareWord(str, word);
        LogArray(check);
        for(int i = 0; i<cubeList.Count; i++){
            var cube = cubeList[i];

            if(check[i] == -1){
                cube.Set(Cube.WRONG);
            }else if(check[i] == i){
                cube.Set(Cube.RIGHT);
            }else{
                cube.Set(Cube.OFFSET);
            }
        }

        for(int i = 0; i<cubeList.Count;i++){
            if(check[i] != i) return false;
        }
        return true;
    }

    public string GetWord(){
        string str = "";
        for(int i = 0; i<cubeList.Count; i++){
            if(cubeList[i].GetCharacter() != '0'){
                str += cubeList[i].GetCharacter();
            }
        }
        return str;
    }

    public int GetCount(){
        int r = 0;
        for(int i = 0; i<cubeList.Count; i++){
            if(cubeList[i].GetCharacter()!='0') r++;
        }
        return r;
    }

    public static int[] CompareWord(string current, string target){
        var count = current.Length;
        if(target.Length != count) {
            return null;
        }

        var cDest = new int[count];
        var tDest = new int[count];

        var c = current.ToUpper().ToCharArray();
        var t = target.ToUpper().ToCharArray();

        // 找出完全正确位置
        for(int i = 0; i<count; i++){
            if(c[i] == t[i]){
                cDest[i] = i;
                tDest[i] = i;
            }else{
                cDest[i] = -1;
                tDest[i] = -1;
            }
        }

        // 找出剩余位置的对应关系
        for(int i = 0; i<count; i++){
            // 仅当该位置没有完全匹配时进行检测
            if(cDest[i] != i){
                for(int j = 0; j<count; j++){
                    if(j != i && c[i] == t[j] && tDest[j] == -1){
                        cDest[i] = j;
                        tDest[j] = i;
                        break;
                    }
                }
            }
        }

        return cDest;

    }

    public static void LogArray(int[] array){
        string str = "";
        for(int i = 0; i<array.Length; i++){
            str += array[i]+" ";
        }
        // Debug.Log(str);
    }
}
