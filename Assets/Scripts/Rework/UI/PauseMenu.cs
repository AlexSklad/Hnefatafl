using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PauseMenu : UIPage
{
    [SerializeField]
    private InGameUI _ingame;
    [SerializeField]
    private MainMenu _mainMenu;

    public override void Open()
    {
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }

    public void Resume()
    {
        Game.Instance.Resume();
    }

    public void Restart()
    {
        Game.Instance.Restart();
    }

    public void MainMenu()
    {
        Game.Instance.StopGame();
    }
}
