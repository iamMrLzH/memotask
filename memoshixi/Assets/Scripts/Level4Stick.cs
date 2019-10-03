using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4Stick : MonoBehaviour
{
    GameObject stickPre;
    public List<GameObject> walls = new List<GameObject>();
    float timeInternal = 2;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed=Head.instance.speed / 2;
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
            foreach (GameObject wall in walls)
            {
                if (wall.transform.position.x < 0)
                {
                    GameObject s = Instantiate(stickPre, wall.transform.position, Quaternion.identity);
                    s.transform.Rotate(0, 0, -1*90);
                    s.GetComponent<Rigidbody2D>().velocity = new Vector2(speed,0);
                }
                else
                {
                    GameObject s = Instantiate(stickPre, wall.transform.position, Quaternion.identity);
                    s.transform.Rotate(0, 0, 90);
                    s.GetComponent<Rigidbody2D>().velocity = new Vector2(-1*speed,0);
                }
                yield return new WaitForSeconds(timeInternal);
            }
            yield return new WaitForSeconds(10f);
        }
    }
}
