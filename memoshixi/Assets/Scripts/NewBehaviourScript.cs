using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
   public GameObject obj;
    Vector3 a = new Vector3();
    // Start is called before the first frame update
    void Start()
    {
        a= Camera.main.ScreenToWorldPoint(Vector3.zero);
        Instantiate(obj, a, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
