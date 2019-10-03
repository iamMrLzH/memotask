using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Vector3 position;//每一帧坐标
    public float redius = 1.78f;
    public Rigidbody2D rig;
    public float speed = 15f;
    public float scale = 0.5f;//放缩
    public static float score = 0;
    public static string headSkin = "_origin";
    public static string bodySkin = "_origin";
    public List<Vector2> pos = new List<Vector2>();
}
