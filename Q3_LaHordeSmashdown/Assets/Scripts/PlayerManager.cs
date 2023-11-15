using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public List<GameObject> _playerList;
    public GameObject _map;
    public int _playerAlive;

    private bool _gameBegin;

    public List<Color> playerColors;

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
                    player.SetActive(true);
                    player.GetComponent<PlayerAttack>().ResetStat();
                }
                _playerAlive = _playerList.Count;
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
        _playerList.Add(player);
        UIManagement.instance.AddPlayerUI(player.GetComponent<PlayerAttack>());
    }
}
