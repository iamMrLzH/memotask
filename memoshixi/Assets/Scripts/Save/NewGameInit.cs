using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameInit : MonoBehaviour
{
    public float width;
    float height;
    // Start is called before the first frame update
    void Start()
    {
        float minx = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
        float miny = Camera.main.ScreenToWorldPoint(Vector3.zero).y;
        width = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).x - minx;
        height = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).y - miny;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera(RandomMap.minx, RandomMap.miny, RandomMap.maxx - 1, RandomMap.maxy - 1);
    }
    void MoveCamera(float minx, float miny, float maxx, float maxy)
    {

        Vector2 pos = Head.instance.transform.position;
        float movex = pos.x;
        float movey = pos.y;
        if (pos.x - width / 2 < minx)
            movex = minx + width / 2;
        if (pos.y - height / 2 < miny)
            movey = miny + height / 2;
        if (pos.x + width / 2 > maxx)
            movex = maxx - width / 2;
        if (pos.y + height / 2 > maxy)
            movey = maxy - height / 2;
        gameObject.transform.position = new Vector3(movex, movey, transform.position.z);
    }
}
