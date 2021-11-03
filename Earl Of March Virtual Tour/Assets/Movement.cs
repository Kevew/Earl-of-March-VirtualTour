using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public GameObject cam;

    void Update(){
        if (Input.GetMouseButtonDown(0)){
            Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                if(hitInfo.collider.gameObject != null)
                {
                    int id1 = int.Parse(hitInfo.collider.name.Substring(0,3));
                    GraphInfomation graphinfo = this.gameObject.GetComponent<GraphInfomation>();
                    RenderSettings.skybox = graphinfo.listoflocations[id1];
                    graphinfo.currentLoc = id1;
                    graphinfo.deleteCurrentMovements();
                    graphinfo.LoadNewMovements();
                }
            }
        }
    }
}
