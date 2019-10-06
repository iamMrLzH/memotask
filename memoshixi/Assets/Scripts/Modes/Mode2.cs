using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Mode2 : MonoBehaviour
{
    public Head instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = Head.instance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (instance.sceneName == "Mode3"||instance.sceneName=="Mode4")
            Mode3Move();
    }
    void Mode3Move()
    {
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target.z = 0;
        Vector3 a = target - transform.position;
        transform.up = a.normalized;
        Vector3 b = a.normalized;
        if (Mathf.Abs(Vector3.Distance(target, transform.position)) < 0.1f)
            instance.rig.velocity = new Vector2(Node.speed, 0);
        else
            instance.rig.velocity = new Vector2(Node.speed, b.y * Node.speed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag=="Stick")
        {
            //Debug.Log("s");
            instance.Lengthdec();
            Node.score -= 3;
        }
        if (tag == "Key")
        {
            Destroy(collision.gameObject);
            Destroy(GameObject.FindWithTag("Lock").gameObject);
        }
        if (tag == "Exit")
        {
            InnerPanel.instance.WinPanel();
        }
        if (tag == "Die")
        {
            InnerPanel.instance.DiePanel();
        }
        if (tag == "Sphere")
        {
            Destroy(collision.gameObject);
            //Debug.Log(collision.gameObject.name);
            int text = int.Parse(collision.gameObject.GetComponentInChildren<Text>().text);
            for(int i = 1; i <= text; i++)
            {
                instance.Lengthadd();
                Node.score++;
            }
        }
        if (tag == "Diamond")
        {
            Destroy(collision.gameObject);
            int text = int.Parse(collision.gameObject.GetComponentInChildren<Text>().text);
            for(int i = 1; i <= text; i++)
            {
                instance.Lengthdec();
                Node.score--;
            }
        }
    }
}
