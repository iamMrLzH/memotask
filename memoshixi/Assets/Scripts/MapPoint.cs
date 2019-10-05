using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPoint
{
    public Vector2 pos;
    public bool hasParent = false;
    public MapPoint parent;
    public float minx = -30;
    public float miny = -30;
    public float direct = 1;
    public float G=0;
    public float H=0;
    public int count = 0;
    public float F = 0;
    public MapPoint(Vector2 v)
    {
        int sizex = (int)((v.x - minx) / direct);
        int sizey = (int)((v.y - miny) / direct);
        pos = new Vector2(sizex, sizey); 
    }
}
