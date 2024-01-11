using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangmanInteractable : Interactable
{
    [SerializeField] Transform player;
    [SerializeField] GameObject mainUI;
    override
    public void interact()
    {
        player.transform.Find("Student").transform.Find("Main Camera").gameObject.SetActive(false);
        player.transform.Find("UICanvas").transform.Find("Main").gameObject.SetActive(false);
        player.transform.Find("UICanvas").transform.Find("Hangman").gameObject.SetActive(true);
        PlayerController controller = player.GetComponent<PlayerController>();
        controller.SetActive(false);
        PlayerInteracter interacter = player.GetComponent<PlayerInteracter>();
        interacter.enabled = false;
        transform.Find("Camera").gameObject.SetActive(true);
        deactivateCollider();
        sign.SetActive(false);
        mainUI.SetActive(false);
    }
}
