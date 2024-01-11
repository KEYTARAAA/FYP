using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private string scene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( ( ( (player.position.x <= transform.position.x+1) || (player.position.x >= transform.position.x - 1) ) ||
            (( player.position.x <= transform.position.z + 1) || (player.position.z >= transform.position.x - 1) ) ) &&
            ( Input.GetKey(KeyCode.Return) ) ) {
            SceneManager.LoadScene(scene);
        }
    }
}
