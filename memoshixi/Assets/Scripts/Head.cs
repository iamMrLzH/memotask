using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Head : Node
{
    public List<Node> snake = new List<Node>();//蛇身体
    GameObject bodyObj;
    public Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();
    public AudioSource audi;
    public string sceneName;
    public static Head instance;
    public int num { get { return (int)(redius * scale / speed / Time.fixedDeltaTime); } }
    private void Awake()
    {
        //Debug.Log(1);
        instance = this;
        //num = get{ return (redius * scale / speed / Time.fixedDeltaTime) };
           // (int)(redius*scale/speed / Time.fixedDeltaTime);
        InitPos();
    }
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(Time.timeScale);
        sceneName = SceneManager.GetActiveScene().name;
        gameObject.tag = "Head";
        position = transform.position;
        bodyObj = (GameObject)Resources.Load<GameObject>("Prefabs/SnakeBody" + bodySkin);
        snake.Add(this);
        this.rig = transform.GetComponent<Rigidbody2D>();
       // this.position = transform.position;
        audi = gameObject.AddComponent<AudioSource>();
        audi.loop = false;
        clips["Wall"] = Resources.Load<AudioClip>("Sound/hit wall");
        clips["Bomb"] = Resources.Load<AudioClip>("Sound/boom");
        clips["Food"] = Resources.Load<AudioClip>("Sound/eat");
        clips["AddSpeed"] = Resources.Load<AudioClip>("Sound/get sheild");
        clips["Mogu"] = Resources.Load<AudioClip>("Sound/get energy");
        clips["Grass"] = Resources.Load<AudioClip>("Sound/poisonous grass");
        clips["Hudun"] = Resources.Load<AudioClip>("Sound/small trap");
        clips["SmartGrass"] = Resources.Load<AudioClip>("Sound/get energy");
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(2);
        pos.Add(transform.position);
        if (sceneName != "Mode1")
        {
            HeadMove();
            //Debug.Log(1);
        }
            
        //Debug.Log(num);
    }
    void InitPos()
    {
        float smallDis = redius * scale * 2 / (2 * num);
        for(int i = 0; i <= 2*num - 1; i++)
        {
            pos.Add(new Vector2(-1 * redius * scale + i * smallDis, 0));
        }
        //Debug.Log(num);
        //Debug.Log(pos.Count);
    }
    public void HeadMove() //移动控制以及旋转
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
       // Debug.Log(speed );
        //Debug.Log(b);
    }
    public void Lengthadd()//长度加
    {
        Vector2 newOne = snake[0].pos[snake[0].pos.Count - (snake.Count+1) * num];
        Instantiate(bodyObj, newOne, Quaternion.identity);
    }
    public void Lengthdec()//长度减
    {
        Node last = snake[snake.Count - 1];
        snake.Remove(last);
        Destroy(last.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach(string clipTag in clips.Keys)
        {
            if (clipTag == collision.gameObject.tag)
            {
                string tag = collision.gameObject.tag;
                audi.clip = clips[tag];
                audi.Play();
            }
        }  
        if (collision.gameObject.tag == "Wall")
        {
            if (sceneName == "Mode1")
                Die();
        }
        if (collision.gameObject.tag == "Food")
        {
           
            Destroy(collision.gameObject);
            if (sceneName == "Mode1")
                RandomMap.food.Remove(collision.gameObject.transform.position);
            Lengthadd();
        }
    }
    public void Die()
    {
        InnerPanel.instance.DiePanel();
    }
    /*IEnumerator Timer()
    {
        while (smartTime > 1)
        {
            smartTime -= Time.deltaTime;
            yield return null;
        }
        StopCoroutine(MoveWithGrass(path));
    }*/
}
