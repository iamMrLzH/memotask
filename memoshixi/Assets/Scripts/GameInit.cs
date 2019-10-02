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
    Vector3 initPosition2 = new Vector3(-1 * Node.redius * 1 * Node.scale, 0, 0);
    Vector3 initPosition3 = new Vector3(-1 * Node.redius * 2 * Node.scale, 0, 0);
    // Start is called before the first frame update
    void Start()
    { 
        //head = new GameObject("head");
        headPre= Resources.Load<GameObject>("Prefabs/head");
        head= Instantiate(headPre, initPosition1, Quaternion.identity);
        Sprite headSprite = Resources.Load<Sprite>("Snake/Snakehead" + Node.headSkin);
        //head.AddComponent<SpriteRenderer>();
        head.GetComponent<SpriteRenderer>().sprite = headSprite;
        head.transform.localScale = new Vector3(Node.scale, Node.scale, 1);
        //Rigidbody2D rig = head.AddComponent<Rigidbody2D>();
        //rig.gravityScale = 0;
        //rig.constraints = RigidbodyConstraints2D.FreezeRotation;
        //head.AddComponent<CircleCollider2D>();//碰撞器

        bodyObj = Resources.Load<GameObject>("Prefabs/SnakeBody" + Node.bodySkin);
        //head.transform.position = initPosition1;
        //head.AddComponent<Head>(); 
        GameObject initBody1 = Instantiate(bodyObj, initPosition2, Quaternion.identity);//初始身体
        GameObject initBody2 = Instantiate(bodyObj, initPosition3, Quaternion.identity);
        if (SceneManager.GetActiveScene().name == "Mode1")
            RandomMap.Init();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(head.transform.position.x, head.transform.position.y, transform.position.z);
    }
}
