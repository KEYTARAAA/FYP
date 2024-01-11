using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialCustomizeCharacter : MonoBehaviour
{
    [SerializeField]Transform bodyObj, hairObj, eyesObj, backObj, hatsObj;
    [SerializeField]List<Material> materialList;
    List<GameObject> activeList, body, hair, eyes, back, hats;
    int index, materialIndex;
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

    public void findIndex()
    {
        for (int i = 0; i < activeList.Count; i++)
        {

            if (activeList[i].activeSelf)
            {
                index = i;
            }
        }
        findMaterialIndex();
    }
    void findMaterialIndex()
    {
        for (int i = 0; i < materialList.Count; i++)
        {

            if (activeList[index].TryGetComponent<SkinnedMeshRenderer>(out SkinnedMeshRenderer skinMesh))
            {
                if (materialList[i].name == skinMesh.name)
                {
                    materialIndex = i;
                }
            }
            else
            {
                activeList[index].TryGetComponent<MeshRenderer>(out MeshRenderer mesh);
                if (materialList[i].name == mesh.name)
                {
                    materialIndex = i;
                }
            }
        }
    }

    void changeMaterial()
    {
        if (activeList[index].TryGetComponent<SkinnedMeshRenderer>(out SkinnedMeshRenderer skinMesh))
        {
                skinMesh.material = materialList[materialIndex];
        }
        else
        {
            activeList[index].TryGetComponent<MeshRenderer>(out MeshRenderer mesh);
            mesh.material = materialList[materialIndex];
        }

        for (int i = 0; i < activeList[index].transform.childCount; i++)
        {
            Transform child = activeList[index].transform.GetChild(i);
            if (child.TryGetComponent<SkinnedMeshRenderer>(out SkinnedMeshRenderer skinMeshChild))
            {
                skinMeshChild.material = materialList[materialIndex];
            }
            else
            {
                child.TryGetComponent<MeshRenderer>(out MeshRenderer meshChild);
                meshChild.material = materialList[materialIndex];
            }
        }
    }



    public void change(int change)
    {
        materialIndex += change;
        if (materialIndex > materialList.Count - 1)
        {
            materialIndex = 0;
        }
        if (materialIndex < 0)
        {
            materialIndex = materialList.Count - 1;
        }
        changeMaterial();
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
