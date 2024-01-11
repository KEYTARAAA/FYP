using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragRotate : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] float loss = 1;
    [SerializeField] Transform target;
    Vector3 lastMousePos, mouseVelocity, lastMouseVelocity;
    float XaxisRotation, YaxisRotation, lastXaxisRotation, lastYaxisRotation;
    bool draging;

    public void OnDrag(PointerEventData eventData)
    {
        draging = true;
        mouseVelocity = lastMousePos - Input.mousePosition;
        lastMousePos = Input.mousePosition;

        //rotationSpeed;
        XaxisRotation = Input.GetAxis("Mouse X") * mouseVelocity.x * Time.deltaTime*rotationSpeed;//rotationSpeed;
        YaxisRotation = Input.GetAxis("Mouse Y") * mouseVelocity.y * Time.deltaTime*rotationSpeed;
        target.transform.Rotate(Vector3.down, XaxisRotation);
        target.transform.Rotate(Vector3.right, YaxisRotation);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draging = false;
    }

    private void Update()
    {
        if (!draging) {
            if (XaxisRotation > 0)
            {
                XaxisRotation -= loss * Time.deltaTime;
            }
            else if (XaxisRotation < 0)
            {
                XaxisRotation += loss * Time.deltaTime;
            }

            if ((XaxisRotation >= 0 && lastXaxisRotation < 0) || (XaxisRotation <= 0 && lastXaxisRotation > 0))
            {
                XaxisRotation = 0;
            }

            if (YaxisRotation > 0)
            {
                YaxisRotation -= loss * Time.deltaTime;
            }
            else if (YaxisRotation < 0)
            {
                YaxisRotation += loss * Time.deltaTime;
            }
            if ((YaxisRotation >= 0 && lastYaxisRotation < 0) || (YaxisRotation <= 0 && lastYaxisRotation > 0))
            {
                YaxisRotation = 0;
            }

            if (XaxisRotation != 0) {
                target.transform.Rotate(Vector3.down, XaxisRotation);
            }
            if (YaxisRotation != 0) {
                target.transform.Rotate(Vector3.right, YaxisRotation);
            }

            lastXaxisRotation = XaxisRotation;
            lastYaxisRotation = YaxisRotation;
        }
    }

}
