using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class GraphInfomation: MonoBehaviour
{
    //This creates a class of node.
    //Basically they are the prefabs for where you will go
    //The first variable is which skybox it will lead to. It is a integer which when placed in listofArrowMovement 
    //will give you to conresbonding skybox/scene.
    //The second variable is where the prefab will be in the scene.
    public class node {
        public int arrow;
        public Vector3 location;
        public Vector3 scale;
        public int rotation;
        public node(int a, Vector3 b, Vector3 c, int d)
        {
            arrow = a;
            location = b;
            scale = c;
            rotation = d;
        }
    }

    private Dictionary<int, List<clicknode>> listofPops = new Dictionary<int, List<clicknode>>();
    List<GameObject> currPopups = new List<GameObject>();
    public GameObject popupGO;
    public class clicknode
    {
        public Vector3 location;
        public Vector3 scale;
        public int name;
        public clicknode(Vector3 _location, Vector3 _scale,int _name)
        {
            location = _location;
            scale = _scale;
            name = _name;
        }
    }


    public GameObject sphereMesh;

    public List<Material> listoflocations;
    public GameObject ArrowMovement;
    public int currentLoc;

    public List<GameObject> currArrowMovement;

    private UIController uicontroller;

    //The Material means the scene and each one has a list of nodes, which is all the prefab movement objects in
    //that specific scene.
    public Dictionary<Material, List<node>> arrowlocations = new Dictionary<Material, List<node>>();

    public Dictionary<int, string> toprighttext = new Dictionary<int, string>();
    GuideSystem guidesystem;

    public ClickableInformation click;

    //Sets up the variables as the scripts are not yet connected with the variables.
    //You may notice that I use awake here instead of Start. Awake goes first before Start.
    //Basically I need to setup all the variables as a lot of other scripts relay on this one.
    private void Awake()
    {
        guidesystem = GetComponent<GuideSystem>();
        uicontroller = GetComponent<UIController>();
        click = GetComponent<ClickableInformation>();
        currentLoc = 105;
        readgraphInfo();
        readInformationBanner();
        readtopRight();
        sphereMesh.GetComponent<Renderer>().material = listoflocations[currentLoc];
    }

    //Using InformationBanner.txt, the code gets all the values for where each location should have popups
    private void readInformationBanner()
    {
        Object path = Resources.Load("InformationBanner");
        TextAsset reader = path as TextAsset;
        string temp = reader.text;
        string[] lines = temp.Split("\n"[0]);
        int i = 0;
        int place = 0;
        //Vector3.up is placeholder
        Vector3 location = Vector3.up, scale = Vector3.up;
        for(int j = 0;j <= 200; j++)
        {
            listofPops[j] = new List<clicknode>();
        }
        foreach (string line in lines)
        {
            if (i % 3 == 0)
            {
                place = int.Parse(line.Substring(0, 3));
                location = new Vector3(float.Parse(line.Substring(4, 5)),
                                       float.Parse(line.Substring(10, 5)),
                                       float.Parse(line.Substring(16, 5)));
                scale = new Vector3(float.Parse(line.Substring(22, 5)),
                                       float.Parse(line.Substring(28, 5)),
                                       float.Parse(line.Substring(34, 5)));
                listofPops[place].Add(new clicknode(location, scale,i/3));
            }
            i++;
        }
    }


    void readtopRight()
    {
        Object path = Resources.Load("TopRight");
        TextAsset reader = path as TextAsset;
        string temp = reader.text;
        string[] lines = temp.Split("\n"[0]);
        int i = 0;
        foreach (string line in lines){
            string curr = line.Substring(4, line.Length-4);
            toprighttext[i] = curr;
            i++;
        }
    }

    //This one sets all the locations for the prefabs movement objects.
    void readgraphInfo()
    {
        Object path = Resources.Load("GraphInfo");
        TextAsset reader = path as TextAsset;
        string temp = reader.text;
        string[] lines = temp.Split("\n"[0]);
        int j = 0;
        foreach (string line in lines){
            for(int i = 0;i < line.Length-1;i += 42){
                int curr = int.Parse(line.Substring(i, 3));
                if(i == 0)
                {
                    arrowlocations[listoflocations[j]] = new List<node>();
                }
                arrowlocations[listoflocations[j]].Add(new node(curr,new Vector3(float.Parse(line.Substring(i+4,5)),
                                                                         float.Parse(line.Substring(i+10,5)),
                                                                         float.Parse(line.Substring(i+16,5))),
                                                                     new Vector3(float.Parse(line.Substring(i + 22, 5)),
                                                                         float.Parse(line.Substring(i + 28, 5)),
                                                                         float.Parse(line.Substring(i + 34, 5))),
                                                                     int.Parse(line.Substring(i + 40, 1))));
            }
            j++;
        }
    }


    //This is goes through all the arrow movements in the scene and instantiates them
    public void LoadNewMovements()
    {
        uicontroller.checkTopRight(toprighttext[currentLoc]);
        if(arrowlocations[listoflocations[currentLoc]].Count > 0)
        {
            foreach (node g in arrowlocations[listoflocations[currentLoc]])
            {
                GameObject a = (GameObject)Instantiate(ArrowMovement, g.location, transform.rotation);
                string newname = g.arrow.ToString();
                while(newname.Length < 3)
                {
                    newname = "0"+ newname;
                }
                a.name = newname;
                a.transform.localScale = g.scale;
                a.tag = g.rotation.ToString();
                int curr = int.Parse(a.name.Substring(0,3));
                if (guidesystem.activated[curr])
                {
                    a.GetComponent<Renderer>().material.SetColor("_Color", new Color(0f, 0f, 255f, 0.01f));
                }
                else
                {
                    a.GetComponent<Renderer>().material.SetColor("_Color", new Color(255f, 0f, 0f, 0.001f));
                }
                currArrowMovement.Add(a);
            }
        }
    }

    //This destroys every object currently in the scene. As when you move into the new scene, you 
    //don't want to previous scene's objects to still be there.
    public void deleteCurrentMovements(){
        foreach(GameObject g in currArrowMovement){
            Destroy(g);
        }
        currArrowMovement = new List<GameObject>();
    }

    //SameAsAbove but for clickpopups
    public void LoadNewMovementsClick()
    {
        click.closePopup();
        if (listofPops[currentLoc].Count > 0)
        {
            foreach (clicknode g in listofPops[currentLoc])
            {
                GameObject a = (GameObject)Instantiate(popupGO, g.location, transform.rotation);
                a.transform.localScale = g.scale;
                a.name = g.name.ToString();
                currPopups.Add(a);
            }
        }
    }
    public void deleteCurrentMovementsClick()
    {
        foreach (GameObject g in currPopups)
        {
            Destroy(g);
        }
        currPopups = new List<GameObject>();
    }
}
