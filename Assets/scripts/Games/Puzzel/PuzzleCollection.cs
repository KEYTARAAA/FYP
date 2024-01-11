using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCollection : MonoBehaviour
{
    [SerializeField] Transform UITarget;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
            {
            UITarget.gameObject.SetActive(true);
            gameObject.SetActive(false);
            UITarget.GetComponentInParent<Win>().checkWin();
        }
    }
}
