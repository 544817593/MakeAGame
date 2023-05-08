using Game;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour, IController
{
    public IArchitecture GetArchitecture()
    {
        return GameEntry.Interface;
    }

    private GameObject pauseBtn;

    public bool isAlienation; // 是否异化
    public Material blackoutMat; // 相机黑屏材质


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 异化，引导者将游戏中的暂停按钮异化x秒，Trainee单击异化后的按钮，敌方时停y秒，我方可继续活动。
    /// </summary>
    /// <returns></returns>
    public IEnumerator Alienation(int level)
    {
        //int alienationLevel = 0;
        //float duration = 0;
        //if (level == 1)
        //{
        //    alienationLevel = 1;
        //    duration = 30;
        //}
        //else if (level == 2)
        //{
        //    alienationLevel = 2;
        //    duration = 45;
        //}

        //pauseBtn = GameObject.FindGameObjectWithTag("PauseBtn");
        //if (pauseBtn != null)
        //{
        //    pauseBtn.GetComponent<Button>().GetComponent<UnityEngine.UI.Image>().color = new Color(255, 0, 0);
        //    isAlienation = true;
        //    yield return new WaitForSeconds(duration);
        //    isAlienation = false;
        //    pauseBtn.GetComponent<Button>().GetComponent<UnityEngine.UI.Image>().color = new Color(255, 255, 255);
        //}
        //yield return null;
        return null;
    }

    /// <summary>
    /// 异化技能，敌人时停
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator EnemyTimeStop(float time)
    {
        //List<Monster> enemyList = this.GetSystem<ISpawnSystem>().GetMonsterList();
        //List<List<EntityBoxgrid>> grid2DList = GameManager.Instance.mapMan.grid2DList;
        //List<(EntityBoxgrid, float)> changedGrids = new List<(EntityBoxgrid, float)>();
        //foreach (Monster piece in enemyList)
        //{
        //    int x = piece.leftTopGridPos.x;
        //    int y = piece.leftTopGridPos.y;
        //    changedGrids.Add((grid2DList[x][y], grid2DList[x][y].TimeMultiplier));
        //    grid2DList[x][y].TimeMultiplier = 0.0001f;
        //    piece.GetComponent<CompMovement>().ResetMoveDuration();

        //}
        //yield return new WaitForSeconds(time);

        //foreach ((EntityBoxgrid, float) grid in changedGrids)
        //{
        //    grid.Item1.TimeMultiplier = grid.Item2;
        //}

        //foreach (Monster piece in enemyList)
        //{
        //    piece.GetComponent<CompMovement>().ResetMoveDuration();
        //}
        return null;
    }

    /// <summary>
    /// 地震，引导者疯狂抖动游戏屏幕，使得房间内的怪物无法移动
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public IEnumerator Earthquake(int level)
    {
        //Camera camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        //Vector3 originalPos = camera.transform.position;
        //float duration = 0;
        //if (level == 1)
        //{
        //    duration = 15f;
        //}

        //float elapsed = 0f;
        //float intensity = 2f;

        //List<Monster> enemyList = this.GetSystem<ISpawnSystem>().GetMonsterList();
        //foreach (Monster monster in enemyList)
        //{
        //    GameManager.Instance.buffMan.AddBuff(new BuffEarthquake(piece, duration));
        //    piece.GetComponent<CompMovement>().ResetMoveDuration();
        //}

        //while (elapsed < duration)
        //{
        //    float x = Random.Range(-1f, 1f) * intensity;
        //    float y = Random.Range(-1f, 1f) * intensity;

        //    camera.transform.position = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

        //    elapsed += Time.deltaTime;

        //    yield return null;
        //}

        //camera.transform.position = originalPos;
        return null;
    }

    /// <summary>
    /// 引导者帮助Trainee将房间内一半的怪物生命将为1，但Trainee将会失明7秒。
    /// </summary>
    /// <returns></returns>
    public IEnumerator DarkArrival()
    {
        List<Monster> enemyList = this.GetSystem<ISpawnSystem>().GetMonsterList();
        int counter = 0;
        foreach (Monster monster in enemyList)
        {
            if (counter % 2 == 0)
            {
                monster.hp.Value = 1f;
            }
            counter++;
        }

        Camera camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        camera.SetReplacementShader(blackoutMat.shader, "RenderType");
        yield return new WaitForSeconds(7f);
        camera.ResetReplacementShader();
    }

    /// <summary>
    /// 你的镜头出现了一些问题，需要10秒才能修好，但你的改变移动方向次数似乎增加了x次。
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public IEnumerator Focus(int level)
    {
        if (level == 1)
        {
            // 改变移动方向次数+10
        }
        else if (level == 2)
        {
            // 改变移动方向次数+20
        }

        GameManager.Instance.camMan.cameraLock = true;
        yield return new WaitForSeconds(10);
        GameManager.Instance.camMan.cameraLock = false;

        yield return null;
    }
}
