using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzelFloat : MonoBehaviour
{
    [SerializeField] float hover, maxHover, rotate;
    private Vector3 origin;
    void Start()
    {
        origin = transform.position;
    }

    private void FixedUpdate()
    {
        //transform.localRotation = new Quaternion(transform.localRotation.x, transform.localRotation.y + rotate, transform.localRotation.z, transform.localRotation.w);
        transform.Rotate(0,rotate,0);
        if (transform.position.y >= origin.y + maxHover)
        {
            hover *= -1;
        }
        else if(transform.position.y <= origin.y - maxHover)
        {
            hover *= -1;
        }
        transform.Translate(0,hover,0);
    }
}
