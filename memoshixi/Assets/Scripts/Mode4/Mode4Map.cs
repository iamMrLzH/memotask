using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mode4Map : MonoBehaviour
{
    float height;
    float width;
    public  float minx;
    float miny;
    float time = 0;//走完一张地图需要时间
    public List<GameObject> maps = new List<GameObject>();
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
        if (SceneManager.GetActiveScene().name != "Save")
            InitMap();//初始前两个地图
        else
        {
            // Destroy(maps[0]);
            // maps.RemoveAt(0);//去除上个地图
            minx = LoadInit.minx;
            InitMap();
        }
        StartCoroutine(DelMap());
        StartCoroutine(CreMap());
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
    void RandomColor(GameObject nextMap)
    {
        Transform[] allObjs = nextMap.GetComponentsInChildren<Transform>();
        foreach(Transform obj in allObjs)
        {
            if (obj.parent!=nextMap.transform&&obj!=nextMap.transform)
            {
                obj.gameObject.AddComponent<ColorCheck>();
               // Debug.Log(obj.name);
                int color = Random.Range(0, 4);
                if (color == 0)
                    obj.GetComponent<SpriteRenderer>().color = Color.red;
                if (color == 1)
                    obj.GetComponent<SpriteRenderer>().color = Color.green;
                if (color == 2)
                    obj.GetComponent<SpriteRenderer>().color = Color.blue;
                if (color == 3)
                    obj.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        }
    }
    IEnumerator CreMap()
    {
        while (true)
        {
            yield return new WaitForSeconds(time-0.5f);
            minx += width;
            GameObject nextMap = new GameObject();//添加下个地图
            nextMap.tag = "Map4";
            maps.Add(nextMap);
            InitContents(nextMap);
            RandomColor(nextMap);
        }
    }
    IEnumerator DelMap()
    {
        while (true)
        {
            yield return new WaitForSeconds(time+0.5f);
            Destroy(maps[0]);
            maps.RemoveAt(0);//去除上个地图
        }
    }
    void InitMap()
    {
        GameObject map1 = new GameObject();
        map1.tag = "Map4";
        maps.Add(map1);
        Instantiate(line, new Vector2(width + minx, miny + height / 2), Quaternion.identity, map1.transform);
        minx += width;
        GameObject map2 = new GameObject();
        map2.tag = "Map4";
        maps.Add(map2);
        InitContents(map2);
        RandomColor(map2);
    }
}
