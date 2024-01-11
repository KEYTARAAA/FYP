using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    [SerializeField] string type, name, interaction;
    [SerializeField] protected  GameObject sign;
    SphereCollider collider = null;
    private bool specialInteraction;
    private char button;

    private Vector3 originalPosition;

    private void Awake()
    {
        collider = GetComponent<SphereCollider>();
        originalPosition = collider.center;
        button = 'E';
    }

    public virtual void interact()
    {
    }

    public void editColliderRadius()
    {
        collider.radius /= gameObject.transform.localScale.x;
    }

    public string getType()
    {
        return type;
    }
    public string getName()
    {
        return name;
    }
    public string getInteraction()
    {
        if (specialInteraction)
        {
            return interaction;
        }
        else
        {
            return "Press " + button + " to " + interaction;
        }
    }
    public void setInteraction(string interaction, bool special)
    {
        this.interaction = interaction;
        this.specialInteraction = special;
    }
    public void setButton(char button)
    {
        this.button = button;
    }
    public void activateCollider()
    {
        collider.enabled = true;
    }
    public void deactivateCollider()
    {
        collider.enabled =false;
    }
}
