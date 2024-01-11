using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOutcome : MonoBehaviour
{
    [SerializeField] [TextArea] string won, lost;
    [SerializeField] TextMeshProUGUI text;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            gameObject.SetActive(false);
        }
    }

    public void win()
    {
        text.text = won;
    }
    public void lose()
    {
        text.text = lost;
    }
}
