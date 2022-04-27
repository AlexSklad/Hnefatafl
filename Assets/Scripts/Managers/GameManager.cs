using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Board mBoard;
   
    [HideInInspector]
    public static GameManager Instance { private set; get; }
    public PieceManager mPieceManager;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        StartCoroutine(mBoard.WaitForLoadScenes("GameScene"));

        mPieceManager.Setup(mBoard);
    }

    public void ResetGame()

    {
        mBoard.KillAllPiece();
        StopAllCoroutines();
        StartCoroutine(mBoard.WaitForLoadScenes("GameScene", true));
        mPieceManager.Setup(mBoard);
        mBoard.IsKingAlive = true;

    }

}
