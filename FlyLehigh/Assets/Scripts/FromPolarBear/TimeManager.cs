using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static bool GamePaused = false;

    public float TimeSpentFlying = 0f;

    private void Update()
    {
        if (GamePaused == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
            TimeSpentFlying += Time.deltaTime;
        }
        else {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }
    }

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
