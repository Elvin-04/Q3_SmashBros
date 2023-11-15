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


    private void Start()
    {
        StartTimer();
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
