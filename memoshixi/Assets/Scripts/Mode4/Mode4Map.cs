using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mode4Map : MonoBehaviour
{
    float height;
    float width;
    float minx;
    float miny;
    float time = 0;//走完一张地图需要时间
    List<GameObject> maps = new List<GameObject>();
    GameObject[] RandomObjs;
    Sprite[] diaColor;
    GameObject line;
    int count = 0;//Mode4物体个数
    // Start is called before the first frame update
    void Start()
    {
        line = Resources.Load<GameObject>("Mode4Line/Line");
        RandomObjs = Resources.LoadAll<GameObject>("Mode4");
        count = RandomObjs.Length;
        minx = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
        miny = Camera.main.ScreenToWorldPoint(Vector3.zero).y;
        height = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).y - miny;
        width = (Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).x - minx) * 2;
        time = width / Node.speed;
        InitMap();//初始前两个地图
        StartCoroutine(RangeMap());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void InitContents(GameObject nextMap)
    {
        Random.InitState(System.DateTime.Now.Second);
        int random1 = Random.Range(0, count);
        int random2 = Random.Range(0, count);
        int random3 = Random.Range(0, 2);
        Instantiate(RandomObjs[random1], new Vector2(width / 3 + minx, miny + height / 2), Quaternion.identity, nextMap.transform);
        Instantiate(RandomObjs[random2], new Vector2(width*2 / 3 + minx, miny + height / 2), Quaternion.identity, nextMap.transform);
        if(random3==1)
            Instantiate(line, new Vector2(width + minx, miny + height / 2), Quaternion.identity, nextMap.transform);
        else
        {
            int random4 = Random.Range(0, count);
            Instantiate(RandomObjs[random4], new Vector2(width + minx, miny + height / 2), Quaternion.identity, nextMap.transform);
        }
    }
    void RandomColor()
    {

    }
}
