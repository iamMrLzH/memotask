using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AudioChange : MonoBehaviour
{
    AudioSource audi;
    public Slider sli;
    // Start is called before the first frame update
    void Start()
    {
        audi = gameObject.transform.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CtrlSound()
    {
        audi.volume = sli.value;
    }
}
