using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class InnerPanel : MonoBehaviour
{
    GameObject inPanel;
    GameObject settingPanel;
    AudioSource camAudio;
    AudioSource canvAudio;
    public Text length;
    public Text speed;
    public Text score;
    public Button settingBtn;
    public Button bgmBtn;
    public Button effectBtn;
    public Button retBtn;
    public Button mainPanelBtn;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)//对panel赋值
        {
            if (child.name == "InPanel")
                inPanel = child.gameObject;
            else
                settingPanel = child.gameObject;
        }
        inPanel.AddComponent<CanvasGroup>();
        settingPanel.AddComponent<CanvasGroup>();
        ReturnBtn();
        camAudio = Camera.main.gameObject.AddComponent<AudioSource>();
        camAudio.loop = true;
        canvAudio = Camera.main.gameObject.AddComponent<AudioSource>();
        canvAudio.playOnAwake = false;
        canvAudio.loop = false;
        bgmBtn.onClick.AddListener(delegate () { BgmChange(bgmBtn); });
        effectBtn.onClick.AddListener(delegate () { EffectChange(effectBtn); });
        settingBtn.onClick.AddListener(OnSettingClick);
        retBtn.onClick.AddListener(ReturnBtn);
        mainPanelBtn.onClick.AddListener(ReturnMainPanel);
        //还未赋值
    }

    // Update is called once per frame
    void Update()
    {
        length.text = Head.snake.Count + 1.ToString();
        speed.text = Node.speed.ToString();
        score.text = Node.score.ToString();
    }
    void OnSettingClick()
    {
        inPanel.GetComponent<CanvasGroup>().alpha = 0;
        inPanel.GetComponent<CanvasGroup>().interactable = false;
        inPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        settingPanel.GetComponent<CanvasGroup>().alpha = 1;
        settingPanel.GetComponent<CanvasGroup>().interactable = true;
        settingPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    void ReturnBtn()
    {
        inPanel.GetComponent<CanvasGroup>().alpha = 1;
        inPanel.GetComponent<CanvasGroup>().interactable = true;
        inPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        settingPanel.GetComponent<CanvasGroup>().alpha = 0;
        settingPanel.GetComponent<CanvasGroup>().interactable = false;
        settingPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    void ReturnMainPanel()
    {
        SceneManager.LoadScene("Start");//要切换到的场景名
    }
    void BgmChange(Button btn)
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
    void EffectChange(Button btn)
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
