using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public static class RandomMap
{
    public static float minx = -30f;
    public static float maxx = 40f;
    public static float miny = -30f;
    public static float maxy = 12f;
    public static int number = 30;
    static float direct = 1;
    static GameObject[] Objs;
    public static Vector2[,] map;
    static bool[,] putMap;
    public static bool[,] canMap;
    static int sizex;
    static int sizey;
    public static GameObject wall;
    public static GameObject foodPre;
    public static GameObject head;
    public static List<Vector2> food = new List<Vector2>();
    // Start is called before the first frame update
    public static void Init()
    {
        foodPre = Resources.Load<GameObject>("RandomItems/score");
        wall = Resources.Load<GameObject>("RandomItems/smallwall100");
        Objs = Resources.LoadAll<GameObject>("RandomItems");
        sizex = (int)((maxx - minx) / direct);
        sizey = (int)((maxy - miny) / direct);
        map = new Vector2[sizex,sizey];
        putMap = new bool[sizex, sizey];
        canMap = new bool[sizex, sizey];
        float a = minx;
        float b = miny;
        for (int i = 0; i <= sizex - 1; i++)
        {
            for (int j = 0; j <= sizey - 1; j++)
            {
                map[i,j] = new Vector2(a, b);
                b += direct;
            }
            b = miny;
            a += direct;
        }
        for (int i = 0; i <= sizex - 1; i++)
        {
            for (int j = 0; j <= sizey - 1; j++)
            {
                putMap[i, j] = true;
                canMap[i, j] = true;
            }
        }
        GetRandomMap();
    }
    public static Vector2 PostoMap(Vector2 pos)
    {
        for (int i = 0; i <= sizex - 1; i++)
        {
            for (int j = 0; j <= sizey - 1; j++)
            {
                if (map[i, j] == pos)
                    return new Vector2(i, j);
            }

        }
        return Vector2.zero;
    }
    static void GetRandomMap()
    {
        for (int i = 0; i <= sizex - 1; i++)//制造边界围墙
        {
            canMap[i,0] = false;
            canMap[i, sizey - 1] = false;
            putMap[i, 0] = false;
            putMap[i, sizey - 1] = false;
            Object.Instantiate(wall, map[i,0], Quaternion.identity);
            Object.Instantiate(wall, map[i, sizey - 1], Quaternion.identity);
        }
        for (int j = 0; j <= sizey - 1; j++)
        {
            putMap[0, j] = false;
            putMap[sizex - 1, j] = false;
            canMap[0, j] = false;
            canMap[sizex - 1, j] = false;
            Object.Instantiate(wall, map[0, j], Quaternion.identity);
            Object.Instantiate(wall, map[sizex - 1, j], Quaternion.identity);
        }
        Random.InitState(DateTime.Now.Second);
        int x;
        int y;
        int ran;
        for (int i = 1; i <= number; i++)
        {
            x = Random.Range(0, sizex);
            y = Random.Range(0, sizey);
            while (putMap[x, y] == false)
            {
                x = Random.Range(0, sizex);
                y = Random.Range(0, sizey);
            }
            ran = Random.Range(0, Objs.Length);
            Vector2 pos = map[x, y];
            Object.Instantiate(Objs[ran], pos, Quaternion.identity);
            putMap[x, y] = false;
            if (Objs[ran].tag == "Wall" || Objs[ran].tag == "Grass" || Objs[ran].tag == "Bomb")//标注不可通过
                canMap[x, y] = false;
            if (Objs[ran].tag == "Food")
                food.Add(pos);
        }
        for (int i = 1; i <= 10; i++)
        {
            x = Random.Range(0, sizex);
            y = Random.Range(0, sizey);
            while (putMap[x, y] == false)
            {
                x = Random.Range(0, sizex);
                y = Random.Range(0, sizey);
            }
            Vector2 pos = map[x, y];
            Object.Instantiate(foodPre, pos, Quaternion.identity);
            putMap[x, y] = false;
                food.Add(pos);
        }
    }
    static List<MapPoint> openList;
    static List<MapPoint> closeList;
    static MapPoint originPoint;
    static MapPoint curPoint;
    public static List<Vector2> Astar(MapPoint target)//A*算法
    {
        head = GameObject.FindGameObjectWithTag("Head");
        openList = new List<MapPoint>();
        closeList = new List<MapPoint>();
       // GameObject head = GameObject.FindGameObjectWithTag("Head");
        originPoint = new MapPoint(head.transform.position);
        //originPoint.parent.pos=Vector2.zero;
        originPoint.H= (Mathf.Abs(target.pos.x - originPoint.pos.x) + Mathf.Abs(target.pos.y - originPoint.pos.y)) * direct;
        //Debug.Log(originPoint.H);
        //Debug.Log(head.transform.position);
        openList.Add(originPoint);
        for(int i = 0; i <= 100; i++)
        {
            MapPoint point = SeekOpenList();
            openList.Remove(point);
            closeList.Add(point);
            SerNear(point, target);
            if (IsInOpenList(target))
            {
                break;
            }
                
        }
        /*while (!openList.Contains(target))//找到前一直循环
        {
            MapPoint point = SeekOpenList();
            openList.Remove(point);
            closeList.Add(point);
            SerNear(point, target);
        }*/
        MapPoint realTar = target;
        foreach(MapPoint p in openList)
        {
            if (p.pos == target.pos)
            {
                realTar = p;
                break;
            }
               
        }
        return Finish(realTar);
    }
    static MapPoint SeekOpenList()//遍历openlist
    {
        float f = openList[0].F;
        MapPoint ret = openList[0];
        foreach (MapPoint p in openList)
        {
            if (p.F < f)
            {
                f = p.F;
                ret = p;
            }
            
        }
       // Debug.Log(ret.F +"-" + ret.G + "-" + ret.H + "-" + (ret.pos.x-30) + "-" + (ret.pos.y-30));
       // GameObject.Instantiate(wall, new Vector2(ret.pos.x - 30, ret.pos.y - 30), Quaternion.identity);
        return ret;
    }
    static void SerNear(MapPoint curPoint, MapPoint target)
    {
        Vector2 s = curPoint.pos;
        Vector2 a = map[(int)s.x,(int)s.y];
       // Debug.Log(a);
        //Debug.Log(new MapPoint(a).pos);
        MapPoint up = new MapPoint(new Vector2(a.x, a.y + direct)); up.count = 0;
        MapPoint down = new MapPoint(new Vector2(a.x, a.y - direct)); down.count = 0;
        MapPoint left = new MapPoint(new Vector2(a.x - direct, a.y)); left.count = 0;
        MapPoint right = new MapPoint(new Vector2(a.x + direct, a.y)); right.count = 0;
        MapPoint upLeft = new MapPoint(new Vector2(a.x - direct, a.y + direct)); upLeft.count = 1;
        MapPoint upRight = new MapPoint(new Vector2(a.x + direct, a.y + direct)); upRight.count = 1;
        MapPoint downLeft = new MapPoint(new Vector2(a.x - direct, a.y - direct)); downLeft.count = 1;
        MapPoint downRight = new MapPoint(new Vector2(a.x + direct, a.y + direct)); downRight.count = 1;
        List<MapPoint> near = new List<MapPoint>();
        near.Add(up);
        near.Add(down);
        near.Add(left);
        near.Add(right);
        near.Add(upLeft);
        near.Add(upRight);
        near.Add(downLeft);
        near.Add(downRight);
        foreach (MapPoint p in near)
        {
          
            if (IsInCloseList(p) || CanNotReach(p, near))
            {
                //Debug.Log("我");
                continue;
            }
               
            if (!IsInOpenList(p))
            {
                openList.Add(p);
                p.parent = curPoint;
                p.hasParent = true;
                if (p.count == 0)
                    p.G = curPoint.G + direct;
                else
                    p.G = curPoint.G + direct * 1.41f;
                p.H = (Mathf.Abs(target.pos.x - p.pos.x) + Mathf.Abs(target.pos.y - p.pos.y)) * direct;
            }
            else
            {
                float g;
                if (p.count == 0)
                    g = curPoint.G + direct;
                else
                    g = curPoint.G + direct * 1.41f;
                if (g < p.G)
                {
                    p.G = g;
                    p.parent = curPoint;
                    p.hasParent = true;
                }
            }
        }
    }
    static bool CanNotReach(MapPoint p, List<MapPoint> near)
    {
        //Debug.Log(p.pos);
        if (canMap[(int)p.pos.x, (int)p.pos.y] == false)
            return true;
        if (p.count == 1)
        {
            //Debug.Log(1);
            int index = near.IndexOf(p);
            if (index == 4)
            {
                if (canMap[(int)near[0].pos.x, (int)near[0].pos.y] == false || canMap[(int)near[2].pos.x, (int)near[2].pos.y] == false)
                    return true;
            }
            if (index == 5)
            {
                if (canMap[(int)near[0].pos.x, (int)near[0].pos.y] == false || canMap[(int)near[3].pos.x, (int)near[3].pos.y] == false)
                    return true;
            }
            if (index == 6)
            {
                if (canMap[(int)near[1].pos.x, (int)near[1].pos.y] == false || canMap[(int)near[2].pos.x, (int)near[2].pos.y] == false)
                    return true;
            }
            if (index == 7)
            {
                if (canMap[(int)near[1].pos.x, (int)near[1].pos.y] == false || canMap[(int)near[3].pos.x, (int)near[3].pos.y] == false)
                    return true;
            }
        }
        // Debug.Log(2);
        return false;
    }
    static List<Vector2> Finish(MapPoint target)
    {
        List<Vector2> path = new List<Vector2>();
        MapPoint p = target;
        while (p.hasParent==true)
        {
            path.Add(map[(int)p.pos.x, (int)p.pos.y]);
            p = p.parent;
        }
        path.Add(head.transform.position);
        if (path.Count == 1)
            path.Add(head.transform.position);
        //Debug.Log(path.Count+"mmm");
        path.Reverse();
        return path;
    }
    static bool IsInCloseList(MapPoint point)
    {
        foreach(MapPoint p in closeList)
        {
            if (p.pos == point.pos)
                return true;
        }
        return false;
    }
    static bool IsInOpenList(MapPoint point)
    {
        foreach (MapPoint p in openList)
        {
            if (p.pos == point.pos)
                return true;
        }
        return false;
    }
}
