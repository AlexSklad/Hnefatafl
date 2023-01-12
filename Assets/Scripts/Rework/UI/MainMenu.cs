using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : UIPage
{
    [SerializeField]
    private LevelSelectMenu _levelSelect;

    public override void Open()
    {
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }

    public void StartGame()
    {
        Close();

        _levelSelect.Open();
    }
}
