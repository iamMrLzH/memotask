using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownMoveSphere : MonoBehaviour
{
    float moveSpeed = 1.5f;
    float time = 2;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DownMove());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator DownMove()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            transform.Translate(new Vector2(0, moveSpeed * -1*Time.fixedDeltaTime));
            time -= Time.fixedDeltaTime;
            if (time < 0)
            {
                time = 2;
                moveSpeed *= -1;
            }
        }
    }
}
