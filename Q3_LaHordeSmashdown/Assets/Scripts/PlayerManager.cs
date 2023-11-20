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
    public int _countPlayerPseudo;

    private bool _gameBegin;

    public CameraMovement camMove;
    public List<Color> playerColors;
    public GameObject _redFire;
    public GameObject _blueFire;
    public GameObject _yellowFire;
    public GameObject _greenFire;

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
                    if (player.GetComponent<PlayerAttack>()._dead == false)
                    {
                        PlayerPrefs.SetFloat("WinRed",player.GetComponent<PlayerMovements>().normaColor.r);
                        PlayerPrefs.SetFloat("WinBlue",player.GetComponent<PlayerMovements>().normaColor.b);
                        PlayerPrefs.SetFloat("WinGreen",player.GetComponent<PlayerMovements>().normaColor.g);
                        PlayerPrefs.SetString("WinName",player.GetComponent<PlayerAttack>()._name);
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
        player.GetComponent<PlayerMovements>().normaColor = playerColors[_playerList.Count];
        player.GetComponent<PlayerAttack>()._map = _map;
        _playerList.Add(player);
        if (_playerList.Count == 1)
        {
            player.GetComponent<PlayerAttack>().SetEjectEffect(_blueFire);
        }
        else if (_playerList.Count == 2)
        {
            player.GetComponent<PlayerAttack>().SetEjectEffect(_redFire);
        }
        else if (_playerList.Count == 3)
        {
            player.GetComponent<PlayerAttack>().SetEjectEffect(_greenFire);
        }
        else
        {
            player.GetComponent<PlayerAttack>().SetEjectEffect(_yellowFire);
        }
        camMove.players.Add(player.transform);
        UIManagement.instance.AddPlayerUI(player.GetComponent<PlayerAttack>());
    }

    public void CheckAllPseudo()
    {
        foreach (var player in _playerList)
        {
            if (player.GetComponent<PlayerAttack>()._pseudoEntree == true)
            {
                _countPlayerPseudo++;
            }
        }

        if (_countPlayerPseudo == _playerList.Count)
        {
            _countPlayerPseudo = 0;
            Time.timeScale = 1;
        }
        else
        {
            _countPlayerPseudo = 0;
        }
    }

    public void RemovePlayer(GameObject player)
    {
        _playerList.Remove(player);
        camMove.players.Remove(player.transform);
    }
}
