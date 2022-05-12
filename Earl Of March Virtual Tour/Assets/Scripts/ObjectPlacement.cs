using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    public Vector3 loc;
    public bool hashit;
    public GameObject follow;
    public List<Transform> angles;

    public void Start()
    {
        follow.gameObject.layer = 2;
    }
    void Update()
    {
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.layer == 8)
        {
            if (!follow.activeSelf)
            {
                follow.SetActive(true);
            }
            follow.transform.rotation = Quaternion.Euler(angles[int.Parse(hit.transform.tag)].transform.eulerAngles);
            loc = hit.point;
            hashit = true;
            follow.transform.position = loc;
        }
        else
        {
            hashit = false;
            if (follow.activeSelf)
            {
                follow.SetActive(false);
            }
        }
    }
}
