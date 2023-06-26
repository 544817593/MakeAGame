using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BuffManager : MonoBehaviour
{
    public static BuffManager instance;
    public List<BuffBase> listBuffs = new List<BuffBase>();
    private List<BuffBase> listToAddBuffs = new List<BuffBase>();
    private List<BuffBase> listToRemoveBuffs = new List<BuffBase>();

    public void Start()
    {
        instance = this;
        if (GameManager.Instance.buffMan != null)
        GameManager.Instance.buffMan = this;
    }


    public void Update()
    {
        // 先执行，再移除，再加入

        foreach (var buff in listBuffs)
        {
            buff.OnBuffRefresh();

        }

        foreach (var buff in listToAddBuffs)
        {
            if (buff.OnBuffCreate())
            {
                listBuffs.Add(buff);
                buff.OnBuffStart();
            }
            else
            {
                Debug.Log("create buff failed");
            }
        }

        foreach (var buff in listToRemoveBuffs)
        {
            buff.OnBuffRemove();
            listBuffs.Remove(buff);
        }

        listToAddBuffs.Clear();
        listToRemoveBuffs.Clear();
    }

    public void AddBuff(BuffBase buff)
    {
        listToAddBuffs.Add(buff);
    }

    public void RemoveBuff(BuffBase buff)
    {
        listToRemoveBuffs.Add(buff);
    }

    public int BuffNum()
    {
        return listBuffs.Count;
    }

    public void SetBuffIcon(ViewPieceBase target, string name)
    {
        Debug.LogWarning(target);
        GameObject go = new GameObject(name);
        go.transform.SetParent(target.transform);
        SpriteRenderer spr = go.AddComponent<SpriteRenderer>();
        spr.sprite = Resources.Load<Sprite>("Sprites/Items/Usable/" + name);
        spr.transform.localScale = new Vector2(0.02f, 0.02f);
        spr.transform.localPosition = new Vector3(-0.5f,1.2f,0);
        spr.sortingOrder = 10;
    }
}
