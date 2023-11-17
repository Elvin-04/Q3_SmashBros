using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    public GameObject optionPanel;

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenOptionsPanel()
    {
        optionPanel.SetActive(true);
    }

    public void CloseOptionPanel()
    {
        optionPanel.SetActive(false);
    }
}
