using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public List<GameObject> _playerList;
    public GameObject _map;
    public List<GameObject> _mapChilder;

    void Start()
    {
        instance = this;
        _playerList = new List<GameObject>();
        _mapChilder = new List<GameObject>();
        for (int i = 0; i < _map.transform.childCount; i++)
        {
            _mapChilder.Add(_map.transform.GetChild(i).gameObject);
        }
    }

    public void SpawningPlayer()
    {
        foreach (var player in _playerList)
        {
            player.GetComponent<Player>().ResetStat();
        }
    }
}
