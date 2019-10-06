using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SaveGame : MonoBehaviour
{
    public Head instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = Head.instance;
    }
    /*public Dictionary<string, List<float>> objsPosx = new Dictionary<string, List<float>>();
    public Dictionary<string, List<float>> objsPosy = new Dictionary<string, List<float>>();
    public List<float> headPosx = new List<float>();
    public List<float> headPosy = new List<float>();*/
    void Update()
    {
        
    }
    public Save SaveInit()
    {
        Save save = new Save();
        save.sceneName = instance.sceneName;
        save.score = Node.score;
        save.speed = Node.speed;
        save.snakeLength = instance.snake.Count;
        foreach(Vector2 v in instance.pos)
        {
            save.headPosx.Add(v.x);
            save.headPosy.Add(v.y);
        }
        Scene s = SceneManager.GetActiveScene();
        GameObject[] allObjs = s.GetRootGameObjects();
        Dictionary<string, List<float>> objsPosx = new Dictionary<string, List<float>>();
        Dictionary<string, List<float>> objsPosy = new Dictionary<string, List<float>>();
        foreach (GameObject g in allObjs)
        {
            if (!g.tag.Contains("Map"))
            {
                if (objsPosx.ContainsKey(g.tag))
                {
                    objsPosx[g.tag].Add(g.transform.position.x);
                    objsPosy[g.tag].Add(g.transform.position.y);
                }
                else
                {
                    List<float> Listx = new List<float>();
                    List<float> Listy = new List<float>();
                    Listx.Add(g.transform.position.x);
                    Listy.Add(g.transform.position.y);
                    objsPosx.Add(g.tag, Listx);
                    objsPosy.Add(g.tag, Listy);
                }
            }
            /*public Dictionary<string, List<float>> inMapsx = new Dictionary<string, List<float>>();
                public Dictionary<string, List<float>> inMapsy = new Dictionary<string, List<float>>();
                public Dictionary<string, int> inMapNum = new Dictionary<string, int>();?
            public List<float> text = new List<float>();*/
            //List<int> eachNum
            else if(g.tag=="Map3")//Mode3  Mode4
            {
                foreach(Transform tran in g.transform)
                {
                    if (tran.gameObject.tag != "Map3")
                    {

                        if (save.inMapsx.ContainsKey(tran.tag))
                        {
                            save.inMapsx[tran.tag].Add(tran.transform.position.x);
                            save.inMapsy[tran.tag].Add(tran.transform.position.y);
                            if (tran.tag != "Wall")
                                save.text[tran.tag].Add(tran.GetComponentInChildren<Text>().text);
                        }
                        else
                        {
                            List<float> Listx = new List<float>();
                            List<float> Listy = new List<float>();
                            List<string> Lists = new List<string>();
                            Listx.Add(tran.transform.position.x);
                            Listy.Add(tran.transform.position.y);
                            if(tran.tag!="Wall")
                            Lists.Add(tran.GetComponentInChildren<Text>().text);
                            save.inMapsx.Add(tran.tag, Listx);
                            save.inMapsy.Add(tran.tag, Listy);
                            if (tran.tag != "Wall")
                                save.text.Add(tran.tag, Lists);
                        }
                    }
                }
            }
        }
        save.scale = Node.scale;
        save.redius = Node.redius;
        /*foreach(string tag in save.inMapsx.Keys)//获取每张地图每种物体的个数
        {
            List<int> h = new List<int>();
            foreach(GameObject g in allObjs)
            {
                if (g.tag == "Map3")
                {
                    int num = 0;
                    foreach (Transform tran in g.transform)
                    {
                        if (tran.gameObject.tag =="tag")
                        {
                            num++;
                        }
                    }
                    h.Add(num);
                }
            }
            save.inMapNum.Add(tag, h);
        }*/
        save.objsPosx = objsPosx;
        save.objsPosy = objsPosy;
        save.canMap = RandomMap.canMap;
        foreach(Vector2 v in RandomMap.food)
        {
            save.foodx.Add(v.x);
            save.foody.Add(v.y);
        }
        if (instance.sceneName != "Mode4")
            save.color = instance.GetComponent<SpriteRenderer>().sprite.name.Split('_')[1];
        else
            save.color = "origin";
        save.headx = instance.transform.position.x;
        save.heady = instance.transform.position.y;
        if (instance.sceneName == "Mode3")
            save.minx = Camera.main.gameObject.GetComponent<Mode3Map>().minx;
        if(instance.sceneName=="Mode4")
            save.minx = Camera.main.gameObject.GetComponent<Mode4Map>().minx;
        return save;
    }
    public void saveTheGame()
    {
        Save save = SaveInit();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.dataPath + "/gamesave.txt");
        bf.Serialize(file, save);
        file.Close();
    }
}
