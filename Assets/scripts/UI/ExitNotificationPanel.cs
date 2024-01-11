using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitNotificationPanel : MonoBehaviour
{
    [SerializeField] GameObject mainUI;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            mainUI.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
