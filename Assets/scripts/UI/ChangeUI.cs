using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeUI : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] GameObject parent;

    public void toggleUI()
    {
        target.SetActive(true);
        parent.SetActive(false);
    }
}
