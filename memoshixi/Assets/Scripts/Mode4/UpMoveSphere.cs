using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpMoveSphere : MonoBehaviour
{
    float moveSpeed = 1.5f;
    float time = 2;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpMove());   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator UpMove()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            transform.Translate(new Vector2(0, moveSpeed*Time.fixedDeltaTime));
            time -= Time.fixedDeltaTime;
            if (time < 0)
            {
                time = 2;
                moveSpeed *= -1;
            }
        }
    }
}
