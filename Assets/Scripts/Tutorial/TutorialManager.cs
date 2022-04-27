using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

public class TutorialManager : MonoBehaviour
{
    public TutorialBoard mBoard;
    public static TutorialManager Instance { private set; get; }
    public TutorialPM mTutorialPM;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        mBoard.Create();
        //StartCoroutine(mBoard.WaitForLoadScenes("TutorialScene"));

        mTutorialPM.Setup(mBoard);

    }

    // Update is called once per frame
    public void UpdateScene()
    {
        mBoard.KillAllPiece();
        mTutorialPM.Setup(mBoard);
    }
}
