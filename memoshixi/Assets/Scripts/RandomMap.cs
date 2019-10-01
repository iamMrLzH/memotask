using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RandomMap : MonoBehaviour
{
    public float minx = -18f;
    public float maxx = 30f;
    public float miny = -13f;
    public float maxy = 30f;
    public int number = 15;
    GameObject[] Objs;
    // Start is called before the first frame update
    void Start()
    {

        Objs = Resources.LoadAll<GameObject>("RandomItems");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void GetRandomMap()
    {
        UnityEngine.Random.InitState(DateTime.Now.Second);
        float x;
        float y;
        int ran;
        for(int i = 1; i <= number; i++)
        {
            x = minx + UnityEngine.Random.value * (maxx - minx);
            y = miny + UnityEngine.Random.value * (maxy - miny);
            ran = (int)UnityEngine.Random.value * 6;
            Vector3 pos = new Vector3(x, y, 0);
            Instantiate(Objs[ran], pos, Quaternion.identity);
        }
    }
}
