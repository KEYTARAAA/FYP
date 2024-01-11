using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TourManager : MonoBehaviour
{
    [SerializeField] int maxRooms = 3;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Transform player, areas, guide, puzzle;
    [SerializeField] GameObject sign, mainUI;
    int current = 0;
    List<string> visited;
    Vector3 origin;

    private void Start()
    {
        visited = new List<string>();
        origin = guide.position;
    }

    public void Refresh()
    {
        visited = new List<string>();
        change();
    }
    public void addRoom(string room)
    {
        bool add = true;
        foreach (string s in visited)
        {
            if (s.Equals(room))
            {
                add = false;
            }
        }
        if (add)
        {
            visited.Add(room);
                change();
        }
    }

    public void checkWin()
    {
        if (visited.Count >= maxRooms)
        {
            win();
        }
    }

    void change()
    {
        text.text = "Rooms Visited: " + visited.Count + " / " + maxRooms;
    }
    void win()
    {
        player.transform.Find("Student").transform.Find("Main Camera").gameObject.SetActive(true);
        player.transform.Find("UICanvas").transform.Find("Main").gameObject.SetActive(true);
        player.transform.Find("UICanvas").transform.Find("Tour").gameObject.SetActive(false);

        foreach (Transform area in areas)
        {
            area.GetComponent<TourInfo>().deactivate();
        }

        PlayerController controller = player.GetComponent<PlayerController>();
        controller.SetActive(true);
        PlayerInteracter interacter = player.GetComponent<PlayerInteracter>();
        interacter.enabled = true;
        interacter.activate();
        puzzle.position = new Vector3(guide.position.x, guide.position.y+20, guide.position.z);
        guide.GetComponent<Interactable>().activateCollider();
        guide.GetComponent<RegController>().setMoving(false);
        guide.GetComponent<RegController>().setTarget(origin);
        guide.GetComponent<SphereCollider>().radius = 2f;
        GetComponent<PuzzelAppear>().appear();
        gameObject.SetActive(false);
        sign.SetActive(true);
        mainUI.SetActive(true);
    }
}
