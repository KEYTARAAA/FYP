using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MathManager : MonoBehaviour
{
    [SerializeField] int maxScore = 10;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Transform player, game;
    [SerializeField] GameObject sign, mainUI;
    int current=0;
    
    public void setCurrent(int current)
    {
        this.current = current;
        if (current>=maxScore)
        {
            win();
        }
        change();
    }

    public int getCurrent()
    {
        return current;
    }

    void change()
    {
        text.text = "Score: "+current + " / " + maxScore;
    }

    void win()
    {
        player.transform.Find("Student").transform.Find("Main Camera").gameObject.SetActive(true);
        player.transform.Find("UICanvas").transform.Find("Main").gameObject.SetActive(true);
        player.transform.Find("UICanvas").transform.Find("Math").gameObject.SetActive(false);
        PlayerController controller = player.GetComponent<PlayerController>();
        controller.SetActive(true);
        PlayerInteracter interacter = player.GetComponent<PlayerInteracter>();
        interacter.enabled = true;
        game.Find("Camera").gameObject.SetActive(false);
        game.GetComponent<Interactable>().activateCollider();
        GetComponent<PuzzelAppear>().appear();
        gameObject.SetActive(false);
        sign.SetActive(true);
        mainUI.SetActive(true);
    }
}
