using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectMenu : UIPage
{
    [SerializeField]
    private MainMenu _mainMenu;
    [SerializeField]
    private GameObject _levelButtonPrefab;
    [SerializeField]
    private RectTransform _levelListParent;

    public override void Open()
    {
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }

    public void GoBack()
    {
        Close();
        _mainMenu.Open();
    }

    void Start()
    {
        var levels = LevelManager.Levels;

        for (int i = 0; i < levels.Length; i++)
        {
            var levelHeader = levels[i];

            var button = Instantiate(_levelButtonPrefab, _levelListParent);

            button.GetComponentInChildren<TMPro.TMP_Text>().text = levelHeader.PublicName;

            button.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                Close();
                Game.Instance.StartGame(levelHeader);
            });
        }
    }

}
