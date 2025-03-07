using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovetileUISript : MonoBehaviour
{
    public UpdateUI UIManager;

    Button button;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("movetile instantiated");
        button = this.gameObject.GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
        //Debug.Log(button);
    }


    void OnButtonClick()
    {
        //Debug.Log("movetile clicked");
        Vector2Int boardPos = GetBoardPosFromWorldPos();

        UIManager.MoveTileClicked(boardPos);
    }

    Vector2Int GetBoardPosFromWorldPos()
    {
        Vector3 worldPos = this.gameObject.transform.position;
        Vector2Int boardPos = new Vector2Int();

        boardPos.x = (int)((worldPos.x + 120) / (240/7)) - 12; 
        boardPos.y = (int)((worldPos.y + 120) / (240/7)) - 5; 
        return boardPos;
    }
}
