using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Navgation : MonoBehaviour
{
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //将鼠标点击的2维屏幕坐标转换为世界坐标，并发射射线
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            //如果射线扫到了物体，就向被扫到的点移动
            if (hit.collider != null)
                agent.destination = hit.point;
        }
    }
}
