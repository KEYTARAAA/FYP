using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GreenMaze : Interactable
{
    [SerializeField] string scene;
    [SerializeField] GameObject enemies = null, mazePlayer = null, worldPlayer = null, UI = null, mainUI;
    [SerializeField] Transform fog;
    [SerializeField] bool night = false;
    override
    public void interact()
    {
        PlayerController controller = worldPlayer.GetComponent<PlayerController>();
        controller.SetActive(false);
        PlayerInteracter interacter = worldPlayer.GetComponent<PlayerInteracter>();
        interacter.enabled = false;
        Transform camera = worldPlayer.GetComponentInChildren<Camera>().transform;
        camera.gameObject.SetActive(false);

        GenerateRandomMap generateRandomMap = GetComponent<GenerateRandomMap>();
        generateRandomMap.placeParticipants();

        mazePlayer.SetActive(true);


        foreach (Transform enemy in enemies.transform)
        {
            enemy.gameObject.SetActive(true);
        }
        
        if (night)
        {
            fog.gameObject.SetActive(true);
        }

        UI.SetActive(true);

        deactivateCollider();

        sign.SetActive(false);
        mainUI.SetActive(false);
    }

    public GameObject getMazePlayer()
    {
        return mazePlayer;
    }
}
