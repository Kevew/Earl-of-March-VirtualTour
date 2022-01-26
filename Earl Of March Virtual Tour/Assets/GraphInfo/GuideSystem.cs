using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GuideSystem : MonoBehaviour
{

    public List<Material> a;
    public Dictionary<Material, List<Material>> edges = new Dictionary<Material, List<Material>>();
    public List<bool> activated;
    GraphInfomation graphinformation;

    public Material currentguide;
    public UIController uicontroller;

    //Sets up the variables as the scripts are not yet connected with the variables
    void Awake(){
        //Sets the entire path to nonexistant
        activated = new List<bool>();
        for (int b = 0; b < 1000; b++)
        {
            activated.Add(false);
        }
        uicontroller = GetComponent<UIController>();
        graphinformation = GetComponent<GraphInfomation>();
        a = new List<Material>();
        //This section brings in all the skyboxs as material
        Object[] templist = Resources.LoadAll("Skyboxs");
        int i = 0;
        foreach (Object temp in templist)
        {
            a.Add(Resources.Load("Skyboxs/" + temp.name) as Material);
            i++;
        }
        InitializeDict();
        //Sets all prefabs in the current location
        graphinformation.LoadNewMovements();
    }


    //Set's up all the edges between the scenes so that I can create the path.
    void InitializeDict(){
        string path = "Assets/GraphInfo/GraphInfo.txt";
        StreamReader reader = new StreamReader(path, true);
        string temp = reader.ReadToEnd();
        string[] lines = temp.Split("\n"[0]);
        int j = 0;
        for(int i = 0;i < a.Count; i++)
        {
            edges[a[i]] = new List<Material>();
        }
        foreach(string line in lines){
            for(int i = 0;i < line.Length-1; i+=22)
            {
                int curr = int.Parse(line.Substring(i, 3));
                edges[a[j]].Add(a[curr]);
            }
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
    public void findpath(Material a,Material b)
    {
        List<fastestnode> fastest = new List<fastestnode>();
        Queue<node> bfs = new Queue<node>();
        //Adds the current location to the queue
        bfs.Enqueue(new node(0,a,a));
        for(int i = 0;i < 1000; i++){
            fastest.Add(new fastestnode(1000,a));
        }
        //This is the bfs section,it takes the first value in the queue, checks if reached this location the fastest
        //If it isn't then just destroy that value. If it is, then still destroy but set this location's time to the
        //current one and set where we previously came from (From backtracking). From here you add all the edges
        //attached to this location to the queue. This ends when the queue is empty.
        while(bfs.Count > 0){
            node temp = bfs.Peek();
            bfs.Dequeue();
            if(temp.time < fastest[int.Parse(temp.location.name)].time)
            {
                fastest[int.Parse(temp.location.name)].time = temp.time;
                fastest[int.Parse(temp.location.name)].prev = temp.prev;
                foreach (Material edge in edges[temp.location])
                {
                    bfs.Enqueue(new node(temp.time + 1, edge, temp.location));
                }
            }
        }
        //Resets the previous path
        Material temp2 = b;
        for (int i = 0; i < 1000; i++){
            activated[i] = false;
        }
        //This begings the backtrack. We start off with temp2 which is the location we want to go. Using the
        //fastestnode set this location prefab movement to true so it will display blue when shown.
        //Using the fastestnode go to the prev node and continue until you reach your current location.
        while (temp2 != a){
            activated[int.Parse(temp2.name)] = true;
            temp2 = fastest[int.Parse(temp2.name)].prev;
        }
        //Change color of arrows of the prefabs in the current scenes as it doesn't update automatically.
        foreach(GameObject obj in graphinformation.currArrowMovement)
        {
            if (activated[int.Parse(obj.name.Substring(0,3))])
            {
                obj.GetComponent<Renderer>().material.color = Color.blue;
            }
        }
        currentguide = b;
    }

    //It gets the location it wants to go to
    //Deletes the current prefabs in the scene
    //And load the new prefabs
    public void teleportsystem(Material setnew)
    {
        graphinformation.deleteCurrentMovements();
        RenderSettings.skybox = setnew;
        graphinformation.currentLoc = int.Parse(setnew.name);
        graphinformation.LoadNewMovements();
    }
}
