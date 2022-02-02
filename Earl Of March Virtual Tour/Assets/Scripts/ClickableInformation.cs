using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableInformation : MonoBehaviour
{
    public class node
    {
        public int selection;
        public Vector3 location;
        public node(int a, Vector3 b)
        {
            selection = a;
            location = b;
        }
    }

    List<GameObject> create;
    Dictionary<string, node> openandclose = new Dictionary<string,node>();
    GraphInfomation graphinformation;

    GameObject currentpop;

    void Awake()
    {
        currentpop = null;
        graphinformation = GetComponent<GraphInfomation>();
        setup();
    }


    //Using InformationBanner.txt, the code gets all the values for where each location should have popups
    private void setup()
    {
        Object path = Resources.Load("GuideChangeInformation");
        TextAsset reader = path as TextAsset;
        string temp = reader.text;
        string[] lines = temp.Split("\n"[0]);
        foreach (string line in lines)
        {
            openandclose[line.Substring(0, 4)] = (new node(int.Parse(line.Substring(23,1)), new Vector3(float.Parse(line.Substring(5, 5)),
                                                                            float.Parse(line.Substring(11, 5)),
                                                                            float.Parse(line.Substring(17, 5)))));
        }
    }

    //When me move to a new scene, we want to instantite the new popoops
    void onNewUI()
    {
        if (openandclose.ContainsKey(graphinformation.listoflocations[graphinformation.currentLoc].name))
        {
            currentpop = (GameObject)Instantiate(create[openandclose[graphinformation.listoflocations[graphinformation.currentLoc].name].selection],
                                     openandclose[graphinformation.listoflocations[graphinformation.currentLoc].name].location,
                                     transform.rotation);
        }
    }

    //When me move to a new scene, we want to disable the previous scenes popups;
    void onCloseUI()
    {
        if (currentpop != null)
        {
            Destroy(currentpop);
            currentpop = null;
        }
    }

}
