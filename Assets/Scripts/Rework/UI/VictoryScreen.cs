using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VictoryScreen : UIPage
{
    [SerializeField]
    private TMPro.TMP_Text _scoreText;

    public override void Open()
    {
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }

    public void Restart()
    {
        Close();

        Game.Instance.Restart();
    }

    public void MainMenu()
    {
        Close();
        
        Game.Instance.StopGame();
    }
}
