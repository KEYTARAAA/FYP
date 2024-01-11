using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlueMaze : Interactable
{
    [SerializeField] string scene;
    [SerializeField] GameObject enemy = null, mazePlayer = null, worldPlayer, mainUI;
    override
    public void interact()
    {
        //Debug.Log("MAZEEEE");
        PlayerController controller = worldPlayer.GetComponent<PlayerController>();
        controller.SetActive(false);
        PlayerInteracter interacter = worldPlayer.GetComponent<PlayerInteracter>();
        interacter.enabled = false;
        Transform camera = worldPlayer.GetComponentInChildren<Camera>().transform;//.transform.GetChild(0);
        camera.gameObject.SetActive(false);

        MakeMaze makeMaze = GetComponent<MakeMaze>();
        makeMaze.placeParticipants();
        
        mazePlayer.SetActive(true);
        enemy.SetActive(true);
        deactivateCollider();

        BlueMazeExit blueMazeExit = GetComponentInChildren<BlueMazeExit>();
        blueMazeExit.activateCollider();

        sign.SetActive(false);
        mainUI.SetActive(false);
    }
}
