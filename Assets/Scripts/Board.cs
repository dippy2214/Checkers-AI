using UnityEngine;
public enum PIECETYPE
{
    Empty,
    Basic,
    King
}

public enum PIECECOLOR
{
    empty, black, white
}

public class PieceData
{
    public PIECETYPE type;
    public PIECECOLOR color;

    public Vector2Int piecePos;
}


public class Board : MonoBehaviour
{
    public PieceData[,] board = new PieceData[8, 8];

    public PIECECOLOR currentPlayer = PIECECOLOR.white;

    public Vector2 playerScores;
    
    public AINode rootNode = new AINode();

    public bool isAI = true;

    const int AIMaxIterations = 1000;
    //public delegate void Moved();

    //public Moved moved;


    void Start()
    {
        Screen.SetResolution(816, 355, true);
        //Debug.Log("board constructor");
        SetupBoard();
    }

    public void SetupBoard()
    {     
        //Debug.Log("setting up board" + this.gameObject.name);  
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 8; j+=2)
            {
                if (i % 2 != 0)
                {
                    board[j, i] = new PieceData { color = PIECECOLOR.white, type = PIECETYPE.Basic, piecePos = new Vector2Int(j, i) };
                }
                else
                {
                    board[j+1, i] = new PieceData { color = PIECECOLOR.white, type = PIECETYPE.Basic, piecePos = new Vector2Int(j+1, i) };
                }
            }
        }

        for (int i = 7; i > 4; i--)
        {
            for (int j = 7; j > 0; j-=2)
            {
                if (i % 2 == 0)
                {
                    board[j, i] = new PieceData { color = PIECECOLOR.black, type = PIECETYPE.Basic, piecePos = new Vector2Int(j, i) };
                }
                else
                {
                    board[j-1, i] = new PieceData { color = PIECECOLOR.black, type = PIECETYPE.Basic, piecePos = new Vector2Int(j-1, i) };
                }
            }
        }

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (board[j, i] == null)
                {
                    board[j, i] = new PieceData {};
                }
            }
        }

        for (int i = 3; i < 5; i++)
        {
            for (int j = 0; j < 7; j+= 2)
            {
                if (i % 2 != 0)
                {
                    board[j, i] = new PieceData { color = PIECECOLOR.empty, type = PIECETYPE.Empty, piecePos = new Vector2Int(j, i) };
                }
                else
                {
                    board[j+1, i] = new PieceData { color = PIECECOLOR.empty, type = PIECETYPE.Empty, piecePos = new Vector2Int(j+1, i) };
                }
            }
        }

        playerScores = new Vector2(0, 0);
        currentPlayer = PIECECOLOR.white;
    }

    public void Update()
    {
        if (isAI == true && currentPlayer == PIECECOLOR.black)
        {
            //Debug.Log("black Turn");
            RunAI();
            
        }
    }

    public void OutputBoardData()
    {
        Debug.Log("current player: " + currentPlayer + ", outputting board data");
        for (int i = 7; i > -1; i--)
        {
            string output = "";
            output += i;
            output += ": ";
            for (int j = 0; j < 8; j++)
            {
                output += board[j, i].color;
                output += ", ";
            }
            Debug.Log(output);
        }
    }

    public void MovePiece(int startx, int starty, int endx, int endy)
    {
        if (IsLegalMove(startx, starty, endx, endy))
        {
            
            PieceData movingPiece = board[startx, starty];
            board[startx, starty] = new PieceData {color = PIECECOLOR.empty, type = PIECETYPE.Empty};
            movingPiece.piecePos = new Vector2Int(endx, endy);
            board[endx, endy] = movingPiece;
            
            if (Mathf.Abs(endx - startx) == 2)
            {
                Vector2Int moveVec = new Vector2Int(endx - startx, endy - starty);
                moveVec /= 2;
                board[startx + moveVec.x, starty + moveVec.y].color = PIECECOLOR.empty;
                board[startx + moveVec.x, starty + moveVec.y].type = PIECETYPE.Empty;
                UpdateScoreOnTake();
            }
            
            CheckForPromotionOnMove(endx, endy);
            //OutputBoardData();
            SwapPlayer();
            //rootNode.SetGameState(this.Clone());
            //rootNode.board.isAI = false;
            
            //moved?.Invoke();
        }
        else
        {
            Debug.Log("Attempted to make illegal move");
            OutputBoardData();
        }
    }

    public bool IsLegalMove(int startx, int starty, int endx, int endy)
    {
        if (startx < 0 || startx > 7 || starty < 0 || starty > 7)
        {
            //Debug.Log("startPos off board (" + startx + ", " + starty + ")");
            return false;
        }
        if (endx < 0 || endx > 7 || endy < 0 || endy > 7)
        {
            //Debug.Log("endPos off board (" + endx + ", " + endy + ")");
            return false;
        }
        //black basic piece cant move down board
        if (board[startx, starty].color == PIECECOLOR.black && board[startx, starty].type == PIECETYPE.Basic)
        {
            if (endy > starty)
            {
                //Debug.Log("cant move black basic piece backwards");
                return false;
            }
        }
        //white basic piece cant move up board
        if (board[startx, starty].color == PIECECOLOR.white && board[startx, starty].type == PIECETYPE.Basic)
        {
            if (endy < starty)
            {
                //Debug.Log("cant move white basic piece backwards");
                return false;
            }
        }
        //check if start square is empty
        if (board[startx, starty].color == PIECECOLOR.empty)
        {
            //Debug.Log("moving from empty square");
            return false;
        }
        //check if end square is taken already
        if (board[endx, endy].color != PIECECOLOR.empty)
        {
            //Debug.Log("moving too taken square");
            return false;
        }
        //if we're moving one square and all previous conditions are met allow it
        if (Mathf.Abs(endx - startx) == 1 && Mathf.Abs(endy - starty) == 1)
        {
            return true;   
        }
        //if we are moving more than one square this means we are taking so we can check to see if we are actually taking something
        else if (Mathf.Abs(endx - startx) == 2 && Mathf.Abs(endy - starty) == 2)
        {    
            Vector2Int moveVec = new Vector2Int(endx - startx, endy - starty);
            moveVec.x /= 2;
            moveVec.y /= 2;
            if ((board[startx, starty].color != board[startx + moveVec.x, starty + moveVec.y].color) && (board[startx + moveVec.x, starty + moveVec.y].type != PIECETYPE.Empty))
            {
                return true;
            }
            else
            {
                //Debug.Log("Taking nothing");
            }
        }
        return false;
    }

    public void TryMove(Vector2Int selectedPiecePos, Vector2Int TargetSquare)
    {
        MovePiece(selectedPiecePos.x, selectedPiecePos.y, TargetSquare.x, TargetSquare.y);
    }

    public PieceData[,] GetBoard()
    {
        return board;
    }

    void SwapPlayer()
    {
        if (currentPlayer == PIECECOLOR.white)
        {
            //Debug.Log("swap to black");
            currentPlayer = PIECECOLOR.black;
        }
        else
        {
            //Debug.Log("swap to white");
            currentPlayer = PIECECOLOR.white;
        }
        //OutputBoardData();
    }

    void CheckForPromotionOnMove(int endx, int endy)
    {
        if (board[endx, endy].color == PIECECOLOR.white && endy == 7)
        {
            board[endx, endy].type = PIECETYPE.King;
        }
        else if (board[endx, endy].color == PIECECOLOR.black && endy == 0)
        {
            board[endx, endy].type = PIECETYPE.King;
        }
    }

    void UpdateScoreOnTake()
    {
        if (currentPlayer == PIECECOLOR.white)
        {
            playerScores.x += 1.0f;
        }
        else
        {
            playerScores.y += 1.0f;
        }
    }

    void RunAI()
    {

        rootNode.SetGameState(this.Clone());

        int runCount = 0;
        do
        {

            AINode selectedNode = rootNode.Select();
            AINode expandedNode = selectedNode.Expand();
            if (expandedNode != null)
            {
                expandedNode.Simulate(PIECECOLOR.black);
            }

            runCount++;
        } while (runCount < AIMaxIterations);

        AINode highestRankedChild = rootNode.FindHighestRankingChild();
        
        if (highestRankedChild != null)
        {
            AIMove bestMove = highestRankedChild.thisNodeMove;
            TryMove(bestMove.startPos, bestMove.endPos);
        }

        rootNode.clearNode();
    }

    public PieceData[,] GetBoardCopy()
    {
        PieceData[,] newBoard = new PieceData[8,8];
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                newBoard[i, j] = new PieceData();
                newBoard[i, j].color = board[i, j].color;
                newBoard[i, j].type = board[i, j].type;
            }
        }
        return newBoard;
    }

    public Board Clone()
    {
        //Debug.Log("Cloning");
        //string json = jsonSerializer.Serialization(this);
        Board newBoard = Instantiate<Board>(this);
        newBoard.currentPlayer = this.currentPlayer;
        newBoard.playerScores.x = this.playerScores.x;
        newBoard.playerScores.y = this.playerScores.y;
        newBoard.board = this.GetBoardCopy();
        newBoard.isAI = false;
        return newBoard;
    }

    public void OutputBoardFromArray(PieceData[,] data)
    {
        Debug.Log("outputting board data");
        Debug.Log(" 0    1   2   3   4   5   6   7");
        for (int i = 7; i > -1; i--)
        {
            string output = "";
            for (int j = 0; j < 8; j++)
            {
                output += data[j, i].color;
                output += ", ";
            }
            Debug.Log(output);
        }
    }
}
