using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Win : MonoBehaviour
{
    [SerializeField] GameObject ui, timer;
    [SerializeField] TextMeshProUGUI text;
    [TextArea][SerializeField] string winText;
    bool won = false;

    public void checkWin()
    {
        int count = 0;
        for(int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeInHierarchy)
            {
                count++;
            }
        }

        if (count == transform.childCount)
        {
            win();

        }
    }
    void win()
    {
        ui.SetActive(true);
        text.text = winText;
        if (timer.activeInHierarchy)
        {
            text.text += "\nCompletion Time: " + timer.GetComponent<UITimer>().getTimeString();
        }
        won = true;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && won)
        {
            ui.SetActive(!ui.activeInHierarchy);
        }
    }
}
