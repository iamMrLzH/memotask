using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        save.objsPosx = objsPosx;
        save.objsPosy = objsPosy;
        save.canMap = RandomMap.canMap;
        foreach(Vector2 v in RandomMap.food)
        {
            save.foodx.Add(v.x);
            save.foody.Add(v.y);
        }
        save.color = instance.GetComponent<SpriteRenderer>().sprite.name.Split('_')[1];
        save.headx = instance.transform.position.x;
        save.heady = instance.transform.position.y;
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
