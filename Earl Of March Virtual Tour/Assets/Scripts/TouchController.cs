using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchController : MonoBehaviour
{
    public float Speed = 5f;
    private float pitch = 0f;
    private float yaw = 0f;

    private void Start(){
        yaw = 0f;
        pitch = 0f;
    }

    //The two camera movement methods
    private void Update(){
        //This code basically moves the camera based on the mouse being held down.
        if (Input.GetMouseButton(0))
        {
            if (!IsPointerOverUIElement())
            {
                pitch += Speed * Input.GetAxis("Mouse Y");

                yaw += Speed * -Input.GetAxis("Mouse X");

                pitch = Mathf.Clamp(pitch, -90f, 90f);

                //Prevants the screen to flip upside down.
                while (yaw < 0f)
                {
                    yaw += 360f;
                }
                while (yaw >= 360f)
                {
                    yaw -= 360f;
                }
            }
        }
        //Code for moving the camera by using key arrows. This moves the camera right
        if (Input.GetKey(KeyCode.RightArrow))
        {
            yaw += Speed * 0.1f;
            while (yaw < 0f)
            {
                yaw += 360f;
            }
            while (yaw >= 360f)
            {
                yaw -= 360f;
            }
        }
        //Code for moving the camera by using key arrows. This moves the camera left
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            yaw += -Speed * 0.1f;
            while (yaw < 0f)
            {
                yaw += 360f;
            }
            while (yaw >= 360f)
            {
                yaw -= 360f;
            }
        }
        //Code for moving the camera by using key arrows. This moves the camera up
        if (Input.GetKey(KeyCode.UpArrow))
        {
            pitch += -Speed * 0.1f;
            //Mathf clamp is just a fancy way of keeping the variable in -90f and 90f.
            pitch = Mathf.Clamp(pitch, -90f, 90f);
        }
        //Code for moving the camera by using key arrows. This moves the camera down
        if (Input.GetKey(KeyCode.DownArrow))
        {
            pitch += Speed * 0.1f;
            pitch = Mathf.Clamp(pitch, -90f, 90f);
        }
        //Finally with the new pitch and yaw you can set the new camera rotation.
        transform.eulerAngles = new Vector3(pitch, yaw, 0f);
        
    }

    //Ignore these 3 functions
    public static bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }
    public static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int i = 0; i < eventSystemRaysastResults.Count; i++){
            RaycastResult curRaysastResult = eventSystemRaysastResults[i];
            if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
            {
                return true;
            }
        }
        return false;
    }
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

}
