using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    #region Game and UI Objects
    [SerializeField] private Transform wordParent;
    
    [SerializeField] private Text timeText, attemptsText, answerText, warningText;

    [SerializeField] private NoteGroup noteGroup;

    [SerializeField] private Slider wordLengthSlider;
    [SerializeField] private Toggle mustEnterWordToggle, noteEnableToggle;

    #endregion

    #region Prefabs
    [SerializeField] private GameObject wordPrefab = null;
    #endregion

    private CubeGroup currentWord;
    private List<CubeGroup> wordList;

    // 同屏显示单词数量
    private int maxCount = 8;

    // 单词长度
    private int wordLength = 5;

    // 是否处于游戏中
    private bool playing = false;

    // 本局游戏时间
    private float playingTime = 0f;
    // 本局尝试次数
    private int playingAttempts = 0;

    // 本局目标单词
    private string targetWord;


    // 笔记功能
    private bool noteEnabled = true;
    // 是否必须输入单词
    private bool mustEnterWord = true;
    // 最多尝试次数
    // private int maxAttempts = 10;

    #region UI operations
    private void ShowWarning(){
        warningText.transform.parent.gameObject.SetActive(true);
    }

    private void HideWarning(){
        warningText.transform.parent.gameObject.SetActive(false);
    }

    private void ShowAnswer(string word){
        answerText.transform.parent.gameObject.SetActive(true);
        answerText.text = "The answer is:\n"+word;
    }

    private void HideAnswer(){
        answerText.transform.parent.gameObject.SetActive(false);
    }

    private void ShowTime(float seconds){
        timeText.text = "Time: " + FormatTime(seconds);
    }

    private void ShowAttempts(int c){
        attemptsText.text = "Attempts: "+c;
    }


    #endregion

    void Start()
    {
        wordList = new List<CubeGroup>();
        
        StartGame();
    }

    private void DeleteWord(int pos = 0){
        if(pos >= 0 && pos < wordList.Count){
            if(wordList[pos] != null){
                var cube = wordList[pos];
                wordList.RemoveAt(pos);
                Destroy(cube.gameObject);
            }
        }
    }

    private void Clear(){
        // 清空对象
        var c = wordList.Count;
        for(int i = 0; i<c; i++){
            var wl = wordList[0];
            wordList.RemoveAt(0);
            Destroy(wl.gameObject);
        }
        currentWord = null;

        // 清空提示
        noteGroup.Refresh();
    }

    public void StartGame(){
        Clear();

        HideAnswer();
        HideWarning();

        GetSettings();

        currentWord = CreateEmptyWord();
        targetWord = GameDictionary.Instance.GetRandomWord(wordLength);

        playing = true;
        playingTime = 0f;
        playingAttempts = 0;
    }

    /// <summary>
    /// 从UI组件获得设置信息
    /// </summary>
    private void GetSettings(){
        wordLength = (int)wordLengthSlider.value;
        mustEnterWord = mustEnterWordToggle.isOn;
        noteEnabled = noteEnableToggle.isOn;
    }

    public void GiveUp(){
        ShowAnswer(targetWord);
        EndGame();
    }

    public void EndGame(){
        playing = false;
    }

    private CubeGroup CreateEmptyWord(){

        if(wordList.Count >= maxCount) DeleteWord();

        var obj = Instantiate(wordPrefab, wordParent);
        var cube = obj.GetComponent<CubeGroup>();
        cube.CreateEmpty(wordLength);

        wordList.Add(cube);

        return cube;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
        else if(Input.GetKeyDown(KeyCode.F1)){
            StartGame();
        }
        else if(Input.GetKeyDown(KeyCode.F2)){
            GiveUp();
        }

        if(!playing) return;

        // 控制
        if(currentWord != null){
            if(Input.anyKeyDown){
                // 输入
                for(KeyCode kc = KeyCode.A; kc <= KeyCode.Z; kc++){
                    if(Input.GetKeyDown(kc)){
                        currentWord.AddCharacter((char)kc);
                        HideWarning();
                        break;
                    }
                }
                if(Input.GetKeyDown(KeyCode.Minus)){
                    currentWord.AddCharacter('-');
                    HideWarning();
                }
                // 退格
                else if(Input.GetKeyDown(KeyCode.Backspace)){
                    currentWord.DeleteCharacter();
                    HideWarning();
                }
                // 提交
                else if(Input.GetKeyDown(KeyCode.Return)){
                    if(IsValid()){
                        var result = currentWord.CheckWord(targetWord);
                        playingAttempts ++;

                        // 如果笔记功能打开，则更新笔记
                        if(noteEnabled){
                            var w = currentWord.GetWord();
                            var check = CubeGroup.CompareWord(w, targetWord);
                            noteGroup.EnterWord(w, check);
                        }

                        if(result){
                            EndGame();
                        }else{
                            currentWord = CreateEmptyWord();
                        }
                        
                    }else{
                        ShowWarning();
                    }
                }
            }
        }

        // 显示游玩状态
        ShowAttempts(playingAttempts);

        playingTime += Time.deltaTime;
        ShowTime(playingTime);
    }

    /// <summary>
    /// 判断当前输入是否符合条件：字符数正确，（可选）是词库中的单词
    /// </summary>
    /// <returns></returns>
    private bool IsValid(){
        if(currentWord.GetCount() != wordLength) return false;
        else {
            if(mustEnterWord) return GameDictionary.Instance.IsValidWord(currentWord.GetWord());
            else return true;
        }
    }

    /// <summary>
    /// 格式化时间
    /// </summary>
    /// <param name="seconds">秒</param>
    /// <returns></returns>
    public static string FormatTime(float seconds)
    {
        TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(seconds));
        string str = "";

        if (ts.Hours > 0)
        {
            str = ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
        }
        if (ts.Hours == 0 && ts.Minutes > 0)
        {
            str =ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
        }
        if (ts.Hours == 0 && ts.Minutes == 0)
        {
            str = "00:" + ts.Seconds.ToString("00");
        }

        return str;
    }

}
