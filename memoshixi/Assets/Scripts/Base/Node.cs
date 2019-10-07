using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Vector3 position;//每一帧坐标
    public static float redius = 1.9f;//这里其实是直径不是半径
    public Rigidbody2D rig;
    public static float speed = 10f;
    public static float scale = 0.5f/2;//放缩
    public static float score = 0;
    public static string headSkin = "_origin";
    public static string bodySkin = "_origin";
    public List<Vector2> pos = new List<Vector2>();
}
