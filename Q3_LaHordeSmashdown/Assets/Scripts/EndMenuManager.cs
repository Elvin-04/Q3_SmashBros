using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenuManager : MonoBehaviour
{
    public TextMeshProUGUI _WinnerName;
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

        _WinnerName.color = new Color(r, g, b);
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
