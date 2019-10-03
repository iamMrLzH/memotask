using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mode3Map : MonoBehaviour
{
    float height;
    float width;
    float minx;
    float miny;
    GameObject wall;
    // Start is called before the first frame update
    void Start()
    {
        wall = Resources.Load<GameObject>("Prefabs/Wall");
        minx = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
        miny = Camera.main.ScreenToWorldPoint(Vector3.zero).y;
        height = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).y;
        width = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).x;
        InitWall();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void InitWall()
    {
        float dis = wall.GetComponent<SpriteRenderer>().size.y/2;
        for(int i = 0; i <= width-minx; i++)
        {
            Vector2 v = new Vector2(minx + i * dis * 2, miny + dis);
            Vector2 w = new Vector2(minx + i * dis * 2, height - dis);
            Instantiate(wall, v, Quaternion.identity);
            Instantiate(wall, w, Quaternion.identity);
        }
    }
    void RangeContents()
    {

    }
}
