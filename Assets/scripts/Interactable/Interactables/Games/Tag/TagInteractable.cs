using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagInteractable : Interactable
{
    // Start is called before the first frame update
    [SerializeField] GameObject ui, mainUI;
    [SerializeField] Transform player;
    override
    public void interact()
    {
        GetComponent<SphereCollider>().radius = 0.05f;
        GetComponent<TagInteractable>().enabled = false;
        PlayerInteracter interacter = player.GetComponent<PlayerInteracter>();
        interacter.deactivate();
        interacter.enabled = false;
        ui.SetActive(true);
        sign.SetActive(false);
        mainUI.SetActive(false);
    }
}
