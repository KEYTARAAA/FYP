using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathInteractable : Interactable
{
    [SerializeField] Transform player;
    [SerializeField] GameObject mainUI;
    override
    public void interact()
    {
        player.transform.Find("Student").transform.Find("Main Camera").gameObject.SetActive(false);
        player.transform.Find("UICanvas").transform.Find("Main").gameObject.SetActive(false);
        player.transform.Find("UICanvas").transform.Find("Math").gameObject.SetActive(true);
        player.transform.Find("UICanvas").transform.Find("Math").GetComponent<MathManager>().setCurrent(0);
        PlayerController controller = player.GetComponent<PlayerController>();
        controller.SetActive(false);
        PlayerInteracter interacter = player.GetComponent<PlayerInteracter>();
        interacter.enabled = false;
        transform.Find("Camera").gameObject.SetActive(true);
        deactivateCollider();
        mainUI.SetActive(false);
        sign.SetActive(false);
    }
}
