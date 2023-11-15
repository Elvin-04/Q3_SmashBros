using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManagement : MonoBehaviour
{
    public static UIManagement instance;
    public List<TextMeshProUGUI> _percentTextList;
    public List<TextMeshProUGUI> _lifesTextList;

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
                _percentTextList[i].color = _playerAttackList[i].GetComponent<PlayerMovements>().normaColor;
                _lifesTextList[i].color = _playerAttackList[i].GetComponent<PlayerMovements>().normaColor;
                _percentTextList[i].text = _playerAttackList[i]._pourcent + " %\n";
                switch(_playerAttackList[i]._life)
                {
                    case 3:
                        _lifesTextList[i].text = "- - -";
                        break;
                    case 2:
                        _lifesTextList[i].text = "- -";
                        break;
                    case 1:
                        _lifesTextList[i].text = "-";
                        break;
                    default:
                        _lifesTextList[i].text = "";
                        break;
                }
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
