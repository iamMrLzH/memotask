using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickDistroy : MonoBehaviour
{
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        time = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        if (time <= 0)
        {
            Destroy(gameObject);
        }
        time -= Time.deltaTime;
       // transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Head")
        Head.instance.Die();
    }
}
