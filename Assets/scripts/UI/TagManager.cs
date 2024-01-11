using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TagManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField]int maxTime, maxCountDownTime;
    [SerializeField] Transform enemy, puzzle;
    float timeRemaining, time;
    int minutes, seconds;
    bool running = false, searching;
    // Start is called before the first frame update
    void Start()
    {
        resetTimer();
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
            enemy.GetComponent<RegController>().enabled = true;
            enemy.GetComponent<RegController>().setMoving(true);
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

    public void win()
    {
        enemy.GetComponent<TagDie>().die(false);
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
        running = false;
    }
}
