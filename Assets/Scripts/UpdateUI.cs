
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour
{
    // Start is called before the first frame update

    const int boardWidth = 240;
    const int boardHeight = 240;

    public GameObject piecePrefab;
    public GameObject MovetilePrefab;
    public Canvas canvas;
    public GameObject[,] Pieces = new GameObject[8,8];
    public Sprite blackBasic;
    public Sprite whiteBasic;
    public Sprite blackKing;
    public Sprite whiteKing;

    public Board board;

    public GameObject winScreen;




    //Vector<GameObject> moveTiles = new Vector<GameObject>();
    List<GameObject> moveTiles = new List<GameObject>();

    Vector2Int SelectedPiecePos = new Vector2Int();
    //PieceData[,] pieces;

    void Start()
    {
        //pieces = new PieceData[8,8];
        //pieces = board.GetBoard();

        //board.moved += //SetupAllPieces(board.GetBoard());
        for (int j = 0; j < 8; j++)
        {
            for (int i = 0; i < 8; i++)
            {
                Vector3 position;
                position = CalculateWorldPosFromBoardPos(new Vector2Int(i, j));
                Pieces[i, j] = Instantiate(piecePrefab, position, Quaternion.identity);
                Pieces[i,j].gameObject.transform.SetParent(canvas.transform, false);
                
                //Pieces[i, j].gameObject.transform.position = position;
                Pieces[i, j].SetActive(false);
            }
        }
        //SetupAllPieces(board.GetBoard());

        winScreen.SetActive(false);
        winScreen.transform.SetAsLastSibling();
    }

    public void SetupAllPieces(PieceData[,] board)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (board[i, j].color == PIECECOLOR.white)
                {
                    if (board[i, j].type == PIECETYPE.Basic)
                    {
                        Pieces[i, j].SetActive(true);
                        Pieces[i, j].GetComponent<Image>().sprite = whiteBasic;
                    }  
                    if (board[i, j].type == PIECETYPE.King)
                    {
                        Pieces[i, j].SetActive(true);
                        Pieces[i, j].GetComponent<Image>().sprite = whiteKing;
                    }                  
                }
                if (board[i, j].color == PIECECOLOR.black)
                {
                    if (board[i, j].type == PIECETYPE.Basic)
                    {
                        Pieces[i, j].SetActive(true);
                        Pieces[i, j].GetComponent<Image>().sprite = blackBasic;
                    }  
                    if (board[i, j].type == PIECETYPE.King)
                    {
                        Pieces[i, j].SetActive(true);
                        Pieces[i, j].GetComponent<Image>().sprite = blackKing;
                    }                  
                }

                if (board[i, j].color == PIECECOLOR.empty || board[i, j].type == PIECETYPE.Empty)
                {
                    Pieces[i, j].SetActive(false);
                }
            }
        }

        
    }
    void Update()
    {
        //board.OutputBoardData();
        SetupAllPieces(board.GetBoard());
        if (board.playerScores.x == 12)
        {
            DeclareWinner(PIECECOLOR.white);
        }
        else if (board.playerScores.y == 12)
        {
            DeclareWinner(PIECECOLOR.black);
        }
    }

    public void PieceClicked(Vector2Int piecePos)
    {
        ClearMoveTiles();
        SelectedPiecePos = piecePos;
        //Debug.Log("Piece clicked: " + piecePos);
        GameObject selectedPiece = Pieces[piecePos.x, piecePos.y];
        PieceData selectedPieceData = board.GetBoard()[piecePos.x, piecePos.y];

        if (selectedPieceData.color == board.currentPlayer)
        {
            if (selectedPieceData.color == PIECECOLOR.white || selectedPieceData.type == PIECETYPE.King)
            {
                //positive y movement
                if (IsOnBoard(new Vector2Int(piecePos.x + 1, piecePos.y + 1)) && board.GetBoard()[piecePos.x + 1, piecePos.y + 1].color == PIECECOLOR.empty)
                {
                    Vector3 moveTilePos = CalculateWorldPosFromBoardPos(new Vector2Int(piecePos.x + 1, piecePos.y + 1));
                    //Debug.Log("trying to instantiate movetile");
                    GameObject newMoveTile = Instantiate(MovetilePrefab, moveTilePos, Quaternion.identity);
                    newMoveTile.transform.SetParent(canvas.transform, false);
                    moveTiles.Add(newMoveTile);

                }
                else if (IsOnBoard(new Vector2Int(piecePos.x + 2, piecePos.y + 2)) && board.GetBoard()[piecePos.x + 1, piecePos.y + 1].color != board.currentPlayer && board.GetBoard()[piecePos.x + 2, piecePos.y + 2].color == PIECECOLOR.empty)
                {
                    Vector3 moveTilePos = CalculateWorldPosFromBoardPos(new Vector2Int(piecePos.x + 2, piecePos.y + 2));
                    //Debug.Log("trying to instantiate movetile");
                    GameObject newMoveTile = Instantiate(MovetilePrefab, moveTilePos, Quaternion.identity);
                    newMoveTile.transform.SetParent(canvas.transform, false);
                    moveTiles.Add(newMoveTile);
                }

                if (IsOnBoard(new Vector2Int(piecePos.x - 1, piecePos.y + 1)) && board.GetBoard()[piecePos.x - 1, piecePos.y + 1].color == PIECECOLOR.empty)
                {
                    Vector3 moveTilePos = CalculateWorldPosFromBoardPos(new Vector2Int(piecePos.x - 1, piecePos.y + 1));
                    GameObject newMoveTile = Instantiate(MovetilePrefab, moveTilePos, Quaternion.identity);
                    newMoveTile.transform.SetParent(canvas.transform, false);
                    moveTiles.Add(newMoveTile);
                }
                else if (IsOnBoard(new Vector2Int(piecePos.x - 2, piecePos.y + 2)) && board.GetBoard()[piecePos.x - 1, piecePos.y + 1].color != board.currentPlayer && board.GetBoard()[piecePos.x - 2, piecePos.y + 2].color == PIECECOLOR.empty)
                {
                    Vector3 moveTilePos = CalculateWorldPosFromBoardPos(new Vector2Int(piecePos.x - 2, piecePos.y + 2));
                    //Debug.Log("trying to instantiate movetile");
                    GameObject newMoveTile = Instantiate(MovetilePrefab, moveTilePos, Quaternion.identity);
                    newMoveTile.transform.SetParent(canvas.transform, false);
                    moveTiles.Add(newMoveTile);
                }
            }

            if (selectedPieceData.color == PIECECOLOR.black || selectedPieceData.type == PIECETYPE.King)
            {
                //negative y movement
                if (IsOnBoard(new Vector2Int(piecePos.x + 1, piecePos.y - 1)) && board.GetBoard()[piecePos.x + 1, piecePos.y - 1].color == PIECECOLOR.empty)
                {
                    Vector3 moveTilePos = CalculateWorldPosFromBoardPos(new Vector2Int(piecePos.x + 1, piecePos.y - 1));
                    GameObject newMoveTile = Instantiate(MovetilePrefab, moveTilePos, Quaternion.identity);
                    newMoveTile.transform.SetParent(canvas.transform, false);
                    moveTiles.Add(newMoveTile);

                }
                else if (IsOnBoard(new Vector2Int(piecePos.x + 2, piecePos.y - 2)) && board.GetBoard()[piecePos.x + 1, piecePos.y - 1].color != board.currentPlayer && board.GetBoard()[piecePos.x + 2, piecePos.y - 2].color == PIECECOLOR.empty)
                {   
                    Vector3 moveTilePos = CalculateWorldPosFromBoardPos(new Vector2Int(piecePos.x + 2, piecePos.y - 2));
                    //Debug.Log("trying to instantiate movetile");
                    GameObject newMoveTile = Instantiate(MovetilePrefab, moveTilePos, Quaternion.identity);
                    newMoveTile.transform.SetParent(canvas.transform, false);
                    moveTiles.Add(newMoveTile);
                }

                if (IsOnBoard(new Vector2Int(piecePos.x - 1, piecePos.y - 1)) && board.GetBoard()[piecePos.x - 1, piecePos.y - 1].color == PIECECOLOR.empty)
                {
                    Vector3 moveTilePos = CalculateWorldPosFromBoardPos(new Vector2Int(piecePos.x - 1, piecePos.y - 1));
                    GameObject newMoveTile = Instantiate(MovetilePrefab, moveTilePos, Quaternion.identity);
                    newMoveTile.transform.SetParent(canvas.transform, false);
                    moveTiles.Add(newMoveTile);
                }
                else if (IsOnBoard(new Vector2Int(piecePos.x - 2, piecePos.y - 2)) && board.GetBoard()[piecePos.x - 1, piecePos.y - 1].color != board.currentPlayer && board.GetBoard()[piecePos.x - 2, piecePos.y - 2].color == PIECECOLOR.empty)
                {
                    Vector3 moveTilePos = CalculateWorldPosFromBoardPos(new Vector2Int(piecePos.x - 2, piecePos.y - 2));
                    //Debug.Log("trying to instantiate movetile");
                    GameObject newMoveTile = Instantiate(MovetilePrefab, moveTilePos, Quaternion.identity);
                    newMoveTile.transform.SetParent(canvas.transform, false);
                    moveTiles.Add(newMoveTile);
                }
            }

        }

    }

    Vector3 CalculateWorldPosFromBoardPos(Vector2Int boardPos)
    {
        Vector3 position;
        float posStep = boardWidth/7;
        if (boardPos.x == 0)
        {
            position.x = -boardWidth/2.0f;
        }
        else 
        {
            position.x = (posStep * boardPos.x)-(boardWidth/2.0f);
        }
        if (boardPos.y == 0)
        {
            position.y = -boardHeight/2.0f;
        }
        else
        {
            position.y = (posStep * boardPos.y)-(boardHeight/2.0f);
        }
        position.z = 0;
        return position;
    }

    bool IsOnBoard(Vector2Int pos)
    {
        if (pos.x > 7 || pos.x < 0 || pos.y > 7 || pos.y < 0)
        {
            return false;
        }
        return true;
    }

    void ClearMoveTiles()
    {
        foreach(GameObject obj in moveTiles)
        {
            Destroy(obj);
        }
        moveTiles.Clear();
    }

    public void MoveTileClicked(Vector2Int pos)
    {
        ClearMoveTiles();
        board.MovePiece(SelectedPiecePos.x, SelectedPiecePos.y, pos.x, pos.y);
    }

    public void DeclareWinner(PIECECOLOR winner)
    {
        board.currentPlayer = PIECECOLOR.empty;
        string winText = winner + " wins";
        winScreen.GetComponent<Text>().text = winText;
        winScreen.SetActive(true);
    }

    public void ResetGame()
    {
        Debug.Log("resetting game");
        winScreen.SetActive(false);
        board.SetupBoard();
    }
}
