using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoneCustomize : MonoBehaviour
{
    [SerializeField] GameObject player, toCopy;
    [SerializeField] List<Transform> others;

    public void onClick()
    {
        CharacterController characterController = GetComponentInParent<CharacterController>();
        characterController.enabled = false;
        Quaternion rot = Quaternion.Euler(0, -90, 0);
        player.transform.rotation = rot;
        player.transform.position = new Vector3(544.9f, 13.06f, 644.8f);
        characterController.enabled = true;
        Transform camera = player.transform.Find("Student").transform.Find("Main Camera");
        rot = Quaternion.Euler(15.695f, 0, 0);
        camera.localRotation = rot;
        camera.localPosition = new Vector3(0, 1.411f, -1.18f);
        player.GetComponent<PlayerController>().enabled = true;
        player.GetComponent<PlayerInteracter>().enabled = true;
        changeAll();
    }


    void changeAll()
    {
        foreach (Transform other in others)
        {
            Destroy(other.Find("Body").gameObject);
            Transform b = Instantiate(toCopy.transform);
            b.name = "Body";
            b.SetParent(other);
            b.localPosition = new Vector3(0, 0, 0);
            Quaternion rot = Quaternion.Euler(0, 0, 0);
            b.localRotation = rot;
            b.localScale = new Vector3(1, 1, 1);
        }
    }
}
