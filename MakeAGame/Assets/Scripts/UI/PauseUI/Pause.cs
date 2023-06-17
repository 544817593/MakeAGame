using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UnityEngine.SceneManagement;
using Game;

namespace PauseUI
{
	public class PauseData : UIPanelData
	{
	}
	public partial class Pause : UIPanel
	{
		
        protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as PauseData ?? new PauseData();
			// please add init code here
			PauseButton.onClick.AddListener(() =>
			{
				if(Alienation.alienationLevel == 0)
				{
                    PauseMenu.gameObject.SetActive(true);
                    Time.timeScale = 0;
                }
				else if(Alienation.alienationLevel == 1)
				{
                    Alienation1 alienation1 = new Alienation1();
					alienation1.ClickAfterStart(PauseButton.gameObject);
                }
				else if(Alienation.alienationLevel == 2)
				{
                    Alienation2 alienation2 = new Alienation2();
                    alienation2.ClickAfterStart(PauseButton.gameObject);
                }
				
			});

			BackButton.onClick.AddListener(() => 
			{
				PauseMenu.gameObject.SetActive(false);
                Time.timeScale = 1;
            });

			ExitButton.onClick.AddListener(() =>
			{
                PauseMenu.gameObject.SetActive(false);
                Time.timeScale = 1;
                string sceneName = GameSceneManager.Instance.GetCurrentSceneName();
                StartCoroutine(GameManager.Instance.gameSceneMan.LoadScene("Main", false));
                StartCoroutine(GameManager.Instance.gameSceneMan.UnloadScene(sceneName));
            });
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
		}
		
		protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}
	}
}
