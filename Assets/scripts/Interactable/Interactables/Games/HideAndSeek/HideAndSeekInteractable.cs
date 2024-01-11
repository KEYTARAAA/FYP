using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAndSeekInteractable : Interactable
{

    [SerializeField] GameObject ui, mainUI;
    [SerializeField] Transform player;
    override
    public void interact()
    {
        GetComponent<SphereCollider>().radius = 0.05f;
        ui.SetActive(true);
        ui.GetComponent<HideAndSeekManager>().enabled = true;
        ui.GetComponent<HideAndSeekManager>().Refresh();
        PlayerInteracter interacter = player.GetComponent<PlayerInteracter>();
        interacter.deactivate();
        interacter.enabled = false;
        sign.SetActive(false);
        mainUI.SetActive(false);
        enabled = false;
    }
}
