using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Mode1 : MonoBehaviour
{
    public float originSpeed;
    public float addSpeedTime = 0;//加速时间
    bool defence = false;//是否有护盾
    public float smartTime = 0;
    public LineRenderer line;
    public Head instance;
    List<Vector2> path = new List<Vector2>();
    Image[] allImage;
    Image HudunImage;
    Text[] texts;
    // Start is called before the first frame update
    void Start()
    {
        instance = Head.instance;
        line = gameObject.GetComponent<LineRenderer>();
        originSpeed = Node.speed;
        allImage = instance.GetComponentsInChildren<Image>();
        foreach(Image a in allImage)
        {
            if (a.gameObject.name == "Image")
                HudunImage = a;
        }
        texts = instance.GetComponentsInChildren<Text>(true);
        addSpeedTime = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //this.position = transform.position; //跟新坐标
        if (addSpeedTime > 0)
            addSpeedTime -= Time.fixedDeltaTime;
        else
            Node.speed = originSpeed;
        if (smartTime <= 0)
        {
            instance.GetComponent<SpriteRenderer>().sprite = instance.headSprite;
            instance.HeadMove();
            line.positionCount = 0;//消掉线段
        }
        else
            smartTime -= Time.fixedDeltaTime;
        if (addSpeedTime < 4 && addSpeedTime > 1)
        {
            texts[1].GetComponent<CanvasGroup>().alpha = 1;
            texts[1].text = ((int)addSpeedTime).ToString();
        }
        else
            texts[1].GetComponent<CanvasGroup>().alpha = 0;
        if (smartTime < 4 && smartTime > 1)
        {
            texts[0].GetComponent<CanvasGroup>().alpha = 1;
            texts[0].text = ((int)smartTime).ToString();
        }
        else
            texts[0].GetComponent<CanvasGroup>().alpha = 0;
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
                HudunImage.GetComponent<CanvasGroup>().alpha = 0;
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

            if (addSpeedTime <= 0)
            {
                addSpeedTime = 6f;
                Node.speed *= 1.5f;
            }
            else
                addSpeedTime = 6f;
            Node.score += 2;
        }
        if (collision.gameObject.tag == "Mogu")
        {
            Destroy(collision.gameObject);
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
            HudunImage.GetComponent<CanvasGroup>().alpha = 1;
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
                HudunImage.GetComponent<CanvasGroup>().alpha = 0;
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

            if (smartTime <= 0)
            {
                instance.GetComponent<SpriteRenderer>().sprite = instance.dizzySprite;
                smartTime = 6f;
                StartCoroutine(MoveWithGrass());
            }
            else
                smartTime = 6f;
        }
    }
    
    public List<Vector2> EatSmartGrass()
    {
        if (RandomMap.food.Count == 0)
            return null;
        Vector2 next = RandomMap.food[0];
        foreach (Vector2 v in RandomMap.food)//找最近的食物
        {
            if (Vector3.Distance(transform.position, v) < Vector3.Distance(transform.position, next))
                next = v;
        }
        MapPoint target = new MapPoint(next);
        List<Vector2> path = new List<Vector2>();
        path = RandomMap.Astar(target);
        line.positionCount = path.Count; //画线
        for (int i = 0; i <= path.Count - 1; i++)
        {
            line.SetPosition(i, path[i]);
        }
        return path;
    }
    IEnumerator MoveWithGrass()
    {
        while (true)
        {
            path = EatSmartGrass();
            if (path == null)
            {
                smartTime = 0;
                yield break;
            }
            Vector2 start = path[0];
            Vector2 end = path[1];
            Vector2 dir = end - start;
            instance.rig.velocity = dir.normalized * Node.speed;
            transform.up = instance.rig.velocity.normalized;
            int a;
            for (int index =1; index <= path.Count - 2;)
            {
                a = index;
                while (index == a)
                {
                    if (Vector3.Distance(transform.position, end) <= Node.redius * Node.scale*0.5f)
                    {
                        start = transform.position;
                        end = path[index + 1];
                        dir = end - start;
                        instance.rig.velocity = dir.normalized * Node.speed;
                        transform.up = instance.rig.velocity.normalized;
                        index++;
                    }
                    yield return new WaitForFixedUpdate();
                }
                if (smartTime <= 0)
                    yield break;
            }
            start = transform.position;
            dir = end - start;
            float dis = Vector3.Distance(start, end);
            while (dis > Node.redius * Node.scale)
            {
                start = transform.position;
                dir = end - start;
                dis = Vector3.Distance(transform.position, end);
                yield return new WaitForFixedUpdate();
            }
        }

    }
}
