using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    static int x = 1;
    static int y = 2;
   Vector2 [,] a=new Vector2[x,y];
    // Start is called before the first frame update
    void Start()
    {
        a[0,0] = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
