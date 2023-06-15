using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using QFramework;
using Game;

public class GameSceneManager : MonoBehaviour, ICanSendEvent, ICanGetSystem, ICanSendCommand
{
    private static GameSceneManager instance; // 场景管理器实例
    public Slider slider; // 进度条
    private string currentSceneName; // 当前场景名称

    /// <summary>
    /// 场景管理器Getter
    /// </summary>
    public static GameSceneManager Instance { get { return instance; } }


    public GameObject loadingScreen; // 读条背景

    private void Awake()
    {
        // 确保单例
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        // 把自身赋予GM
        GameManager.Instance.gameSceneMan = this;
    }

    private void Start()
    {
        StartCoroutine(LoadScene("Main", false)); // 加载初始场景
    }

    /// <summary>
    /// 实际加载场景的协程
    /// </summary>
    /// <param name="sceneName">场景名</param>
    /// <param name="loadingScreenOn">是否显示加载页面</param>
    public IEnumerator LoadScene(string sceneName, bool loadingScreenOn = true)
    {

        if (loadingScreenOn)
        {
            loadingScreen.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            operation.allowSceneActivation = false;
            while (!operation.isDone)
            {
                slider.value = operation.progress;
                if (operation.progress == 0.9f)
                {
                    slider.value = 1f;
                    currentSceneName = sceneName;
                    operation.allowSceneActivation = true;
                }
                yield return null;

            }
        }
        else
        {
            //yield return new WaitForSeconds(0.5f);           
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            operation.allowSceneActivation = false;
            while (!operation.isDone)
            {
                if (operation.progress == 0.9f)
                {
                    currentSceneName = sceneName;
                    operation.allowSceneActivation = true;
                }
                yield return null;

            }
        }
            


    }

    /// <summary>
    /// 实际卸载场景的协程
    /// </summary>
    /// <param name="sceneName">想要卸载的场景名</param>
    /// <returns></returns>
    public IEnumerator UnloadScene(string sceneName)
    {
        // 退出局内场景时的处理
        this.SendEvent(new UnloadSceneEvent { sceneName = sceneName });
        if (sceneName == "Combat")
        {
            // 停止抽卡协程
            //Debug.LogError(this.GetSystem<IHandCardSystem>().handCardList == null);
            for (int i = this.GetSystem<IHandCardSystem>().handCardList.Value.Count - 1; i >= 0; i--)
            {
                ViewCard viewCard = this.GetSystem<IHandCardSystem>().handCardList.Value[i];
                this.GetSystem<IInventorySystem>().SpawnBagCardInBag(viewCard.card);
                this.SendCommand(new SubHandCardCommand(viewCard));
            }
        }
        yield return SceneManager.UnloadSceneAsync(sceneName);
        if (sceneName == "Combat")
        {
            Debug.Log("进入sceneName == \"Combat\" HideAllPanel");
            UIKit.HideAllPanel();
        }
        yield return null;
    }

    /// <summary>
    /// 提供当前的场景名，返回下一幕应该读取的场景名
    /// </summary>
    /// <param name="currSceneName">当前场景名</param>
    /// <returns></returns>
    public string GetNextScene(string currSceneName)
    {
        return "";
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// 当前场景名称Getter
    /// </summary>
    /// <returns></returns>
    public string GetCurrentSceneName()
    {
        return currentSceneName;
    }

    public IArchitecture GetArchitecture()
    {
        return GameEntry.Interface;
    }
}
