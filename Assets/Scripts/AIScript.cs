using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public struct AIMove
{
    public Vector2Int startPos;
    public Vector2Int endPos;
    public bool Capture;
}

public class AINode
{
    public AINode parent;
    public List<AINode> children = new List<AINode>();
    public Board board;

    public PieceData[,] startBoard = new PieceData[8,8];
    public bool isEndState = false;

    public AIMove thisNodeMove;

    List<AIMove> legalMoves = new List<AIMove>();

    public float ranking  = 0.0f;

    void Start()
    {
        //Debug.Log("new AI node");
    }

    public void SetGameState(Board board)
    {
        this.board = board;
        startBoard = board.GetBoardCopy();
        this.board.isAI = false;
        //Debug.Log("legal moves on generation: " + legalMoves.Count);
        GenerateLegalMoves();
    }

    void OutputLegalMoves()
    {

        Debug.Log(board.currentPlayer + ", Legal moves: ");
        for (int i = 0; i < legalMoves.Count; i++)
        {
            Debug.Log(legalMoves[i].startPos + ", " + legalMoves[i].endPos);
        }
    }

    void GenerateLegalMoves()
    {
        legalMoves = GetAllLegalMovesOnTempBoard(board.GetBoardCopy(), board.currentPlayer);
        //OutputLegalMoves();
        if (legalMoves.Count == 0)
        {
            //Debug.Log("no legal moves found");
            isEndState = true;
        }
        
        PIECECOLOR winner = PIECECOLOR.empty;
        if (board.playerScores.x == 12)
        {
            winner = PIECECOLOR.white;
        }
        else if (board.playerScores.y == 12)
        {
            winner = PIECECOLOR.black;
        }

        if (winner != PIECECOLOR.empty)
        {
            isEndState = true;
        }
    }

    public AINode Select()
    {
        if (children.Count == 0 || legalMoves.Count == 0 || children.Count < legalMoves.Count)
        {
            return this;
        }
        else
        {
            int randomBranch = Random.Range(0, children.Count);
            return children[randomBranch].Select();
        }
    }

    public AINode Expand()
    {
        if (isEndState || legalMoves.Count == 0)
        {
            if (children.Count == 0)
            {
                return null;
            }
            int randomChild = Random.Range(0, children.Count);

            AINode expandedChild = children[randomChild].Expand();

            return null;
        }
        else 
        {
            int randomMove = Random.Range(0, legalMoves.Count);
            AIMove move = new AIMove();
            move.startPos = legalMoves[randomMove].startPos;
            move.endPos = legalMoves[randomMove].endPos;
            legalMoves.RemoveAt(randomMove);
            if (Mathf.Abs(move.startPos.x - move.endPos.x) == 2)
            {
                move.Capture = true;
            }
            else
            {
                move.Capture = false;
            }

            AINode childNode = new AINode();
            childNode.parent = this;

            Board childBoard = board.Clone();
            childBoard.TryMove(move.startPos, move.endPos);
            
            childNode.SetGameState(childBoard);
            GameObject.Destroy(childBoard);
            childNode.thisNodeMove = move;

            children.Add(childNode);

            return childNode;
        }
    }

    int SearchLegalMovesForCapture(List<AIMove> moves)
    {
        int output = -1;
        for (int i = 0; i < moves.Count; i++)
        {
            if (moves[i].Capture == true)
            {
                output = i;
            }
        }
        return output;
    }

    public void Simulate(PIECECOLOR startingTurn)
    {
        Board copyOfBoard = this.board.Clone();
        PIECECOLOR currentPlayer = board.currentPlayer;
        bool endState = false;
        List<AIMove> TempLegalMoves = new List<AIMove>();
        

        PIECECOLOR winner = PIECECOLOR.empty;
        if (board.playerScores.x == 12)
        {
            endState = true;
            winner = PIECECOLOR.white;
        }
        else if (board.playerScores.y == 12)
        {
            endState = true;
            winner = PIECECOLOR.black;
        }

        if (winner != PIECECOLOR.empty)
        {
            CalcResult(winner);
            return;
        }
        int itCount = 0;
        while (!endState && itCount < 100)
        {
            itCount++;
            TempLegalMoves = GetAllLegalMovesOnTempBoard(copyOfBoard.GetBoardCopy(), copyOfBoard.currentPlayer);
            
            if (TempLegalMoves.Count == 0)
            {
                endState = true;
                break;
            } 
            else 
            {
                int randomMove = Random.Range(0, TempLegalMoves.Count);
                AIMove newMove = TempLegalMoves[randomMove];
                int caps = SearchLegalMovesForCapture(TempLegalMoves);
                if (caps != -1)
                {
                    newMove = TempLegalMoves[caps];
                }
                
                copyOfBoard.TryMove(newMove.startPos, newMove.endPos);
            }

            if (copyOfBoard.playerScores.x == 12)
            {
                winner = PIECECOLOR.white;
                endState = true;
            }
            else if (copyOfBoard.playerScores.y == 12)
            {
                winner = PIECECOLOR.black;
                endState = true;
            }   

            if (winner != PIECECOLOR.empty)
            {
                CalcResult(winner);

                UnityEngine.Object.Destroy(copyOfBoard.gameObject);
                return;
            }

            currentPlayer = copyOfBoard.currentPlayer;
        }
        CalcResult(PIECECOLOR.empty);
        UnityEngine.Object.Destroy(copyOfBoard.gameObject);
    }

