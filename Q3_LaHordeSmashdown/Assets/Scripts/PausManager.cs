using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausManager : MonoBehaviour
{
    public static PausManager instance;
    public Canvas _pausCanvas;

    private bool _paused;

    private void Start()
    {
        instance = this;
        _paused = false;
        _pausCanvas.gameObject.SetActive(false);
    }

    public void PausResumaGame()
    {
        _paused = !_paused;
        _pausCanvas.gameObject.SetActive(_paused);

        foreach (var player in PlayerManager.instance._playerList)
        {
            player.GetComponent<PlayerAttack>()._isPause = _paused;
        }

        if (_pausCanvas.gameObject.activeSelf == true)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void MainMenu(string mainMenuSceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
