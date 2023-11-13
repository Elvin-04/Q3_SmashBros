using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public List<GameObject> _playerList;
    public GameObject _map;

    void Start()
    {
        instance = this;
        _playerList = new List<GameObject>();
    }

    public void SpawningPlayer()
    {
        foreach (var player in _playerList)
        {
            player.GetComponent<PlayerAttack>().ResetStat();
        }
    }
}
