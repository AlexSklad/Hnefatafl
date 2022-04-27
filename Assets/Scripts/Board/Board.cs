using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class Board : MonoBehaviour
{
    public static Board Instance { private set; get; }

    [HideInInspector]
    public bool IsKingAlive = true;

    public static event UnityAction<Color> OnGameWon;
    public static event UnityAction<GameState> OnTurnStart;


    public GameState State
    {
        get
        {
            if (IsKingAlive)
            {
                if (PieceManager.WhosTurn() == Color.black)
                {
                    return GameState.AttackingTurn;
                }
                else if (PieceManager.WhosTurn() == Color.white)
                {
                    return GameState.DefendingTurn;
                }
            }
            return GameState.GameOver;
        }
    }

    public enum GameState
    {
        AttackingTurn,
        DefendingTurn,
        GameOver,
    }


    private Sprite ThroneCell ;
    
    private Color mHightlightingCellColor = Color.green;
    private Color mCellDefaultColor;
    
    [HideInInspector]
    public BasePiece mSelectedPiece;


    public GameObject mCellPrefab;

    [HideInInspector]
    public Cell[,] mAllCells = new Cell[11, 11];

    public int boardSize = 11;


    public IEnumerator WaitForLoadScenes(string SceneName, bool isRestart = false)
    {

        if (!SceneManager.GetSceneByName("HUD").isLoaded)
        {
            SceneManager.LoadSceneAsync("HUD", LoadSceneMode.Additive);
        }

        if (!SceneManager.GetSceneByName(SceneName).isLoaded)
        {
            SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
        }

        while (!SceneManager.GetSceneByName("HUD").isLoaded && !SceneManager.GetSceneByName(SceneName).isLoaded)
        {
            yield return null;
        }
        if (!isRestart) Create();
    }
    public void Create()
    {
        Instance = this;
        ThroneCell = Resources.Load<Sprite>("ThroneCell");
        mSelectedPiece = null;

        for (int y = 0; y < 11; y++)
        {
            for (int x = 0; x < 11; x++)
            {
                GameObject newCell = Instantiate(mCellPrefab, transform);

                RectTransform rectTransform = newCell.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2((x * 90) + 15, (y * 90) + 15);

                mAllCells[x, y] = newCell.GetComponent<Cell>();
                mAllCells[x, y].Setup(new Vector2Int(x, y), this);
                mAllCells[x, y].CellState = CellState.Normal;
            }
        }

        #region Color

        mAllCells[0, 0].GetComponent<Image>().sprite = ThroneCell;
        mAllCells[0, 0].CellState = CellState.Shelter;

        mAllCells[10, 0].GetComponent<Image>().sprite = ThroneCell;
        mAllCells[10, 0].CellState = CellState.Shelter;

        mAllCells[0, 10].GetComponent<Image>().sprite = ThroneCell;
        mAllCells[0, 10].CellState = CellState.Shelter;

        mAllCells[10, 10].GetComponent<Image>().sprite = ThroneCell;
        mAllCells[10, 10].CellState = CellState.Shelter;

        mAllCells[5, 5].GetComponent<Image>().sprite = ThroneCell;
        mAllCells[5, 5].CellState = CellState.Throne;

        mCellDefaultColor = mAllCells[0, 0].GetComponent<Image>().color;

        #endregion
    }

    public CellState ValidateCell(int targetX, int targetY, CellOcupState ocupState)
    {
        if (targetX < 0 || targetX > 10)
            return CellState.OutOfBounds;

        if (targetY < 0 || targetY > 10)
            return CellState.OutOfBounds;

        Cell targetCell = mAllCells[targetX, targetY];

        if (targetCell.mCurrentPiece != null)
        {

            if (targetCell.OcupState == ocupState || (targetCell.OcupState == CellOcupState.King && ocupState == CellOcupState.Defender) ||
                (targetCell.OcupState == CellOcupState.Defender && ocupState == CellOcupState.King))
                return CellState.Friendly;
            else
            {
                return CellState.Enemy;
            }
        }

        return targetCell.CellState;
    }

    public CellOcupState ValidatePiece(Cell cell)
    {
        return cell.OcupState;
    }

    public bool IsFreeCell(int x, int y)
    {
        if (this.mAllCells[x, y].OcupState == CellOcupState.Empty) 
            return true;
        else
            return false;
    }


    public void StartHighlighted(List<Cell> cells)
    {
        foreach (var c in cells)
        {
            c.GetComponent<Image>().color = mHightlightingCellColor;
        }
    }

    public void EndHighlighted(List<Cell> cells)
    {
        foreach (var c in cells)
        {
            c.GetComponent<Image>().color = mCellDefaultColor;
        }
    }

    public void KillAllPiece()
    {
        foreach (Cell c in mAllCells)
        {
            if(c.mCurrentPiece) c.mCurrentPiece.Kill();
            c.OcupState = CellOcupState.Empty;
        }
    }

    public void EndGame(Color won)
    {
        OnGameWon.Invoke(won);
        StopAllCoroutines();
    }

    public virtual void SwitchTurnText()
    {
        OnTurnStart.Invoke(State);
    }

}