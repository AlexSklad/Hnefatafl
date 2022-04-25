using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class TutorialPM : PieceManager
{
    public override void Setup(Board board)
    {
        TutorialSetup(Global.TutorialStep, board);
        //base.Setup(board);
    }

    public override void SwitchSide(Color color)
    {
        //isBlackTurn = color == Color.white ? true : false;

        //SetInteractive(mDefPiece, !isBlackTurn);
        //SetInteractive(mAttackPiece, isBlackTurn);
    }

    private void TutorialSetup(int Step, Board b)
    {
        switch (Step)
        {
            case 1:
                {
                    base.mAttackPiece = CreatePieces(Color.black, 1, b, false);
                    mAttackPiece[0].Place(b.mAllCells[8, 5], CellOcupState.Attacker);
                    SwitchSide(Color.white);
                    Global.TutorialStep++;
                    break;
                }
            case 2: 
                {
                    base.mDefPiece = CreatePieces(Color.white, 2, b, true);
                    base.mAttackPiece = CreatePieces(Color.black, 2, b, false);
                    base.mDefPiece.RemoveAt(0);

                    mDefPiece[0].Place(b.mAllCells[2, 3], CellOcupState.Defender);

                    mAttackPiece[0].Place(b.mAllCells[3, 3], CellOcupState.Attacker);
                    mAttackPiece[1].Place(b.mAllCells[1, 7], CellOcupState.Attacker);

                    SwitchSide(Color.white);
                    Global.TutorialStep++;
                    break;
                }
            case 3:
                {
                    base.mDefPiece = CreatePieces(Color.white, 4, b, true);
                    base.mDefPiece.RemoveAt(0);
                    base.mAttackPiece = CreatePieces(Color.black, 4, b, false);

                    mDefPiece[0].Place(b.mAllCells[7, 3], CellOcupState.Defender);
                    mDefPiece[1].Place(b.mAllCells[8, 4], CellOcupState.Defender);
                    mDefPiece[2].Place(b.mAllCells[9, 3], CellOcupState.Defender);

                    mAttackPiece[0].Place(b.mAllCells[6, 3], CellOcupState.Attacker);
                    mAttackPiece[1].Place(b.mAllCells[8, 5], CellOcupState.Attacker);
                    mAttackPiece[2].Place(b.mAllCells[10, 3], CellOcupState.Attacker);
                    mAttackPiece[3].Place(b.mAllCells[8, 1], CellOcupState.Attacker);

                    SwitchSide(Color.white);
                    Global.TutorialStep++;
                    break;
                }
            default:
                {
                    break;
                }

        }
        
    }
}
