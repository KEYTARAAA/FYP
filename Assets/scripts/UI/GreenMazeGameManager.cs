using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GreenMazeGameManager : MonoBehaviour
{
    [SerializeField] GameObject maze, mazePlayer, collectables;

    private bool updateExit;
    void Start()
    {
        reset();
    }

    private void OnEnable()
    {
        reset();
    }

    // Update is called once per frame
    void Update()
    {

        if (updateExit) {
            if (mazePlayer.GetComponent<GreenMazeCollection>().getCurent() == mazePlayer.GetComponent<GreenMazeCollection>().getGoal())
            {
                maze.GetComponentInChildren<GreenMazeExit>().setExit(true);
                updateExit = false;
            }
        }
    }

   

    public void reset()
    {
        mazePlayer.GetComponent<GreenMazeCollection>().setCurrent(0);
        updateExit = true;
        maze.GetComponentInChildren<GreenMazeExit>().setExit(false);
        for (int i=0; i<collectables.transform.childCount; i++)
        {
            collectables.transform.GetChild(i).gameObject.SetActive(true);
        }
        
    }
}
