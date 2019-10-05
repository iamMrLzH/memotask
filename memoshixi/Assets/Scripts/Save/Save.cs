using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Save
{
    public string sceneName;
    public float score;
    public float speed;
    public int snakeLength;
    public Dictionary<string, List<float>> objsPosx = new Dictionary<string, List<float>>();
    public Dictionary<string, List<float>> objsPosy = new Dictionary<string, List<float>>();
    public List<float> headPosx = new List<float>();
    public List<float> headPosy = new List<float>();
    public bool[,] canMap;
    public List<float> foodx = new List<float>();
    public List<float> foody = new List<float>();
    public string color;
    public float headx;
    public float heady;
    // public Dictionary<string, List<Vector3>> objs = new Dictionary<string, List<Vector3>>();
    //public List<Vector2> headPos = new List<Vector2>();
}
