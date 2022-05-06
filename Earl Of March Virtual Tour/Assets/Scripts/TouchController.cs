using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TouchController : MonoBehaviour
{
    public float Speed = 5f;
    private float pitch = 0f;
    private float yaw = 0f;

    public List<KeyCode> keybinds;
    int keypressed = -1;
    public TextMeshProUGUI selectedChangeText;
    public GameObject keypressUI;
    public UIController ui;

    private void Start(){
        yaw = 0f;
        pitch = 0f;
        keypressed = -1;
    }
    public void onKeyPress(TextMeshProUGUI _text)
    {
        keypressed = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        selectedChangeText = _text;
        keypressUI.SetActive(true);
    }

    public void OnGUI()
    {
        Event e = Event.current;
        if (e.isKey && keypressed != -1)
        {
            keybinds[keypressed - 1] = e.keyCode;
            selectedChangeText.text = e.keyCode.ToString();
            keypressed = -1;
            keypressUI.SetActive(false);
        }
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
        if (Input.GetKey(keybinds[2]))
        {
            yaw += Speed * (ui.scrollcamspeed.value) / 2f;
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
        if (Input.GetKey(keybinds[3]))
        {
            yaw += -Speed * (ui.scrollcamspeed.value)/2f;
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
        if (Input.GetKey(keybinds[0]))
        {
            pitch += -Speed * (ui.scrollcamspeed.value)/2f;
            //Mathf clamp is just a fancy way of keeping the variable in -90f and 90f.
            pitch = Mathf.Clamp(pitch, -90f, 90f);
        }
        //Code for moving the camera by using key arrows. This moves the camera down
        if (Input.GetKey(keybinds[1]))
        {
            pitch += Speed * (ui.scrollcamspeed.value) / 2f;
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
