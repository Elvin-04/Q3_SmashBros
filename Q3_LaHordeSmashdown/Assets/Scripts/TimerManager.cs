using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public int minutes;
    public int seconds;

    private float actMin;
    private float actSec;

    public TextMeshProUGUI timerText;

    bool timerStarted = false;
    bool overtime = false;
    PlayerManager playerManager;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    void StartTimer()
    {
        actMin = minutes;
        actSec = seconds;
        timerStarted = true;
    }

    void StopTimer()
    {
        timerStarted = false;
        overtime = true;
        foreach(GameObject player in playerManager._playerList)
        {
            player.GetComponent<PlayerAttack>()._life = 1;
            player.GetComponent<PlayerAttack>()._pourcent = 300;
        }
    }

    void SetText()
    {
        if((int)actSec / 10 != 0)
            timerText.text = (int)actMin + ":" + (int)actSec;
        else
            timerText.text = (int)actMin + ":0" + (int)actSec;
    }

    private void FixedUpdate()
    {
        if(!timerStarted && playerManager._playerList.Count >= 2 && !overtime)
            StartTimer();

        if(timerStarted)
        {
            actSec -= Time.deltaTime;

            if (actSec <= 0)
            {
                actSec = 60f;
                actMin--;
            }

            if ((int)actSec != 60)
            {
                SetText();
            }

            if ((int)actMin <= 0 && (int)actSec <= 0)
                StopTimer();
        }
        
    }
}
