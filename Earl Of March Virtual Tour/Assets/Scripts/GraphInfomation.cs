using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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
        public node(int a, Vector3 b)
        {
            arrow = a;
            location = b;
        }
    }

    public GameObject sphereMesh;

    public List<Material> listoflocations;
    public List<GameObject> listofArrowMovement;
    public int currentLoc;

    public List<GameObject> currArrowMovement;

    //The Material means the scene and each one has a list of nodes, which is all the prefab movement objects in
    //that specific scene.
    public Dictionary<Material, List<node>> arrowlocations = new Dictionary<Material, List<node>>();
    GuideSystem guidesystem;

    //Sets up the variables as the scripts are not yet connected with the variables.
    //You may notice that I use awake here instead of Start. Awake goes first before Start.
    //Basically I need to setup all the variables as a lot of other scripts relay on this one.
    private void Awake()
    {
        guidesystem = GetComponent<GuideSystem>();
        currentLoc = 0;
        readgraphInfo();
        sphereMesh.GetComponent<Renderer>().material = listoflocations[currentLoc];
        Debug.Log(currentLoc);
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
            for(int i = 0;i < line.Length-1;i += 22){
                int curr = int.Parse(line.Substring(i, 3));
                if(i == 0)
                {
                    arrowlocations[listoflocations[j]] = new List<node>();
                }
                arrowlocations[listoflocations[j]].Add(new node(curr,new Vector3(float.Parse(line.Substring(i+4,5)),
                                                                         float.Parse(line.Substring(i+10,5)),
                                                                         float.Parse(line.Substring(i+16,5)))));
            }
            j++;
        }
    }


    //This is goes through all the arrow movements in the scene and instantiates them
    public void LoadNewMovements()
    {
        if(arrowlocations[listoflocations[currentLoc]].Count > 0)
        {
            foreach (node g in arrowlocations[listoflocations[currentLoc]])
            {
                GameObject a = (GameObject)Instantiate(listofArrowMovement[g.arrow], g.location, transform.rotation);
                int curr = int.Parse(a.name.Substring(0,3));
                if (guidesystem.activated[curr])
                {
                    a.GetComponent<Renderer>().material.color = Color.blue;
                }
                else
                {
                    a.GetComponent<Renderer>().material.color = Color.red;
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
}
