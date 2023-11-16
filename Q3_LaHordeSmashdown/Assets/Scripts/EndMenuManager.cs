using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenuManager : MonoBehaviour
{
    public TextMeshProUGUI _winnerName;
    public string _newGameScene;
    public string _mainMenu;

    private float r;
    private float g;
    private float b;

    private void Start()
    {
        r = PlayerPrefs.GetFloat("WinRed"); 
        b = PlayerPrefs.GetFloat("WinBlue"); 
        g = PlayerPrefs.GetFloat("WinGreen"); 

        _winnerName.color = new Color(r, g, b);
        _winnerName.text = PlayerPrefs.GetString("WinName");
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(_newGameScene);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(_mainMenu);
    }
}
