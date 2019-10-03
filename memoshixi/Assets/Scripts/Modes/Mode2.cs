using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mode2 : MonoBehaviour
{
    public Head instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = Head.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag=="Stick")
        {
            instance.Lengthdec();
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
    }
}
