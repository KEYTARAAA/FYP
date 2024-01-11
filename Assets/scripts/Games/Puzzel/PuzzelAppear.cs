using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzelAppear : MonoBehaviour
{
    [SerializeField] Transform piece, winUI;
    bool open = true;
    public void appear()
    {
        if (open) {
            piece.gameObject.SetActive(true);
            winUI.gameObject.SetActive(true);
            winUI.GetComponent<GameOutcome>().win();
            open = !open;
        }
    }
}
