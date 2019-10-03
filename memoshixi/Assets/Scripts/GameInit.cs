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
    Vector3 initPosition2 ;
    Vector3 initPosition3 ;
    private void Awake()
    {
        //Debug.Log(0);
        headPre = Resources.Load<GameObject>("Prefabs/head");
        head = Instantiate(headPre, initPosition1, Quaternion.identity);
       
    }
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(0);
        //head = new GameObject("head");
        if (SceneManager.GetActiveScene().name == "Mode1")
            head.GetComponent<Mode2>().enabled = false;
        else
            head.GetComponent<Mode1>().enabled = false;
        initPosition2 = new Vector3(-1 * Head.instance.redius * Head.instance.scale, 0, 0);
        initPosition3 = new Vector3(-1 * Head.instance.redius * Head.instance.scale, 0, 0);
        Sprite headSprite = Resources.Load<Sprite>("Snake/Snakehead" + Node.headSkin);
        //head.AddComponent<SpriteRenderer>();
        head.GetComponent<SpriteRenderer>().sprite = headSprite;
        head.transform.localScale = new Vector3(Head.instance.scale, Head.instance.scale, 1);
        bodyObj = Resources.Load<GameObject>("Prefabs/SnakeBody" + Node.bodySkin);
        //head.transform.position = initPosition1;
        //head.AddComponent<Head>(); 
        Debug.Log("Prefabs/SnakeBody" + Node.bodySkin);
        GameObject initBody1 = Instantiate(bodyObj, initPosition2, Quaternion.identity);//初始身体
        GameObject initBody2 = Instantiate(bodyObj, initPosition3, Quaternion.identity);
        if (SceneManager.GetActiveScene().name == "Mode1")
            RandomMap.Init();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(2);
        if(head.gameObject.activeSelf==true)
        gameObject.transform.position = new Vector3(head.transform.position.x, head.transform.position.y, transform.position.z);
    }
}
