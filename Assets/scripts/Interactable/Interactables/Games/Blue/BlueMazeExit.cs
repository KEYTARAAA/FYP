using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueMazeExit : Interactable
{
    [SerializeField] GameObject enemy = null, mazePlayer = null, worldPlayer, winUI, mainUI;
    override
    public void interact()
    {
        die(false);
        GetComponent<PuzzelAppear>().appear();
    }

    public void die(bool loss)
    {

        mazePlayer.SetActive(false);
        enemy.SetActive(false);
        PlayerController controller = worldPlayer.GetComponent<PlayerController>();
        controller.SetActive(true);
        PlayerInteracter interacter = worldPlayer.GetComponent<PlayerInteracter>();
        interacter.enabled = true;
        Transform camera = worldPlayer.transform.Find("Student").transform.Find("Main Camera");
        camera.gameObject.SetActive(true);

        BlueMaze blueMaze = GetComponentInParent<BlueMaze>();
        blueMaze.activateCollider();
        deactivateCollider();
        sign.SetActive(true);


        winUI.gameObject.SetActive(true);
        if (loss)
        {
            winUI.GetComponent<GameOutcome>().lose();
        }
        mainUI.SetActive(true);
    }

}
