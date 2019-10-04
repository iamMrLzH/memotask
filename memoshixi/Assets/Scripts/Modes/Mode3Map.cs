using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class Mode3Map : MonoBehaviour
{
    float height;
    float width;
    float minx;
    float miny;
    GameObject wall;
    GameObject sphere;
    GameObject diamond;
    float time=0;//走完一张地图需要时间
    List<GameObject> maps = new List<GameObject>();
    Sprite[] diaColor;
    // Start is called before the first frame update
    void Start()
    {
        diaColor = Resources.LoadAll<Sprite>("Colors");
        wall = Resources.Load<GameObject>("Prefabs/Wall");
        sphere = Resources.Load<GameObject>("Prefabs/WhiteBody");
        diamond= Resources.Load<GameObject>("Prefabs/WhiteDiamonds");
        minx = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
        miny = Camera.main.ScreenToWorldPoint(Vector3.zero).y;
        height = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).y-miny;
        width = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).x-minx;
        time = width / Node.speed;
       // Debug.Log(height);
        InitMap();//初始前两个地图
        StartCoroutine(RangeMap());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void InitWall(GameObject nextMap)
    {
        float dis = wall.GetComponent<BoxCollider2D>().size.y/2;
        for(int i = 0; i <= width; i++)
        {
            Vector2 v = new Vector2(minx + i * dis * 2, miny + dis);
            Vector2 w = new Vector2(minx + i * dis * 2, height - dis+miny);
            GameObject wall1= Instantiate(wall, v, Quaternion.identity);
            GameObject wall2= Instantiate(wall, w, Quaternion.identity);
            wall1.transform.parent = nextMap.transform;
            wall2.transform.parent = nextMap.transform;
        }
    }
    void InitContents(GameObject nextMap)
    {
        float d = wall.GetComponent<BoxCollider2D>().size.y/2;
        float bounds = d;
        float interval = (height - 4 * d-2*bounds) / 5f;
        for(int i=0;i<=5;i++)
        {
            Vector2 pos1 = new Vector2(width / 3+minx, d*2 +bounds+ interval*i+miny);
            Vector2 pos2 = new Vector2(width * 2 / 3+minx, d *2+bounds+ interval * i+miny);
            Instantiate(sphere, pos1, Quaternion.identity).transform.parent = nextMap.transform;
            Instantiate(sphere, pos2, Quaternion.identity).transform.parent = nextMap.transform;
        }
        float w = diamond.GetComponent<BoxCollider2D>().size.y;
        for(int i = 0; i <= 3; i++)
        {
            Vector2 pos = new Vector2(width+minx, w / 2 +d*2+ w * i+miny);
            Instantiate(diamond, pos, Quaternion.identity).transform.parent = nextMap.transform;
        }
    }
    void RandomText(GameObject nextMap)
    {
        Random.InitState(DateTime.Now.Second);
        GameObject[] allSpheres = GameObject.FindGameObjectsWithTag("Sphere");
        foreach(GameObject sphere in allSpheres)
        {
            if (sphere.transform.parent == nextMap.transform)
            {
                Text t = sphere.GetComponentInChildren<Text>();
                t.text = ((int)Random.Range(0, 5)).ToString();
                if (t.text == "0")
                {
                    Destroy(sphere);
                }
            } 
        }
        GameObject[] allDiamonds = GameObject.FindGameObjectsWithTag("Diamond");
        int index = 0;
        foreach (GameObject diamond in allDiamonds)
        {
            if (diamond.transform.parent == nextMap.transform)
            {
                Text t = diamond.GetComponentInChildren<Text>();
                t.text = ((int)Random.Range(1, 15)).ToString();
                diamond.GetComponent<SpriteRenderer>().sprite = diaColor[index];
                index++;
            }
        }
    }
    IEnumerator RangeMap()
    {
        while (true)
        {

            yield return new WaitForSeconds(time);
            minx += width;
            maps.RemoveAt(0);//去除上个地图
            GameObject nextMap = new GameObject();//添加下个地图
            maps.Add(nextMap);
            InitWall(nextMap);
            InitContents(nextMap);
            RandomText(nextMap);
        }
    }
    void InitMap()
    {
        GameObject map1 = new GameObject();
        InitWall(map1);
        maps.Add(map1);
        minx += width;
        GameObject map2 = new GameObject();
        InitWall(map2);
        maps.Add(map2);
    }
}
