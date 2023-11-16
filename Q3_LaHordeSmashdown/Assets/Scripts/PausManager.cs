using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausManager : MonoBehaviour
{
    public static PausManager instance;
    public Canvas _pausCanvas;
    public Animator _pausAnimator;

    private bool _paused;

    private void Start()
    {
        instance = this;
        _paused = false;
        _pausCanvas.gameObject.SetActive(false);
        _pausAnimator.SetBool("UnPaus", false);
    }

    public void PausResumaGame()
    {
        _paused = !_paused;
        _pausAnimator.SetBool("UnPaus", _paused);
        StartCoroutine(WaitSecond(1f));
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

    IEnumerator WaitSecond(float second)
    {
        yield return new WaitForSeconds(second);
    }

    public void MainMenu(string mainMenuSceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
