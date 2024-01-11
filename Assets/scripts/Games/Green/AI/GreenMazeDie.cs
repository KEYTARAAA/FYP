using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenMazeDie : MonoBehaviour
{
    [SerializeField] GameObject sign;
    [SerializeField] Transform winUI;
    Transform exit;

    private void OnTriggerEnter(Collider other)
    {
        exit.GetComponent<GreenMazeExit>().die();
        winUI.gameObject.SetActive(true);
        winUI.GetComponent<GameOutcome>().lose();
        sign.SetActive(true);
    }

    public void setExit(Transform exit)
    {
        this.exit = exit;
    }
}
