using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Head : Node
{
    public static List<Node> snake = new List<Node>();//蛇身体
    GameObject bodyObj;
    public float time = 0;//加速时间
    public float originSpeed;
    public float addSpeedTime = 5f;
    bool defence = false;
    // Start is called before the first frame update
    void Start()
    {
        originSpeed = speed;
        bodyObj = (GameObject)Resources.Load<GameObject>("Prefabs/SnakeBody" + bodySkin);
        snake.Add(this);
        this.rig = transform.GetComponent<Rigidbody2D>();
        this.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.position = transform.position; //跟新坐标
        HeadMove();
        if (time > 0)
            time -= Time.deltaTime;
        else
            speed = originSpeed;
    }
    void HeadMove() //移动控制以及旋转
    {
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target.z = 0;
        Vector3 a = target - transform.position;
        transform.up = a.normalized;
        Vector3 b = a.normalized;
        if (Mathf.Abs(Vector3.Distance(target,transform.position)) < 0.2f)
            rig.velocity = Vector2.zero;
        else
            rig.velocity = new Vector2(b.x * speed, b.y * speed);
        
    }
    void Lengthadd()//长度加
    {
        Node last = snake[snake.Count - 1];
        Vector3 lastPos = snake[snake.Count - 1].position;//末尾节点坐标
        Vector3 unit = last.rig.velocity.normalized;
        float a = 2 * redius * scale;
        Vector3 newOne = Vector3.zero;
        newOne.x = lastPos.x - unit.x * a;
        newOne.y = lastPos.y - unit.y * a;
        Instantiate(bodyObj, newOne, Quaternion.identity);
    }
    void Lengthdec()//长度减
    {
        Node last = snake[snake.Count - 1];
        snake.Remove(last);
        Destroy(last.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            if (SceneManager.GetActiveScene().name == "Mode1")
                Die();
        }
        if (collision.gameObject.tag == "Food")
        {
            
            Destroy(collision.gameObject);
            Lengthadd();
        }

        if (collision.gameObject.tag == "Bomb")
        {

            Destroy(collision.gameObject);
            if (defence == true)
            {
                defence = false;
                return;
            }
               
            int length = snake.Count + 1;
            for (int i = 1; i <= length / 2; i++)
            {
                Lengthdec();
            }
        }
        if (collision.gameObject.tag == "AddSpeed")
        {
            Destroy(collision.gameObject);
            time = addSpeedTime;
            speed *= 2;
        }
        if (collision.gameObject.tag == "Mogu")
        {
            Destroy(collision.gameObject);
            if (defence == true)
            {
                defence = false;
                return;
            }
            int length = snake.Count + 1;
            for (int i = 0; i <= length - 1; i++)
            {
                Lengthadd();
            }
        }
        if (collision.gameObject.tag == "Hudun")
        {
            Destroy(collision.gameObject);
            defence = true;
        }
        if (collision.gameObject.tag == "Grass")
        {
            Destroy(collision.gameObject);
            if (defence == true)
            {
                defence = false;
                return;
            }
            Lengthdec();
            Lengthdec();
        }
        if (collision.gameObject.tag == "SmartGrass")
        {

        }
    }
    void Die()
    {

    }
}
