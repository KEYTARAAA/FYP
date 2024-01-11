using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITimerPauseOnDisplay : MonoBehaviour
{
    [SerializeField] GameObject timer;
    UITimer uiTimer;
    // Start is called before the first frame update

    private void OnEnable()
    {
        uiTimer = timer.GetComponent<UITimer>();
        uiTimer.pause();
    }

    private void Update()
    {
        if (uiTimer.getRunning())
        {
            uiTimer.pause();
        }
    }
    private void OnDisable()
    {
        uiTimer.play();
    }
}
