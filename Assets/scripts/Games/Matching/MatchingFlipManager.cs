using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingFlipManager : MonoBehaviour
{
    [SerializeField] Transform card;
    [SerializeField] float rotationSpeed = 5;
    bool flipped = false, activated = false, lastActivated = false;

    private void Update()
    {
        if ((!(getRotationInRange(180) || getRotationInRange(0))) && flipped)
        {
            card.localRotation = Quaternion.Euler(0, card.localRotation.eulerAngles.y + rotationSpeed, 0);
        }
        else
        {
            flipped = false;
            lastActivated = activated;
            activated = getRotationInRange(0);
            if (lastActivated != activated)
            {
                GetComponentInParent<MatchingManager>().cardClicked();
            }
        }
    }

    bool getRotationInRange(float rot)
    {
        if (card.localRotation.eulerAngles.y > rot-1 && card.localRotation.eulerAngles.y < rot+1)
        {
            return true;
        }
        return false;
    }

    public void click()
    {
        card.localRotation = Quaternion.Euler(0, card.localRotation.eulerAngles.y + rotationSpeed, 0);
        flipped = true;
    }
    public bool getActivated()
    {
        return activated;
    }
    private void OnDisable()
    {
        activated = false;
    }

    private void OnEnable()
    {
        activated = false;
        card.localRotation = Quaternion.Euler(0, 180, 0);
        flipped = false;
    }
}
