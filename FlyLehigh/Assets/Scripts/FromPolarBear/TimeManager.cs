using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static bool GamePaused = false;

    public void PauseTime()
    {
        GamePaused = true;
    }

    public void ResumeTime()
    {
        GamePaused = false;
    }

    public bool CheckPaused()
    {
        return GamePaused;
    }
}
