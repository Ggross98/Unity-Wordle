using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteGroup : MonoBehaviour
{
    #region UI objects
    [SerializeField] private Transform cubeParent;

    #endregion

    [SerializeField] private GameObject cubePrefab;

    [SerializeField] private int cubeSize = 50;
    private GridLayoutGroup layout;

    private Dictionary<char, Cube> characterDict = new Dictionary<char, Cube>();
    private List<Cube> cubeList = new List<Cube>();

    public void CreateEmpty(List<char> extra = null){

        Clear();

        // 获得所有字符
        var list = new List<char>();
        for(char c = 'A'; c<= 'Z'; c++){
            list.Add(c);
        }
        if(extra!=null) list.AddRange(extra);

        // 建立对象并关联字典
        layout = GetComponentInChildren<GridLayoutGroup>();
        layout.cellSize = new Vector2(cubeSize, cubeSize);
        foreach(var ch in list){
            var cube = CreateCube();
            cube.SetCharacter(ch);
            cube.Set(Cube.EMPTY);

            cubeList.Add(cube);
            characterDict.Add(ch, cube);
        }

    }

    private void Clear(){
        // 清空字典
        characterDict = new Dictionary<char, Cube>();
        // 清空对象
        var cubeCount = cubeList.Count;
        for(int i = 0; i<cubeCount; i++){
            var cube = cubeList[0];
            cubeList.RemoveAt(0);
            Destroy(cube.gameObject);
        }
    }

    public void Refresh(){
        foreach(var cube in cubeList){
            cube.Set(Cube.EMPTY);
        }
    }

    public Cube CreateCube(){
        var obj = Instantiate(cubePrefab, cubeParent);
        var cube = obj.GetComponent<Cube>();
        cube.SetSize(cubeSize);
        return cube;
    }

    public void SetCharacter(char ch, int state){
        if(characterDict.ContainsKey(ch)){
            SetCube(characterDict[ch], state);
        }
    }

    public void EnterWord(string word, int[] check){

        if(word == null || check == null || word.Length!=check.Length){
            Debug.Log("Note input incorrect!");
        }

        var charArray = word.ToCharArray();

        for(int i = 0; i<word.Length; i++){
            var ch = charArray[i];
            if(characterDict.ContainsKey(ch)){
                var cube = characterDict[ch];

                if(cube.state == Cube.EMPTY){
                    if(check[i] == i){
                        cube.Set(Cube.RIGHT);
                    }else if(check[i] == -1){
                        cube.Set(Cube.WRONG);
                    }else{
                        cube.Set(Cube.OFFSET);
                    }
                }
                else if(cube.state == Cube.OFFSET){
                    if(check[i] == i){
                        cube.Set(Cube.RIGHT);
                    }
                }

            }
        }
    }

    private void SetCube(Cube cube, int state){
        cube.Set(state);
    }

    void Awake()
    {
        CreateEmpty(new List<char>(){'-'});
    }
}
