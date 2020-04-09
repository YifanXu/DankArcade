using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public static UIScript staticInstance;

    public GameObject JunkKillCount;
    public GameObject lifeCount;
    public GameObject GameOverPanel;

    private int junkKilled = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (staticInstance != null) Destroy(staticInstance.gameObject);
        staticInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddJunkKill ()
    {
        this.junkKilled++;
        JunkKillCount.GetComponent<Text>().text = junkKilled.ToString();
    }

    public void ChangeLifeCount (int lifeCount)
    {
        this.lifeCount.GetComponent<Text>().text = lifeCount.ToString();
    }
}
