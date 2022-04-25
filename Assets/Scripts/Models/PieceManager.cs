using System.Collections.Generic;
using UnityEngine;
using System;
using static Board;
using Assets.Scripts;
using System.Threading.Tasks;

public class PieceManager : MonoBehaviour
{

    public GameObject mPiecePrefab;

    public List<BasePiece> mDefPiece = null;
    protected List<BasePiece> mAttackPiece = null;

    protected static bool isBlackTurn = true;

    private Dictionary<string, Type> mPieceLibrary = new Dictionary<string, Type>()
    {
        {"K", typeof(King) },
        {"R", typeof(Regular)}
    };

    public static Color WhosTurn()
    {
        if (isBlackTurn)
            return Color.black;
        else
            return Color.white;
    }
    public virtual void Setup (Board board)
    {
        mDefPiece = CreatePieces(Color.white, 12, board, true);

        mAttackPiece = CreatePieces(Color.black, 24, board, false);

        PlaceDefPiece(mDefPiece, board);
        PlaceAttackPiece(mAttackPiece, board);

        SwitchSide(Color.white);
    }

    protected void SetInteractive(List<BasePiece> allPieces, bool value)
    {
        foreach (BasePiece p in allPieces)
            p.enabled = value;
    }

    public virtual void SwitchSide(Color color)
    {
        if (isBlackTurn)
        {
            if (!CheckPossibleMoveOfDef()) Board.Instance.EndGame(Color.black);
        }
        isBlackTurn = color == Color.white ? true : false;

        SetInteractive(mDefPiece, !isBlackTurn);
        SetInteractive(mAttackPiece, isBlackTurn);
    }

    private bool CheckPossibleMoveOfDef()
    {
        Global.mHiglightedCells.Clear();
        foreach (var p in mDefPiece)
        {
            p.CreateCellPatch();
            if (Global.mHiglightedCells.Count > 0)
            {
                Global.mHiglightedCells.Clear();
                return true;
            }
        }
        Global.mHiglightedCells.Clear();
        return false;
    }

    protected List<BasePiece> CreatePieces(Color teamColor, int number, Board board, bool isDefTeam)
    {
        Sprite _Sprite;
        if (teamColor == Color.white)
            _Sprite = Resources.Load<Sprite>("DefFigure");
        else
            _Sprite = Resources.Load<Sprite>("AttackFigure");

        List<BasePiece> newPieces = new List<BasePiece>();

        for (int i = 0; i < number; i++)
        {
            Sprite _picedSprite = _Sprite;
            GameObject newPieceObject = Instantiate(mPiecePrefab);
            newPieceObject.transform.SetParent(transform);

            newPieceObject.transform.localScale = new Vector3(1, 1, 1);
            newPieceObject.transform.localRotation = Quaternion.identity;
            //newPieceObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);


            string key = "R";
            //if (isDefTeam)
            //{
            //    key = "K";
            //    _picedSprite = Resources.Load<Sprite>("KingFigure");
            //    isDefTeam = false;
            //}

            Type pieceType = mPieceLibrary[key];

            BasePiece newPiece = (BasePiece)newPieceObject.AddComponent(pieceType);
             
            if (key == "K") newPiece.mIsKing = true;
            newPieces.Add(newPiece);

            newPiece.Setup(teamColor, _picedSprite, this);
        }


        return newPieces;
    }

    protected BasePiece CreateKing()
    {
        Sprite _picedSprite = Resources.Load<Sprite>("KingFigure");

        GameObject newPieceObject = Instantiate(mPiecePrefab);
        newPieceObject.transform.SetParent(transform);

        newPieceObject.transform.localScale = new Vector3(1, 1, 1);
        newPieceObject.transform.localRotation = Quaternion.identity;

        Type pieceType = mPieceLibrary["K"];

        BasePiece newKing = (BasePiece)newPieceObject.AddComponent(pieceType);

        newKing.Setup(Color.white, _picedSprite, this);


        return newKing;
    }

    private void PlaceAttackPiece(List<BasePiece> pieces, Board board)
    {
        int c = 3;
        for (int i = 0; i < 5; i++ )
        {
            pieces[i].Place(board.mAllCells[c, 0], CellOcupState.Attacker);
            c++;
        }

        c = 3;
        for (int i = 5; i < 10; i++)
        {
            pieces[i].Place(board.mAllCells[c, 10], CellOcupState.Attacker);
            c++;
        }

        c = 3;
        for (int i = 10; i < 15; i++)
        {
            pieces[i].Place(board.mAllCells[0, c], CellOcupState.Attacker);
            c++;
        }

        c = 3;
        for (int i = 15; i < 20; i++)
        {
            pieces[i].Place(board.mAllCells[10, c], CellOcupState.Attacker);
            c++;
        }

        pieces[20].Place(board.mAllCells[1, 5], CellOcupState.Attacker);
        pieces[21].Place(board.mAllCells[9, 5], CellOcupState.Attacker);
        pieces[22].Place(board.mAllCells[5, 1], CellOcupState.Attacker);
        pieces[23].Place(board.mAllCells[5, 9], CellOcupState.Attacker);
    }

    private void PlaceDefPiece(List<BasePiece> pieces, Board board)
    {
        pieces[0].Place(board.mAllCells[5, 5], CellOcupState.King);

        int c = 5; int r = 3;

        int itter = 1;
        for (int i = 1; i < 13; i++)
        {
            if (c == 5 & r == 5)
            {
                c++;
            }

            pieces[i].Place(board.mAllCells[c, r], CellOcupState.Defender);
            c++;
            itter++;
            ChangeCell();

        }

        bool ChangeCell()
        {
            switch(r)
            {
                case 3:
                    {
                        if (itter > 1)
                        {
                            c = 4;
                            r++;
                            itter = 1;
                            return true;
                        }
                        else return false;
                    }
                case 4:
                    {
                        if (itter > 3)
                        {
                            c = 3;
                            r++;
                            itter = 1;
                            return true;
                        }
                        else return false;
                    }
                case 5:
                    {
                        if (itter > 4)
                        {
                            c = 4;
                            r++;
                            itter = 1;
                            return true;
                        }
                        else return false;
                    }
                case 6:
                    {
                        if (itter > 3)
                        {
                            c = 5;
                            r++;
                            itter = 1;
                            return true;
                        }
                        else return false;
                    }

            }
            return false;
        }



    }

    public GameState CheckGameStatus(BasePiece piece)
    {
        if (piece.mIsKing)
        {
            if (((King)piece).CheckCell())
            {
                return GameState.GameOver;
            }
        }
        
        return Board.Instance.State;

    }
}


