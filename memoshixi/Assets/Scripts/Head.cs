using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Head : Node
{
    public List<Node> snake = new List<Node>();//蛇身体
    public GameObject bodyObj;
    public Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();
    public AudioSource audi;
    public string sceneName;
    public static Head instance;
    public Sprite Mode4Sprite;
    public Sprite dizzySprite;
    public Sprite headSprite;
    public int num { get { return (int)(redius * scale / speed / Time.fixedDeltaTime); } }
    private void Awake()
    {
        instance = this;
        Mode4Sprite = Resources.Load<Sprite>("Mode4Line/WhiteBody");
        sceneName = SceneManager.GetActiveScene().name;
    }
    // Start is called before the first frame update
    void Start()
    {
        headSprite = Resources.Load<Sprite>("Snake/Snakehead" + Node.headSkin);
        dizzySprite = Resources.Load<Sprite>("Snake/snakeDizzy" + Node.headSkin);
        gameObject.GetComponent<SpriteRenderer>().sprite = headSprite;
        //Debug.Log(Time.timeScale);
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
        transform.localScale = new Vector3(scale, scale, 1);
        if(SceneManager.GetActiveScene().name!="Save")
            InitBody();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(num);
        //Debug.Log(2);
        pos.Add(transform.position);
        if (sceneName.Contains("Level"))//mode2移动方式
        {
            HeadMove();
            //Debug.Log(1);
        }
        //Debug.Log(num);
    }
    public void InitPos(int startNum)
    {
        float smallDis = redius * scale * 2 / (startNum * num);
        for(int i = 0; i <= startNum*num - 1; i++)
        {
            pos.Add(new Vector2(-1 * redius * scale + i * smallDis, 0));
        }
        //Debug.Log(num);
        //Debug.Log(pos.Count);
    }
    void InitBody()
    {
        int bodys = 0;
        bodys = 4;
        if (sceneName == "Mode4")
        {
            bodys= (int)(GameInit.instance.width / 2 / (redius * scale))*2;
        }
        for(int i = 1; i <= bodys; i++)
        {
            Lengthadd();
        }
              
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
        if (snake.Count <= 1)
        {
            Die();
        }
        else
        {
            Node last = snake[snake.Count - 1];
            snake.Remove(last);
            Destroy(last.gameObject);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
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
            Node.score += 2;
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
