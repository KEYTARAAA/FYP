using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetActiveButtonCustomize : MonoBehaviour
{
    [SerializeField] Color inactiveButton, inactiveText, activeButton, activeText;
    [SerializeField] Transform obj;
    List<Button> buttons;
    List<TextMeshProUGUI> texts;
    void Start()
    {
        buttons = new List<Button>();
        texts = new List<TextMeshProUGUI>();
        for (int i = 0; i < obj.childCount; i++)
        {
            Transform child = obj.GetChild(i);
            if (child.TryGetComponent<Button>(out Button b))
            {
                buttons.Add(b);
                TextMeshProUGUI t = child.GetComponentInChildren<TextMeshProUGUI>(true);
                texts.Add(t);
            }
        }
        changeActive("Body");
    }

    public void changeActive(string name)
    {
        foreach (Button b in buttons)
        {
            b.GetComponent<Image>().color = inactiveButton;
        }
        foreach (TextMeshProUGUI t in texts)
        {
            t.color = inactiveText;
        }

        for (int i = 0; i < texts.Count; i++)
        {
            
            if (texts[i].text.ToUpper() == name.ToUpper())
            {
                texts[i].color = activeText;
                buttons[i].GetComponent<Image>().color = activeButton;
            }
        }
    }
}
