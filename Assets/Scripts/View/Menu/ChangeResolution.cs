using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeResolution : MonoBehaviour
{
    public Dropdown _dropdown;
    public void Change()
    {
        if (_dropdown.value == 0)
        {
            Screen.SetResolution(1024, 768, true);
        }
        else if (_dropdown.value == 1)
        {
            Screen.SetResolution(1280, 1024, true);

        }
        else if (_dropdown.value == 2)
        {
            Screen.SetResolution(1366, 768, true);

        }
        else if (_dropdown.value == 3)
        {
            Screen.SetResolution(11600, 900, true);

        }
        else if (_dropdown.value == 4)
        {
            Screen.SetResolution(1920, 1080, true);

        }

    }
}
