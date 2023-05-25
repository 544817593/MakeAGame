using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadNext : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene()
    {
        GameObject.Find("Room")?.transform.GetComponent<Game.SceneFlow>().LoadRoom();

    }
}
