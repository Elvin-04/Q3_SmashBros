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
        _pausAnimator.SetBool("UnPaus", false);
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
            _pausAnimator.SetBool("UnPaus", false);
        }
        else if (_paused == false)
        {
            _pausAnimator.SetBool("UnPaus", true);
            StartCoroutine(WaitToUnableCanvasPausMenu());
        }
    }

    IEnumerator WaitToUnableCanvasPausMenu()
    {
        Time.timeScale = 1;
        yield return new WaitForSeconds(1f);
        _pausCanvas.gameObject.SetActive(_paused);
        _pausAnimator.SetBool("UnPaus", false);
    }
    
    public void MainMenu(string mainMenuSceneName)
    {
        if (_paused)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}
