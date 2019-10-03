using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PanelManger : MonoBehaviour
{
    Dictionary<string,GameObject> panels =new Dictionary<string, GameObject>();//存储全部开始界面panel
    Button[] allButton;
    GameObject helpPanel;
    AudioSource audi;
    // Start is called before the first frame update
    void Start()
    {
        audi = gameObject.transform.GetComponent<AudioSource>();
        helpPanel = GameObject.Find("Canvas/HelpPanel");
        foreach(Transform child in transform)
        {
            child.gameObject.AddComponent<CanvasGroup>();
            if (panels.ContainsKey(gameObject.name))
            {
                continue;
            }
            panels.Add(child.gameObject.name, child.gameObject);
        }
        ResetPanels();
        panels["MainPanel"].GetComponent<CanvasGroup>().alpha = 1; //设置主界面显示其他界面不显示
        panels["MainPanel"].GetComponent<CanvasGroup>().interactable = true;
        panels["MainPanel"].GetComponent<CanvasGroup>().blocksRaycasts = true;
        allButton = transform.GetComponentsInChildren<Button>();
        foreach(Button btn in allButton)
        {
            btn.onClick.AddListener(ButtonClickSound); //为每个按钮添加音效
            if (btn.name.Contains("Panel"))

                btn.onClick.AddListener(delegate () { OnButtonClick1(btn.name); });

            if (btn.name == "Exit")
                btn.onClick.AddListener(Exit);
            if (btn.name.Contains("Mode"))
                btn.onClick.AddListener(delegate () { ModeSelect(btn.name); });
            if (btn.name.Contains("Switch"))
                btn.onClick.AddListener(delegate (){ Switch(btn); });
            if(btn.name.Contains("_"))//选皮肤
                btn.onClick.AddListener(delegate () { SkinChoose(btn.name); });
            if (btn.name.Contains("snake"))
                btn.onClick.AddListener(delegate () { ImageChange(btn.name); });
            if (btn.name.Contains("Level")&&btn.name!="LevelPanel")
                btn.onClick.AddListener(delegate () { LevelChoose(btn.name); });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ResetPanels()
    {
        foreach(GameObject panel in panels.Values)
        {
            panel.GetComponent<CanvasGroup>().alpha = 0;
            panel.GetComponent<CanvasGroup>().interactable = false;
            panel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }
    void OnButtonClick1(string name) //主界面按钮响应
    {
        ResetPanels();
        panels[name].GetComponent<CanvasGroup>().alpha = 1;
        panels[name].GetComponent<CanvasGroup>().interactable = true;
        panels[name].GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    void Exit()
    {
        Application.Quit();
    }
    void ModeSelect(string name)//模式选择
    {
            Time.timeScale = 1;
            SceneManager.LoadSceneAsync(name); 
    }
    void Switch(Button btn)//音量恢复与置零
    {
        if (btn.name == "SwitchBig")
        {
            if (btn.GetComponent<Image>().sprite.name == "true")
            {
                Camera.main.transform.GetComponent<AudioSource>().enabled = false;
                btn.GetComponent<Image>().sprite = (Sprite)Resources.Load<Sprite>("PanelObj/false");
            }

            else
            {
                Camera.main.transform.GetComponent<AudioSource>().enabled = true;
                btn.GetComponent<Image>().sprite = (Sprite)Resources.Load<Sprite>("PanelObj/true");
            }

        }
        else
        {
            if (btn.GetComponent<Image>().sprite.name == "true")
            {
                transform.GetComponent<AudioSource>().enabled = false;
                btn.GetComponent<Image>().sprite = (Sprite)Resources.Load<Sprite>("PanelObj/false");
            }

            else
            {
                transform.GetComponent<AudioSource>().enabled = true;
                btn.GetComponent<Image>().sprite = (Sprite)Resources.Load<Sprite>("PanelObj/true");
            }

        }

    }
    void SkinChoose(string name)
    {
        Node.bodySkin = name;
        Node.headSkin = name;
        OnButtonClick1("SettingPanel");
    }
    void ImageChange(string name)
    {
        Sprite im = (Sprite)Resources.Load<Sprite>("PanelObj/" + name);
        helpPanel.GetComponent<Image>().sprite = im;
    }
    void ButtonClickSound()
    {
        if (audi.enabled == true)
            audi.Play();
    }
    void LevelChoose(string name)
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(name);
    }
}
