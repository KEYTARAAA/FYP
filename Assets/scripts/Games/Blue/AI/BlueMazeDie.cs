using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueMazeDie : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<BlueMazeExit>(out BlueMazeExit exit))
        {
            exit.die(true);
        }
    }
}
