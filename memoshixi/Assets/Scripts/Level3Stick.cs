using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Stick : MonoBehaviour
{
    GameObject stickPre;
    public List<GameObject> walls = new List<GameObject>();
    List<GameObject> upWalls = new List<GameObject>();
    List<GameObject> downWalls = new List<GameObject>();
    float timeInternal = 0.5f;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = Node.speed / 2;
        stickPre = Resources.Load<GameObject>("Prefabs/BigStick");
        StartCoroutine(MakeStick());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator MakeStick()
    {
        while (true)
        {
            foreach(GameObject wall in walls)
            {
                if (wall.transform.position.y > 0)
                    upWalls.Add(wall);
                else
                    downWalls.Add(wall);
            }
            foreach(GameObject wall in upWalls)
            {
                GameObject s= Instantiate(stickPre, wall.transform.position, Quaternion.identity);
                s.transform.Rotate(0, 0, 180);
                //StartCoroutine(Run());
                s.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -1*speed);
                yield return new WaitForSeconds(timeInternal);
            }
            foreach (GameObject wall in downWalls)
            {
                GameObject s = Instantiate(stickPre, wall.transform.position, Quaternion.identity);
                s.GetComponent<Rigidbody2D>().velocity = new Vector2(0, speed);
                yield return new WaitForSeconds(timeInternal);
            }
            yield return new WaitForSeconds(10f);
        }
    }
   /* IEnumerator Run()
    {
        stickPre.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -1 * speed);
        yield return null;
    }*/
}
