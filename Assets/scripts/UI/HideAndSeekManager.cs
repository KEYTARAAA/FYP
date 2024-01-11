using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HideAndSeekManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] int maxTime, maxCountDownTime;
    [SerializeField] Transform enemy, puzzle;
    float timeRemaining, time;
    int minutes, seconds;
    bool running = false, searching;
    // Start is called before the first frame update
    void Start()
    {

    }


    void Update()
    {
        if (running)
        {
            timeRemaining -= Time.deltaTime;
            seconds = Mathf.FloorToInt(timeRemaining % 60);
            minutes = Mathf.FloorToInt(timeRemaining / 60);
            string timeString = "";
            if (minutes < 10)
            {
                timeString += "0" + minutes.ToString();
            }
            else
            {
                timeString += minutes.ToString();
            }

            timeString += ":";

            if (seconds < 10)
            {
                timeString += "0" + seconds.ToString();
            }
            else
            {
                timeString += seconds.ToString();
            }
            text.text = timeString;

            if (seconds <= 0 && minutes <= 0)
            {
                if (searching)
                {
                    win();
                }
                else
                {
                    searching = true;
                    resetTimer();
                }
            }
        }
    }

    void startCountdown()
    {
        searching = false;
        resetTimer();
    }

    void resetTimer()
    {
        if (searching)
        {
            time = maxTime;
            enemy.GetComponent<HideAndSeekVision>().enabled = true;
            enemy.GetComponent<HideAndSeekChase>().enabled = true;
        }
        else
        {
            time = maxCountDownTime;
        }
        timeRemaining = time;
    }

    public void Refresh()
    {
        startCountdown();
    }
    void win()
    {
        enemy.GetComponent<HideAndSeekDie>().die(false);
        puzzle.position = enemy.position;
        enemy.GetComponent<PuzzelAppear>().appear();
    }
    private void OnEnable()
    {
        running = true;
        Refresh();
    }

    private void OnDisable()
    {
        enemy.GetComponent<HideAndSeekVision>().enabled = false;
        enemy.GetComponent<HideAndSeekChase>().enabled = false;
        running = false;
    }
}
