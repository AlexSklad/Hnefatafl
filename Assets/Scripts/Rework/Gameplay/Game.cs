using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Game : MonoBehaviour
{
    [SerializeField]
    private Player _player;
    public enum GameState
    {
        Playing,
        Paused,
        EndGame
    }
    #region GameAction
    public Action OnStateChanged;
    public Action OnVictory;
    public Action OnLoss;
    public Action OnGameStop;
    #endregion
    private static Game _instance;

    public static Game Instance
    {
        get 
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Game>(true);
            }

            return _instance;
        }
    }

    private GameState _currentState = GameState.EndGame;
    public GameState CurrentState
    {
        get { return _currentState; }
        private set
        {
            _currentState = value;
            OnStateChanged?.Invoke();
        }
    }

    public Player ActivePlayer => _player;

    public LevelHeader CurrentLevel { get; private set; }

    public void StartGame(LevelHeader level)
    {
        CurrentLevel = level;
        LevelManager.OpenLevel(CurrentLevel.LevelName);
        Time.timeScale = 1f;
        ActivePlayer.Reset();

        CurrentState = GameState.Playing;
    }

    public void StopGame()
    {
        LevelManager.UnloadLevel();
        Time.timeScale = 1f;
        CurrentState = GameState.EndGame;

        OnGameStop?.Invoke();
    }

    public void Pause()
    {
        if (CurrentState !=GameState.Playing)
        {
            return;
        }
        Time.timeScale = 0f;
        CurrentState = GameState.Paused;
    }

    public void Resume()
    {
        if (CurrentState != GameState.Paused)
        {
            return;
        }
        Time.timeScale = 1f;
        CurrentState = GameState.Playing;
    }

    public void Restart()
    {
        StartGame(CurrentLevel);
    }

    public void Win()
    {
        CurrentState = GameState.EndGame;
        OnVictory?.Invoke();
    }

    public void Loss()
    {
        CurrentState = GameState.EndGame;
        OnLoss?.Invoke();
    }

    public void CheckWinCondition()
    {

    }

    //public void CheckLossCondition()
    //{
    //    if (_player.LastShotBubble.CollisionLayer == Layer.LastShotBubble && )
    //}
}
