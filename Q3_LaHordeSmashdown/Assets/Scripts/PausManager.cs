using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausManager : MonoBehaviour
{
    public static PausManager instance;
    public Canvas _pausCanvas;
    public Animator _pausAnimator;

    public bool _paused;

    private void Awake()
    {
        instance = this;
        _paused = false;
        _pausCanvas.gameObject.SetActive(false);
    }

    public void PausResumaGame()
    {
        _paused = !_paused;

        foreach (var player in PlayerManager.instance._playerList)
        {
            player.GetComponent<PlayerAttack>()._isPause = _paused;
        }

        if (_paused == true)
        {
            Time.timeScale = 0;
            _pausCanvas.gameObject.SetActive(_paused);
            _pausAnimator.Play("PausMenu");
        }
        else if (_paused == false)
        {
            StartCoroutine(WaitToUnableCanvasPausMenu());
        }
    }

    IEnumerator WaitToUnableCanvasPausMenu()
    {
        _pausAnimator.Play("UnPausMenu");
        Time.timeScale = 1;
        yield return new WaitForSeconds(1f);
        _pausCanvas.gameObject.SetActive(_paused);
    }
    
    public void MainMenu(string mainMenuSceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
