using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject pauseMenuUI;

    void Start()
    {
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    public void Pause()
    {
        if (isPaused)
        {
            Time.timeScale = 1f;
            Debug.Log("game sedang berjalan");
            pauseMenuUI.SetActive(false);
            isPaused = false;
        }
        else
        {
            Time.timeScale = 0f;
            Debug.Log("game sedang di pause");
            pauseMenuUI.SetActive(true);
            isPaused = true;
        }
    }

    public void Quit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}


