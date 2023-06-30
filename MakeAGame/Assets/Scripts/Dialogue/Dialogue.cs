using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;
using QFramework;
using PackOpen;
using DialogueUI;
using BagUI;
using Game;
using System;

public class Dialogue : ViewController
{
    public TextAsset ink_file;
    public Story story;
    public TextMeshProUGUI text_Pre;
    public Button[] ChoiceP;
    public TextMeshProUGUI[] choicesText;
    private float typingSpeed = 0.04f;
    public GameObject name_text;
    public UIOpenPackPanel m_Pack;
    //public GameObject DialogueP;
    TextMeshProUGUI story_text;
    public CheckControl m_checkControl;
    public ShowGift m_showGift;

    public string popName = "";
    private const string SPEAKER_TAG = "speaker";
    private const string PAUSE_TAG = "pause";
    private const string CHOICE_TAG = "CHOICE";
    private const string BEGIN_TAG = "start";
    private const string SHOW_TAG = "show";
    private const string SHOWNPC_TAG = "showNPC";
    private const string PLAYER_TAG = "player";
    private const string NPC_TAG = "NPC";
    private const string Wait_TAG = "wait";
    private const string Control_TAG = "control"; //等待玩家操作
    private const string ControlInGame_TAG = "controlG";//等待玩家进行游戏内操作
    private const string Pass_TAG = "pass";//等待该房间通关
    private const string Camera_TAG = "camera";
    private const string Reward_TAG = "reward";//奖励机制
    private const string Gift_TAG = "gift";//NPC赠送
    private const string Pop_TAG = "pop";//NPC赠送
    private const string Finshed_TAG = "Finish";
    //private const string SPRITE_DECISION_TAG = "Decision_Sprite";//精神判定机制
    //private const string STRENGTH_DECISION_TAG = "Decision_Strength";//力量判定机制
    public string bgPath = "UI/IntroUI/初始界面-背景图";
    private Coroutine displayCoroutine;

   
    //GameObject m_packUI;


    public bool canContinue = false;
    bool spacePressed = false;
    public bool pauseD = false;
    bool waitForChoice = false;
    public bool d_finish = false;
    public bool waitForScene = false;
    public bool waitForControl = false;
    public bool waitForInGamecontrol = false;
    public bool getControl = false;
    public bool nextLine = false;
    bool reward = false;
    public bool waitForPass = false;
    public bool showGift = false;
  
    
    
   
    public GameObject npc;
    public GameObject backGround;
    //private bool canContinueNext = false;
    // Start is called before the first frame update
    void Start()
    {
        ResKit.Init();
        
        story = new Story(ink_file.text);

        if (npc)
        {
            npc.SetActive(false);
        }
        if (backGround)
        {
            backGround.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/MainUI/Background");
        }


        StoryUI();
        UIKit.OpenPanel<UIOpenPackPanel>();
        UIKit.HidePanel<UIOpenPackPanel>();


        m_Pack = UIKit.GetPanel<UIOpenPackPanel>()?.GetComponent<UIOpenPackPanel>();
        for (int i = 0; i < ChoiceP.Length; i++)
        {
            int m_i = i;
            ChoiceP[i].onClick.AddListener(() =>
            {

                ChooseChoice(m_i);

            });

        }


    }

    void Update()
    {

        SubmitPressed();
        CheckPause();
        if (GetSubmitPressed() && canContinue && !pauseD && !waitForChoice && !waitForControl && !showGift && !waitForInGamecontrol && !waitForPass && !waitForScene)
        {
            GameManager.Instance.soundMan.Play_Click_Dialogue();
            StoryUI();
        }
       
    }
    public void ShowBG(int choice)
    {
        if (choice == 1)
        {
            backGround.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/IntroUI/睁眼背景");
        }
        else if (choice == 2)
        {
            backGround.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(bgPath);
        }
        else if (choice == 3)
        {
            backGround.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/DialogueUI/背景图");
        }


    }
    public void CheckPause()
    {
        if (m_Pack?.openFinish == true)
        {
            pauseD = false;
            UIKit.ClosePanel<UIOpenPackPanel>();

        }
        if (m_checkControl?.c_finish == true)
        {
            waitForControl = false;

        }

        if (m_showGift?.m_check == true)
        {
            showGift = false;
        }

        if (nextLine == true )
        {
            UIKit.HidePanel<UIHandCard>();
            UIKit.ShowPanel<DialoguePanel>();
            spacePressed = true;
            nextLine = false;
            GameManager.Instance.PauseGame();
           
        }
        if (waitForInGamecontrol || waitForPass || waitForScene)
        {
            if (GetSubmitPressed())
            {
                UIKit.ShowPanel<UIHandCard>();
                UIKit.HidePanel<DialoguePanel>();
            }
        }
        //if(UIKit.GetPanel<DiceUI.AllDiceUIPanel>()?.finish == true)
        //{
        //    decision = UIKit.GetPanel<DiceUI.AllDiceUIPanel>().decision;
        //    story.variablesState["CheckP"] = decision.ToString();

        //    WaitForDecision = false;
        //    UIKit.ClosePanel<DiceUI.AllDiceUIPanel>();
        //}





    }
    public void SubmitPressed()
    {
        if (Input.GetKeyDown("space") || Input.GetMouseButtonDown(0))
        {
            spacePressed = true;
        }
        else if (Input.GetKeyUp("space") || Input.GetMouseButtonUp(1))
        {
            spacePressed = false;
        }
    }
    public bool GetSubmitPressed()
    {
        bool result = spacePressed;
        spacePressed = false;
        return result;
    }
    void StoryUI()
    {
        UIKit.OpenPanel<DialoguePanel>();
        EraseUI();

        story_text = Instantiate(text_Pre) as TextMeshProUGUI;




        LoadStory();
        ChoiceP = UIKit.GetPanel<DialoguePanel>().choice.GetComponentsInChildren<Button>(true);
        choicesText = new TextMeshProUGUI[ChoiceP.Length];
        int index = 0;
        foreach (Button choice_p in ChoiceP)
        {
            choicesText[index] = choice_p.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }




        story_text.transform.SetParent(UIKit.GetPanel<DialoguePanel>().dialogue, false);


    }

