using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GraphInfomation: MonoBehaviour
{

    public class node {
        public int arrow;
        public Vector3 location;
        public node(int a, Vector3 b)
        {
            arrow = a;
            location = b;
        }
    }

    public List<Material> listoflocations;
    public List<GameObject> listofArrowMovement;
    public int currentLoc;

    public List<GameObject> currArrowMovement;

    public Dictionary<Material, List<node>> arrowlocations = new Dictionary<Material, List<node>>();

    private void Awake()
    {
        currentLoc = 0;
        intializeLocations();
        intializePrefabs();
        readgraphInfo();
    }
    public void Start()
    {
        LoadNewMovements();
    }
    void intializeLocations()
    {
        Object[] templist = Resources.LoadAll("Skyboxs");
        int i = 0;
        foreach (Object temp in templist){
            listoflocations.Add(Resources.Load("Skyboxs/" + temp.name) as Material);
            i++;
        }
        RenderSettings.skybox = listoflocations[currentLoc];
    }

    void intializePrefabs()
    {
        Object[] templist = Resources.LoadAll("MovementArrows");
        int i = 0;
        foreach (Object temp in templist)
        {
            listofArrowMovement.Add(Resources.Load("MovementArrows/" + temp.name) as GameObject);
            i++;
        }
    }

    void readgraphInfo()
    {
        string path = "Assets/GraphInfo/GraphInfo.txt";
        StreamReader reader = new StreamReader(path, true);
        string temp = reader.ReadToEnd();
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


    public void LoadNewMovements()
    {
        if(arrowlocations[listoflocations[currentLoc]].Count > 0)
        {
            foreach (node g in arrowlocations[listoflocations[currentLoc]])
            {
                GameObject a = (GameObject)Instantiate(listofArrowMovement[g.arrow], g.location, transform.rotation);
                Debug.Log(a.name);
                currArrowMovement.Add(a);
            }
        }
    }

    public void deleteCurrentMovements()
    {
        foreach(GameObject g in currArrowMovement)
        {
            Destroy(g);
        }
        currArrowMovement = new List<GameObject>();
    }
}
