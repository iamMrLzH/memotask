using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class LoadInit : MonoBehaviour
{
    public Head instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = Head.instance;
        if (File.Exists(Application.dataPath + "/gamesave.txt"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.dataPath + "/gamesave.txt", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();
            LoadGameInit(save);
        }
    }
        // Update is called once per frame
        void Update()
    {
        
    }
    /* save.sceneName = instance.sceneName;
         save.score = Node.score;
         save.speed = Node.speed;
         save.snakeLength = instance.snake.Count;
         Scene s = SceneManager.GetActiveScene();
     GameObject[] allObjs = s.GetRootGameObjects();
     Dictionary<string, Vector3> objs = new Dictionary<string, Vector3>();
         foreach(GameObject g in allObjs)
         {
             objs.Add(g.tag, g.transform.position);
         }
 save.objs = objs;
         save.headPos = instance.pos;*/
    GameObject[] savePrefabs;
    GameObject[] bodySkins;
    public void LoadGameInit(Save save)
    {
        instance.transform.position = new Vector2(save.headx, save.heady);
        savePrefabs = Resources.LoadAll<GameObject>("SavePrefabs");
        bodySkins = Resources.LoadAll<GameObject>("SavePrefabs/BodySkin");
        instance.sceneName = save.sceneName;
        Node.score = save.score;
        Node.speed = save.speed;
        instance.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("snake/snakehead_" + save.color);
        instance.bodyObj = (GameObject)Resources.Load<GameObject>("Prefabs/SnakeBody_" + save.color);
        Vector2 v = new Vector2();
        for (int i = 0; i <= save.headPosx.Count - 1; i++)
        {
            v = new Vector2(save.headPosx[i], save.headPosy[i]);
            instance.pos.Add(v);
        }
        for (int i = 1; i <= save.snakeLength; i++)
        {
            instance.Lengthadd();
        }
        foreach (GameObject g in savePrefabs)
        {
            List<float> posx = new List<float>();
            List<float> posy = new List<float>();
            foreach (KeyValuePair<string, List<float>> objx in save.objsPosx)
            {
                if (objx.Key == g.tag)
                {
                    posx = objx.Value;
                } 
            }
            foreach (KeyValuePair<string, List<float>> objy in save.objsPosy)
            {
                if (objy.Key == g.tag)
                {
                    posy = objy.Value;
                }
            }
            for(int i = 0; i <= posx.Count - 1; i++)
            {
                Instantiate(g, new Vector2(posx[i], posy[i]), Quaternion.identity);
            }
        }
        RandomMap.canMap = save.canMap;
        for(int i = 0; i <= save.foodx.Count - 1; i++)
        {
            RandomMap.food.Add(new Vector2(save.foodx[i], save.foody[i]));
        }
    }
}
