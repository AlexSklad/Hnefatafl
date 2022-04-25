using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regular : BasePiece
{
    public void CheckNextPiece(int x, int y, bool onX, int direction)
    {
        if (onX)
        {
            int xDir = x + direction;
            if (CheckX(direction, x, y) == CellState.Enemy)
            {
                Global.mToRemovedPices.Add(this.mCurrentCell);
                return;
            }
            else
            {
                if (CheckX(direction, x, y) == CellState.Throne && mCurrentCell.mBoard.IsFreeCell(xDir, y))
                {
                    Global.mToRemovedPices.Add(this.mCurrentCell);
                    return;
                }
            }
        }
        else
        {
            int yDir = y + direction;
            if (CheckY(direction, x, y) == CellState.Enemy)
            {
                Global.mToRemovedPices.Add(this.mCurrentCell);
                return;
            }
            else
            {
                if (CheckY(direction, x, y) == CellState.Throne && mCurrentCell.mBoard.IsFreeCell(x, yDir))
                {
                    Global.mToRemovedPices.Add(this.mCurrentCell);
                    return;
                }
            }
            
        }
    }
}
