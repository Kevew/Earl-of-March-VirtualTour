using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Movement : MonoBehaviour
{
    public GameObject cam;
    public static bool test = false;
    private GuideSystem guidesystem;
    public UIController uicontroller;

    public Material testsky;

    public Text enableAtLocationText;
    public GameObject enableAtLocation;

    public GameObject sphereMesh;

    public class Direction
    {
        public float x;
        public float z;
        public Direction(float _z, float _x)
        {
            x = _x;
            z = _z;
        }
    }

    List<Direction> dirc = new List<Direction>();

    //Get variables
    //Set Direction for Movement
    void Awake()
    {
        guidesystem = GetComponent<GuideSystem>();
        uicontroller = GetComponent<UIController>();
        dirc.Add(new Direction(1, 0));
        dirc.Add(new Direction(1, 1));
        dirc.Add(new Direction(0, 1));
        dirc.Add(new Direction(-1, 1));
        dirc.Add(new Direction(-1, 0));
        dirc.Add(new Direction(-1, -1));
        dirc.Add(new Direction(0, -1));
        dirc.Add(new Direction(1, -1));
    }

    //Check's for when movement begins + animation zoom in
    private int camDirMovement = 0;
    private float timeAnim;
    private float currentTimeAnim;
    void Update(){
        //This section checks whenever a button is clicked. In doing so, it shoots out a ray and checks if it hits
        //The prefab for movement. From there, we get the information about the prefab and sent it to the function
        //that will perform the movement.
        if(Input.GetMouseButtonUp(0)){
            Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hitInfo) && !IsPointerOverUIElement()){
                if(hitInfo.collider.gameObject != null){
                    int id1 = int.Parse(hitInfo.collider.name.Substring(0,3));
                    camDirMovement = int.Parse(hitInfo.transform.tag);
                    timeAnim = Mathf.Max(uicontroller.scrollcamdepth.value * 50, 15f)/Mathf.Max(uicontroller.scrollzoom.value * 100, 20f);
                    currentTimeAnim = 0f;
                    GraphInfomation graphinfo = this.gameObject.GetComponent<GraphInfomation>();
                    Destroy(hitInfo.collider.gameObject);
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
            //This is the zoom in effect
            if (uicontroller.zoomInEnable.isOn)
            {
                float scrollvalue = uicontroller.scroll.value;
                float zoomdepth = Mathf.Max(uicontroller.scrollcamdepth.value * 50, 15f);
                if (cam.GetComponent<Camera>().fieldOfView >= Mathf.Max(scrollvalue * 100, 20f) - zoomdepth)
                {
                    cam.GetComponent<Camera>().fieldOfView -= Time.deltaTime * Mathf.Max(uicontroller.scrollzoom.value * 100, 20f);
                }
                else
                {
                    cam.GetComponent<Camera>().fieldOfView = scrollvalue * 100;
                    test = false;
                    cam.transform.position = new Vector3(50f, 50f, 50f);
                }
            }
            else
            {
                //The camera movement effect
                if(currentTimeAnim <= timeAnim)
                {
                    currentTimeAnim += Time.deltaTime;
                    cam.transform.position = cam.transform.position + new Vector3(dirc[camDirMovement].x, 0f, dirc[camDirMovement].z) / (uicontroller.scrollzoom.value * 50);
                }
                else
                {
                    cam.transform.position = new Vector3(50f, 50f, 50f);
                    test = false;
                }
            }
        }
    }

    //Performs the movement or change skybox
    IEnumerator movementTime(int id1){
        //We wait for the zoom in effect depending on the zoom speed
        yield return new WaitForSeconds(timeAnim);
        GraphInfomation graphinfo = this.gameObject.GetComponent<GraphInfomation>();
        //This line sets the location as a new place
        sphereMesh.GetComponent<Renderer>().material = graphinfo.listoflocations[id1];
        //This checks whether the guide path exists and if we reached our location, if it does it disables the path
        if (guidesystem.currentguide != null)
        {
            if (int.Parse(guidesystem.currentguide.name.Substring(0, 3)) == id1)
            {
                enableAtLocationText.text = "You've reached " + guidesystem.checkLoc[guidesystem.currentguide.name.Substring(0, 3)];
                guidesystem.currentguide = null;
                enableAtLocation.SetActive(true);
            }
        }
        graphinfo.currentLoc = id1;
        //Deletes the current scene prefabs
        graphinfo.deleteCurrentMovements();
      //  graphinfo.deleteCurrentMovementsClick();
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
       // graphinfo.LoadNewMovementsClick();
    }

    //Check if mouse over UI
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
}
