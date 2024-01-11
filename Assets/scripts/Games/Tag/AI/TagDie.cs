using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagDie : MonoBehaviour
{
    [SerializeField] GameObject ui, sign, mainUI;
    [SerializeField] Transform player, winUI;
    Vector3 origin;
    private void OnTriggerEnter(Collider other)
    {
        die(true);
    }

    private void Start()
    {
        origin = transform.position;
    }

    public void die(bool loss)
    {
        if (loss && GetComponent<SphereCollider>().radius < 2f)
        {
            winUI.gameObject.SetActive(true);
            winUI.GetComponent<GameOutcome>().lose();
        }
        GetComponent<RegController>().setMoving(false);
        GetComponent<RegController>().setTarget(origin);
        GetComponent<SphereCollider>().radius = 2f;
        GetComponent<TagInteractable>().enabled = true;
        PlayerInteracter interacter = player.GetComponent<PlayerInteracter>();
        interacter.activate();
        interacter.enabled = true;
        ui.SetActive(false);
        sign.SetActive(true);
        mainUI.SetActive(true);
    }

}
