using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Assets.Scripts;
using System;
using static Board;

public abstract class BasePiece : EventTrigger
{
    [HideInInspector]
    public Color mColor = Color.clear;

    protected Cell mCurrentCell = null;
    
    [HideInInspector]
    public Cell mMovedCell = null;

    private Color mCheckedColor = Color.red;
    private Color mFigureColor;

    protected RectTransform mRectTransform = null;

    [HideInInspector]
    public PieceManager mPieceManager;

    protected Cell mTargetCell = null;

    [HideInInspector]
    public bool mIsKing = false;

    public virtual void Setup(Color newTeamSprite, Sprite newSprite, PieceManager newPieceManager)
    {
        mPieceManager = newPieceManager;

        mColor = newTeamSprite;
        GetComponent<Image>().sprite = newSprite;
        mRectTransform = GetComponent<RectTransform>();
    }

    public void Place(Cell newCell, CellOcupState ocupState)
    {
        mCurrentCell = newCell;
        mCurrentCell.mCurrentPiece = this;
        mCurrentCell.OcupState = ocupState;
        mFigureColor = this.GetComponent<Image>().color;

        transform.position = newCell.transform.position;
        gameObject.SetActive(true);
    }

    public virtual void Kill()
    {
        mCurrentCell.mCurrentPiece = null;
        Destroy(gameObject);
        //gameObject.SetActive(false);
    }

    private void UnselectPiece(Board b, BasePiece p)
    {
        p.GetComponent<Image>().color = mFigureColor;
        b.EndHighlighted(Global.mHiglightedCells);
        Global.mHiglightedCells.Clear();

    }

    private void SelectPiece(Board b)
    {
        if (b.mSelectedPiece && mMovedCell) return;

        if (b.mSelectedPiece == null)
        {
            b.mSelectedPiece = mCurrentCell.mCurrentPiece;
        }
        else
        {
            if (b.mSelectedPiece == this)
            {
                b.mSelectedPiece = null;
                UnselectPiece(b, mCurrentCell.mCurrentPiece);
            }
            else
            {
                if (this.enabled)
                {
                    UnselectPiece(b, b.mSelectedPiece);
                    b.mSelectedPiece = mCurrentCell.mCurrentPiece;
                }
            }
        }

    }

    #region Movement

