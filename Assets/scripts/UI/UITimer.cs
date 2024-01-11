using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    float  time;
    int minutes, seconds, hours;
    bool running;
    // Start is called before the first frame update
    void Start()
    {
        running = true;
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (running) 
        {
            time += Time.deltaTime;
            seconds = Mathf.FloorToInt(time % 60);
            minutes = Mathf.FloorToInt(time / 60);

            if (minutes > 60)
            {
                hours = Mathf.FloorToInt(minutes / 60);
                minutes = minutes % 60;
            }
            string timeString = "";

            if (hours > 0)
            {
                if (hours < 10)
                {
                    timeString += "0" + minutes.ToString();
                }
                else
                {
                    timeString += minutes.ToString();
                }
            }

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
        }
    }

    public void pause()
    {
        running = false;
    }

    public void play()
    {
        running = true;
    }

    public bool getRunning()
    {
        return running;
    }

    public string getTimeString()
    {
        return text.text;
    }
}
