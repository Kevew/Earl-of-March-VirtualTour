using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArrowMovement : MonoBehaviour
{
    private MeshRenderer renderer;
    private GuideSystem guide;
    public float transparancy = 0.001f;
    //Set all the variables such as rendere and guide so I can use them later
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        guide = GameObject.FindGameObjectWithTag("Unique").GetComponent<GuideSystem>();
    }

    //This function gets called whenever the mouse hovers over the object that this script is attached to.
    //In this case, I attached it to every movement prefab so that when the mouse goes over it, it turns green
    //Allowing for more player feedback
    private void OnMouseEnter()
    {
        //    if (IsPointerOverUIElement()){
        renderer.material.color = new Color(0f, 255f, 0f, 0.01f);
       // }
    }
    //The next 3 functions were my attempt to make it so that it doesn't turn green when it's over a UI element, say
    //the settings. Unfortunately, it caused other bugs so, I don't use this section. That's why if you look 
    //at void OnMouseEnter() I commented out the if function.
    public static bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }
    public static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int i = 0; i < eventSystemRaysastResults.Count; i++)
        {
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

    //This is basically to set the color of the prefab when the mouse is not over the prefab
    //The if statement checks whether the guide system has a path, if it does turn blue, else turn red
    private void OnMouseExit()
    {
        if (guide.activated[int.Parse(renderer.name.Substring(0,3))])
        {
            renderer.material.SetColor("_Color", new Color(0f, 0f, 255f, 0.01f));
        }
        else
        {
            renderer.material.SetColor("_Color", new Color(255f, 0f, 0f, transparancy));
        }
    }
}
