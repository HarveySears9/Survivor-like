using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void Pause()
    {
        Time.timeScale = 0f;
        this.gameObject.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        this.gameObject.SetActive(false);
    }
}