    void CalcResult(PIECECOLOR winner)
    {
        if (winner == PIECECOLOR.black)
        {
            BackPropegate(1);
        }
        else if (winner == PIECECOLOR.white)
        {
            BackPropegate(-1);
        }
        else
        {
            BackPropegate(0);
        }
    }

    void BackPropegate(float result)
    {
        ranking += result;
        //Debug.Log(ranking);
        if (this.parent != null)
        {
            this.parent.BackPropegate(result);
        }
    }

    public AINode FindHighestRankingChild()
    {

        if (children.Count == 0)
        {
            return null;
        }
        else
        {
            float maxRanking = 0.0f;
            int maxIndex = 0;
            for (int i = 0; i < children.Count; i++)
            {
                //board.OutputBoardFromArray(children[i].startBoard);
                if (children[i].ranking < maxRanking)
                {
                    maxRanking = children[i].ranking;
                    maxIndex = i;
                }
            }
            if (children[maxIndex].board.currentPlayer == PIECECOLOR.black)
            {
                return children[maxIndex].parent;
            }
            return children[maxIndex];
        }
    }

    public void clearNode()
    {
        foreach (AINode child in children)
        {
            child.clearNode();
        }
        children.Clear();
        ranking = 0;
        legalMoves.Clear();
        UnityEngine.Object.Destroy(board.gameObject);
        
        
    }

    List<AIMove> GetAllLegalMovesOnTempBoard(PieceData[,] board, PIECECOLOR player)
    {
        List<AIMove> legalMoves = new List<AIMove>();

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (board[i, j].color == player)
                {
                    if (IsLegalMoveFromArray(board, i, j, i+1, j+1))
                    {
                        AIMove newMove = new AIMove();
                        newMove.startPos = new Vector2Int(i, j);
                        newMove.endPos = new Vector2Int(i+1, j+1);
                        newMove.Capture = false;
                        //Debug.Log("added move: " + newMove.startPos + " to " + newMove.endPos);
                        legalMoves.Add(newMove);
                    }

                    if (IsLegalMoveFromArray(board, i, j, i-1, j+1))
                    {
                        AIMove newMove = new AIMove();
                        newMove.startPos = new Vector2Int(i, j);
                        newMove.endPos = new Vector2Int(i-1, j+1);
                        newMove.Capture = false;
                        //Debug.Log("added move: " + newMove.startPos + " to " + newMove.endPos);

                        legalMoves.Add(newMove);
                    }

                    if (IsLegalMoveFromArray(board, i, j, i+1, j-1))
                    {
                        AIMove newMove = new AIMove();
                        newMove.startPos = new Vector2Int(i, j);
                        newMove.endPos = new Vector2Int(i+1, j-1);
                        newMove.Capture = false;
                        //Debug.Log("added move: " + newMove.startPos + " to " + newMove.endPos);

                        legalMoves.Add(newMove);
                    }

                    if (IsLegalMoveFromArray(board, i, j, i-1, j-1))
                    {
                        AIMove newMove = new AIMove();
                        newMove.startPos = new Vector2Int(i, j);
                        newMove.endPos = new Vector2Int(i-1, j-1);
                        newMove.Capture = false;
                        //Debug.Log("added move: " + newMove.startPos + " to " + newMove.endPos);

                        legalMoves.Add(newMove);
                    }

                    if (IsLegalMoveFromArray(board, i, j, i+2, j+2))
                    {
                        AIMove newMove = new AIMove();
                        newMove.startPos = new Vector2Int(i, j);
                        newMove.endPos = new Vector2Int(i+2, j+2);
                        newMove.Capture = true;
                        //Debug.Log("added move: " + newMove.startPos + " to " + newMove.endPos);

                        legalMoves.Add(newMove);
                    }

                    if (IsLegalMoveFromArray(board, i, j, i-2, j+2))
                    {
                        AIMove newMove = new AIMove();
                        newMove.startPos = new Vector2Int(i, j);
                        newMove.endPos = new Vector2Int(i-2, j+2);
                        newMove.Capture = true;
                        //Debug.Log("added move: " + newMove.startPos + " to " + newMove.endPos);
                        legalMoves.Add(newMove);
                    }

                    if (IsLegalMoveFromArray(board, i, j, i+2, j-2))
                    {
                        AIMove newMove = new AIMove();
                        newMove.startPos = new Vector2Int(i, j);
                        newMove.endPos = new Vector2Int(i+2, j-2);
                        newMove.Capture = true;
                        //Debug.Log("added move: " + newMove.startPos + " to " + newMove.endPos);
                        legalMoves.Add(newMove);
                    }

                    if (IsLegalMoveFromArray(board, i, j, i-2, j-2))
                    {
                        AIMove newMove = new AIMove();
                        newMove.startPos = new Vector2Int(i, j);
                        newMove.endPos = new Vector2Int(i-2, j-2);
                        newMove.Capture = true;
                        //Debug.Log("added move: " + newMove.startPos + " to " + newMove.endPos);
                        legalMoves.Add(newMove);
                    }
                }
            }
        }
        //Debug.Log("Legal moves in generated list: " + legalMoves.Count);
        return legalMoves;
    }
    
    bool IsLegalMoveFromArray(PieceData[,] board, int startx, int starty, int endx, int endy)
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
}
