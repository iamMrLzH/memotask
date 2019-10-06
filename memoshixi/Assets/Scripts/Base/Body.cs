using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Body : Node
{
    public Head instance;
    int num;
    string sceneName;
    // Start is called before the first frame update

    void Start()
    {
        instance = Head.instance;
        sceneName = instance.sceneName;
        num = instance.num;
       // Debug.Log(num+"*");
        rig = transform.GetComponent<Rigidbody2D>();
        instance.snake.Add(this);//添加进链表
        transform.localScale = new Vector3(scale, scale, 1);
       // gameObject.GetComponent<SpriteRenderer>().sprite;
        if (sceneName == "Mode4")
            gameObject.GetComponent<SpriteRenderer>().sprite = instance.Mode4Sprite;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        BodyMove();
        num = instance.num;
    }
    
    void BodyMove()
    {
        int index = instance.snake.IndexOf(this);
        //Debug.Log( num);
        transform.position = instance.snake[0].pos[instance.snake[0].pos.Count - index * num];
    }
}
