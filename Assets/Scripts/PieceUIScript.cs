
using UnityEngine;
using UnityEngine.UI;

public class PieceScript : MonoBehaviour
{
    public UpdateUI UIManager;
    Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = this.gameObject.GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        Vector2Int boardPos = GetBoardPosFromWorldPos();

        UIManager.PieceClicked(boardPos);

        //UIManager.board.OutputBoardData();
        //Debug.Log("UI board" + UIManager.board.gameObject.name);
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
