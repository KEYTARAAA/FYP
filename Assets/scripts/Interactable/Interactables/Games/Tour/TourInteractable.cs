using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourInteractable : Interactable
{

    [SerializeField] Transform player, areas, ui;
    [SerializeField] GameObject mainUI;
    override
    public void interact()
    {
        GetComponent<RegController>().enabled = true;
        GetComponent<RegController>().setMoving(true);
        GetComponent<SphereCollider>().radius = 0.05f;
        PlayerInteracter interacter = player.GetComponent<PlayerInteracter>();
        interacter.deactivate();
        interacter.enabled = false;
        GetComponent<Interactable>().deactivateCollider();
        player.transform.Find("UICanvas").transform.Find("Main").gameObject.SetActive(false);
        ui.gameObject.SetActive(true);
        ui.GetComponent<TourManager>().Refresh();
        foreach (Transform area in areas)
        {
            area.GetComponent<TourInfo>().activate();
        }
        sign.SetActive(false);
        mainUI.SetActive(false);
    }
}
