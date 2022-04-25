using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public TMP_Text resetButton, quitButton;
    public TMP_Text turnText, winText;

    public static event Action OnGameQuit;
    public static event Action OnGameReset;

    private void Start()
    {
        // Add the GameOver method to the OnGameOver event call
        Board.OnGameWon += GameOver;
        // Add the turn indicator
        Board.OnTurnStart += UpdatePlayerTurnDisplay;
        // Add the reset game method to the event
        OnGameReset += ResetHud;
        OnGameReset += GameManager.Instance.ResetGame;
        // Add the quit to menu method to the event
        OnGameQuit += QuitToMenu;

        // Set up buttons
        SetButton(resetButton, "RESET", Color.white, Color.black, Color.gray, Color.black);
        SetButton(quitButton, "X", Color.white, Color.black, Color.gray, Color.black);
        // Add listeners
        resetButton.GetComponent<Button>().onClick.AddListener(() => OnResetGameClicked());
        quitButton.GetComponent<Button>().onClick.AddListener(() => OnQuitToMenuClicked());

        // Hide the win text
        winText.gameObject.SetActive(false);
    }

    public void ResetHud()
    {
        //UpdatePlayerTurnDisplay(Board.Instance.State);
        UnloadHUD();
    }

    public void OnQuitToMenuClicked()
    {
        OnGameQuit.Invoke();
    }

    public void OnResetGameClicked()
    {
        OnGameReset.Invoke();
    }


    //private void QuitGame()
    //{
    //    Application.Quit();
    //}


    private void QuitToMenu()
    {
        UnloadHUD();

        SceneManager.LoadScene(0);
    }

    private void OnDestroy()
    {
        UnloadHUD();
    }

    public void UnloadHUD()
    {
        if (SceneManager.GetSceneByName("HUD").isLoaded)
        {
            // Remove all event calls
            Board.OnGameWon -= GameOver;
            Board.OnTurnStart -= UpdatePlayerTurnDisplay;
            OnGameReset -= ResetHud;
            OnGameReset -= GameManager.Instance.ResetGame;
            OnGameQuit -= QuitToMenu;
            resetButton.GetComponent<Button>().onClick.RemoveAllListeners();
            quitButton.GetComponent<Button>().onClick.RemoveAllListeners();

            // Unload this scene
            SceneManager.UnloadSceneAsync("HUD");
        }
    }

    private void SetButton(TMP_Text t, string message, Color colour, Color outline, Color hover, Color pressed)
    {
        SetText(t, message, colour, outline);

        Button b = t.GetComponent<Button>();
        if (b)
        {
            // Set the colours here
            ColorBlock c = ColorBlock.defaultColorBlock;
            c.normalColor = colour;
            c.highlightedColor = hover;
            c.pressedColor = pressed;
            b.colors = c;
        }
    }

    private void SetText(TMP_Text t, String message, Color c, Color outline)
    {
        // Set the properties and display
        t.text = message;
        t.color = c;
        t.outlineWidth = 0.25f;
        t.outlineColor = outline;
        t.enabled = true;
    }


    private void UpdatePlayerTurnDisplay(Board.GameState state)
    {
        string team;
        Color colour, outline;
        if (state == Board.GameState.AttackingTurn)
        {
            team = "Черных";
            colour = Color.black;
            outline = Color.white;
        }
        else if(state == Board.GameState.DefendingTurn)
        {
            team = "Белых";
            colour = Color.white;
            outline = Color.black;
        }
        else
        {
            return;
        }

        SetText(turnText, "ход " + team, colour, outline);
    }

    private void GameOver(Color team)
    {
        int waitForSeconds = 4;

        Debug.Log("OnGameOver called with team " + team);
        if (team.Equals(Color.black))
        {
            StartCoroutine(DisplayWon("Черные", Color.black, Color.white, waitForSeconds));
        }
        else
        {
            StartCoroutine(DisplayWon("Белые", Color.white, Color.black, waitForSeconds));
        }
    }

    private IEnumerator DisplayWon(string team, Color colour, Color outline, int seconds)
    {
        SetText(winText, team + " выиграли!", colour, outline);
        winText.gameObject.SetActive(true);

        // Wait seconds seconds then disable the text
        yield return new WaitForSeconds(seconds);
    }

}
