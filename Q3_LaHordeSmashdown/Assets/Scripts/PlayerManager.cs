using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public List<GameObject> _playerList;
    public GameObject _map;
    public int _playerAlive;
    public string _endSceneName;

    private bool _gameBegin;

    public CameraMovement camMove;




    void Start()
    {
        instance = this;
        _playerList = new List<GameObject>();
        _gameBegin = false;
        _playerAlive = 0;
    }

    private void Update()
    {
        if (_playerList.Count > 1)
        {
            _gameBegin = true;
        }
        else if(_playerList.Count == 1)
        {
            _playerList[0].GetComponent<PlayerAttack>().ResetStat();
        }

        if (_gameBegin)
        {
            if (_playerAlive == 1)
            {
                foreach (var player in _playerList)
                {
                    if (player.activeSelf == true)
                    {
                        PlayerPrefs.SetFloat("WinRed",player.GetComponent<PlayerAttack>()._colorPlayer.r);
                        PlayerPrefs.SetFloat("WinBlue",player.GetComponent<PlayerAttack>()._colorPlayer.b);
                        PlayerPrefs.SetFloat("WinGreen",player.GetComponent<PlayerAttack>()._colorPlayer.g);
                        SceneManager.LoadScene(_endSceneName);
                    }
                }
            }
        }
    }

    public void SpawningPlayer()
    {
        foreach (var player in _playerList)
        {
            player.GetComponent<PlayerAttack>().ResetStat();
        }

        _playerAlive++;
    }

    public void AddPlayer(GameObject player)
    {
        _playerList.Add(player);
        camMove.players.Add(player.transform);
        UIManagement.instance.AddPlayerUI(player.GetComponent<PlayerAttack>());
    }
}
