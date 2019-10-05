using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameInit : MonoBehaviour//初始化游戏内部
{
    GameObject headPre;
    GameObject bodyObj;
    GameObject head;
    Vector3 initPosition1 = new Vector3(0, 0, 0);
    string sceneName;
    public float width;
    float height;
    public static GameInit instance;
    private void Awake()
    {
        instance = this;
        sceneName = SceneManager.GetActiveScene().name;
        float minx = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
        float miny = Camera.main.ScreenToWorldPoint(Vector3.zero).y;
        width = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).x - minx;
        height = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).y - miny;
        headPre = Resources.Load<GameObject>("Prefabs/head");
        head = Instantiate(headPre, initPosition1, Quaternion.identity);
        //Sprite headSprite = 
        // = headSprite;
        if (SceneManager.GetActiveScene().name == "Mode3")
            Node.redius /= 2;
        if (sceneName == "Mode4")
        {
            
            Node.scale /=2;
            Node.redius /= 2;
            Head.instance.InitPos((int)(width / 2 / (Node.redius * Node.scale)) * 2+1);
            head.GetComponent<SpriteRenderer>().sprite = Head.instance.Mode4Sprite;
        }
        else
            Head.instance.InitPos(30);
    }
    // Start is called before the first frame update
    void Start()
    {
        if (sceneName == "Mode1")
            head.GetComponent<Mode2>().enabled = false;
        else
            head.GetComponent<Mode1>().enabled = false;
        //head.transform.localScale = new Vector3(Node.scale, Node.scale, 1);
        //bodyObj = Resources.Load<GameObject>("Prefabs/SnakeBody" + Node.bodySkin);
        //GameObject initBody1 = Instantiate(bodyObj);//初始身体
        //GameObject initBody2 = Instantiate(bodyObj);
        if (sceneName == "Mode1")
            RandomMap.Init();
        //获取屏幕宽高
       
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(2);
        if (head.gameObject.activeSelf == true)
        {
            if (sceneName == "Mode3"||sceneName=="Mode4")
                gameObject.transform.position = new Vector3(head.transform.position.x, 0, transform.position.z);
            if (sceneName == "Mode1")
                MoveCamera(RandomMap.minx, RandomMap.miny, RandomMap.maxx - 1, RandomMap.maxy - 1);
            if (sceneName.Contains("Level"))
                MoveCamera(-4.5f, -9.5f, 19.5f, 7.5f);

        }
    }
    void MoveCamera(float minx,float miny, float maxx, float maxy)
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
