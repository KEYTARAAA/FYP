using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TourInfo : MonoBehaviour
{
    [SerializeField] string name, info;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject ui;

    bool activated = false;

    public void activate()
    {
        activated = true;
    }
    public void deactivate()
    {
        activated = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (activated)
        {
            ui.SetActive(true);
            text.text = info;
            text.GetComponentInParent<TourManager>().addRoom(name);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (activated)
        {
            text.GetComponentInParent<TourManager>().checkWin();
            ui.SetActive(false);
        }
    }
}
