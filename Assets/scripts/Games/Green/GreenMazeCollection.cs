using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GreenMazeCollection : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    [SerializeField] int goal =0 , current = 0;
    public int getGoal()
    {
        return goal;
    }
    public int getCurent()
    {
        return current;
    }
    public void setGoal(int goal)
    {
        this.goal = goal;
        change();
    }
    public void setCurrent(int current)
    {
        this.current = current;
        change();
    }

    void change()
    {
        text.text = current + " / " + goal;
    }
}
