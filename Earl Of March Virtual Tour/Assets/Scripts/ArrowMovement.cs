using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    private MeshRenderer mrenderer;
    private GuideSystem guide;
    public float transparancy = 0.001f;
    //Set all the variables such as rendere and guide so I can use them later
    void Start()
    {
        mrenderer = GetComponent<MeshRenderer>();
        guide = GameObject.FindGameObjectWithTag("Unique").GetComponent<GuideSystem>();
    }

    //This function gets called whenever the mouse hovers over the object that this script is attached to.
    //In this case, I attached it to every movement prefab so that when the mouse goes over it, it turns green
    //Allowing for more player feedback
    private void OnMouseEnter()
    {
        //    if (IsPointerOverUIElement()){
        mrenderer.material.color = new Color(0f, 255f, 0f, 0.01f);
       // }
    }

    //This is basically to set the color of the prefab when the mouse is not over the prefab
    //The if statement checks whether the guide system has a path, if it does turn blue, else turn red
    private void OnMouseExit()
    {
        if (guide.activated[int.Parse(mrenderer.name.Substring(0,3))])
        {
            mrenderer.material.SetColor("_Color", new Color(0f, 0f, 255f, 0.01f));
        }
        else
        {
            mrenderer.material.SetColor("_Color", new Color(255f, 0f, 0f, transparancy));
        }
    }
}
