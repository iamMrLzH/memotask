using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Head : Node
{
    public static List<Node> snake = new List<Node>();//蛇身体
    GameObject bodyObj;
    public float time = 0;
    public float originSpeed;//加速时间
    public float addSpeedTime = 5f;
    bool defence = false;//是否有护盾
    public Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();
    public AudioSource audi;
    public string sceneName;
    public float smartTime = 0;
    public LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        sceneName = SceneManager.GetActiveScene().name;
        gameObject.tag = "Head";
        originSpeed = speed;
        bodyObj = (GameObject)Resources.Load<GameObject>("Prefabs/SnakeBody" + bodySkin);
        snake.Add(this);
        this.rig = transform.GetComponent<Rigidbody2D>();
        this.position = transform.position;
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
    void Update()
    {
        this.position = transform.position; //跟新坐标
        if (time > 0)
            time -= Time.deltaTime;
        else
            speed = originSpeed;
        if (smartTime <= 0)
        {
            HeadMove();
            line.positionCount = 0;//消掉线段
        }
            
        else
            smartTime -= Time.deltaTime;
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
        string tag = collision.gameObject.tag;
        audi.clip = clips[tag];
        audi.Play();
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

        if (collision.gameObject.tag == "Bomb")
        {
            if (sceneName == "Mode1")//修改canmap
            {
                Vector2 m = RandomMap.PostoMap(collision.transform.position);
                RandomMap.canMap[(int)m.x, (int)m.y] = true;
            }
              
            Destroy(collision.gameObject);
            if (defence == true)
            {
                defence = false;
                return;
            }
            
            int length = snake.Count + 1;
            if (length <= 1)
                Die();
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
            if (sceneName == "Mode1")//修改canmap
            {
                Vector2 m = RandomMap.PostoMap(collision.transform.position);
                RandomMap.canMap[(int)m.x, (int)m.y] = true;
            }
            Destroy(collision.gameObject);
            if (defence == true)
            {
                defence = false;
                return;
            }
            if (snake.Count + 1 <= 2)
                Die();
            Lengthdec();
            Lengthdec();
        }
        if (collision.gameObject.tag == "SmartGrass")
        {
            Destroy(collision.gameObject);
            smartTime = 3f;
            EatSmartGrass();
        }
    }
    void Die()
    {
        InnerPanel innerPanel = GameObject.Find("Canvas").GetComponent<InnerPanel>();
        innerPanel.DiePanel();
    }
    void Win()
    {
        InnerPanel innerPanel = GameObject.Find("Canvas").GetComponent<InnerPanel>();
        innerPanel.WinPanel();
    }
    List<Vector2> path=new List<Vector2>();
    void EatSmartGrass()
    {
        if (RandomMap.food.Count == 0)
            return;
        Vector2 next = RandomMap.food[0];
        foreach(Vector2 v in RandomMap.food)//找最近的食物
        {
            if (Vector3.Distance(transform.position, v) < Vector3.Distance(transform.position, next))
                next = v;
        }
        //Debug.Log(next);
        MapPoint target = new MapPoint(next);
        List<Vector2> path = new List<Vector2>();
        path=RandomMap.Astar(target);
        line.positionCount = path.Count; //画线
        for(int i = 0; i <= path.Count - 1; i++)
        {
            line.SetPosition(i, path[i]);
        }
        StartCoroutine(MoveWithGrass(path));
        //MoveWithGrass(path);
    }
    IEnumerator MoveWithGrass(List<Vector2> path)
    {
        //StartCoroutine(Timer());
        //Debug.Log(path.Count + "////");
        Vector2 start = path[0];
        Vector2 end = path[1];
        Vector2 dir = end - start;
        //rig.velocity = new Vector2(dir.normalized.x * speed, dir.normalized.y * speed);
        rig.velocity = dir.normalized * speed;
        transform.up = rig.velocity.normalized;
        float distance = Vector3.Distance(transform.position, end);
        while (distance > redius * scale)
        {
            distance = Vector3.Distance(transform.position, end);
            yield return null;
        }
        //yield return new WaitUntil(() => Vector3.Distance(transform.position, end) <= redius * scale * 0.1);
        for (int index = 1; index <= path.Count - 2; )
        {
            int a = index;
            while (index==a)
            {
                if (Vector3.Distance(transform.position, end) <= redius * scale)
                {
                    //Debug.Log(dir.normalized);
                    start = transform.position;
                    end = path[index + 1];
                    dir = end - start;
                    rig.velocity = dir.normalized * speed;
                    transform.up = rig.velocity.normalized;
                    index++;
                }
                yield return null;
            }
            // Debug.Log("?");
            //yield return new WaitUntil(() => index!=a );
            if (smartTime <= 0)
                yield break;
        }
        //Debug.Log("q");
        start = transform.position;
        dir = end - start;
       // Debug.Log(dir);
       // Debug.Log(end);
        rig.velocity = dir.normalized * speed;
        float dis = Vector3.Distance(transform.position, end);
        Debug.Log(dis);
        //yield return new WaitUntil(() => Vector3.Distance(transform.position, end) <= redius * scale);
        while (dis > redius * scale)
        {
            //Debug.Log("q");
            start = transform.position;
            dir = end - start;
            rig.velocity = dir.normalized * speed;
            dis = Vector3.Distance(transform.position, end);
            yield return null;
        }
        EatSmartGrass();
        yield break;
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
