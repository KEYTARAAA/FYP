using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MatchingManager : MonoBehaviour
{
    [SerializeField] int lives = 9;
    [SerializeField] Transform cards, player, game, winUI;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject sign, mainUI;
    List<Vector3> possiblePositions, positionsLeft;
    List<Transform> active;
    int currentLives = 0;
    public void cardClicked()
    {
        resetActive();
        if (active.Count == 2)
        {
            checkMatch();
        }
    }
    public void checkMatch()
    {
        if (active[0].name.Substring(0,2).Equals(active[1].name.Substring(0, 2)))
        {
            removeCards();
        }
        else
        {
            flipCards();
        }
    }
    void removeCards()
    {
        foreach (Transform card in active)
        {
            card.gameObject.SetActive(false);
        }
        resetActive();

        if (checkCardsLeft() == false)
        {
            win();
        }
    }
    void lose(bool loss)
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
        if (loss)
        {
            winUI.gameObject.SetActive(true);
            winUI.GetComponent<GameOutcome>().lose();
        }
        sign.SetActive(true);
        mainUI.SetActive(true);
        gameObject.SetActive(false);
    }
    void win()
    {
        game.GetComponent<PuzzelAppear>().appear();
        lose(false);
    }
    bool checkCardsLeft()
    {
        int left = 0;
        for (int i = 0; i < cards.transform.childCount; i++)
        {
            if (cards.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                left++;
            }
        }
        if (left > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void flipCards()
    {
        currentLives--;
        text.text = "Lives: " + currentLives.ToString();
        foreach (Transform card in active)
        {
            card.GetComponent<MatchingFlipManager>().click();
        }
        if (currentLives <= 0)
        {
            lose(true);
        }
        resetActive();
    }

    void rearrange()
    {
        foreach (Transform card in cards)
        {
            Vector3 pos = positionsLeft[Random.Range(0, positionsLeft.Count - 1)];
            card.GetComponent<RectTransform>().position = pos;
            positionsLeft.Remove(pos);
        }
    }

    void Refresh()
    {
        possiblePositions = new List<Vector3>();
        foreach (Transform card in cards)
        {
            possiblePositions.Add(card.GetComponent<RectTransform>().position);
        }

        currentLives = lives;
        text.text = "Lives: " + currentLives.ToString();
        for (int i = 0; i < cards.transform.childCount; i++)
        {
            cards.transform.GetChild(i).gameObject.SetActive(true);
        }
        resetActive();
        positionsLeft = new List<Vector3>();
        foreach (Vector3 pos in possiblePositions)
        {
            positionsLeft.Add(pos);
        }
        rearrange();

    }

    void resetActive()
    {
        active = new List<Transform>();
        for(int i=0; i<cards.transform.childCount; i++)
        {
            if (cards.transform.GetChild(i).GetComponent<MatchingFlipManager>().getActivated())
            {
                active.Add(cards.transform.GetChild(i));
            }
        }
    }

    private void OnEnable()
    {
        Refresh();
    }
}
