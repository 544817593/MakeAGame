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
    public DialogueUI.Choice[] ChoiceP;
    public TextMeshProUGUI[] choicesText;
    private float typingSpeed = 0.04f;
    public GameObject name_text;
    //public GameObject DialogueP;
    TextMeshProUGUI story_text;
    
    private const string SPEAKER_TAG = "speaker";
    private const string PAUSE_TAG = "pause";
    private const string CHOICE_TAG = "CHOICE";
    private const string BEGIN_TAG = "start";
    private const string SHOW_TAG = "show";
    private const string SHOWNPC_TAG = "showNPC";
    private const string PLAYER_TAG = "player";
    private const string NPC_TAG = "NPC";
    


    private Coroutine displayCoroutine;

   
    //GameObject m_packUI;
  
    
    bool canContinue = false;
    bool spacePressed = false;
    bool pauseD = false;
    bool waitForChoice = false;
    public bool d_finish = false;

    public GameObject npc;
    public GameObject backGround;
    //private bool canContinueNext = false;
    // Start is called before the first frame update
    void Start()
    {
        ResKit.Init();
        story = new Story(ink_file.text);

        //ChoiceP = DialogueUI.Choice.;
       
        
        npc.SetActive(false);
        backGround.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/MainUI/Background");
        choicesText = new TextMeshProUGUI[ChoiceP.Length];
        int index = 0;
        foreach (DialogueUI.Choice choice_p in ChoiceP)
        {
            choicesText[index] = choice_p.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
        StoryUI();

    }

    void Update()
    {
        SubmitPressed();
        CheckPause();
        if (GetSubmitPressed() && canContinue && !pauseD && !waitForChoice)
        {
            StoryUI();
        }
        
        //StoryUI();


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
        if ( PackModel.finish== true)
        {

            pauseD = false;
            
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
       
        eraseUI();
        UIKit.OpenPanel<DialoguePanel>();
        story_text = Instantiate(text_Pre) as TextMeshProUGUI;
       
        loadStory();

        story_text.transform.SetParent(UIKit.GetPanel<DialoguePanel>().dialogue, false);
        
        
    }

    void eraseUI()
    {
        //for(int i=0; i< UIKit.GetPanel<DialoguePanel>().dialogue.childCount; i++)
        //{
        //    Destroy(UIKit.GetPanel<DialoguePanel>().dialogue.GetChild(i).gameObject);
        //}
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

    
    public void ChooseChoice(int choiceIndex)
    {
     
        story.ChooseChoiceIndex(choiceIndex);
        waitForChoice = false;
        StoryUI();


    }

    void loadStory()
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
            UIKit.ClosePanel<DialoguePanel>();
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
                    UIKit.OpenPanel<UIOpenPackPanel>();
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
               

            }
        }
    }

   
   
    
}
