using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenMazeCollectPiece : MonoBehaviour
{
    //[SerializeField] Transform player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<GreenMazeCollection>(out GreenMazeCollection collection))
        {
            collection.setCurrent(collection.getCurent() + 1);
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Cant find");
        }
    }
}
