using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;


public class King : BasePiece
{
    public bool CheckCell()
    {
        if (this.mCurrentCell.CellState == CellState.Shelter) 
            return true;
        else
            return false;
    }

    public void CheckEnemyAround(int x, int y)
    {
        CellState cs = CheckX(1, x, y);
        if (cs == CellState.Throne) cs = CellState.Enemy;
        if (cs != CellState.Enemy) return;

        cs = CheckX(-1, x, y);
        if (cs == CellState.Throne) cs = CellState.Enemy;
        if (cs != CellState.Enemy) return;

        cs = CheckY(1, x, y);
        if (cs == CellState.Throne) cs = CellState.Enemy;
        if (cs != CellState.Enemy) return;

        cs = CheckY(-1, x, y);
        if (cs == CellState.Throne) cs = CellState.Enemy;
        if (cs != CellState.Enemy) return;

        Global.mToRemovedPices.Add(this.mCurrentCell);
        this.mCurrentCell.mBoard.IsKingAlive = false;
    }

}
