using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StyleCustomizeCharacter : MonoBehaviour
{
    [SerializeField]Transform bodyObj, hairObj, eyesObj, backObj, hatsObj;
    List<GameObject> activeList, body, hair, eyes, back, hats;
    int index=0;
    void Start()
    {
        body = new List<GameObject>();
        hair = new List<GameObject>();
        eyes = new List<GameObject>();
        back = new List<GameObject>();
        hats = new List<GameObject>();

        makeList(bodyObj, body);
        makeList(hairObj, hair);
        makeList(eyesObj, eyes);
        makeList(backObj, back);
        makeList(hatsObj, hats);

        changeList("Body");
    }

    void makeList(Transform obj, List<GameObject> list)
    {
        for (int i = 0; i < obj.childCount; i++)
        {
            Transform child = obj.GetChild(i);
            if (child.TryGetComponent<SkinnedMeshRenderer>(out SkinnedMeshRenderer junk) || child.TryGetComponent<MeshRenderer>(out MeshRenderer junkie))
            {
                list.Add(child.gameObject);
            }
        }
    }

    void findIndex()
    {
        for (int i = 0; i < activeList.Count; i++)
        {
            
                if (activeList[i].activeSelf)
                {
                    index = i;
                }
        }
    }



    public void change(int change)
    {
        activeList[index].gameObject.SetActive(false);
        index += change;
        if (index > activeList.Count - 1)
        {
            index = 0;
        }
        if (index < 0)
        {
            index = activeList.Count - 1;
        }
        activeList[index].SetActive(true);
    }


    public void changeList(string name)
    {
        name = name.ToUpper();
        switch (name)
        {
            case "BODY":
                activeList = body;
                break;
            case "HAIR":
                activeList = hair;
                break;
            case "EYES":
                activeList = eyes;
                break;
            case "BACK":
                activeList = back;
                break;
            case "HATS":
                activeList = hats;
                break;
        }
        findIndex();
    }






}