    void EraseUI()
    {
        for (int i = 0; i < UIKit.GetPanel<DialoguePanel>().dialogue.childCount; i++)
        {
            Destroy(UIKit.GetPanel<DialoguePanel>().dialogue.GetChild(i).gameObject);
        }
        for (int j = 0; j < ChoiceP.Length; j++)
        {
            ChoiceP[j].gameObject.SetActive(false);
        }
    }

    private void DisplayChoices()
    {
        List<Ink.Runtime.Choice> currentChoices = story.currentChoices;

        if (currentChoices.Count > ChoiceP.Length)
        {
            Debug.LogError("error");
        }

        int index = 0;
        foreach (Ink.Runtime.Choice choice in currentChoices)
        {
            UIKit.GetPanel<DialoguePanel>().choice.Enable();

            waitForChoice = true;
            ChoiceP[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < ChoiceP.Length; i++)
        {
            ChoiceP[i].gameObject.SetActive(false);
        }




    }

    public void MakeDecision1()
    {

        if (PlayerManager.Instance.player.GetStats(PlayerStatsEnum.Spirit) >= 5)
        {
            story.variablesState["CheckSprite"] = true;
            PlayerManager.Instance.player.ModifyStats(PlayerStatsEnum.Spirit, 1);
        }
        else
        {
            story.variablesState["CheckSprite"] = false;
            PlayerManager.Instance.player.ModifyStats(PlayerStatsEnum.Spirit, -1);
        }

        if (PlayerManager.Instance.player.GetStats(PlayerStatsEnum.Strength) >= 5)
        {
            story.variablesState["CheckStrength"] = true;
            PlayerManager.Instance.player.ModifyStats(PlayerStatsEnum.Stamina, -1);
        }
        else
        {
            story.variablesState["CheckStrength"] = false;
            PlayerManager.Instance.player.ModifyStats(PlayerStatsEnum.Spirit, 1);
        }
    }

    public void MakeDecision2()
    {
        if (PlayerManager.Instance.player.GetStats(PlayerStatsEnum.Stamina) >= 3)
        {
            story.variablesState["CheckStamina"] = true;
            PlayerManager.Instance.player.ModifyStats(PlayerStatsEnum.Stamina, -1);
        }
        else
        {
            story.variablesState["CheckStamina"] = false;
            PlayerManager.Instance.player.ModifyStats(PlayerStatsEnum.Spirit, 1);
        }
    }
    public void MakeDecision3()
    {
        if (PlayerManager.Instance.player.GetStats(PlayerStatsEnum.Charisma) >= 5)
        {
            story.variablesState["CheckCharisma"] = true;
            PlayerManager.Instance.player.ModifyStats(PlayerStatsEnum.Charisma, 1);
        }
        else
        {
            story.variablesState["CheckCharisma"] = false;

        }
        if (PlayerManager.Instance.player.GetStats(PlayerStatsEnum.Charisma) >= 4 && PlayerManager.Instance.player.GetStats(PlayerStatsEnum.Skill) >= 4)
        {
            story.variablesState["CheckP"] = true;
            PlayerManager.Instance.player.ModifyStats(PlayerStatsEnum.Skill, 1);
        }
        else
        {
            story.variablesState["CheckP"] = false;

        }
    }
    public void ChooseChoice(int i)
    {
        story.ChooseChoiceIndex(i);
        waitForChoice = false;
        StoryUI();
    }

    void LoadStory()
    {

        story_text.text = "";
        if (ink_file.name == "NPC1")
        {
            MakeDecision1();
        }
        else if (ink_file.name == "NPC2")
        {
            MakeDecision2();
        }
        else if (ink_file.name == "NPC3")
        {
            MakeDecision3();
        }


        if (story.canContinue)
        {
            d_finish = false;
            if (displayCoroutine != null)
            {
                StopCoroutine(displayCoroutine);
            }
            displayCoroutine = StartCoroutine(DisplayText(story.Continue()));

            


        }
        else if (!story.canContinue)
        {
            d_finish = true;
            UIKit.HidePanel<DialoguePanel>();
            if(SceneFlow.combatSceneCount == 2)
            {
                GameManager.Instance.ResumeGame();
                UIKit.ShowPanel<UIHandCard>();
              
            }
        }



    }

    IEnumerator DisplayText(string line)
    {

        HandleTags(story.currentTags);

        story_text.text = line;
        story_text.maxVisibleCharacters = 0;

        canContinue = false;

        foreach (char letter in line.ToCharArray())
        {
            if (GetSubmitPressed())
            {
                story_text.maxVisibleCharacters = line.Length;
                break;
            }

            story_text.maxVisibleCharacters++;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
        DisplayChoices();
        canContinue = true;
        HandleCombatTags(story.currentTags);
    }

   public void InGameControl()
    {
        if (waitForInGamecontrol)
        {
            if (getControl)
            {
                waitForInGamecontrol = false;
                nextLine = true;
                story.variablesState["CheckControl"] = true;
            }
            else if (!getControl)
            {
                waitForInGamecontrol = false;
                nextLine = true;
                story.variablesState["CheckControl"] = false;
            }
            getControl = false;           
        }

        
    }

    public void WaitforScene()
    {
        waitForScene = false;
        nextLine = true;
    }
    public void WaitForPass()
    {
        waitForPass = false;
        nextLine = true;
        
    }
   
    void HandleTags(List<string> current_Tag)
    {
        foreach (string tag in current_Tag)
        {
            string[] splitTag = tag.Split(":");
            if (splitTag.Length != 2)
            {
                Debug.LogError("wrong tag" + tag);
            }
            string tagK = splitTag[0].Trim();
            string tagV = splitTag[1].Trim();

            switch (tagK)
            {
                case SPEAKER_TAG:
                    GameObject _Name = Instantiate(name_text);
                    TextMeshProUGUI nameText = _Name.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    nameText.text = tagV;
                    _Name.transform.SetParent(UIKit.GetPanel<DialoguePanel>().dialogue.transform, false);
                    UIKit.GetPanel<DialoguePanel>().NPC.Hide();
                    UIKit.GetPanel<DialoguePanel>().Player.Hide();
                    break;
                case BEGIN_TAG:
                    ShowBG(1);
                    break;
                case SHOW_TAG:
                    ShowBG(2);
                    break;
                case PAUSE_TAG:
                    UIKit.ShowPanel<UIOpenPackPanel>();
                    pauseD = true;
                    break;
                case CHOICE_TAG:
                    waitForChoice = true;
                    break;
                case SHOWNPC_TAG:
                    npc.SetActive(true);
                    break;
                case PLAYER_TAG:
                    UIKit.GetPanel<DialoguePanel>().NPC.Hide();
                    UIKit.GetPanel<DialoguePanel>().Player.Show();
                    break;
                case NPC_TAG:
                    UIKit.GetPanel<DialoguePanel>().NPC.Show();
                    UIKit.GetPanel<DialoguePanel>().Player.Hide();
                    break;
                case Camera_TAG:

                    break;
               
                case Control_TAG:
                    waitForControl = true;
                    ShowBG(2);
                    npc.SetActive(true);
                    UIKit.HidePanel<DialoguePanel>();
                    break;
                
                case Reward_TAG:
                    reward = true;
                    UIKit.OpenPanel<CardRewardUI.UICardRewardPanel>();
                    break;
                case Gift_TAG:
                    m_showGift?.PopGift();
                    showGift = true;
                    break;

            }
        }
    }
    void HandleCombatTags(List<string> current_Tag)
    {
        foreach (string tag in current_Tag)
        {
            string[] splitTag = tag.Split(":");
            if (splitTag.Length != 2)
            {
                Debug.LogError("wrong tag" + tag);
            }
            string tagK = splitTag[0].Trim();
            string tagV = splitTag[1].Trim();

            switch (tagK)
            {
                case ControlInGame_TAG:
                    //waitForControl = true;                
                        waitForInGamecontrol = true;
                        GameManager.Instance.ResumeGame();
                   // InGameControl();
                    break;
                case Pass_TAG:
                    waitForPass = true;
                    GameManager.Instance.ResumeGame();                 
                    break;
                case Wait_TAG:
                    waitForScene = true;
                    GameManager.Instance.ResumeGame();                     
                    break;
                case Pop_TAG:  
                    UIKit.GetPanel<UIHandCard>().PopCard(popName);
                    break;
                case Finshed_TAG:
                   
                    break;
            }


        }
    }


}
