using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UnityEngine.SceneManagement;

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
				PauseMenu.gameObject.SetActive(true);
				Time.timeScale = 0;
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
                //UIKit.ClosePanel<DialogueUI.DialoguePanel?>();
                //UIKit.OpenPanel<Pause>();
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
