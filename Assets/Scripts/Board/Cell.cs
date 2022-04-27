using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum CellState
{
    Friendly,
    Enemy,
    OutOfBounds,
    Normal,
    Throne,
    Shelter
}

public enum CellOcupState
{
    Attacker,
    Defender,
    King,
    Empty
}
public class Cell : EventTrigger
{
    [HideInInspector]
    public Vector2Int mBoardPosition = Vector2Int.zero;
    [HideInInspector]
    public Board mBoard = null;
    [HideInInspector]
    public RectTransform mRectTransform = null;
    [HideInInspector]
    public BasePiece mCurrentPiece = null;

    private CellState _CellState;
    public CellState CellState
    {
        get
        {
            return _CellState;
        }
        set
        {
            _CellState = value;
        }
    }
    private CellOcupState _OcupState;
    public CellOcupState OcupState
    {
        get
        {
            return _OcupState;
        }
        set
        {
            _OcupState = value;
        }
    }

    public void Setup (Vector2Int newBoardPos, Board newBoard)
    {
        mBoardPosition = newBoardPos;
        mBoard = newBoard;
        OcupState = CellOcupState.Empty;

        mRectTransform = GetComponent<RectTransform>();
    }

    public void RemovePiece()
    {
        if(mCurrentPiece != null) 
        {
            mCurrentPiece.mPieceManager.mDefPiece.Remove(mCurrentPiece);
            mCurrentPiece.Kill();
            this.OcupState = CellOcupState.Empty;
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (mBoard.mSelectedPiece)
        {
            mBoard.mSelectedPiece.mMovedCell = this;
            mBoard.mSelectedPiece.OnPointerClick(eventData);

        }
    }


}
