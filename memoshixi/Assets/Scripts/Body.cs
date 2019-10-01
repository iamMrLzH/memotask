using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : Node
{
    // Start is called before the first frame update
    void Start()
    {
        rig = transform.GetComponent<Rigidbody2D>();
        this.position = transform.position;
        Head.snake.Add(this);//添加进链表
    }

    // Update is called once per frame
    void Update()
    {
        this.position = transform.position;//跟新坐标
        BodyMove();//移动控制
    }
    void BodyMove()
    {
        int index = Head.snake.IndexOf(this);
        Vector3 target = Head.snake[index - 1].position;
        Vector3 a = target - transform.position;
        Vector3 b = a.normalized;
        float distance = Vector3.Distance(transform.position, target);
        if (distance < redius * scale)
        {
            rig.velocity = Vector2.zero;
            return;
        }
        rig.velocity = new Vector2(b.x * speed, b.y * speed);
    }
}
