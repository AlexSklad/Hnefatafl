using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameLimiter : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = -1;
    }
}
