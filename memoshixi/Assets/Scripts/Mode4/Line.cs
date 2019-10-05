using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("ColorChange");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator ColorChange()
    {
        while (true)
        {
            int color = Random.Range(0, 4);
            if (color == 0)
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            if (color == 1)
                gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            if (color == 2)
                gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
            if (color == 3)
                gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            yield return new WaitForSeconds(1f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(0);
        StopCoroutine("ColorChange");
        collision.gameObject.GetComponent<SpriteRenderer>().color = gameObject.GetComponent<SpriteRenderer>().color;
        if (collision.tag == "Head")
        {
            GameObject[] allObjs = GameObject.FindGameObjectsWithTag("Color");
            //Debug.Log(allObjs.Length);
            foreach(GameObject obj in allObjs)
            {
                Transform[] objContents = obj.GetComponentsInChildren<Transform>();
                int ran = Random.Range(1, objContents.Length);
                objContents[ran].GetComponent<SpriteRenderer>().color= gameObject.GetComponent<SpriteRenderer>().color;
            }
        }
    }
}
