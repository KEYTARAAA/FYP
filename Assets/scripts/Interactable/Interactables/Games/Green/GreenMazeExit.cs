using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenMazeExit : Interactable
{
    [SerializeField] GameObject enemies = null, mazePlayer = null, worldPlayer = null, UI = null, mainUI;
    private bool exit = false;

    override
    public void interact()
    {
        if (exit) {
            mazePlayer.SetActive(false);
            PlayerController controller = worldPlayer.GetComponent<PlayerController>();
            controller.SetActive(true);
            PlayerInteracter interacter = worldPlayer.GetComponent<PlayerInteracter>();
            interacter.enabled = true;
            Transform camera = worldPlayer.transform.Find("Student").transform.Find("Main Camera");
            camera.gameObject.SetActive(true);

            foreach (Transform enemy in enemies.transform)
            {
                enemy.gameObject.SetActive(false);
            }


            UI.SetActive(false);

            GreenMaze greenMaze = GetComponentInParent<GreenMaze>();
            greenMaze.activateCollider();
            deactivateCollider();
            GetComponentInParent<PuzzelAppear>().appear();
            sign.SetActive(true);
            mainUI.SetActive(true);
        }
    }

    public void setExit(bool exit)
    {
        this.exit = exit;
        if (exit == true)
        {
            setInteraction("exit maze", false);
            deactivateCollider();
            activateCollider();
        }
        else
        {
            setInteraction("Collect all pieces to exit.", true);
            deactivateCollider();
            activateCollider();
        }
    }

    public void die()
    {
        mazePlayer.SetActive(false);
        PlayerController controller = worldPlayer.GetComponent<PlayerController>();
        controller.SetActive(true);
        PlayerInteracter interacter = worldPlayer.GetComponent<PlayerInteracter>();
        interacter.enabled = true;
        Transform camera = worldPlayer.transform.Find("Student").transform.Find("Main Camera");
        camera.gameObject.SetActive(true);

        foreach (Transform enemy in enemies.transform)
        {
            enemy.gameObject.SetActive(false);
        }


        UI.SetActive(false);

        GreenMaze greenMaze = GetComponentInParent<GreenMaze>();
        greenMaze.activateCollider();
        deactivateCollider();
        sign.SetActive(true);
        mainUI.SetActive(true);
    }

    public void setUp(GameObject enemies, GameObject mazePlayer, GameObject worldPlayer, GameObject UI, GameObject sign, GameObject mainUI)
    {
        this.enemies = enemies;
        this.mazePlayer = mazePlayer;
        this.worldPlayer = worldPlayer;
        this.UI = UI;
        this.sign = sign;
        this.mainUI = mainUI;
    }

}
