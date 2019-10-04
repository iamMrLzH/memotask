using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mode1 : MonoBehaviour
{
    public float time = 0;
    public float originSpeed;//加速时间
    public float addSpeedTime = 5f;
    bool defence = false;//是否有护盾
    public float smartTime = 0;
    public LineRenderer line;
    public Head instance;
    List<Vector2> path = new List<Vector2>();
    // Start is called before the first frame update
    void Start()
    {
        instance = Head.instance;
        line = gameObject.GetComponent<LineRenderer>();
        originSpeed = Node.speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //this.position = transform.position; //跟新坐标
        if (time > 0)
            time -= Time.deltaTime;
        else
            Node.speed = originSpeed;
        if (smartTime <= 0)
        {
            instance.HeadMove();
            line.positionCount = 0;//消掉线段
        }

        else
            smartTime -= Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (collision.gameObject.tag == "Bomb")
        {
            Vector2 m = RandomMap.PostoMap(collision.transform.position);//重置地图
            RandomMap.canMap[(int)m.x, (int)m.y] = true;
            Destroy(collision.gameObject);
            if (defence == true)
            {
                defence = false;
                return;
            }
            Node.score -= 10;
            int length = Head.instance.snake.Count + 1;
            if (length <= 1)
                instance.Die();
            for (int i = 1; i <= length / 2; i++)
            {
                instance.Lengthdec();
            }
        }
        if (collision.gameObject.tag == "AddSpeed")
        {
            Destroy(collision.gameObject);
            time = addSpeedTime;
            Node.speed *= 1.5f;
            Node.score += 2;
        }
        if (collision.gameObject.tag == "Mogu")
        {
            Destroy(collision.gameObject);
            if (defence == true)
            {
                defence = false;
                return;
            }
            int length = instance.snake.Count + 1;
            for (int i = 0; i <= length - 1; i++)
            {
                instance.Lengthadd();
            }
            Node.score += 10;
        }
        if (collision.gameObject.tag == "Hudun")
        {
            Destroy(collision.gameObject);
            defence = true;
        }
        if (collision.gameObject.tag == "Grass")
        {
            //修改canmap
            Vector2 m = RandomMap.PostoMap(collision.transform.position);
            RandomMap.canMap[(int)m.x, (int)m.y] = true;
            Destroy(collision.gameObject);
            if (defence == true)
            {
                defence = false;
                return;
            }
            if (instance.snake.Count + 1 <= 2)
            {
                instance.Die();
                instance.Lengthdec();
                instance.Lengthdec();
            }
            Node.score -= 2;
        }
        if (collision.gameObject.tag == "SmartGrass")
        {
            Destroy(collision.gameObject);
            smartTime = 3f;
            EatSmartGrass();
        }
    }
    
    public void EatSmartGrass()
    {
        if (RandomMap.food.Count == 0)
            return;
        Vector2 next = RandomMap.food[0];
        foreach (Vector2 v in RandomMap.food)//找最近的食物
        {
            if (Vector3.Distance(transform.position, v) < Vector3.Distance(transform.position, next))
                next = v;
        }
        //Debug.Log(next);
        MapPoint target = new MapPoint(next);
        List<Vector2> path = new List<Vector2>();
        path = RandomMap.Astar(target);
        line.positionCount = path.Count; //画线
        for (int i = 0; i <= path.Count - 1; i++)
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
        instance.rig.velocity = dir.normalized * Node.speed;
        transform.up = instance.rig.velocity.normalized;
        float distance = Vector3.Distance(transform.position, end);
        while (distance > Node.redius * Node.scale)
        {
            distance = Vector3.Distance(transform.position, end);
            yield return new WaitForFixedUpdate();
        }
        //yield return new WaitUntil(() => Vector3.Distance(transform.position, end) <= redius * scale * 0.1);
        for (int index = 1; index <= path.Count - 2;)
        {
            int a = index;
            while (index == a)
            {
                if (Vector3.Distance(transform.position, end) <= Node.redius * Node.scale)
                {
                    //Debug.Log(dir.normalized);
                    start = transform.position;
                    end = path[index + 1];
                    dir = end - start;
                    instance.rig.velocity = dir.normalized * Node.speed;
                    transform.up = instance.rig.velocity.normalized;
                    index++;
                }
                yield return new WaitForFixedUpdate();
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
        instance.rig.velocity = dir.normalized * Node.speed;
        float dis = Vector3.Distance(transform.position, end);
        //Debug.Log(dis);
        //yield return new WaitUntil(() => Vector3.Distance(transform.position, end) <= redius * scale);
        while (dis > Node.redius * Node.scale)
        {
            //Debug.Log("q");
            start = transform.position;
            dir = end - start;
            instance.rig.velocity = dir.normalized * Node.speed;
            dis = Vector3.Distance(transform.position, end);
            yield return new WaitForFixedUpdate();
        }
        EatSmartGrass();
        yield break;
    }
}
