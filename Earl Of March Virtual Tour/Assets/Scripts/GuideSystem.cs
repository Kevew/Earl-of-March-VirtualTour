using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GuideSystem : MonoBehaviour
{

    public List<Material> a;
    public Dictionary<string, List<Material>> edges = new Dictionary<string, List<Material>>();
    public List<bool> activated;
    GraphInfomation graphinformation;

    public Dictionary<int,int> dis = new Dictionary<int,int>();
    public Dictionary<string, string> checkLoc = new Dictionary<string, string>();

    public Material currentguide;
    public UIController uicontroller;

    public GameObject sphereMesh;

    //Sets up the variables as the scripts are not yet connected with the variables
    void Awake(){
        //Sets the entire path to nonexistant
        activated = new List<bool>();
        for (int b = 0; b < 200; b++)
        {
            activated.Add(false);
        }
        uicontroller = GetComponent<UIController>();
        graphinformation = GetComponent<GraphInfomation>();
        InitializeDict();
        //Sets all prefabs in the current location
        graphinformation.LoadNewMovements();
        graphinformation.LoadNewMovementsClick();
        IntializaeDistance();
    }


    //Set's up all the edges between the scenes so that I can create the path.
    void InitializeDict(){
        Object path = Resources.Load("GraphInfo");
        TextAsset reader = path as TextAsset;
        string temp = reader.text;
        string[] lines = temp.Split("\n"[0]);
        int j = 0;
        for(int i = 0;i < a.Count; i++)
        {
            edges[a[i].name] = new List<Material>();
        }
        foreach(string line in lines){
            for(int i = 0;i < line.Length-1; i+=42)
            {
                int curr = int.Parse(line.Substring(i, 3));
                edges[a[j].name].Add(a[curr]);
            }
            j++;
        }
    }

    //Set's up all the dist between the locations so that you don't go through classrooms
    void IntializaeDistance()
    {
        Object path = Resources.Load("TopRight");
        TextAsset reader = path as TextAsset;
        string temp = reader.text;
        string[] lines = temp.Split("\n"[0]);
        int j = 0;
        foreach (string line in lines)
        {
            if (line.Substring(4, 4) == "Room")
            {
                dis[int.Parse(line.Substring(0, 3))] = 90;
            }
            else if (line.Substring(4, 3) == "Out")
            {
                dis[int.Parse(line.Substring(0, 3))] = 5;
            }
            else
            {
                dis[int.Parse(line.Substring(0, 3))] = 1;
            }
            checkLoc[line.Substring(0, 3)] = line.Substring(4, line.Length - 4);
            j++;
        }
    }


    //This create the node which is the edge
    //Basically the first variable is the time, which allows me to later find the fastest way to reach this edge
    //location variable is for the current skybox area.
    //prev variable is for the prev skybox area so that I can backtrack and find the path.
    public class node
    {
        public int time;
        public Material location;
        public Material prev;
        public node(int a, Material b, Material c)
        {
            time = a;
            location = b;
            prev = c;
        }
    }

    //This create the node which is the prefab
    //Basically the first variable is the time, which allows me to later find the fastest way to reach this edge
    //prev variable is for the prev skybox area so that I can backtrack and find the path.
    public class fastestnode
    {
        public int time;
        public Material prev;
        public fastestnode(int a, Material b)
        {
            time = a;
            prev = b;
        }
    }

    //This function gets the path by using BFS (Breath first search) and then backtracks
    public void findpath(Material ab,Material b)
    {
        List<fastestnode> fastest = new List<fastestnode>();
        Queue<node> bfs = new Queue<node>();
        //Adds the current location to the queue
        bfs.Enqueue(new node(0,ab,ab));
        for(int i = 0;i < 200; i++){
            fastest.Add(new fastestnode(1000,ab));
        }
        //This is the bfs section,it takes the first value in the queue, checks if reached this location the fastest
        //If it isn't then just destroy that value. If it is, then still destroy but set this location's time to the
        //current one and set where we previously came from (From backtracking). From here you add all the edges
        //attached to this location to the queue. This ends when the queue is empty.
        while(bfs.Count > 0){
            node temp = bfs.Peek();
            bfs.Dequeue();
            if (temp.time < fastest[int.Parse(temp.location.name.Substring(0, 3))].time)
            {
                fastest[int.Parse(temp.location.name.Substring(0, 3))].time = temp.time;
                fastest[int.Parse(temp.location.name.Substring(0, 3))].prev = temp.prev;
                foreach (Material edge in edges[temp.location.name.Substring(0,3)])
                {
                    bfs.Enqueue(new node(temp.time + dis[int.Parse(temp.location.name.Substring(0, 3))], edge, temp.location));
                }
            }
        }
        //Resets the previous path
        for (int i = 0; i < 200; i++){
            activated[i] = false;
        }
        int fastestspeed = 9999, fastestloc = -1;
        for (int i = 0; i < 200; i++)
        {
            if(fastest[i].time < fastestspeed && checkLoc[b.name.Substring(0,3)] == checkLoc[a[i].name.Substring(0,3)])
            {
                fastestspeed = fastest[i].time;
                fastestloc = i;
            }
        }
        Material temp2 = a[fastestloc];
        currentguide = temp2;
        //This begings the backtrack. We start off with temp2 which is the location we want to go. Using the
        //fastestnode set this location prefab movement to true so it will display blue when shown.
        //Using the fastestnode go to the prev node and continue until you reach your current location.
        while (temp2 != ab){
            activated[int.Parse(temp2.name.Substring(0, 3))] = true;
            temp2 = fastest[int.Parse(temp2.name.Substring(0, 3))].prev;
        }
        //Change color of arrows of the prefabs in the current scenes as it doesn't update automatically.
        foreach(GameObject obj in graphinformation.currArrowMovement)
        {
            if (activated[int.Parse(obj.name.Substring(0,3))])
            {
                obj.GetComponent<Renderer>().material.SetColor("_Color", new Color(0f, 0f, 255f, 0.01f));
            }
        }
    }

    //It gets the location it wants to go to
    //Deletes the current prefabs in the scene
    //And load the new prefabs
    public void teleportsystem(Material setnew)
    {
        for (int i = 0; i < 200; i++)
        {
            activated[i] = false;
        }
        currentguide = null;
        graphinformation.deleteCurrentMovements();
        graphinformation.deleteCurrentMovementsClick();
        sphereMesh.GetComponent<Renderer>().material = setnew;
        graphinformation.currentLoc = int.Parse(setnew.name.Substring(0, 3));
        graphinformation.LoadNewMovements();
        graphinformation.LoadNewMovementsClick();
    }
}
