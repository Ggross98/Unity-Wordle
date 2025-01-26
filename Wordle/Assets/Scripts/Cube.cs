using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cube : MonoBehaviour
{
    # region UI Objects
    [SerializeField] private Image image;
    [SerializeField] private Text text;
    # endregion

    [SerializeField] private Sprite CUBE, CUBE_FRAME;

    public int state = -1;

    void Awake()
    {
        if(image == null || text == null){
            Debug.LogError("Character cube UI objects not found!");
        }

        if(CUBE == null || CUBE_FRAME == null){
            Debug.LogError("Character cube sprite not found!");
        }
    }
    
    private void SetSprite(bool fill){
        if(fill) image.sprite = CUBE;
        else image.sprite = CUBE_FRAME;
    }

    private void SetColor(Color color){
        image.color = color;
    }

    private void SetCharacterColor(Color color){
        text.color = color;
    }

    public void SetCharacter(char c){
        if(c == '0') text.text = "";
        else text.text = (c+"").ToUpper();
    }

    public char GetCharacter(){
        if(text.text == "") return '0';
        return text.text.ToCharArray()[0];
    }

    public void SetSize(int size){
        GetComponent<RectTransform>().anchoredPosition = new Vector2(size, size);
        text.fontSize = size/2;
    }

    // **********************************************************************************
    
    public const int EMPTY = 0, RIGHT = 1, OFFSET = 2, WRONG = -1;

    public void Set(int i){
        state = i;

        switch(i){
            case EMPTY:
                SetCharacterColor(Color.black);
                SetSprite(false);
                SetColor(Color.gray);
                break;
            case RIGHT:
                SetCharacterColor(Color.white);
                SetSprite(true);
                SetColor(new Color(75f/255f, 190f/255f, 75f/255f));
                break;
            case OFFSET:
                SetCharacterColor(Color.white);
                SetSprite(true);
                SetColor(new Color(210f/255f, 200f/255f, 75f/255f));
                break;
            case WRONG:
                SetCharacterColor(Color.white);
                SetSprite(true);
                SetColor(Color.gray);
                break;


        }
    }
}
