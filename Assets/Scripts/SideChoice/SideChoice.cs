using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SideChoice : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    public void DefSideChoiced(string SceneName)
    {
        //Global._AIIsAttaker = true;
        SceneManager.LoadScene(SceneName);
    }
    public void AttakSideChoiced(string SceneName)
    {
        //Global._AIIsAttaker = false;
        SceneManager.LoadScene(SceneName);
    }
}
