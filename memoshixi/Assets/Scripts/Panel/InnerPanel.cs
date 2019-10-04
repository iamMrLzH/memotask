using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class InnerPanel : MonoBehaviour
{
    //GameObject inPanel;
    GameObject settingPanel;
    GameObject diePanel;
    GameObject winPanel;
    AudioSource camAudio;
    AudioSource canvAudio;
    public static InnerPanel instance;
    public Text length;
    public Text speed;
    public Text score;
    public Text finalScore;
    public Button settingBtn;
    public Button bgmBtn;
    public Button effectBtn;
    public Button retBtn;
    public Button mainPanelBtn;
    public Button[] buttons;
    public AudioClip bgm;
    public AudioClip btnSound;
    public Button nextBtn;//处理第四关
    public void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Level4")
            Destroy(nextBtn);
        //inPanel = GameObject.Find("BgCanvas/InPanel");
        foreach (Transform child in transform)//对panel赋值
        {
            /*if (child.name == "InPanel")
                inPanel = child.gameObject;*/
            if(child.name=="SettingPanel")
                settingPanel = child.gameObject;
            if (child.name == "DiePanel")
                diePanel = child.gameObject;
            if (child.name == "WinPanel")
                winPanel = child.gameObject;
        }
        //inPanel.AddComponent<CanvasGroup>();
        settingPanel.AddComponent<CanvasGroup>();
        Init();
        camAudio = Camera.main.gameObject.AddComponent<AudioSource>();
        camAudio.loop = true;
        camAudio.clip = bgm;
        camAudio.Play();
        canvAudio = gameObject.AddComponent<AudioSource>();
        canvAudio.playOnAwake = false;
        canvAudio.loop = false;
        canvAudio.clip = btnSound;
        bgmBtn.onClick.AddListener(delegate () { BgmChange(bgmBtn); });
        effectBtn.onClick.AddListener(delegate () { EffectChange(effectBtn); });
        settingBtn.onClick.AddListener(OnSettingClick);
        retBtn.onClick.AddListener(ReturnBtn);
        mainPanelBtn.onClick.AddListener(ReturnMainPanel);
        //还未赋值
        buttons = transform.GetComponentsInChildren<Button>();
        foreach(Button btn in buttons)//canvas里面所有button
        {
            if (btn.name == "ResetButton")
                btn.onClick.AddListener(ResetGame);
            if (btn.name == "RetButton")
                btn.onClick.AddListener(ReturnMainPanel);
            if (btn.name == "NextButton")
                btn.onClick.AddListener(NextBtn);
            if (btn.name == "Low" || btn.name == "Mid" || btn.name == "High")
                btn.onClick.AddListener(delegate () { HardSelect(btn); });
        }

    }

    // Update is called once per frame
    void Update()
    {
        length.text = (Head.instance.snake.Count + 1).ToString();
        speed.text = Node.speed.ToString();
        score.text = Node.score.ToString();
        finalScore.text = Node.score.ToString();
        //Debug.Log(Node.score.ToString());
    }
    void OnSettingClick() //点击设置
    {
        if (canvAudio.enabled == true)
            canvAudio.Play();
        Time.timeScale = 0;
       // inPanel.GetComponent<CanvasGroup>().alpha = 0;
        //inPanel.GetComponent<CanvasGroup>().interactable = false;
        //inPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        settingPanel.GetComponent<CanvasGroup>().alpha = 1;
        settingPanel.GetComponent<CanvasGroup>().interactable = true;
        settingPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    void Init() //初始化
    {
        //inPanel.GetComponent<CanvasGroup>().alpha = 1;
       // inPanel.GetComponent<CanvasGroup>().interactable = true;
       // inPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        settingPanel.GetComponent<CanvasGroup>().alpha = 0;
        settingPanel.GetComponent<CanvasGroup>().interactable = false;
        settingPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    void ReturnBtn()//返回游戏
    {
        if (canvAudio.enabled == true)
            canvAudio.Play();
        Time.timeScale = 1;
        //inPanel.GetComponent<CanvasGroup>().alpha = 1;
        //inPanel.GetComponent<CanvasGroup>().interactable = true;
        //inPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        settingPanel.GetComponent<CanvasGroup>().alpha = 0;
        settingPanel.GetComponent<CanvasGroup>().interactable = false;
        settingPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    void ReturnMainPanel()//回主菜单
    {
        Node.score=0;
        Node.scale = 0.5f;
        Node.redius = 1.78f;
        if (canvAudio.enabled == true)
            canvAudio.Play();
        SceneManager.LoadScene("Start");//要切换到的场景名
    }
    void BgmChange(Button btn)//bgm音量有无
    {
        if (canvAudio.enabled == true)
            canvAudio.Play();
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
    void EffectChange(Button btn)//音效音量有无
    {
        if (canvAudio.enabled == true)
            canvAudio.Play();
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
    void ResetGame()//重置游戏
    {
        Time.timeScale = 1;
        string name = SceneManager.GetActiveScene().name;
        Node.score = 0;
        Node.scale = 0.5f;
        Node.redius = 1.78f;
        SceneManager.LoadSceneAsync(name);
    }
    void NextBtn()//下一关
    {
        Time.timeScale = 1;
        string name = SceneManager.GetActiveScene().name;
        if (name == "Level4")//***
            return;
        string last = name.Substring(5, 1);
        string forwardStr = "Level";
        int a = int.Parse(last);
        string newLast = (a + 1).ToString();
        string newName = forwardStr + newLast;
        SceneManager.LoadScene(newName);
    }
    public void DiePanel()
    {
        Time.timeScale = 0;
        diePanel.GetComponent<CanvasGroup>().alpha = 1;
        diePanel.GetComponent<CanvasGroup>().interactable = true;
        diePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    public void WinPanel()
    {
        Time.timeScale = 0;
        winPanel.GetComponent<CanvasGroup>().alpha = 1;
        winPanel.GetComponent<CanvasGroup>().interactable = true;
        winPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    public void HardSelect(Button btn)
    {
        if (btn.name == "Low")
            Node.speed = 7;
        if (btn.name == "Mid")
            Node.speed = 10;
        if (btn.name == "High")
            Node.speed = 13;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
