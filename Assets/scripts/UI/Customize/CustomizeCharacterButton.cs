using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizeCharacterButton : MonoBehaviour
{
    [SerializeField] GameObject player;

    public void onClick()
    {
        CharacterController characterController = GetComponentInParent<CharacterController>();
        characterController.enabled = false;
        Quaternion rot = Quaternion.Euler(0, -90, 0);
        player.transform.rotation = rot;
        player.transform.position = new Vector3(544.9f,13.06f,644.8f);
        characterController.enabled = true;
        Transform camera = player.transform.Find("Student").transform.Find("Main Camera");
        rot = Quaternion.Euler(15.695f, 180, 0);
        camera.localRotation = rot;
        camera.localPosition = new Vector3(0, 1.2f, 1.56f);
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<PlayerInteracter>().enabled = false;
    }
}
