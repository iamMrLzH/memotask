using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : Node
{
    public Head instance;
    int num;
    // Start is called before the first frame update

    void Start()
    {
        instance = Head.instance;
        num = instance.num;
       // Debug.Log(num+"*");
        rig = transform.GetComponent<Rigidbody2D>();
        instance.snake.Add(this);//添加进链表
        transform.localScale = new Vector3(scale, scale, 1);
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
        //Debug.Log(instance.snake[0].pos.Count);
        //Debug.Log( num);
        transform.position = instance.snake[0].pos[instance.snake[0].pos.Count - index * num];
    }
}