    public void CreateCellPatch()
    {
        CreateCellPathX(1);
        CreateCellPathX(-1);
        CreateCellPathY(1);
        CreateCellPathY(-1);

    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        Board b = mCurrentCell.mBoard;

        SelectPiece(b);

        if (b.mSelectedPiece != null)
        {
            mCurrentCell.mCurrentPiece.GetComponent<Image>().color = mCheckedColor;

            CreateCellPatch();

            b.StartHighlighted(Global.mHiglightedCells);
        }

        if (Global.mHiglightedCells.Count > 0)
        {
            foreach (Cell c in Global.mHiglightedCells)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(c.mRectTransform, Input.mousePosition))
                {
                    mTargetCell = c;
                    break;
                }
                mTargetCell = null;
            }
        }

        if (mTargetCell)
        {
            Move();
            UnselectPiece(b, b.mSelectedPiece);
            mPieceManager.SwitchSide(b.mSelectedPiece.mColor);
            Board.Instance.SwitchTurnText();

            b.mSelectedPiece = null;
        }
    }

    private void CreateCellPathY(int yDirection)
    {
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;
        Board board = mCurrentCell.mBoard;

        do
        {
            currentY += yDirection;

            CellState _cellstate = mCurrentCell.mBoard.ValidateCell(currentX, currentY, this.mCurrentCell.OcupState);
            if (_cellstate == CellState.OutOfBounds) break;

            CellOcupState _cellocupstate = board.ValidatePiece(board.mAllCells[currentX, currentY]);

            if (_cellstate == CellState.Shelter && !this.mIsKing) break;

            if (_cellstate == CellState.Throne && !this.mIsKing) continue;

            if (_cellocupstate != CellOcupState.Empty) break;



            Global.mHiglightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);


        } while (currentY > 0 || currentY < board.boardSize);
    }

    private void CreateCellPathX(int xDirection)
    {
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;
        Board board = mCurrentCell.mBoard;

        do
        {
            currentX += xDirection;

            CellState _cellstate;
            _cellstate = board.ValidateCell(currentX, currentY, this.mCurrentCell.OcupState);
            if (_cellstate == CellState.OutOfBounds) break;
            CellOcupState _cellocupstate = board.ValidatePiece(board.mAllCells[currentX, currentY]);

            if (_cellstate == CellState.Shelter && !this.mIsKing) break;

            if (_cellocupstate != CellOcupState.Empty) break;

            if (_cellstate == CellState.Throne && !this.mIsKing) continue;

            Global.mHiglightedCells.Add(board.mAllCells[currentX, currentY]);

        } while (currentX > 0 || currentX < board.boardSize);
    }

    protected virtual void Move()
    {
        Debug.Log($"Cell{mTargetCell.mBoardPosition.x}, {mTargetCell.mBoardPosition.y}, OcupState: {mTargetCell.OcupState}");
        mTargetCell.OcupState = mCurrentCell.OcupState;
        mTargetCell.RemovePiece();
        mCurrentCell.mCurrentPiece = null;
        mCurrentCell.OcupState = CellOcupState.Empty;
        mCurrentCell = mTargetCell;
        mCurrentCell.mCurrentPiece = this;
        mCurrentCell.mCurrentPiece.mMovedCell = null;

        transform.position = mCurrentCell.transform.position;
        mTargetCell = null;

        CheckEnemiesAround();

        if (Global.mToRemovedPices.Count > 0)
        {
            foreach (Cell c in Global.mToRemovedPices)
            {
                c.RemovePiece();
            }
        }
        Global.mToRemovedPices.Clear();

        if (mPieceManager.CheckGameStatus(this) == Board.GameState.GameOver)
        {
            Board.Instance.EndGame(this.mColor);
        }
        Debug.Log($"Cell{mCurrentCell.mBoardPosition.x}, {mCurrentCell.mBoardPosition.y}, OcupState: {mCurrentCell.OcupState}");
    }

    #endregion

    #region CheckEnemies

    protected CellState CheckX(int directionX, int x, int y)
    {
        return mCurrentCell.mBoard.ValidateCell(x + directionX, y, this.mCurrentCell.OcupState);
    }
    protected CellState CheckY(int directionY, int x, int y)
    {
        return mCurrentCell.mBoard.ValidateCell(x, y + directionY, this.mCurrentCell.OcupState);
    }

    private void CheckEnemiesAround()
    {
        int x = mCurrentCell.mBoardPosition.x;
        int y = mCurrentCell.mBoardPosition.y;

        if (CheckX(1, x, y) == CellState.Enemy)
        {
            if (!mCurrentCell.mBoard.mAllCells[x + 1, y].mCurrentPiece.mIsKing)
            {
                Regular p = (Regular)mCurrentCell.mBoard.mAllCells[x + 1, y].mCurrentPiece;
                p.CheckNextPiece(x + 1, y, true, 1);
            }
            else
            {
                King p = (King)mCurrentCell.mBoard.mAllCells[x + 1, y].mCurrentPiece;
                p.CheckEnemyAround(p.mCurrentCell.mBoardPosition.x, p.mCurrentCell.mBoardPosition.y);
            }
        }
        if (CheckX(-1, x, y) == CellState.Enemy)
        {
            if (!mCurrentCell.mBoard.mAllCells[x - 1, y].mCurrentPiece.mIsKing)
            {
                Regular p = (Regular)mCurrentCell.mBoard.mAllCells[x - 1, y].mCurrentPiece;
                p.CheckNextPiece(x - 1, y, true, -1);
            }
            else
            {
                King p = (King)mCurrentCell.mBoard.mAllCells[x - 1, y].mCurrentPiece;
                p.CheckEnemyAround(p.mCurrentCell.mBoardPosition.x, p.mCurrentCell.mBoardPosition.y);
            }
        }

        if (CheckY(1, x, y) == CellState.Enemy)
        {
            if (!mCurrentCell.mBoard.mAllCells[x, y + 1].mCurrentPiece.mIsKing)
            {
                Regular p = (Regular)mCurrentCell.mBoard.mAllCells[x, y + 1].mCurrentPiece;
                p.CheckNextPiece(x, y + 1, false, 1);
            }
            else
            {
                King p = (King)mCurrentCell.mBoard.mAllCells[x, y + 1].mCurrentPiece;
                p.CheckEnemyAround(p.mCurrentCell.mBoardPosition.x, p.mCurrentCell.mBoardPosition.y);
            }

        }
        if (CheckY(-1, x, y) == CellState.Enemy)
        {
            if (!mCurrentCell.mBoard.mAllCells[x, y - 1].mCurrentPiece.mIsKing)
            {
                Regular p = (Regular)mCurrentCell.mBoard.mAllCells[x, y - 1].mCurrentPiece;
                p.CheckNextPiece(x, y - 1, false, -1);
            }
            else
            {
                King p = (King)mCurrentCell.mBoard.mAllCells[x, y - 1].mCurrentPiece;
                p.CheckEnemyAround(p.mCurrentCell.mBoardPosition.x, p.mCurrentCell.mBoardPosition.y);
            }

        }

    }

    #endregion
}
