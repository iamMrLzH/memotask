using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NewGameInit : MonoBehaviour
{
    public float width;
    float height;
    Head instance;
    public Sprite image;
    // Start is called before the first frame update
    void Start()
    {
        instance = Head.instance;
        float minx = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
        float miny = Camera.main.ScreenToWorldPoint(Vector3.zero).y;
        width = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).x - minx;
        height = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).y - miny;
        if (instance.sceneName == "Mode1")
            instance.GetComponent<Mode2>().enabled = false;
        else
            instance.GetComponent<Mode1>().enabled = false;
        if (instance.sceneName == "Mode4")
        {
            GameObject.Find("BgCanvas/InPanel").GetComponent<Image>().sprite = image;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (instance.sceneName == "Mode3" || instance.sceneName == "Mode4")
            gameObject.transform.position = new Vector3(instance.transform.position.x, 0, transform.position.z);
        if (instance.sceneName == "Mode1")
            MoveCamera(RandomMap.minx, RandomMap.miny, RandomMap.maxx - 1, RandomMap.maxy - 1);
        if (instance.sceneName.Contains("Level"))
            MoveCamera(-4.5f, -9.5f, 19.5f, 7.5f);
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
