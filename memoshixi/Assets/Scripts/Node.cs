using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Vector3 position;
    public static float redius = 1.78f;
    public Rigidbody2D rig;
    public static float speed = 15f;
    public static float scale = 0.5f;//放缩
    public static float score = 0;
    public static string headSkin = "_origin";
    public static string bodySkin = "_origin";
}
