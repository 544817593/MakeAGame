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

public class Dialogue : ViewController
{
    public TextAsset ink_file; 
    public Story story;
    public TextMeshProUGUI text_Pre;
    public Button[] ChoiceP ;
    public TextMeshProUGUI[] choicesText;
    private float typingSpeed = 0.04f;
    public GameObject name_text;
    public UIOpenPackPanel m_Pack;
    //public GameObject DialogueP;
    TextMeshProUGUI story_text;
    public CheckControl m_checkControl;
    public ShowGift m_showGift;
    
    private const string SPEAKER_TAG = "speaker";
    private const string PAUSE_TAG = "pause";
    private const string CHOICE_TAG = "CHOICE";
    private const string BEGIN_TAG = "start";
    private const string SHOW_TAG = "show";
    private const string SHOWNPC_TAG = "showNPC";
    private const string PLAYER_TAG = "player";
    private const string NPC_TAG = "NPC";
    private const string Wait_TAG = "wait";
    private const string Control_TAG = "control";
    private const string Pass_TAG = "pass";
    private const string Camera_TAG = "camera";
    private const string Reward_TAG = "reward";
    private const string Gift_TAG = "gift";


    private Coroutine displayCoroutine;

   
    //GameObject m_packUI;
  
    
    public bool canContinue = false;
    bool spacePressed = false;
    public bool pauseD = false;
    bool waitForChoice = false;
    public bool d_finish = false;
    bool waitForScene = false;
    public bool waitForControl = false;
    bool reward = false;
    bool waitForPass = false;
    public bool showGift = false;

    public GameObject npc;
    public GameObject backGround;
    //private bool canContinueNext = false;
    // Start is called before the first frame update
    void Start()
    {
        ResKit.Init();
        story = new Story(ink_file.text);

        if(npc)
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
        if (GetSubmitPressed() && canContinue && !pauseD && !waitForChoice && !waitForControl && !waitForScene && !waitForPass &&!showGift)
        {
            StoryUI();
        }
    
    }
    public void ShowBG(int choice)
    {
        if(choice ==1)
        {
            backGround.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/IntroUI/睁眼背景");
        }
        else if(choice ==2)
        {
            backGround.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/IntroUI/初始界面-背景图");
        }else if (choice ==3 )
        {
            backGround.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/DialogueUI/背景图");
        }
        
       
    }
    public void CheckPause()
    {
        if (m_Pack.openFinish== true)
        {
            pauseD = false;
            UIKit.HidePanel < UIOpenPackPanel >();
            
        }      
        if (m_checkControl?.c_finish == true)
        {
            waitForControl = false;
           
        }

        if (m_showGift?.m_check == true)
        {
            showGift = false;
        }
        
            
       

    
       
    }
    public void SubmitPressed()
    {
        if(Input.GetKeyDown("space") || Input.GetMouseButtonDown(0))
        {
            spacePressed = true;
        }
        else if(Input.GetKeyUp("space") || Input.GetMouseButtonUp(1))
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
        ChoiceP = UIKit.GetPanel<DialoguePanel>().choice.GetComponentsInChildren<Button>(true);
        choicesText = new TextMeshProUGUI[ChoiceP.Length];
        int index = 0;
        foreach (Button choice_p in ChoiceP)
        {
            choicesText[index] = choice_p.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }


        LoadStory();

        story_text.transform.SetParent(UIKit.GetPanel<DialoguePanel>().dialogue, false);
        
        
    }

    void EraseUI()
    {
        for(int i=0; i< UIKit.GetPanel<DialoguePanel>().dialogue.childCount; i++)
        {
            Destroy(UIKit.GetPanel<DialoguePanel>().dialogue.GetChild(i).gameObject);
        }
    }

    private void DisplayChoices()
    {
        List<Ink.Runtime.Choice> currentChoices = story.currentChoices;
        
        if(currentChoices.Count > ChoiceP.Length)
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

        for (int i = index; i<ChoiceP.Length; i++)
        {
            ChoiceP[i].gameObject.SetActive(false);
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

        
           
            if (story.canContinue)
            {
            d_finish = false;
            if (displayCoroutine != null)
                {
                    StopCoroutine(displayCoroutine);
                }
                displayCoroutine = StartCoroutine(DisplayText(story.Continue()));
           
            HandleTags(story.currentTags);
            DisplayChoices();
           
        }
        

        else if(!story.canContinue)
        {
            d_finish = true;
            UIKit.HidePanel<DialoguePanel>();
        }
       


    }

    IEnumerator DisplayText(string line)
    {
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
            yield return new WaitForSeconds(typingSpeed);
        }
        canContinue = true;
        
    }
    void HandleTags(List<string> current_Tag)
    {
        foreach (string tag in current_Tag)
        {
            string[] splitTag = tag.Split(":");
            if (splitTag.Length !=2)
            {
                Debug.LogError("wrong tag" + tag);
            }
            string tagK = splitTag[0].Trim();
            string tagV = splitTag[1].Trim();

            switch(tagK)
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
                case Wait_TAG:
                    waitForScene = true;
                    break;
                case Control_TAG:
                    waitForControl = true;
                    ShowBG(2);
                    npc.SetActive(true);
                    UIKit.HidePanel<DialoguePanel>();
                    break;
                case Pass_TAG:
                    waitForPass = true;
                    break;
                case Reward_TAG:
                    reward = true;
                    break;
                case Gift_TAG:
                    m_showGift?.PopGift();
                    showGift = true;                  
                    break;
            }
        }
    }

   
   
    
}
