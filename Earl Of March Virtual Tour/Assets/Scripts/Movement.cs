using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public GameObject cam;
    public static bool test = false;
    private GuideSystem guidesystem;
    public UIController uicontroller;

    public Text enableAtLocationText;
    public GameObject enableAtLocation;

    public GameObject sphereMesh;

    //Get variables
    void Awake()
    {
        guidesystem = GetComponent<GuideSystem>();
        uicontroller = GetComponent<UIController>();
    }

    //Check's for when movement begins + animation zoom in
    void Update(){
        //This section checks whenever a button is clicked. In doing so, it shoots out a ray and checks if it hits
        //The prefab for movement. From there, we get the information about the prefab and sent it to the function
        //that will perform the movement.
        if(Input.GetMouseButtonDown(0)){
            Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hitInfo)){
                if(hitInfo.collider.gameObject != null){
                    int id1 = int.Parse(hitInfo.collider.name.Substring(0,3));
                    GraphInfomation graphinfo = this.gameObject.GetComponent<GraphInfomation>();
                    if (!test && !uicontroller.options.activeSelf)
                    {
                        StartCoroutine(movementTime(id1));
                    }
                    test = true;
                }
            }
        }
        //One of the variables is the "test", basically when it's acitvated it will sent zoom in the camera or 
        //decrease field of view. It creates the zoom in effect for when you move.
        if (test){
            if(cam.GetComponent<Camera>().fieldOfView >= Mathf.Max(uicontroller.scroll.value*100,20f)-15){
                cam.GetComponent<Camera>().fieldOfView -= Time.deltaTime*Mathf.Max(uicontroller.scrollzoom.value*100,20f);
            }
            else{
                cam.GetComponent<Camera>().fieldOfView = uicontroller.scroll.value * 100;
                test = false;
            }
        }
    }

    //Performs the movement or change skybox
    IEnumerator movementTime(int id1){
        //We wait for the zoom in effect depending on the zoom speed
        yield return new WaitForSeconds(15/Mathf.Max(uicontroller.scrollzoom.value*100, 20f));
        GraphInfomation graphinfo = this.gameObject.GetComponent<GraphInfomation>();
        //This line sets the location as a new place
        sphereMesh.GetComponent<Renderer>().material = graphinfo.listoflocations[id1];
        //This checks whether the guide path exists and if we reached our location, if it does it disables the path
        if(guidesystem.currentguide != null)
        {
            if (int.Parse(guidesystem.currentguide.name.Substring(0, 3)) == id1)
            {
                enableAtLocationText.text = "You've reached " + uicontroller.seinfo[int.Parse(guidesystem.currentguide.name.Substring(0, 3))];
                guidesystem.currentguide = null;
                enableAtLocation.SetActive(true);
            }
        }
        graphinfo.currentLoc = id1;
        //Deletes the current scene prefabs
        graphinfo.deleteCurrentMovements();
        //You want to keep checking if we are on the path if it exists. If it exists and we just enter a new scene
        //that is on the graph, we set the path we just went on to false, so it will no longer display a blue prefab
        //If the path is activated but we did not go on a blue prefab, we have to recalculate the path as we went off
        //the path.
        if (guidesystem.activated[id1])
        {
            guidesystem.activated[id1] = false;
        }
        else
        {
            if(guidesystem.currentguide != null)
            {
                //Create new path
                guidesystem.findpath(sphereMesh.GetComponent<Renderer>().material, guidesystem.currentguide);
            }
        }
        //Adds the new scene prefabs
        graphinfo.LoadNewMovements();
    }
}
