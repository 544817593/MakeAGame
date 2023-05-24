using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager inst;
    public List<BuffBase> listBuffs = new List<BuffBase>();
    private List<BuffBase> listToAddBuffs = new List<BuffBase>();
    private List<BuffBase> listToRemoveBuffs = new List<BuffBase>();

    public void Start()
    {
        inst = this;
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
}
