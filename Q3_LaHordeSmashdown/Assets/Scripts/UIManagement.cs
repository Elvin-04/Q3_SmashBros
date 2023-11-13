using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManagement : MonoBehaviour
{
    public static UIManagement instance;
    public List<TextMeshProUGUI> _percentTextList;

    public List<PlayerAttack> _playerAttackList;

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        for (int i = 0; i < _percentTextList.Count; i++)
        {
            if (i < _playerAttackList.Count)
            {
                _percentTextList[i].color = _playerAttackList[i]._colorPlayer;
                _percentTextList[i].text = _playerAttackList[i]._pourcent + " %\n" + _playerAttackList[i]._life + " vie restante ";
                _percentTextList[i].gameObject.SetActive(true);
            }
            else
            {
                _percentTextList[i].gameObject.SetActive(false);
            }
        }
    }

    public void AddPlayerUI(PlayerAttack player)
    {
        _playerAttackList.Add(player);
    }

    public void RemovePlayerUI(PlayerAttack player)
    {
        _playerAttackList.Remove(player);
    }
}
