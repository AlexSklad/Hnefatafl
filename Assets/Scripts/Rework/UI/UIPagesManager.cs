using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPagesManager : MonoBehaviour
{
    [SerializeField]
    private MainMenu _mainMenu;
    [SerializeField]
    private LevelSelectMenu _levelSelect;
    [SerializeField]
    private InGameUI _inGame;
    [SerializeField]
    private PauseMenu _pauseMenu;
    [SerializeField]
    private VictoryScreen _victoryScreen;


    private void ProcessGameState()
    {
        switch (Game.Instance.CurrentState)
        {
            case Game.GameState.Playing:
                {
                    _inGame.Open();
                    _pauseMenu.Close();
                    break;
                }
            case Game.GameState.Paused:
                {
                    _inGame.Close();
                    _pauseMenu.Open();
                    break;
                }
            default:
                {
                    _inGame.Close();
                    _pauseMenu.Close();
                    break;
                }
        }
    }

    private void ShowWinScreen()
    {
        _victoryScreen.Open();
    }

    private void ReturnToMainMenu()
    {
        _mainMenu.Open();
    }

    private void Start()
    {
        Game.Instance.OnStateChanged += ProcessGameState;
        Game.Instance.OnVictory += ShowWinScreen;
        Game.Inctance.OnGameStop += ReturnToMainMenu;
    }
}
