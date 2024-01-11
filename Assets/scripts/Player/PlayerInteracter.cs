using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteracter : MonoBehaviour
{
    [SerializeField] GameObject UI = null;
    [SerializeField] Text text = null;
    Interactable interactable = null;
    private bool interacted = false, activated = true;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && interactable != null && !interacted)
        {
            interactable.interact();
            interacted = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Interactable>(out Interactable newInteractable) && activated)
        {
            interactable = newInteractable;
            text.text = interactable.getInteraction();
            UI.SetActive(true);
            interacted = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (activated) 
        {
            UI.SetActive(false);
            interactable = null;
        }
    }

    private void OnDisable()
    {
        UI.SetActive(false);
    }

    public void activate()
    {
        activated = true;
    }
    public void deactivate()
    {
        activated = false;
    }
}


